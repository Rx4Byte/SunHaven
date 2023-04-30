using BepInEx;
using HarmonyLib;
using QFSW.QC.Utilities;
using QFSW.QC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Wish;
using static System.Net.Mime.MediaTypeNames;

namespace Testing
{
    public class PluginInfo
    {
        public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Rx4Byte's Test Mod";
        public const string PLUGIN_GUID = "com.Rx4Byte.Rx4BytesTestMod";
        public const string PLUGIN_VERSION = "1.0.0";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public partial class CommandExtension : BaseUnityPlugin
    {
        private static readonly bool debug = true;
        #region Awake() | Update() | OnGui()   -   BASE UNITY OBJECT METHODES
        private void Awake()
        {
            if (debug)
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }
        private void Update()
        {

        }
        private void OnGUI()
        {

        }
        #endregion

        #region Base
        // get the player for singleplayer/multiplayer
        public static Player GetPlayer()
        {
            return Player.Instance;
        }
        // PRINT FUNCTION
        private static void LogChat(string text) => QuantumConsole.Instance.LogPlayerText(text);
        // check and execute methode
        public static bool TestCommand(string mayCmd, bool exit = false)
        {
            if (!debug)
                return true;
            string[] mayCmdParam = mayCmd.ToLower().Split(' ');
            if (!(mayCmdParam.Length >= 1))
            {
                LogChat("no params");
                return true;
            }
            if (exit)
            {
                foreach (string cmd in new string[] { "test1", "test2", "test3", "test4" })
                {
                    if (mayCmdParam[0] == cmd)
                        break;
                    return false;
                }
                return true;
            }
            switch (mayCmdParam[0])
            {
                case "test1":
                    LogChat("[TEST-1]".ColorText(Color.black) + " start".ColorText(Color.white));
                    LogChat("[TEST-1]".ColorText(Color.black) + (TestMethode1(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                case "test2":
                    LogChat("[TEST-2]".ColorText(Color.black) + " start".ColorText(Color.white));
                    LogChat("[TEST-2]".ColorText(Color.black) + (TestMethode2(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                case "test3":
                    LogChat("[TEST-3]".ColorText(Color.black) + " start".ColorText(Color.white));
                    LogChat("[TEST-3]".ColorText(Color.black) + (TestMethode3(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                case "test4":
                    LogChat("[TEST-4]".ColorText(Color.black) + " start".ColorText(Color.white));
                    LogChat("[TEST-4]".ColorText(Color.black) + (TestMethode4(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                default:
                    return false;
            }
        }
        #region two base patches
        [HarmonyPatch(typeof(Player), nameof(Player.DisplayChatBubble))]
        class Patch_PlayerDisplayChatBubble
        {
            static bool Prefix(ref string text)
            {
                if (TestCommand(text, true))
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.SendChatMessage), new[] { typeof(string), typeof(string) })]
        class Patch_PlayerSendChatMessage
        {
            static bool Prefix(string characterName, string message)
            {
                if (TestCommand(message))
                    return false;  // SEND COMMAND 
                return true;  // SEND CHAT
            }
        }
        #endregion
        #endregion

        #region Methode's
        // TEST 1
        private static bool TestMethode1(string[] mayCmdParam)
        {
            var flag = true;
            if (mayCmdParam.Length >= 2)
            {
                if (mayCmdParam[1].Contains("on") || mayCmdParam[1].StartsWith("ac"))
                    flag = true;
                else if (mayCmdParam[1].Contains("of") || mayCmdParam[1].StartsWith("de"))
                    flag = false;
                else
                    return false;
            }
            else
                return false;
            GameObject gameObject = Utilities.FindObject(GameObject.Find("Player"), "ActionBar");
            if (gameObject != null)
            {
                gameObject.SetActive(flag);
            }
            GameObject gameObject2 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "ActionBar");
            if (gameObject2 != null)
            {
                gameObject2.SetActive(flag);
            }
            GameObject gameObject3 = Utilities.FindObject(GameObject.Find("Player"), "ExpBars");
            if (gameObject3 != null)
            {
                gameObject3.SetActive(flag);
            }
            GameObject gameObject4 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "ExpBars");
            if (gameObject4 != null)
            {
                gameObject4.SetActive(flag);
            }
            GameObject gameObject5 = Utilities.FindObject(GameObject.Find("Player"), "QuestTracking");
            if (gameObject5 != null)
            {
                gameObject5.SetActive(flag);
            }
            GameObject gameObject6 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracking");
            if (gameObject6 != null)
            {
                gameObject6.SetActive(flag);
            }
            GameObject gameObject7 = Utilities.FindObject(GameObject.Find("Player"), "QuestTracker");
            if (gameObject7 != null)
            {
                gameObject7.SetActive(flag);
            }
            GameObject gameObject8 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracker");
            if (gameObject8 != null)
            {
                gameObject8.SetActive(flag);
            }
            GameObject gameObject9 = Utilities.FindObject(GameObject.Find("Player"), "HelpNotifications");
            if (gameObject9 != null)
            {
                gameObject9.SetActive(flag);
            }
            GameObject gameObject10 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "HelpNotifications");
            if (gameObject10 != null)
            {
                gameObject10.SetActive(flag);
            }
            GameObject gameObject11 = Utilities.FindObject(GameObject.Find("Player"), "NotificationStack");
            if (gameObject11 != null)
            {
                gameObject11.SetActive(flag);
            }
            GameObject gameObject12 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "NotificationStack");
            if (gameObject12 != null)
            {
                gameObject12.SetActive(flag);
            }
            GameObject gameObject13 = Utilities.FindObject(GameObject.Find("Manager"), "UI");
            if (gameObject13 != null)
            {
                gameObject13.SetActive(flag);
            }
            GameObject gameObject14 = GameObject.Find("QuestTrackerVisibilityToggle");
            if (gameObject14 != null)
            {
                gameObject14.SetActive(flag);
            }
            return true;
        }
        // TEST 2
        private static bool TestMethode2(string[] mayCmdParam)
        {
            return true;
        }
        // TEST 3
        private static bool TestMethode3(string[] mayCmdParam)
        {
            return true;
        }
        // TEST 4
        private static bool TestMethode4(string[] mayCmdParam)
        {
            return true;
        }
        #endregion

        #region Patches

        #endregion
    }
}
