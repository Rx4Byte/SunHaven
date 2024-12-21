using BepInEx;
using HarmonyLib;
using QFSW.QC;
using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using Wish;
using System.Runtime.Remoting.Messaging;


namespace CommandExtension
{
    public class PluginInfo
    {
        public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Command Extension";
        public const string PLUGIN_GUID = "com.Rx4Byte.CommandExtension";
        public const string PLUGIN_VERSION = "1.2.1";
    }
    [CommandPrefix("!")]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public partial class CommandExtension : BaseUnityPlugin
    {
        #region VAR's
        // debug var's
        public const bool debug = false;
        public const bool debugLog = debug;
        #region COMMAND's
        // DEAFULT COMMAND PARAMS
        public static class CommandParamDefaults
        {
            public const float timeMultiplier = 0.2F;
        }
        // COMMANDS
        public const string CmdPrefix = "!"; // just the prefix, no command
        public const string CmdHelp = CmdPrefix + "help";
        public const string CmdMineReset = CmdPrefix + "minereset";
        public const string CmdPause = CmdPrefix + "pause";
        public const string CmdCustomDaySpeed = CmdPrefix + "timespeed";
        public const string CmdMoney = CmdPrefix + "money";
        public const string CmdCoins = CmdPrefix + "coins";
        public const string CmdOrbs = CmdPrefix + "orbs";
        public const string CmdTickets = CmdPrefix + "tickets";
        public const string CmdSetDate = CmdPrefix + "time";
        public const string CmdWeather = CmdPrefix + "weather";
        public const string CmdDevKit = CmdPrefix + "devkit";
        public const string CmdJumper = CmdPrefix + "jumper";
        public const string CmdState = CmdPrefix + "state";
        public const string CmdPrintItemIds = CmdPrefix + "getitemids";
        public const string CmdSleep = CmdPrefix + "sleep";
        public const string CmdDasher = CmdPrefix + "dasher";
        public const string CmdManaFill = CmdPrefix + "manafill";
        public const string CmdManaInf = CmdPrefix + "manainf";
        public const string CmdHealthFill = CmdPrefix + "healthfill";
        public const string CmdNoHit = CmdPrefix + "nohit";
        public const string CmdMineOverfill = CmdPrefix + "mineoverfill";
        public const string CmdMineClear = CmdPrefix + "mineclear";
        public const string CmdNoClip = CmdPrefix + "noclip";
        public const string CmdPrintHoverItem = CmdPrefix + "printhoveritem";
        public const string CmdName = CmdPrefix + "name";
        public const string CmdFeedbackDisabled = CmdPrefix + "feedback";
        public const string CmdGive = CmdPrefix + "give";
        public const string CmdShowItems = CmdPrefix + "items";
        public const string CmdAutoFillMuseum = CmdPrefix + "autofillmuseum";
        public const string CmdCheatFillMuseum = CmdPrefix + "cheatfillmuseum";
        public const string CmdUI = CmdPrefix + "ui";
        public const string CmdTeleport = CmdPrefix + "tp";
        public const string CmdTeleportLocations = CmdPrefix + "tps";
        public const string CmdDespawnPet = CmdPrefix + "despawnpet";
        public const string CmdSpawnPet = CmdPrefix + "pet";
        public const string CmdPetList = CmdPrefix + "pets";
        public const string CmdAppendItemDescWithId = CmdPrefix + "showid";
        public const string CmdRelationship = CmdPrefix + "relationship";
        public const string CmdUnMarry = CmdPrefix + "divorce";
        public const string CmdMarryNpc = CmdPrefix + "marry";
        public const string CmdSetSeason = CmdPrefix + "season";
        public const string CmdFixYear = CmdPrefix + "yearfix";
        public const string CmdIncDecYear = CmdPrefix + "years";
        public const string CmdCheats = CmdPrefix + "cheats";
        public enum CommandState { None, Activated, Deactivated }
        // COMMAND CLASS
        public class Command
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public CommandState State { get; set; }
            public Command(string name, string description, CommandState state)
            {
                Name = name;
                Description = description;
                State = state;
            }
        }

        // COMMAND CREATION
        public static Command[] Commands = new Command[]
        {
            new Command(CmdHelp,                    "print commands to chat",                                                   CommandState.None),
            new Command(CmdMineReset,               "refill all mine shafts!",                                                  CommandState.None),
            new Command(CmdPause,                   "toggle time pause!",                                                       CommandState.Deactivated),
            new Command(CmdCustomDaySpeed,          "toggle or change dayspeed, paused if '!pause' is activ!",                  CommandState.Deactivated),
            new Command(CmdMoney,                   "give or remove coins",                                                     CommandState.None),
            new Command(CmdOrbs,                    "give or remove Orbs",                                                      CommandState.None),
            new Command(CmdTickets,                 "give or remove Tickets",                                                   CommandState.None),
            new Command(CmdSetDate,                 "set HOURE '6-23' e.g. 'set h 12'\nset DAY '1-28' e.g. 'set d 12'",         CommandState.None),
            new Command(CmdWeather,                 "'!weather [raining|heatwave|clear]'",                                      CommandState.None),
            new Command(CmdDevKit,                  "get dev items",                                                            CommandState.None),
            new Command(CmdJumper,                  "jump over object's (actually noclip while jump)",                          CommandState.Deactivated),
            new Command(CmdState,                   "print activ commands",                                                     CommandState.None),
            new Command(CmdPrintItemIds,            "print item ids [xp|money|all|bonus]",                                      CommandState.None),
            new Command(CmdSleep,                   "sleep to next the day",                                                    CommandState.None),
            new Command(CmdDasher,                  "infinite dashes",                                                          CommandState.Deactivated),
            new Command(CmdManaFill,                "mana refill",                                                              CommandState.None),
            new Command(CmdManaInf,                 "infinite mana",                                                            CommandState.Deactivated),
            new Command(CmdHealthFill,              "health refill",                                                            CommandState.None),
            new Command(CmdNoHit,                   "no hit (disable hitbox)",                                                  CommandState.Deactivated),
            new Command(CmdMineOverfill,            "fill mine completely with rocks & ores",                                   CommandState.None),
            new Command(CmdMineClear,               "clear mine completely from rocks & ores",                                  CommandState.None),
            new Command(CmdNoClip,                  "walk trough everything",                                                   CommandState.Deactivated),
            new Command(CmdPrintHoverItem,          "print item id to chat",                                                    CommandState.Deactivated),
            new Command(CmdName,                    "set name for command target ('!name Lynn') only '!name resets it' ",       CommandState.None),
            new Command(CmdFeedbackDisabled,        "toggle command feedback on/off",                                           CommandState.Deactivated),
            new Command(CmdGive,                    "give [ID] [AMOUNT]*",                                                      CommandState.None),
            new Command(CmdShowItems,               "print items with the given name",                                          CommandState.None),
            new Command(CmdAutoFillMuseum,          "toggle museum's auto fill upon entry",                                     CommandState.Deactivated),
            new Command(CmdCheatFillMuseum,         "toggle fill museum completely upon entry",                                 CommandState.Deactivated),
            new Command(CmdUI,                      "turn ui on/off",                                                           CommandState.None),
            new Command(CmdTeleport,                "teleport to some locations",                                               CommandState.None),
            new Command(CmdTeleportLocations,       "get teleport locations",                                                   CommandState.None),
            new Command(CmdDespawnPet,              "despawn current pet'",                                                     CommandState.None),
            new Command(CmdSpawnPet,                "spawn a specific pet 'pet [name]'",                                        CommandState.None),
            new Command(CmdPetList,                 "get the full list of pets '!pets'",                                        CommandState.None),
            new Command(CmdAppendItemDescWithId,    "toggle id shown to item description",                                      CommandState.Deactivated),
            new Command(CmdRelationship,            "'!relationship [name/all] [value] [add]*'",                                CommandState.None),
            new Command(CmdUnMarry,                 "unmarry an NPC '!divorce [name/all]'",                                     CommandState.None),
            new Command(CmdMarryNpc,                "marry an NPC '!marry [name/all]'",                                         CommandState.None),
            new Command(CmdSetSeason,               "change season",                                                            CommandState.None),
            new Command(CmdFixYear,                 "fix year (if needed)",                                                     CommandState.Activated),
            new Command(CmdIncDecYear,              "add or sub years '!years [value]' '-' to sub",                             CommandState.None),
            new Command(CmdCheats,                  "Toggle Cheats and hotkeys like F7,F8",                                     CommandState.Deactivated)
        };
        #endregion
        // ITEM ID's

