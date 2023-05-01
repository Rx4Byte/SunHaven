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
using Wish;

namespace CommandExtension
{
    public class PluginInfo
    {
        public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Command Extension";
        public const string PLUGIN_GUID = "com.Rx4Byte.CommandExtension";
        public const string PLUGIN_VERSION = "1.1.6";
    }

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
        private static readonly Command[] Commands = new Command[]
        {
            new Command(CmdHelp,                "print commands to chat",                                                   CommandState.None),
            new Command(CmdMineReset,           "refill all mine shafts!",                                                  CommandState.None),
            new Command(CmdPause,               "toggle time pause!",                                                       CommandState.Deactivated),
            new Command(CmdCustomDaySpeed,      "toggle or change dayspeed, ignored if paused!",                            CommandState.Deactivated),
            new Command(CmdMoney,               "give or remove coins",                                                     CommandState.None),
            new Command(CmdOrbs,                "give or remove Orbs",                                                      CommandState.None),
            new Command(CmdTickets,             "give or remove Tickets",                                                   CommandState.None),
            new Command(CmdSetDate,             "set HOURE '6-23' e.g. 'set h 12'\nset DAY '1-28' e.g. 'set d 12'",         CommandState.None),
            new Command(CmdWeather,             "set DAY '1-28' e.g. 'set d 12'",                                           CommandState.None),
            new Command(CmdDevKit,              "get dev items",                                                            CommandState.None),
            new Command(CmdJumper,              "jump over object's (actually noclip while jump)",                          CommandState.Deactivated),
            new Command(CmdState,               "print activ commands",                                                     CommandState.None),
            new Command(CmdPrintItemIds,        "print item ids [xp|money|all|bonus]",                                      CommandState.None),
            new Command(CmdSleep,               "sleep to next the day",                                                    CommandState.None),
            new Command(CmdDasher,              "infinite dashes",                                                          CommandState.Deactivated),
            new Command(CmdManaFill,            "mana refill",                                                              CommandState.None),
            new Command(CmdManaInf,             "infinite mana",                                                            CommandState.Deactivated),
            new Command(CmdHealthFill,          "health refill",                                                            CommandState.None),
            new Command(CmdNoHit,               "no hit (disable hitbox)",                                                  CommandState.Deactivated),
            new Command(CmdMineOverfill,        "fill mine completely with rocks & ores",                                   CommandState.None),
            new Command(CmdMineClear,           "clear mine completely from rocks & ores",                                  CommandState.None),
            new Command(CmdNoClip,              "walk trough everything",                                                   CommandState.Deactivated),
            new Command(CmdPrintHoverItem,      "print item id to chat",                                                    CommandState.Deactivated),
            new Command(CmdName,                "set name for command target ('!name Lynn') only '!name resets it' ",       CommandState.None),
            new Command(CmdFeedbackDisabled,    "toggle command feedback on/off",                                           CommandState.Deactivated),
            new Command(CmdGive,                "give [ID] [AMOUNT]*",                                                      CommandState.None),
            new Command(CmdShowItems,           "print items with the given name",                                          CommandState.None),
            new Command(CmdAutoFillMuseum,      "toggle museum's auto fill upon entry",                                     CommandState.Deactivated),
            new Command(CmdCheatFillMuseum,     "toggle fill museum completely upon entry",                                 CommandState.Deactivated),
            new Command(CmdUI,                  "turn ui on/off",                                                           CommandState.None),
            new Command(CmdTeleport,            "teleport to some locations",                                               CommandState.None),
            new Command(CmdTeleportLocations,   "get teleport locations",                                                   CommandState.None)
        };
        #endregion
        // ITEM ID's
        private static Dictionary<string, int> allIds = ItemDatabase.ids;
        private static Dictionary<string, int> moneyIds = new Dictionary<string, int> { { "coins", 60000 }, { "orbs", 18010 }, { "tickets", 18011 } };
        private static Dictionary<string, int> xpIds = new Dictionary<string, int> { { "combatexp", 60003 }, { "farmingexp", 60004 }, { "miningexp", 60006 }, { "explorationexp", 60005 }, { "fishingexp", 60008 } };
        private static Dictionary<string, int> bonusIds = new Dictionary<string, int> { { "health", 60009 }, { "mana", 60007 } };
        private static List<string> tpLocations = new List<string>() { "throneroom", "nelvari6", "wishingwell", "altar", "hospital", "sunhaven", "farm", "nelvarifarm", "nelvarimine", "nelvarihome",
                                                                        "withergatefarm", "castle", "withergatehome", "grandtree", "taxi", "dynus", "sewer", "nivara", "barracks", "dragon", "dungeon", "store", "beach" };
        // COMMAND STATE VAR'S FOR FASTER ACCESS (inside patches)
        private static bool jumpOver = false;
        private static bool noclip = false;
        private static bool printOnHover = false;
        private static bool infMana = false;
        private static bool infAirSkips = false;
        // ...
        private static float timeMultiplier = CommandParamDefaults.timeMultiplier;
        private static string playerNameForCommandsFirst;
        private static string playerNameForCommands;
        private static string gap = "  -  ";
        private static int ranOnceOnPlayerSpawn = 0;
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
                    return CommandFunction_TeleportLoactions();

