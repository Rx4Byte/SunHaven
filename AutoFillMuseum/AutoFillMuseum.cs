using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text.RegularExpressions;
using UnityEngine;
using Wish;

namespace AutoFillMuseum
{
    public class PluginInfo
    {
        public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Automatic Museums Filler";
        public const string PLUGIN_GUID = "com.Rx4Byte.AutomaticMuseumsFiller";
        public const string PLUGIN_VERSION = "1.0";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public partial class AutoFillMuseum : BaseUnityPlugin
    {
        private void Awake() => Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

        // get player
        public static Player GetPlayerForCommand() { return Player.Instance; }

        // auto fill museum
        [HarmonyPatch(typeof(HungryMonster))]
        [HarmonyPatch("SetMeta")]
        class Patch_HungryMonsterSetMeta
        {
            static void Postfix(HungryMonster __instance, DecorationPositionData decorationData)
            {
                if (__instance.bundleType == BundleType.MuseumBundle)
                {
                    Player player = GetPlayerForCommand();
                    if (player == null)
                        return;
                    HungryMonster monster = __instance;
                    if (monster.sellingInventory != null) // && monster.sellingInventory.Items != null && monster.sellingInventory.Items.Count >= 1
                    {
                        foreach (SlotItemData slotItemData in monster.sellingInventory.Items)
                        {
                            if (!monster.name.ToLower().Contains("money") && slotItemData.item != null && player.Inventory != null)
                            {
                                if (slotItemData.slot.numberOfItemToAccept == 0 || slotItemData.amount == slotItemData.slot.numberOfItemToAccept)
                                    continue;
                                Inventory pInventory = player.Inventory;
                                foreach (var pItem in pInventory.Items)
                                {
                                    if (pItem.id == slotItemData.slot.itemToAccept.id)
                                    {
                                        int amount = Math.Min(pItem.amount, slotItemData.slot.numberOfItemToAccept - slotItemData.amount);
                                        monster.sellingInventory.AddItem(ItemDatabase.GetItemData(slotItemData.slot.itemToAccept.id).GetItem(), amount, slotItemData.slotNumber, false);
                                        //CommandFunction_PrintToChat($"transferred: {amount.ToString().ColorText(Color.white)} * {ItemDatabase.GetItemData(pItem.id).name.ColorText(Color.white)}");
                                        player.Inventory.RemoveItem(pItem.item, amount);
                                        monster.UpdateFullness();
                                    }
                                }
                            }
                        }
                        Array.ForEach(FindObjectsOfType<MuseumBundleVisual>(), vPodium => typeof(MuseumBundleVisual).GetMethod("OnSaveInventory", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(vPodium, null));
                    }
                }
            }
        }
    }
}