        private static Dictionary<string, int> allIds = ItemDatabaseWrapper.ItemDatabase.ids;
        private static Dictionary<string, int> moneyIds = new Dictionary<string, int>() { { "coins", 60000 }, { "orbs", 18010 }, { "tickets", 18011 } };
        private static Dictionary<string, int> xpIds = new Dictionary<string, int>() { { "combatexp", 60003 }, { "farmingexp", 60004 }, { "miningexp", 60006 }, { "explorationexp", 60005 }, { "fishingexp", 60008 } };
        private static Dictionary<string, int> bonusIds = new Dictionary<string, int>() { { "health", 60009 }, { "mana", 60007 } };
        private static Dictionary<string, Dictionary<string, int>> categorizedItems = new Dictionary<string, Dictionary<string, int>>()
            { { "Furniture Items", new Dictionary<string, int>() },  { "Craftable Items", new Dictionary<string, int>() },
            { "Useable Items", new Dictionary<string, int>() }, { "Monster Items", new Dictionary<string, int>() },
            { "Equipable Items", new Dictionary<string, int>() }, { "Quest Items", new Dictionary<string, int>() }, { "Other Items", new Dictionary<string, int>() } };
        private static Dictionary<string, Pet> petList = null;
        private static List<string> tpLocations = new List<string>()
            { "throneroom", "nelvari", "wishingwell", "altar", "hospital", "sunhaven", "sunhavenfarm/farm/home", "nelvarifarm", "nelvarimine", "nelvarihome",
                "withergatefarm", "castle", "withergatehome", "grandtree", "taxi", "dynus", "sewer", "nivara", "barracks", "elios", "dungeon", "store", "beach" };
        // COMMAND STATE VAR'S FOR FASTER ACCESS (inside patches)
        private static bool jumpOver = Commands[Array.FindIndex(Commands, command => command.Name == CmdJumper)].State == CommandState.Activated;
        private static bool cheatsOff = Commands[Array.FindIndex(Commands, command => command.Name == CmdCheats)].State == CommandState.Activated;
        private static bool noclip = Commands[Array.FindIndex(Commands, command => command.Name == CmdNoClip)].State == CommandState.Activated;
        private static bool printOnHover = Commands[Array.FindIndex(Commands, command => command.Name == CmdPrintHoverItem)].State == CommandState.Activated;
        private static bool infMana = Commands[Array.FindIndex(Commands, command => command.Name == CmdManaInf)].State == CommandState.Activated;
        private static bool infAirSkips = Commands[Array.FindIndex(Commands, command => command.Name == CmdDasher)].State == CommandState.Activated;
        private static bool appendItemDescWithId = Commands[Array.FindIndex(Commands, command => command.Name == CmdAppendItemDescWithId)].State == CommandState.Activated;
        private static bool yearFix = Commands[Array.FindIndex(Commands, command => command.Name == CmdFixYear)].State == CommandState.Activated;
        // ...
        private static float timeMultiplier = CommandParamDefaults.timeMultiplier;
        private static string playerNameForCommandsFirst;
        private static string playerNameForCommands;
        private static string lastPetName = "";
        private static string gap = "  -  ";
        private static int ranOnceOnPlayerSpawn = 0;
        private static string lastScene;
        private static Vector2 lastLocation;
        private static Color Red = new Color(255, 0, 0);
        private static Color Green = new Color(0, 255, 0);
        private static Color Yellow = new Color(240, 240, 0);
        #endregion

