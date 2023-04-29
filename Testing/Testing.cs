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
        public static bool IsTestCommand(string mayCmd, bool exit = false)
        {
            if (!debug)
                return false;
            mayCmd = mayCmd.ToLower();
            if (exit)
                foreach (string cmd in new string[] {"test1", "test2", "test3", "test4"})
                    if (mayCmd == cmd)
                        return true;
                    else
                        return false;
            switch (mayCmd)
            {
                case "test1":
                    LogChat("[TEST-1]".ColorText(Color.black) + " start".ColorText(Color.white));
                    TestMethode1();
                    LogChat("[TEST-1]".ColorText(Color.black) + " end".ColorText(Color.white));
                    return true;

                case "test2":
                    LogChat("[TEST-2]".ColorText(Color.black) + " start".ColorText(Color.white));
                    TestMethode2();
                    LogChat("[TEST-2]".ColorText(Color.black) + " end".ColorText(Color.white));
                    return true;

                case "test3":
                    LogChat("[TEST-3]".ColorText(Color.black) + " start".ColorText(Color.white));
                    TestMethode3();
                    LogChat("[TEST-3]".ColorText(Color.black) + " end".ColorText(Color.white));
                    return true;

                case "test4":
                    LogChat("[TEST-4]".ColorText(Color.black) + " start".ColorText(Color.white));
                    TestMethode4();
                    LogChat("[TEST-4]".ColorText(Color.black) + " end".ColorText(Color.white));
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
                if (IsTestCommand(text, true))
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.SendChatMessage), new[] { typeof(string), typeof(string) })]
        class Patch_PlayerSendChatMessage
        {
            static bool Prefix(string characterName, string message)
            {
                if (IsTestCommand(message))
                    return false;  // SEND COMMAND 
                return true;  // SEND CHAT
            }
        }
        #endregion
        #endregion

        #region Methode's
        // TEST 1
        private static void TestMethode1()
        {

        }
        // TEST 2
        private static void TestMethode2()
        {

        }
        // TEST 3
        private static void TestMethode3()
        {

        }
        // TEST 4
        private static void TestMethode4()
        {

        }
        #endregion

        #region Patches

        #endregion
    }
}
