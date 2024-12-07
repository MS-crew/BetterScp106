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
            bool flagLogic = Plugin.C.PocketexitRandomZonemode || __instance?.Hub?.roleManager?.CurrentRole.Team == Team.SCPs;
            if (flagLogic && !Warhead.IsDetonated)
            {              
                __result = Methods.RandomZone();
                Log.Debug($"Setting random zone... random value: {__result}");
                return;
            }
            Log.Debug("Using default CapturePosition.");
        }
    }
}