        #region Awake() | Update() | OnGui()   -   BASE UNITY OBJECT METHODES
        private void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            Array.Sort(Commands, (x, y) => x.Name.CompareTo(y.Name));
        }
        private void Update()
        {
        }
        private void OnGUI()
        {
        }
        #endregion

        #region Core
        // get the player for singleplayer/multiplayer
        #region Get Wish.Player for command execution
        public static Player GetPlayerForCommand()
        {
            Player[] players = FindObjectsOfType<Player>();
            foreach (Player player in players)
            {
                if (players.Length == 1 || player.GetComponent<NetworkGamePlayer>().playerName == playerNameForCommands)
                    return player;
            }
            return null;
        }
        #endregion

        // no chat bubble for commands
        #region CheckIfCommand DisplayChatBubble
        private static bool CheckIfCommandDisplayChatBubble(string mayCommand)
        {
            mayCommand = mayCommand.ToLower();
            if (mayCommand[0] != '!' || GetPlayerForCommand() == null && !mayCommand.Contains(CmdName))
                return false;
            foreach (var command in Commands)
            {
                if (mayCommand.Contains(command.Name))
                    return true;
            }
            return false;
        }
        #endregion

        // check and execute methode
        #region CheckIfCommand SendChatMessage
        public static bool CheckIfCommandSendChatMessage(string mayCommand)
        {
            mayCommand = mayCommand.ToLower();
            if ((mayCommand[0] != '!' || GetPlayerForCommand() == null) && !mayCommand.Contains(CmdName))
                return false;

            string[] mayCommandParam = mayCommand.Split(' ');
            switch (mayCommandParam[0])
            {
                case CmdHelp:
                    return CommandFunction_Help();

                case CmdState:
                    return CommandFunction_Help(true);

                case CmdFeedbackDisabled:
                    return CommandFunction_ToggleFeedback();

                case CmdMineReset:
                    return CommandFunction_ResetMines();

                case CmdPause:
                    return CommandFunction_Pause();

                case CmdCustomDaySpeed:
                    return CommandFunction_CustomDaySpeed(mayCommand);

                case CmdMoney:
                    return CommandFunction_AddMoney(mayCommand);

                case CmdCoins:
                    return CommandFunction_AddMoney(mayCommand);

                case CmdOrbs:
                    return CommandFunction_AddOrbs(mayCommand);

                case CmdTickets:
                    return CommandFunction_AddTickets(mayCommand);

                case CmdSetDate:
                    return CommandFunction_ChangeDate(mayCommand);

                case CmdWeather:
                    return CommandFunction_ChangeWeather(mayCommand);

                case CmdDevKit:
                    return CommandFunction_GiveDevItems();

                case CmdJumper:
                    return CommandFunction_Jumper();

                case CmdCheats:
                    return CommandFunction_ToggleCheats();

                case CmdSleep:
                    return CommandFunction_Sleep();

                case CmdDasher:
                    return CommandFunction_InfiniteAirSkips();

                case CmdManaFill:
                    return CommandFunction_ManaFill();

                case CmdManaInf:
                    return CommandFunction_InfiniteMana();

                case CmdHealthFill:
                    return CommandFunction_HealthFill();

                case CmdNoHit:
                    return CommandFunction_NoHit();

                case CmdMineOverfill:
                    return CommandFunction_OverfillMines();

                case CmdMineClear:
                    return CommandFunction_ClearMines();

                case CmdNoClip:
                    return CommandFunction_NoClip();

                case CmdPrintHoverItem:
                    return CommandFunction_PrintItemIdOnHover();

                case CmdName:
                    return CommandFunction_SetName(mayCommand);

                case CmdGive:
                    return CommandFunction_GiveItemByIdOrName(mayCommandParam);

                case CmdShowItems:
                    return CommandFunction_ShowItemByName(mayCommandParam);

                case CmdPrintItemIds:
                    return CommandFunction_PrintItemIds(mayCommandParam);

                case CmdAutoFillMuseum:
                    return CommandFunction_AutoFillMuseum();

                case CmdCheatFillMuseum:
                    return CommandFunction_CheatFillMuseum();

                case CmdUI:
                    return CommandFunction_ToggleUI(mayCommand);

                case CmdTeleport:
                    return CommandFunction_TeleportToScene(mayCommandParam);

                case CmdTeleportLocations:
                    return CommandFunction_TeleportLocations();

                case CmdDespawnPet:
                    return CommandFunction_DespawnPet();

                case CmdSpawnPet:
                    return CommandFunction_SpawnPet(mayCommandParam);

                case CmdPetList:
                    return CommandFunction_GetPetList();

                case CmdAppendItemDescWithId:
                    return CommandFunction_ShowID();

                case CmdRelationship:
                    return CommandFunction_Relationship(mayCommandParam);

                case CmdUnMarry:
                    return CommandFunction_UnMarry(mayCommandParam);

                case CmdMarryNpc:
                    return CommandFunction_MarryNPC(mayCommandParam);

                case CmdSetSeason:
                    return CommandFunction_SetSeason(mayCommandParam);

                case CmdFixYear:
                    return CommandFunction_ToggleYearFix();

                case CmdIncDecYear:
                    return CommandFunction_IncDecYear(mayCommand);

                // no valid command found
                default:
                    return false;
            }
        }
        #endregion

        // Categorize all items
        #region Categorize 'ItemDatabaseWrapper.ItemDatabase.ids' into 'categorizedItems'
        private static bool CategorizeItemList()
        {
            if (ItemDatabaseWrapper.ItemDatabase.ids == null || ItemDatabaseWrapper.ItemDatabase.ids.Count < 1)
                   return false;
               foreach (var item in allIds)
               {
                   if (ItemDatabaseWrapper.ItemDatabase.GetItemData(item.Value).category == ItemCategory.Furniture)
                       categorizedItems["Furniture Items"].Add(item.Key, item.Value);
                   else if (ItemDatabaseWrapper.ItemDatabase.GetItemData(item.Value).category == ItemCategory.Equip)
                       categorizedItems["Equipable Items"].Add(item.Key, item.Value);
                   else if (ItemDatabaseWrapper.ItemDatabase.GetItemData(item.Value).category == ItemCategory.Quest)
                       categorizedItems["Quest Items"].Add(item.Key, item.Value);
                   else if (ItemDatabaseWrapper.ItemDatabase.GetItemData(item.Value).category == ItemCategory.Craftable)
                       categorizedItems["Craftable Items"].Add(item.Key, item.Value);
                   else if (ItemDatabaseWrapper.ItemDatabase.GetItemData(item.Value).category == ItemCategory.Monster)
                       categorizedItems["Monster Items"].Add(item.Key, item.Value);
                   else if (ItemDatabaseWrapper.ItemDatabase.GetItemData(item.Value).category == ItemCategory.Use)
                       categorizedItems["Useable Items"].Add(item.Key, item.Value);
                   else
                       categorizedItems["Other Items"].Add(item.Key, item.Value);
               }
            return true;
        }
        #endregion

        // command methodes
        #region CommandFunction - Methode's
        // PRINT FUNCTION **
        private static void CommandFunction_PrintToChat(string text)
        {
            if (Commands.First(command => command.Name == CmdFeedbackDisabled).State == CommandState.Deactivated)
                QuantumConsole.Instance.LogPlayerText(text);
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        // HELP *
        private static bool CommandFunction_Help(bool status = false)
        {
            if (!status)
            {
                CommandFunction_PrintToChat("[HELP]".ColorText(Color.black));
                foreach (Command command in Commands)
                {
                    if (command.State == CommandState.Activated)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Green)}{gap.ColorText(Color.black)}{command.Description.ColorText(Yellow)}");
                    if (command.State == CommandState.Deactivated)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Red)}{gap.ColorText(Color.black)}{command.Description.ColorText(Yellow)}");
                    if (command.State == CommandState.None)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Yellow)}{gap.ColorText(Color.black)}{command.Description.ColorText(Yellow)}");
                }
            }
            else
            {
                CommandFunction_PrintToChat("[STATE]".ColorText(Color.black));
                foreach (Command command in Commands)
                {
                    if (command.State == CommandState.Activated)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Yellow)}{gap.ColorText(Color.black)}{(command.State.ToString().ColorText(Green))}");
                }
            }
            return true;
        }
        // PRINT SPECIAL ITEMS
        private static bool CommandFunction_PrintItemIds(string[] mayCommandParam)
        {
            switch ((mayCommandParam.Length >= 2) ? mayCommandParam[1][0] : '-')
            {
                case 'x':
                    CommandFunction_PrintToChat("[XP-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in xpIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'm':
                    CommandFunction_PrintToChat("[MONEY-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in moneyIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'a':
                    if (CategorizeItemList())
                    {
                        string file = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")) + "\\itemIDs(CommandExtension).txt";
                        if (!File.Exists(file))
                        {
                            using (File.Create(file)) { }
                        }
                        bool isEmpty = true;
                        File.WriteAllText(file, "");
                        string overviewLine = new string('#', 80);
                        foreach (var category in categorizedItems)
                        {
                            if (category.Value.Count >= 1)
                            {
                                if (isEmpty)
                                {
                                    File.AppendAllText(file, $"[{category.Key}]\n");
                                    isEmpty = false;
                                }
                                else
                                    File.AppendAllText(file, $"\n\n\n{overviewLine}\n[{category.Key}]\n");
                                foreach (var item in category.Value.OrderBy(i => i.Key))
                                {
                                    File.AppendAllText(file, $"{item.Key} : {item.Value}\n");
                                }
                                File.AppendAllText(file, "");
                            }
                        }
                        CommandFunction_PrintToChat("ID list created inside your Sun Haven folder:".ColorText(Color.green));
                        CommandFunction_PrintToChat(file.ColorText(Color.white));
                    }
                    else
                        CommandFunction_PrintToChat("ERROR: ".ColorText(Red) + "ItemDatabaseWrapper.ItemDatabase.ids".ColorText(Color.white) + " is empty!".ColorText(Red));
                    break;
                case 'b':
                    CommandFunction_PrintToChat("[BONUS-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in bonusIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'f':
                    CommandFunction_PrintToChat("[FURNITURE-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in allIds)
                        if (ItemDatabaseWrapper.ItemDatabase.GetItemData(id.Value).category == ItemCategory.Furniture)
                            CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'q':
                    CommandFunction_PrintToChat("[QUEST-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in allIds)
                        if (ItemDatabaseWrapper.ItemDatabase.GetItemData(id.Value).category == ItemCategory.Quest)
                            CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                default:
                    CommandFunction_PrintToChat(CmdPrintItemIds + " [xp|money|all|bonus|furniture|quest]".ColorText(Red));
                    return true;
            }
            return true;
        }
        // RESET MINES
        private static bool CommandFunction_ResetMines()
        {
            if (FindObjectOfType<MineGenerator2>() != null)
            {
                typeof(MineGenerator2).GetMethod("ResetMines", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MineGenerator2.Instance, new object[] { (ushort)0 });
                CommandFunction_PrintToChat($"Mine {"Reseted".ColorText(Green)}!".ColorText(Yellow));
            }
            else
                CommandFunction_PrintToChat(("Must be inside a Mine!").ColorText(Red));
            return true;
        }
        // CLEAR MINES
        private static bool CommandFunction_ClearMines()
        {
            if (FindObjectOfType<MineGenerator2>() != null)
            {
                typeof(MineGenerator2).GetMethod("ClearMine", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MineGenerator2.Instance, null);
                CommandFunction_PrintToChat($"Mine {"Cleared".ColorText(Green)}!".ColorText(Yellow));
            }
            else
                CommandFunction_PrintToChat(("Must be inside a Mine!").ColorText(Red));
            return true;
        }
        // OVERFILL MINES
        private static bool CommandFunction_OverfillMines()
        {
            if (FindObjectOfType<MineGenerator2>() != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    typeof(MineGenerator2).GetMethod("GenerateRocks", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MineGenerator2.Instance, null);
                }
                CommandFunction_PrintToChat($"Mine {"Overfilled".ColorText(Green)}!".ColorText(Yellow));
            }
            else
                CommandFunction_PrintToChat(("Must be inside a Mine!").ColorText(Red));
            return true;
        }
        // ADD MONEY/COINS
        private static bool CommandFunction_AddMoney(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int moneyAmount))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!money 500' or '!coins 500'".ColorText(Red));
                return true;
            }
            if (mayCommand.Contains("-"))
            {
                GetPlayerForCommand().AddMoney(-moneyAmount, true, true, true);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} paid {moneyAmount.ToString().ColorText(Color.white)} Coins!".ColorText(Yellow));
            }
            else
            {
                GetPlayerForCommand().AddMoney(moneyAmount, true, true, true);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {moneyAmount.ToString().ColorText(Color.white)} Coins!".ColorText(Yellow));
            }
            return true;
        }
        // ADD MONEY
        private static bool CommandFunction_AddOrbs(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int moneyAmount))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!orbs 500'".ColorText(Red));
                return true;
            }
            if (mayCommand.Contains("-"))
            {
                GetPlayerForCommand().AddOrbs(-moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} paid {moneyAmount.ToString().ColorText(Color.white)} Orbs!".ColorText(Yellow));
            }
            else
            {
                GetPlayerForCommand().AddOrbs(moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {moneyAmount.ToString().ColorText(Color.white)} Orbs!".ColorText(Yellow));
            }
            return true;
        }
        // ADD MONEY
        private static bool CommandFunction_AddTickets(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int moneyAmount))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!tickets 500'".ColorText(Red));
                return true;
            }
            if (mayCommand.Contains("-"))
            {
                GetPlayerForCommand().AddTickets(-moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} paid {moneyAmount.ToString().ColorText(Color.white)} Tickets!".ColorText(Yellow));
            }
            else
            {
                GetPlayerForCommand().AddTickets(moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {moneyAmount.ToString().ColorText(Color.white)} Tickets!".ColorText(Yellow));
            }
            return true;
        }
        // SET NAME
        private static bool CommandFunction_SetName(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.Split(' ');
            if (mayCommand.Length <= CmdName.Length + 1)
            {
                playerNameForCommands = playerNameForCommandsFirst;
                CommandFunction_PrintToChat($"Command target name {"reseted".ColorText(Green)} to {playerNameForCommandsFirst.ColorText(Color.magenta)}!".ColorText(Yellow));
            }
            else if (mayCommandParam.Length >= 2)
            {
                playerNameForCommands = mayCommandParam[1];
                CommandFunction_PrintToChat($"Command target name changed to {mayCommandParam[1].ColorText(Color.magenta)}!".ColorText(Color.yellow));
            }
            return true;
        }
        // CHANGE DATE (only hour and day!)
        public static bool CommandFunction_ChangeDate(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.ToLower().Split(' ');
            if (mayCommandParam.Length == 3)
            {
                var Date = DayCycle.Instance;
                if (int.TryParse(mayCommandParam[2], out int dateValue))
                {
                    switch (mayCommandParam[1][0])
                    {
                        case 'd':
                            if (dateValue <= 0 || dateValue > 28)
                            { CommandFunction_PrintToChat($"day {"1-28".ColorText(Color.white)} are allowed".ColorText(Red)); return true; }
                            Date.Time = new DateTime(Date.Time.Year, Date.Time.Month, dateValue, Date.Time.Hour + 1, Date.Time.Minute, Date.Time.Second, Date.Time.Millisecond);
                            typeof(DayCycle).GetMethod("SetInitialTime", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DayCycle.Instance, null);
                            CommandFunction_PrintToChat($"{"Day".ColorText(Green)} set to {dateValue.ToString().ColorText(Color.white)}!".ColorText(Yellow));
                            break;
                        case 'h':
                            if (dateValue < 6 || dateValue > 23) // 6-23 
                            { CommandFunction_PrintToChat($"hour {"6-23".ColorText(Color.white)} are allowed".ColorText(Red)); return true; }
                            Date.Time = new DateTime(Date.Time.Year, Date.Time.Month, Date.Time.Day, dateValue + 1, Date.Time.Minute, Date.Time.Second, Date.Time.Millisecond);
                            CommandFunction_PrintToChat($"{"Hour".ColorText(Green)} set to {dateValue.ToString().ColorText(Color.white)}!".ColorText(Yellow));
                            break;
                    }
                }
            }
            return true;
        }
        // CHANGE WEATHER
        public static bool CommandFunction_ChangeWeather(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.ToLower().Split(' ');
            if (mayCommandParam.Length >= 2)
            {
                var Date = DayCycle.Instance;
                switch (mayCommandParam[1][0])
                {
                    case 'r': // rain toggle
                        Date.SetToRaining(Date.Raining ? false : true);
                        CommandFunction_PrintToChat($"{"Raining".ColorText(Green)} turned {(!Date.Raining ? "Off".ColorText(Red) : "On".ColorText(Green))}!".ColorText(Yellow));
                        break;
                    case 'h': // heatwave toggle
                        Date.SetToHeatWave(Date.Heatwave ? false : true);
                        CommandFunction_PrintToChat($"{"Heatwave".ColorText(Green)} turned {(!Date.Heatwave ? "Off".ColorText(Red) : "On".ColorText(Green))}!".ColorText(Yellow));
                        break;
                    case 'c': // clear both
                        Date.SetToHeatWave(false);
                        Date.SetToRaining(false);
                        CommandFunction_PrintToChat($"{"Sunny".ColorText(Green)} weather turned {"On".ColorText(Green)}!".ColorText(Yellow));
                        break;
                }
            }
            return true;
        }
        // GIVE ITEM BY ID/NAME 
        private static bool CommandFunction_GiveItemByIdOrName(string[] mayCommandParam)
        {
            if (mayCommandParam.Length >= 2)
            {
                if (int.TryParse(mayCommandParam[1], out int itemId))
                {
                    if (allIds.Values.Contains(itemId))
                    {
                        int itemAmount = (mayCommandParam.Length >= 3 && int.TryParse(mayCommandParam[2], out itemAmount)) ? itemAmount : 1;
                        GetPlayerForCommand().Inventory.AddItem(itemId, itemAmount, 0, true, true);
                        CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {itemAmount.ToString().ColorText(Color.white)} * {ItemDatabaseWrapper.ItemDatabase.GetItemData(itemId).name.ColorText(Color.white)}!".ColorText(Yellow));
                    }
                    else
                        CommandFunction_PrintToChat($"no item with id: {itemId.ToString().ColorText(Color.white)} found!".ColorText(Red));
                }
                else
                {
                    int itemsFound = 0;
                    int lastItemId = 0;
                    if (mayCommandParam.Length >= 2)
                    {
                      /*  foreach (KeyValuePair<string, int> id in allIds)
                        {
                            if (id.Key.Contains(mayCommandParam[1].ToLower()))
                            {
                                lastItemId = id.Value;
                                itemsFound++;
                                if (mayCommandParam[1] == id.Key)
                                {
                                    int itemAmount = ((mayCommandParam.Length >= 3 && int.TryParse(mayCommandParam[2], out itemAmount)) ? itemAmount : 1);
                                    GetPlayerForCommand().Inventory.AddItem(lastItemId, itemAmount, 0, true, true);
                                    return true;
                                }
                            }
                        }
                        if (itemsFound == 1)
                        {
                            int itemAmount = ((mayCommandParam.Length >= 3 && int.TryParse(mayCommandParam[2], out itemAmount)) ? itemAmount : 1);
                            GetPlayerForCommand().Inventory.AddItem(lastItemId, itemAmount, 0, true, true);
                        }
                        else if (itemsFound > 1)
                        {
                            CommandFunction_PrintToChat("[FOUND ITEMS]".ColorText(Color.black) + "use a unique item-name or id".ColorText(Color.white));
                            foreach (KeyValuePair<string, int> id in allIds)
                            {
                                if (id.Key.Contains(mayCommandParam[1]))
                                    CommandFunction_PrintToChat(id.Key + " : ".ColorText(Color.black) + id.Value.ToString());
                            }
                        }
                        else
                            CommandFunction_PrintToChat($"no item name contains {mayCommandParam[1].ColorText(Color.white)}!".ColorText(Red));*/
                    }
                    else
                        CommandFunction_PrintToChat($"invalid itemId!".ColorText(Red));
                    return true;
                }
            }
            else
                CommandFunction_PrintToChat($"wrong use of !give".ColorText(Red));
            return true;
        }
        // GIVE ITEM BY ID/NAME 
        private static bool CommandFunction_ShowItemByName(string[] mayCommandParam)
        {
            if (mayCommandParam.Length >= 2)
            {
                List<string> items = new List<string>();
               // foreach (KeyValuePair<string, int> id in allIds)
            //    {
            //        if (id.Key.ToLower().Contains(mayCommandParam[1]))
              //          items.Add(id.Key.ColorText(Color.white) + " : ".ColorText(Color.black) + id.Value.ToString().ColorText(Color.white));
             //   }
            //    if (items.Count >= 1)
            //    {
            //        CommandFunction_PrintToChat("[FOUND ITEMS]".ColorText(Color.black));
                //    foreach (string ítem in items)
                //        CommandFunction_PrintToChat(ítem);
               // }
            }
            return true;
        }
        // GIVE DEV ITEMS
        private static bool CommandFunction_GiveDevItems()
        {
            foreach (int item in new int[] { 30003, 30004, 30005, 30006, 30007, 30008 })
                GetPlayerForCommand().Inventory.AddItem(item, 1, 0, true);
            CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got a {"DevKit".ColorText(Color.white)}".ColorText(Yellow));
            return true;
        }
        // INFINITE AIR-SKIPS
        private static bool CommandFunction_InfiniteAirSkips()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdDasher);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = infAirSkips = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // SHOW ID
        private static bool CommandFunction_ShowID()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdAppendItemDescWithId);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = appendItemDescWithId = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // FILL MANA
        private static bool CommandFunction_ManaFill()
        {
            var player = GetPlayerForCommand();
            player.AddMana(player.MaxMana, 1f);
            CommandFunction_PrintToChat(playerNameForCommands.ColorText(Color.magenta) + "'s Mana Refilled".ColorText(Yellow));
            return true;
        }
        // INFINITE MANA
        private static bool CommandFunction_InfiniteMana()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdManaInf);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = infMana = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // FILL HEALTH
        private static bool CommandFunction_HealthFill()
        {
            var player = GetPlayerForCommand();
            player.Heal(player.MaxMana, true, 1f);
            CommandFunction_PrintToChat(playerNameForCommands.ColorText(Color.magenta) + "'s Health Refilled".ColorText(Yellow));
            return true;
        }
        // SLEEP
        private static bool CommandFunction_Sleep()
        {
            GetPlayerForCommand().SkipSleep();
            CommandFunction_PrintToChat($"{"Slept".ColorText(Green)} once! Another Day is a Good Day!".ColorText(Yellow));
            return true;
        }
        // PRINT ID ON HOVER
        private static bool CommandFunction_PrintItemIdOnHover()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdPrintHoverItem);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            printOnHover = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // TOGGLE COMMAND FEEDBACK
        private static bool CommandFunction_ToggleFeedback()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdFeedbackDisabled);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // PAUSE
        private static bool CommandFunction_Pause()
        {
            // get Command values
            int i = Array.FindIndex(Commands, command => command.Name == CmdPause);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // SLOW TIME
        private static bool CommandFunction_CustomDaySpeed(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.Split(' ');
            int i = Array.FindIndex(Commands, command => command.Name == CmdCustomDaySpeed);
            if (mayCommandParam.Length >= 2)
            {
                if (int.TryParse(mayCommandParam[1], out int value))
                {
                    Commands[i].State = CommandState.Activated;
                    timeMultiplier = (float)System.Math.Round(20f / value, 4);
                    CommandFunction_PrintToChat($"Custom Dayspeed {"Activated".ColorText(Green)} and {"changed".ColorText(Green)}! multiplier: {timeMultiplier.ToString().ColorText(Color.white)}".ColorText(Yellow));
                    return true;
                }
                else if (mayCommandParam[1].Contains("r") || mayCommandParam[1].Contains("d")) // r = reset | d = default
                {
                    timeMultiplier = CommandParamDefaults.timeMultiplier;
                    Commands[i].State = CommandState.Activated;
                    CommandFunction_PrintToChat($"Custom Dayspeed {"Activated".ColorText(Green)} and {"reseted".ColorText(Green)}! multiplier: {timeMultiplier.ToString().ColorText(Color.white)}".ColorText(Yellow));
                    return true;
                }
            }
            else
                Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // UI ON/OFF
        private static bool CommandFunction_ToggleUI(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.Split(' ');
            string error = $"try '{"!ui [on/off]".ColorText(Color.white)}'".ColorText(Red);
            if (mayCommandParam.Length >= 2)
            {
                bool flag = true;
                if (mayCommandParam[1].Contains("on"))
                    flag = true;
                else if (mayCommandParam[1].Contains("of"))
                    flag = false;
                else
                    CommandFunction_PrintToChat(error);
                #region gameObject.SetActive (from QuantumConsoleManager commands)
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
                #endregion
                CommandFunction_PrintToChat("UI now ".ColorText(Yellow) + (flag ? "VISIBLE".ColorText(Green) : "HIDDEN".ColorText(Green)));
            }
            else
                CommandFunction_PrintToChat(error);
            return true;
        }
        // JUMPER
        private static bool CommandFunction_Jumper()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdJumper);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            jumpOver = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // NOCLIP
        private static bool CommandFunction_NoClip()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdNoClip);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            GetPlayerForCommand().rigidbody.bodyType = Commands[i].State == CommandState.Activated ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // NO HIT
        private static bool CommandFunction_NoHit()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdNoHit);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            GetPlayerForCommand().Invincible = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // AUTO-FILL MUSEUM
        private static bool CommandFunction_AutoFillMuseum()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdAutoFillMuseum);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // CHEAT-FILL MUSEUM
        private static bool CommandFunction_CheatFillMuseum()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdCheatFillMuseum);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // TELEPORT
        private static bool CommandFunction_TeleportToScene(string[] mayCmdParam)
        {
            if (mayCmdParam.Length <= 1)
                return true;
            string scene = mayCmdParam[1];
            if (scene == "withergatefarm")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(138f, 89.16582f), "WithergateRooftopFarm");
            }
            else if (scene == "throneroom")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(21.5f, 8.681581f), "Throneroom");
            }
            else if (scene == "nelvari")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(320.3333f, 98.76098f), "Nelvari6");
            }
            else if (scene == "wishingwell")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(55.83683f, 61.48384f), "WishingWell");
            }
            else if (scene.Contains("altar"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(199.3957f, 122.6284f), "DynusAltar");
            }
            else if (scene.Contains("hospital"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(80.83334f, 65.58415f), "Hospital");
            }
            else if (scene.Contains("sunhaven"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(268.125f, 299.9311f), "Town10");
            }
            else if (scene.Contains("nelvarifarm"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(139.6753f, 100.4739f), "NelvariFarm");
            }
            else if (scene.Contains("nelvarimine"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(144.7133f, 152.1591f), "NelvariMinesEntrance");
            }
            else if (scene.Contains("nelvarihome"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(51.5f, 54.97755f), "NelvariPlayerHouse");
            }
            else if (scene.Contains("castle"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(133.7634f, 229.2485f), "Withergatecastleentrance");
            }
            else if (scene.Contains("withergatehome"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(63.5f, 54.624f), "WithergatePlayerApartment");
            }
            else if (scene.Contains("grandtree"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(314.4297f, 235.2298f), "GrandTreeEntrance1");
            }
            else if (scene.Contains("taxi"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(101.707f, 123.4622f), "WildernessTaxi");
            }
            else if (scene == "dynus")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(94.5f, 121.09f), "Dynus");
            }
            else if (scene == "sewer")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(134.5833f, 129.813f), "Sewer");
            }
            else if (scene == "nivara")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(99f, 266.6305f), "Nivara");
            }
            else if (scene.Contains("barrack"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(71.58334f, 54.56507f), "Barracks");
            }
            else if (scene.Contains("elios"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(113.9856f, 104.2902f), "DragonsMeet");
            }
            else if (scene.Contains("dungeon"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(136.48f, 193.92f), "CombatDungeonEntrance");
            }
            else if (scene.Contains("store"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(77.5f, 58.55f), "GeneralStore");
            }
            else if (scene.Contains("beach"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(96.491529f, 87.46871f), "BeachRevamp");
            }
            else if (scene == "home" || scene.Contains("sunhavenhome") || scene == "farm")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(316.4159f, 152.5824f), "2Playerfarm");
            }
            else if (scene == "back")
                ScenePortalManager.Instance.ChangeScene(lastLocation, lastScene);
            else
                CommandFunction_PrintToChat("invalid scene name".ColorText(Color.red));

            return true;
        }
        // GET TELEPORT LOCATIONS
        private static bool CommandFunction_TeleportLocations()
        {
            foreach (string tpLocation in tpLocations)
                CommandFunction_PrintToChat(tpLocation.ColorText(Color.white));
            return true;
        }
        // DESPAWN PET
        private static bool CommandFunction_DespawnPet()
        {
            if (lastPetName == "")
                CommandFunction_PrintToChat("No pet spawned by command".ColorText(Red));
            else
            {
                PetManager.Instance.DespawnPet(Player.Instance);
                CommandFunction_PrintToChat($"Pet ({lastPetName.ColorText(Color.white)}) removed!".ColorText(Green));
                lastPetName = "";
            }
            return true;
        }
        // SPAWN PET
        private static bool CommandFunction_SpawnPet(string[] mayCmdParam)
        {
            if (petList == null)
                petList = (Dictionary<string, Pet>)typeof(PetManager).GetField("_petDictionary", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PetManager.Instance);
            if (petList != null)
            {
                if (mayCmdParam.Length >= 2)
                {
                    string petCmd = mayCmdParam[1];
                    List<string> despawnCmds = new List<string> { "despawn", "remove" };
                    foreach (string cmd in despawnCmds)
                    {
                        if (petCmd.Contains(cmd))
                        {
                            if (lastPetName != "")
                            {
                                PetManager.Instance.DespawnPet(Player.Instance);
                                CommandFunction_PrintToChat($"Pet ({lastPetName.ColorText(Color.white)}) removed!".ColorText(Green));
                                lastPetName = "";
                            }
                            else
                                CommandFunction_PrintToChat("No pet spawned by command".ColorText(Red));
                            return true;
                        }
                    }
                    if (petList.ContainsKey(petCmd))
                    {
                        PetManager.Instance.SpawnPet(petCmd, Player.Instance, null);
                        lastPetName = petCmd;
                    }
                    else
                        CommandFunction_PrintToChat($"wrong pet name, get pets using '{"!pets".ColorText(Color.white)}'".ColorText(Red));
                }
                else
                    CommandFunction_PrintToChat($"try '{"!pet [pet name]".ColorText(Color.white)}'!".ColorText(Red));
            }
            else
                CommandFunction_PrintToChat("ISSUE: no 'petList', u can report this bug".ColorText(Red));
            return true;
        }
        // GET PET LIST
        private static bool CommandFunction_GetPetList()
        {
            if (petList == null)
            {
                petList = (Dictionary<string, Pet>)typeof(PetManager).GetField("_petDictionary", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PetManager.Instance);
                if (petList == null)
                    CommandFunction_PrintToChat("ISSUE: no 'petList', u can report this bug".ColorText(Red));
                return true;
            }
            CommandFunction_PrintToChat("[PET-LIST]".ColorText(Color.black));
            foreach (string pet in petList.Keys)
                CommandFunction_PrintToChat(pet);
            return true;
        }
        // UN-MARRY NPC
        private static bool CommandFunction_UnMarry(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 2)
            {
                string name = FirstCharToUpper(mayCmdParam[1]);
                bool all = mayCmdParam[1] == "all";
                NPCAI[] npcs = FindObjectsOfType<NPCAI>();
                foreach (NPCAI npcai in npcs)
                {
                    string npcName = _GetNpcName(npcai);
                    if (npcName == null || !SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.ContainsKey(npcName))
                    {
                        continue;
                    }
                    if (all || npcName == name)
                    {
                        npcai.MarryPlayer();
                        string progressStringCharacter = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
                        if (!progressStringCharacter.IsNullOrWhiteSpace())
                        {
                            SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", "");
                            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Married", false);
                            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("MarriedTo" + progressStringCharacter, false);
                            GameSave.CurrentCharacter.Relationships[progressStringCharacter] = 40f;
                            SingletonBehaviour<NPCManager>.Instance.GetRealNPC(progressStringCharacter).GenerateCycle(false);
                        }
                        if (!all)
                        {
                            CommandFunction_PrintToChat($"You divorced {npcName.ColorText(Color.white)}!".ColorText(Green));
                            return true;
                        }
                    }
                }
                CommandFunction_PrintToChat(all ? "You divorced all NPCs!".ColorText(Green) : $"no npc with the name {name.ColorText(Color.white)} found!".ColorText(Red));
            }
            else
                CommandFunction_PrintToChat("a name or parameter 'all' needed");
            return true;
        }
        // SET RELATIONSHIP
        private static bool CommandFunction_Relationship(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 3)
            {
                float value;
                string name = mayCmdParam[1];
                bool all = mayCmdParam[1] == "all";
                bool add = mayCmdParam.Length >= 4 && mayCmdParam[3] == "add";
                if (float.TryParse(mayCmdParam[2], out value))
                {
                    value = Math.Max(0, Math.Min(100, value));
                    NPCAI[] npcs = FindObjectsOfType<NPCAI>();
                    foreach (NPCAI npc in npcs)
                    {
                        if (all || npc.NPCName.ToLower() == name)
                        {
                            if (add)
                            {
                                npc.AddRelationship(value);
                                if (!all)
                                {
                                    CommandFunction_PrintToChat($"Relationship with {npc.NPCName.ColorText(Color.white)} is now {SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npc.NPCName].ToString().ColorText(Color.white)}!".ColorText(Green));
                                    return true;
                                }
                            }
                            else
                            {
                                SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npc.NPCName] = value;
                                if (!all)
                                {
                                    CommandFunction_PrintToChat($"Relationship with {npc.NPCName.ColorText(Color.white)} set to {value.ToString().ColorText(Color.white)}!".ColorText(Green));
                                    return true;
                                }
                            }
                        }
                    }
                    if (all)
                        CommandFunction_PrintToChat(add ? "Relationships increased!".ColorText(Green) : "Relationships set!".ColorText(Green));
                    else
                        CommandFunction_PrintToChat($"No NPC with the name {name.ColorText(Color.white)} found!".ColorText(Red));
                }
                else
                    CommandFunction_PrintToChat($"NO VALID VALUE, try '{$"!relationship {name.ColorText(Color.white)} 10".ColorText(Color.white)}'".ColorText(Red));
            }
            return true;
        }
        private static Regex npcNameRegex = new Regex(@"[a-zA-Z\s\.]+");
        private static String _GetNpcName(NPCAI npcai)
        {
            var matches = npcNameRegex.Matches(npcai.NPCName);
            foreach (Match match in matches)
            {
                return (String)match.Value;
            }
            return null;
        }

        // MARRY NPC
        private static bool CommandFunction_MarryNPC(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 2)
            {
                string name = FirstCharToUpper(mayCmdParam[1]);
                bool all = mayCmdParam[1] == "all";
                NPCAI[] npcs = FindObjectsOfType<NPCAI>();
                foreach (NPCAI npcai in npcs)
                {
                    string npcName = _GetNpcName(npcai);
                    if (npcName == null || !SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.ContainsKey(npcName))
                    {
                        continue;
                    }
                    if (all || npcName == name)
                    {
                        SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.ContainsKey(npcName);
                        if (SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npcName] < 100f)
                        {
                            SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npcName] = 100f;
                        }
                        npcai.MarryPlayer();
                        if (!all)
                        {
                            CommandFunction_PrintToChat($"You married {npcName.ColorText(Color.white)}!".ColorText(Green));
                            return true;
                        }
                    }
                }
                CommandFunction_PrintToChat(all ? "You have married all NPCs!".ColorText(Green) : $"no npc with the name {name.ColorText(Color.white)} found!".ColorText(Red));
            }
            else
                CommandFunction_PrintToChat("a name or parameter 'all' needed");
            return true;
        }
        // SET SEASON
        private static bool CommandFunction_SetSeason(string[] mayCmdParam)
        {
            if (mayCmdParam.Length < 2)
            { CommandFunction_PrintToChat("specify the season!".ColorText(Red)); return true; }
            if (!Enum.TryParse(mayCmdParam[1], true, out Season season2))
            { CommandFunction_PrintToChat("no valid season!".ColorText(Red)); return true; }
            var Date = DayCycle.Instance;
            int targetYear = Date.Time.Year + ((int)season2 - (int)Date.Season + 4) % 4;
            DayCycle.Instance.Time = new DateTime(targetYear, Date.Time.Month, 1, Date.Time.Hour, Date.Time.Minute, 0, DateTimeKind.Utc).ToUniversalTime();
            typeof(DayCycle).GetMethod("SetInitialTime", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DayCycle.Instance, null);
            CommandFunction_PrintToChat("It's now ".ColorText(Yellow) + season2.ToString().ColorText(Green));
            //DayCycle.Instance.SetSeason(season2);
            return true;
        }
        // YEAR FIX
        private static bool CommandFunction_ToggleYearFix()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdFixYear);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            yearFix = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // CHANGE YEAR
        private static bool CommandFunction_IncDecYear(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int value))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!years 1' or '!years -1'".ColorText(Red));
                return true;
            }
            var Date = DayCycle.Instance;
            int newYear;
            if (mayCommand.Contains("-"))
            {
                if (Date.Time.Year - (value * 4) >= 1)
                    newYear = Date.Time.Year - (value * 4);
                else
                {
                    CommandFunction_PrintToChat("must be in year 1 or above");
                    return true;
                }
            }
            else
                newYear = Date.Time.Year + (value * 4);

            DayCycle.Instance.Time = new DateTime(newYear, Date.Time.Month, Date.Time.Day, Date.Time.Hour, Date.Time.Minute, 0, DateTimeKind.Utc).ToUniversalTime();
            typeof(DayCycle).GetMethod("SetInitialTime", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DayCycle.Instance, null);
            CommandFunction_PrintToChat($"It's now Year {(Date.Time.Year / 4).ToString().ColorText(Green)}!".ColorText(Yellow));
            return true;
        }
        // TOGGLE CHEATS
        private static bool CommandFunction_ToggleCheats()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdCheats);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            Settings.EnableCheats = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        #endregion

        // duplicated "command methodes" (no functions) to use the in-game COMMAND feature
        #region fake methode to show commands while typing
        [Command("help", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm0(string INFO_showAllCommandsWithCurrentState)
        {
        }
        [Command("state", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm1(string INFO_showActivatedCommands)
        {
        }
        [Command("getitemids", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm2(string xp_money_bonus_furniture_quest_all)
        {
        }
        [Command("minereset", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm3()
        {
        }
        [Command("mineclear", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm4()
        {
        }
        [Command("mineoverfill", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm5()
        {
        }
        [Command("coins", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm6(string amountToAddOrSub)
        {
        }
        [Command("orbs", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm7(string amountToAddOrSub)
        {
        }
        [Command("tickets", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm8(string amountToAddOrSub)
        {
        }
        [Command("name", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm9(string playerNameForCommand)
        {
        }
        [Command("time", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        public static void fm10(string DayOrHoure_and_Value)
        {
        }
        [Command("weather", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        public static void fm11(string raining_heatwave_clear)
        {
        }
        [Command("give", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm12(string itemName)
        {
        }
        [Command("items", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm13(string itemName_ToShowItemsWithGivenName)
        {
        }
        [Command("devkit", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm14(string INFO_getDevItems)
        {
        }
        [Command("dasher", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm15(string INFO_infiniteAirJumps)
        {
        }
        [Command("showid", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm16(string INFO_ShowsItemIdsInDescription)
        {
        }
        [Command("manafill", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm17(string INFO_refillMana)
        {
        }
        [Command("manainf", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm18(string INFO_infiniteMana)
        {
        }
        [Command("healthfill", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm19(string INFO_refillHealth)
        {
        }
        [Command("sleep", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm20(string INFO_sleepOnce)
        {
        }
        [Command("printhoveritem", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm21(string INFO_sendItemIdAndNameToChat)
        {
        }
        [Command("feedback", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm22(string INFO_toggleCommandFeedback)
        {
        }
        [Command("pause", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm23(string INFO_pauseTime)
        {
        }
        [Command("timespeed", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm24(string toggleOrSetCustomTimeSpeed)
        {
        }
        [Command("ui", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm25(string OnOrOff_ToToggleHUD)
        {
        }
        [Command("jumper", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm26(string INFO_toggleToJumpOverObjects)
        {
        }
        [Command("noclip", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm27(string INFO_toggleForNoclip)
        {
        }
        [Command("nohit", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm28(string INFO_toggleForGodMode)
        {
        }
        [Command("autofillmuseum", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm29(string INFO_autofillMusuemOnEnterMuseum)
        {
        }
        [Command("cheatfillmuseum", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm30(string INFO_cheatfillMusuemOnEnterMuseum)
        {
        }
        [Command("tp", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm31(string teleportLocation)
        {
        }
        [Command("tps", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm32(string INFO_showTeleportLocations)
        {
        }
        [Command("despawnpet", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm33(string INFO_despawnPet)
        {
        }
        [Command("pet", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm34(string petNameToSpawn)
        {
        }
        [Command("pets", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm35(string INFO_showPetNames)
        {
        }
        [Command("divorce", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm36(string NPCNameToUnmarry)
        {
        }
        [Command("relationship", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm37(string NPCName_and_value_ToChangeRelationship)
        {
        }
        [Command("marry", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm38(string NPCNameToMarry)
        {
        }
        [Command("season", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm39(string seasonToSet)
        {
        }
        [Command("yearfix", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm40(string INFO_toggleToShowTheCorrectYear)
        {
        }
        [Command("years", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm41(string yearsToAddOrSub)
        {
        }
        [Command("cheats", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm42(string INFO_toggleGameCheats)
        {
        }
        #endregion
        #endregion

        #region Patches
        // Year fix
        #region Patch_DayCycle.Year
        [HarmonyPatch(typeof(DayCycle))]
        [HarmonyPatch("Year", MethodType.Getter)]
        public static class Patch_DayCycleYear
        {
            public static bool Prefix(ref int __result)
            {
                __result = (DayCycle.Day - 1) / 112 + 1;
                return !yearFix;
            }
        }
        #endregion
        // auto fill museum
        #region Patch_HungryMonster.SetMeta
        [HarmonyPatch(typeof(HungryMonster))]
        [HarmonyPatch("SetMeta")]
        class Patch_HungryMonsterSetMeta
        {
            static void Postfix(HungryMonster __instance, DecorationPositionData decorationData)
            {
                if (__instance.bundleType == BundleType.MuseumBundle)
                {
                    if (Commands[Array.FindIndex(Commands, command => command.Name == CmdCheatFillMuseum)].State == CommandState.Activated
                    ||
                    Commands[Array.FindIndex(Commands, command => command.Name == CmdAutoFillMuseum)].State == CommandState.Activated)
                    {
                        Player player = GetPlayerForCommand();
                        if (player == null)
                            return;
                        HungryMonster monster = __instance;
                        if (monster.sellingInventory != null) // && monster.sellingInventory.Items != null && monster.sellingInventory.Items.Count >= 1
                        {
                            if (Commands.Any(command => command.Name == CmdCheatFillMuseum && command.State == CommandState.Activated))
                            {
                                foreach (SlotItemData slotItemData in monster.sellingInventory.Items)
                                {
                                    if (slotItemData.item == null || slotItemData.slot.numberOfItemToAccept == 0 || slotItemData.amount == slotItemData.slot.numberOfItemToAccept)
                                        continue;
                                    if (!monster.name.ToLower().Contains("money"))
                                        monster.sellingInventory.AddItem(ItemDatabaseWrapper.ItemDatabase.GetItemData(slotItemData.slot.itemToAccept.id).GetItem(), slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false);
                                    else if (monster.name.ToLower().Contains("money"))
                                    {
                                        if (slotItemData.slot.itemToAccept.id >= 60000 && slotItemData.slot.itemToAccept.id <= 60002)
                                            monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        //if (slotItemData.slot.itemToAccept.id == 60000)
                                        //    monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        //else if (slotItemData.slot.itemToAccept.id == 60001)
                                        //    monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        //else if (slotItemData.slot.itemToAccept.id == 60002)
                                        //    monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                    }
                                    monster.UpdateFullness();
                                }
                            }
                            else
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
                                                monster.sellingInventory.AddItem(ItemDatabaseWrapper.ItemDatabase.GetItemData(slotItemData.slot.itemToAccept.id).GetItem(), amount, slotItemData.slotNumber, false);
                                                CommandFunction_PrintToChat($"transferred: {amount.ToString().ColorText(Color.white)} * {ItemDatabaseWrapper.ItemDatabase.GetItemData(pItem.id).name.ColorText(Color.white)}");
                                                player.Inventory.RemoveItem(pItem.item, amount);
                                                monster.UpdateFullness();
                                            }
                                        }

                                    }
                                }
                            }

                        }
                        Array.ForEach(FindObjectsOfType<MuseumBundleVisual>(), vPodium => typeof(MuseumBundleVisual).GetMethod("OnSaveInventory", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(vPodium, null));
                    }
                }


            }
        }
        #endregion

        // get name for singleplayer
        #region Patch_GameSave.LoadCharacter
        [HarmonyPatch(typeof(GameSave), nameof(GameSave.LoadCharacter))]
        class Patch_GameSaveLoadCharacter
        {
            static void Postfix(int characterNumber, GameSave __instance) => playerNameForCommands = playerNameForCommandsFirst = __instance.CurrentSave.characterData.characterName;
        }
        #endregion

        // infinite airSkips
        #region Patch_Player.AirSkipsUsed
        [HarmonyPatch(typeof(Player), nameof(Player.AirSkipsUsed), MethodType.Setter)]
        class Patch_PlayerAirSkipsUsed
        {
            static bool Prefix(int value)
            {
                return !infAirSkips;
            }
        }
        #endregion

        // skip chat bubble for commands
        #region Patch_Player.DisplayChatBubble
        [HarmonyPatch(typeof(Player), nameof(Player.DisplayChatBubble))]
        class Patch_PlayerDisplayChatBubble
        {
            static bool Prefix(ref string text)
            {
                if (CheckIfCommandDisplayChatBubble(text))
                    return false;
                return true;
            }
        }
        #endregion

        // get chat message for command check
        #region Patch_Player.SendChatMessage
        [HarmonyPatch(typeof(Player), nameof(Player.SendChatMessage), new[] { typeof(string), typeof(string) })]
        class Patch_PlayerSendChatMessage
        {
            static bool Prefix(string characterName, string message)
            {
                if (characterName != playerNameForCommands && characterName != playerNameForCommandsFirst)
                    return true;
                if (CheckIfCommandSendChatMessage(message))
                    return false;  // SEND COMMAND 
                return true;  // SEND CHAT
            }
        }
        #endregion

        // send welcome message and get all items
        #region Patch_Player.Initialize
        [HarmonyPatch(typeof(Player), nameof(Player.Initialize))]
        class Patch_PlayerInitialize
        {
            static void Postfix(Player __instance)
            {
                if (Commands[Array.FindIndex(Commands, command => command.Name == CmdFeedbackDisabled)].State == CommandState.Deactivated)
                {
                    // show welcome message
                    if (ranOnceOnPlayerSpawn < 2)
                        ranOnceOnPlayerSpawn++;
                    else if (ranOnceOnPlayerSpawn == 2)
                    {
                        CommandFunction_PrintToChat("> Command Extension Active! type '!help' for command list".ColorText(Color.magenta) + "\n -----------------------------------------------------------------".ColorText(Color.black));
                        ranOnceOnPlayerSpawn++;
                        // enable test helper
                        if (debug)
                        {
                            CommandFunction_PrintToChat("debug: enable cheat commands".ColorText(Color.magenta));
                            CommandFunction_Jumper();
                            CommandFunction_InfiniteMana();
                            CommandFunction_InfiniteAirSkips();
                            CommandFunction_Pause();
                        }
                    }

                    // Enable in-game command feature 
                    if (Player.Instance != null && QuantumConsole.Instance)
                    {
                        QuantumConsole.Instance.GenerateCommands = Settings.EnableCheats = true;
                        QuantumConsole.Instance.Initialize();
                        Settings.EnableCheats = false;
                    }
                }
            }
        }
        #endregion

        // jump over everything
        #region Patch_Player.Update
        [HarmonyPatch(typeof(Player), "Update")]
        class Patch_PlayerUpdate
        {
            static void Postfix(ref Player __instance)
            {
                if (jumpOver && !noclip) //ignore the wrong turned boolean!
                {
                    if (__instance.Grounded)
                        __instance.rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    else
                        __instance.rigidbody.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
        #endregion

        // no mana use
        #region Patch_Player.UseMana
        [HarmonyPatch(typeof(Player), nameof(Player.UseMana), new[] { typeof(float) })]
        class Patch_PlayerUseMana
        {
            static bool Prefix(float mana)
            {
                return !infMana;
            }
        }
        #endregion

        // pause and custom time multiplier
        #region Patch_DayCycle.DaySpeedMultiplier
        [HarmonyPatch(typeof(Settings))]
        [HarmonyPatch("DaySpeedMultiplier", MethodType.Getter)]
        class Patch_DayCycleDaySpeedMultiplier
        {
            static bool Prefix(ref float __result)
            {
                if (Commands[Array.FindIndex(Commands, command => command.Name == CmdPause)].State == CommandState.Activated)
                    __result = 0f;
                else if (Commands[Array.FindIndex(Commands, command => command.Name == CmdCustomDaySpeed)].State == CommandState.Activated)
                    __result = timeMultiplier;
                else
                    return true;  // vanilla mulitplier
                return false;  // custom mulitplier
            }
        }
        #endregion

        // print itemId on hover
        #region Patch_Item.GetToolTip
        [HarmonyPatch]
        class Patch_ItemGetToolTip
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                Type[] itemTypes = new Type[] { typeof(NormalItem), typeof(ArmorItem), typeof(FoodItem), typeof(FishItem), typeof(CropItem), typeof(WateringCanItem), typeof(AnimalItem), typeof(PetItem), typeof(ToolItem) };
                foreach (Type itemType in itemTypes)
                    yield return AccessTools.Method(itemType, "GetToolTip", new[] { typeof(Tooltip), typeof(int), typeof(bool) });
            }
            static void Prefix(Item __instance)
            {
                int id = __instance.ID();
                if (printOnHover)
                    CommandFunction_PrintToChat($"{id} : {ItemDatabaseWrapper.ItemDatabase.GetItemData(id).name}");
                string text = "ID: ".ColorText(Color.magenta) + id.ToString().ColorText(Color.magenta) + "\"\n\"";
                ItemData itemData = ItemDatabaseWrapper.ItemDatabase.GetItemData(id);
                if (appendItemDescWithId)
                {
                    if (!itemData.description.Contains(text))
                        itemData.description = text + itemData.description;
                }
                else if (itemData.description.Contains(text))
                    itemData.description = itemData.description.Replace(text, "");

            }
        }
        #endregion

        // append itemId to itemDescription
        #region Patch_ItemData.FormattedDescription
        [HarmonyPatch(typeof(ItemData))]
        [HarmonyPatch("FormattedDescription", MethodType.Getter)]
        class Patch_ItemDataFormattedDescription
        {
            static void Postfix(ref string __result, ItemData __instance)
            {
                if (debug)
                    __result = __instance.id.ToString().ColorText(Color.magenta) + "\"\n\"";
            }
        }
        #endregion
        #endregion
    }
}