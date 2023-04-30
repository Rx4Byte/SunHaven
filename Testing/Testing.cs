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
        private static void PrintToChat(string text) => QuantumConsole.Instance.LogPlayerText(text);
        // check and execute methode
        public static bool TestCommand(string mayCmd, bool exit = false)
        {
            if (!debug)
                return true;
            string[] mayCmdParam = mayCmd.ToLower().Split(' ');
            if (!(mayCmdParam.Length >= 1))
            {
                PrintToChat("no params");
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
                    PrintToChat("[TEST-1]".ColorText(Color.black) + " start".ColorText(Color.white));
                    PrintToChat("[TEST-1]".ColorText(Color.black) + (TestMethode1(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                case "test2":
                    PrintToChat("[TEST-2]".ColorText(Color.black) + " start".ColorText(Color.white));
                    PrintToChat("[TEST-2]".ColorText(Color.black) + (TestMethode2(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                case "test3":
                    PrintToChat("[TEST-3]".ColorText(Color.black) + " start".ColorText(Color.white));
                    PrintToChat("[TEST-3]".ColorText(Color.black) + (TestMethode3(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
                    return true;

                case "test4":
                    PrintToChat("[TEST-4]".ColorText(Color.black) + " start".ColorText(Color.white));
                    PrintToChat("[TEST-4]".ColorText(Color.black) + (TestMethode4(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
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
        // hide ui
        private static bool TestMethode1(string[] mayCmdParam)
        {
            PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 2
        private static bool TestMethode2(string[] mayCmdParam)
        {
            PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 3
        private static bool TestMethode3(string[] mayCmdParam)
        {
            PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 4
        private static bool TestMethode4(string[] mayCmdParam)
        {
            PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        #endregion

        #region Patches

        #endregion
    }
}
