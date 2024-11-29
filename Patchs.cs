using HarmonyLib;
using PlayerRoles;
using CustomPlayerEffects;
using RelativePositioning;
using Exiled.API.Features;

namespace BetterScp106
{
    [HarmonyPatch(typeof(PocketCorroding), "get_CapturePosition")]
    public static class CapturePositionPatchs2
    {
        public static void Postfix(PocketCorroding __instance, ref RelativePosition __result)
        {
            if (__instance?.Hub?.roleManager?.CurrentRole == null)
            {
                Log.Error("Hub or roleManager is null");
                return;
            }

            if (Plugin.C.PocketexitRandomZonemode || __instance.Hub.roleManager.CurrentRole.Team == Team.SCPs)
            {
                Log.Debug("Setting random zone...");
                __result = Methods.RandomZone();
                Log.Debug($"Random value: {__result.Relative}");
                return;
            }

            Log.Debug("Using default CapturePosition.");
        }
    }
}
