using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace SunHaven_ControllerDeactivator
{

    [BepInPlugin("Rx4Byte.ControllerDeactivator", "Controller Deactivator", "1.0")]
    public partial class SunHaven_ControllerDeactivator : BaseUnityPlugin
    {
        private void Awake() => Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

        #region INPUT PATCHES
        [HarmonyPatch(typeof(Input), "GetButton")]
        class Patch_GetButton
        {
            static bool Prefix(string buttonName, ref bool __result)
            {
                return __result = false;
            }
        }
        [HarmonyPatch(typeof(Input), "GetButtonDown")]
        class Patch_GetButtonDown
        {
            static bool Prefix(string buttonName, ref bool __result)
            {
                return __result = false;
            }
        }
        [HarmonyPatch(typeof(Input), "GetButtonUp")]
        class Patch_GetButtonUp
        {
            static bool Prefix(string buttonName, ref bool __result)
            {
                return __result = false;
            }
        }
        [HarmonyPatch(typeof(Input), "GetAxis")]
        class Patch_GetAxis
        {
            static bool Prefix(string axisName, ref float __result)
            {
                if (axisName != "Mouse ScrollWheel")
                {
                    __result = 0f;
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(Input), "GetAxisRaw")]
        class Patch_GetAxisRaw
        {
            static bool Prefix(string axisName, ref float __result)
            {
                if (axisName != "Mouse ScrollWheel")
                {
                    __result = 0f;
                    return false;
                }
                return true;
            }
        }
        #endregion
    }
}