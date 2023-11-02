using BepInEx;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using Wish;

namespace AutoFillMuseum
{
    [Harmony]
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class AutoFillMuseum : BaseUnityPlugin
    {
        private const string PluginName = "Automatic Museums Filler";
        private const string PluginGuid = "com.Rx4Byte.AutomaticMuseumsFiller";
        private const string PluginVersion = "1.1";

        private static ConfigEntry<bool> ModEnabled { get; set; }
        private static ConfigEntry<bool> ShowNotifications { get; set; }
    
        private void Awake()
        { 
            ModEnabled = Config.Bind("General", "Enabled", true, $"Enable {PluginName}");
            ShowNotifications = Config.Bind("General", "Show Notifications", true, "Show notifications when items are added to the museum");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HungryMonster), nameof(HungryMonster.SetMeta))]
        private static void HungryMonster_SetMeta(HungryMonster __instance)
        {
            if (!ModEnabled.Value) return;
            if (__instance.bundleType != BundleType.MuseumBundle || Player.Instance == null || Player.Instance.Inventory == null) return;

            var playerInventory = Player.Instance.Inventory;

            if (__instance.sellingInventory == null) return;

            foreach (var slotItemData in __instance.sellingInventory.Items.Where(slotItemData => slotItemData.item != null && slotItemData.slot.numberOfItemToAccept != 0 && slotItemData.amount < slotItemData.slot.numberOfItemToAccept))
            {
                foreach (var pItem in playerInventory.Items)
                {
                    if (pItem.id != slotItemData.slot.itemToAccept.id) continue;
                    var amountToTransfer = Math.Min(pItem.amount, slotItemData.slot.numberOfItemToAccept - slotItemData.amount);
                    var itemData = ItemDatabase.GetItemData(slotItemData.slot.itemToAccept.id);
                    __instance.sellingInventory.AddItem(itemData.GetItem(), amountToTransfer, slotItemData.slotNumber, false);
                    playerInventory.RemoveItem(pItem.item, amountToTransfer);
                    __instance.UpdateFullness();
                    if (ShowNotifications.Value)
                    {
                        SingletonBehaviour<NotificationStack>.Instance.SendNotification($"Added {itemData.name} to the museum!", itemData.id, amountToTransfer);
                    }
                }
            }
        
            foreach (var vPodium in FindObjectsOfType<MuseumBundleVisual>())
            {
                vPodium.OnSaveInventory();
            }
        }
    }
}
