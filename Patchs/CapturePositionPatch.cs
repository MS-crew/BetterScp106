// -----------------------------------------------------------------------
// <copyright file="CapturePositionPatch.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using CustomPlayerEffects;

    using HarmonyLib;
    using PlayerRoles;
    using RelativePositioning;

    /// <summary>
    /// This patch replaces the zone of players exiting the pocket dimension with a random zone.
    /// </summary>
    [HarmonyPatch(typeof(PocketCorroding), nameof(PocketCorroding.CapturePosition), MethodType.Getter)]
    public static class CapturePositionPatch
    {
        /// <summary>
        /// Modifies the return value of the <see cref="PocketCorroding.CapturePosition"/> getter.
        /// Custom logic is injected to check SCP roles and configuration settings before returning a random zone.
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static void Postfix(PocketCorroding __instance, ref RelativePosition __result)
        {
            if (Plugin.Instance.Config.PocketexitRandomZonemode || __instance.Hub.IsSCP())
            {
                __result = Methods.RandomZone();
            }
        }
    }
}