                // no valid command found
                default:
                    return false;
            }
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
                    CommandFunction_PrintToChat("[XP-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in xpIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'm':
                    CommandFunction_PrintToChat("[MONEY-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in moneyIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'a':
                    CommandFunction_PrintToChat("[COMPLETE-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in allIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'b':
                    CommandFunction_PrintToChat("[BONUS-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in bonusIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                default:
                    CommandFunction_PrintToChat(CmdPrintItemIds + " [xp|money|all|bonus]".ColorText(Red));
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
                            DateTime newDate = new DateTime(Date.Time.Year, Date.Time.Month, dateValue, Date.Time.Hour, Date.Time.Minute, Date.Time.Second, Date.Time.Millisecond).AddHours(1);
                            DateTime updatedDate = newDate;
                            typeof(DayCycle).GetField("previousDay", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(FindObjectOfType<DayCycle>(), updatedDate.AddDays(-1));
                            Date.Time = newDate;
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
                        CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {itemAmount.ToString().ColorText(Color.white)} * {ItemDatabase.GetItemData(itemId).name.ColorText(Color.white)}!".ColorText(Yellow));
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
                        foreach (KeyValuePair<string, int> id in allIds)
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
                            CommandFunction_PrintToChat($"no item name contains {mayCommandParam[1].ColorText(Color.white)}!".ColorText(Red));
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
                foreach (KeyValuePair<string, int> id in allIds)
                {
                    if (id.Key.ToLower().Contains(mayCommandParam[1]))
                        items.Add(id.Key.ColorText(Color.white) + " : ".ColorText(Color.black) + id.Value.ToString().ColorText(Color.white));
                }
                if (items.Count >= 1)
                {
                    CommandFunction_PrintToChat("[FOUND ITEMS]".ColorText(Color.black));
                    foreach (string ítem in items)
                        CommandFunction_PrintToChat(ítem);
                }
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
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(126.125f, 83.6743f), "WithergateRooftopFarm"); //works
            else if (scene == "throneroom")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(21.5f, 8.681581f), "Throneroom");  //works - test
            else if (scene == "nelvari6")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(320.3333f, 98.76098f), "Nelvari6"); //nelvari bottom bridge
            else if (scene == "wishingwell" || scene.Contains("wishing"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(55.83683f, 42.80461f), "WishingWell"); //works - test
            else if (scene.Contains("altar"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(199.3957f, 122.6284f), "DynusAltar"); //good
            else if (scene.Contains("hospital"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(80.83334f, 65.58415f), "Hospital"); //good
            else if (scene.Contains("sunhaven"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(268.125f, 299.9311f), "Town10"); //good
            else if (scene.Contains("homefarm") || scene.Contains("sunhavenhome") || scene.Contains("playerfarm") || scene == "farm")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(357f, 124.3919f), "2Playerfarm"); //good
            else if (scene.Contains("nelvarifarm"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(148.25f, 100.8806f), "NelvariFarm"); //good
            else if (scene.Contains("nelvarimine")) //new Vector2(154.1667f, 157.2463f)
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(144.7558f, 111.1503f), "NelvariMinesEntrance"); //works - test
            else if (scene.Contains("nelvarihome"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(51.5f, 54.97755f), "NelvariPlayerHouse"); //good
            else if (scene.Contains("castle")) //new Vector2(24.25f, 86.09025f)
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(133.6865f, 163.3773f), "Withergatecastleentrance"); //works - test
            else if (scene.Contains("withergatehome"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(63.5f, 54.624f), "WithergatePlayerApartment"); //good
            else if (scene.Contains("grandtree"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(314.4297f, 235.2298f), "GrandTreeEntrance1"); //good
            else if (scene.Contains("taxi"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(101.707f, 123.4622f), "WildernessTaxi"); //works
            else if (scene == "dynus")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(94.5f, 121.09f), "Dynus"); //good
            else if (scene == "sewer") // new Vector2(13.70833f, 134.4075f)
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(134.5833f, 129.813f), "Sewer"); //good
            else if (scene == "nivara")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(99.5f, 194.3229f), "Nivara"); //works - test
            else if (scene == "barracks")
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(71.58334f, 54.56507f), "Barracks"); //good
            else if (scene.Contains("dragon"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(114f, 73.7052f), "DragonsMeet"); //works - test
            else if (scene.Contains("dungeon"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(136.48f, 193.92f), "CombatDungeonEntrance"); //good
            else if (scene.Contains("store"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(77.5f, 58.55f), "GeneralStore"); //good
            else if (scene.Contains("beach"))
                SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(96.491529f, 64.69862f), "BeachRevamp");
            else
                CommandFunction_PrintToChat("invalid scene name".ColorText(Color.red));
            return true;
        }

        private static bool CommandFunction_TeleportLoactions()
        {
            foreach (string tpLocation in tpLocations)
                CommandFunction_PrintToChat(tpLocation.ColorText(Color.white) + "\n");
            return true;
        }
        #endregion
        #endregion

        #region Patches
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
                                    {   
                                        monster.sellingInventory.AddItem(ItemDatabase.GetItemData(slotItemData.slot.itemToAccept.id).GetItem(), slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false);
                                        monster.UpdateFullness();
                                    }
                                    else if (monster.name.ToLower().Contains("money"))
                                    {
                                        if (slotItemData.slot.itemToAccept.id == 60000)
                                            monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        else if (slotItemData.slot.itemToAccept.id == 60001)
                                            monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        else if (slotItemData.slot.itemToAccept.id == 60002)
                                            monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                    }
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
                                                monster.sellingInventory.AddItem(ItemDatabase.GetItemData(slotItemData.slot.itemToAccept.id).GetItem(), amount, slotItemData.slotNumber, false);
                                                CommandFunction_PrintToChat($"transferred: {amount.ToString().ColorText(Color.white)} * {ItemDatabase.GetItemData(pItem.id).name.ColorText(Color.white)}");
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
                if (ranOnceOnPlayerSpawn < 2)
                    ranOnceOnPlayerSpawn++;
                else if (ranOnceOnPlayerSpawn == 2)
                    if (Commands[Array.FindIndex(Commands, command => command.Name == CmdFeedbackDisabled)].State == CommandState.Deactivated)
                    { 
                        CommandFunction_PrintToChat("> Command Extension Active! type '!help' for command list".ColorText(Color.magenta) + "\n -----------------------------------------------------------------".ColorText(Color.black));
                        ranOnceOnPlayerSpawn++;
                        if (debug)
                        {
                            CommandFunction_PrintToChat("debug: use helping methodes");
                            CommandFunction_InfiniteAirSkips();
                            CommandFunction_InfiniteMana();
                            CommandFunction_NoClip();
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
            static void Postfix(Item __instance)
            {
                if (printOnHover)
                    CommandFunction_PrintToChat($"{__instance.ID()} : {ItemDatabase.GetItemData(__instance.ID()).name}");
            }
        }
        #endregion
        #endregion
    }
}