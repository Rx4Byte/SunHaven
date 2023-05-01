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
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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
        public static List<string> scenesToLoad = new List<string>();
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
                foreach (string cmd in new string[] { "test1", "test2", "test3", "test4", "test5" })
                {
                    if (mayCmdParam[0] == cmd || mayCmd.Contains(cmd))
                        return true;
                    return false;
                }
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

                case "test5":
                    PrintToChat("[TEST-5]".ColorText(Color.black) + " start".ColorText(Color.white));
                    PrintToChat("[TEST-5]".ColorText(Color.black) + (TestMethode5(mayCmdParam) ? " successful".ColorText(Color.green) : " failed".ColorText(Color.red)));
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

        #region Test Methode's
        // hide ui
        private static bool TestMethode1(string[] mayCmdParam)
        {
            if (true)  // empty/on or not
            {
                // TEST CODE
                int sceneCount = SceneManager.sceneCountInBuildSettings;
                for (int i = 0; i < sceneCount; i++)
                {
                    string scenePatch = SceneUtility.GetScenePathByBuildIndex(i);
                    string sceneName = SceneManager.GetSceneByPath(scenePatch).name;
                    if (sceneName != null && sceneName.Length >= 1)
                        PrintToChat(i.ToString() + ": " + SceneManager.GetSceneByPath(scenePatch).name);
                }

                // FEEDBACK
                PrintToChat("get scene name".ColorText(Color.green));
            }
            else
                PrintToChat("empty methode".ColorText(Color.red));
            return true;
            //PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 2
        private static bool TestMethode2(string[] mayCmdParam)
        {
            if (true)  // empty/on or not
            {
                // TEST CODE
                string filePath = "M:\\SteamLibrary\\steamapps\\common\\Sun Haven\\file.txt";
                foreach (string sceneData in scenesToLoad)
                    File.AppendAllText(filePath, sceneData + "\n");

                // FEEDBACK
                PrintToChat("written to file".ColorText(Color.green));
            }
            else
                PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 3
        private static bool TestMethode3(string[] mayCmdParam)
        {
            if (true)  // empty/on or not
            {
                // TEST CODE
                List<string> progresses = new List<string>() { "UnlockedWithergate", "Apartment", "NelvariFarm", "UnlockedNelvari", "ConfrontingDynus4Quest" };
                foreach (string progress in progresses)
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter(progress, true);

                // FEEDBACK
                PrintToChat("progess set".ColorText(Color.green));
            }
            else
                PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 4
        private static bool TestMethode4(string[] mayCmdParam)
        {
            if (true)  // empty/on or not
            {
                // TEST CODE
                if (mayCmdParam.Length >= 2)
                    teleport(mayCmdParam[1]);

                // FEEDBACK
                PrintToChat("teleport".ColorText(Color.green));
            }
            else
                PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        // TEST 5
        private static bool TestMethode5(string[] mayCmdParam)
        {
            if (true)  // empty/on or not
            {
                // TEST CODE
                PrintToChat(Player.Instance.ExactPosition.x.ToString() + " : " + Player.Instance.ExactPosition.y.ToString());

                // FEEDBACK
                PrintToChat("get coords".ColorText(Color.green));
            }
            else
                PrintToChat("empty methode".ColorText(Color.red));
            return true;
        }
        #endregion

        #region extra Methodes here if needed
        private static void teleport(string sceneName)
        {
            string text = sceneName.ToLower().Replace(" ","");
            if (text == "withergatefarm")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(126.125f, 83.6743f), "WithergateRooftopFarm"); //works
            else if (text == "throneroom")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(21.5f, 8.681581f), "Throneroom");  //works - test
            else if (text == "nelvari6")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(320.3333f, 98.76098f), "Nelvari6"); //nelvari bottom bridge
            else if (text == "wishingwell" || text.Contains("wishing"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(55.83683f, 42.80461f), "WishingWell"); //works - test
            else if (text.Contains("altar"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(199.3957f, 122.6284f), "DynusAltar"); //good
            else if (text.Contains("hospital"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(80.83334f, 65.58415f), "Hospital"); //good
            else if (text.Contains("sunhaven"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(268.125f, 299.9311f), "Town10"); //good
            else if (text.Contains("homefarm") || text.Contains("sunhavenhome") || text.Contains("playerfarm") || text == "farm")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(357f, 124.3919f), "2Playerfarm"); //good
            else if (text.Contains("nelvarifarm"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(148.25f, 100.8806f), "NelvariFarm"); //good
            else if (text.Contains("nelvarimine")) //new Vector2(154.1667f, 157.2463f)
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(144.7558f, 111.1503f), "NelvariMinesEntrance"); //works - test
            else if (text.Contains("nelvarihome"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(51.5f, 54.97755f), "NelvariPlayerHouse"); //good
            else if (text.Contains("castle")) //new Vector2(24.25f, 86.09025f)
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(133.6865f, 163.3773f), "Withergatecastleentrance"); //works - test
            else if (text.Contains("withergatehome"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(63.5f, 54.624f), "WithergatePlayerApartment"); //good
            else if (text.Contains("grandtree"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(314.4297f, 235.2298f), "GrandTreeEntrance1"); //good
            else if (text.Contains("taxi"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(101.707f, 123.4622f), "WildernessTaxi"); //works
            else if (text == "dynus")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(94.5f, 121.09f), "Dynus"); //good
            else if (text == "sewer") // new Vector2(13.70833f, 134.4075f)
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(134.5833f, 129.813f), "Sewer"); //good
            else if (text == "nivara")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(99.5f, 194.3229f), "Nivara"); //works - test
            else if (text == "barracks")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(71.58334f, 54.56507f), "Barracks"); //good
            else if (text.Contains("dragon"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(114f, 73.7052f), "DragonsMeet"); //works - test
            else if (text.Contains("dungeon"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(136.48f, 193.92f), "CombatDungeonEntrance"); //good
            else if (text.Contains("store"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(77.5f, 58.55f), "GeneralStore"); //good
            else if (text.Contains("beach"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(96.491529f, 64.69862f), "BeachRevamp");
            else
                PrintToChat("invalid scene name".ColorText(Color.red));
        }

        #endregion

        #region Patches
        
        #endregion
    }
}
