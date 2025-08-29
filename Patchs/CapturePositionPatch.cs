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
        /// Postfix method that overrides the exit position returned by <see cref="PocketCorroding.CapturePosition"/>.
        /// If the <c>PocketexitRandomZonemode</c> config is enabled or the affected player is an SCP,
        /// the result is replaced with a random zone.
        /// </summary>
        /// <param name="__instance">The <see cref="PocketCorroding"/> instance being patched.</param>
        /// <param name="__result">The exit position, modified to be a random zone if conditions are met.</param>
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
