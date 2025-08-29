// -----------------------------------------------------------------------
// <copyright file="RealisticPocketPatch.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Patchs
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// A patch for modifying the behavior of the <see cref="AlphaWarheadController.CanBeDetonated"/> method.
    /// This patch prevents the warhead from being detonated when the position is inside the Pocket Dimension,
    /// if the RealisticPocket feature is enabled in the plugin's config.
    /// </summary>
    [HarmonyPatch(typeof(AlphaWarheadController), nameof(AlphaWarheadController.CanBeDetonated))]
    public static class RealisticPocketPatch
    {
        /// <summary>
        /// Prefix method that checks if the given position is inside the Pocket Dimension.
        /// If so, it forces the result to <see langword="false"/> and skips the original method.
        /// </summary>
        /// <param name="pos">The world position being checked.</param>
        /// <param name="__result">The return value of the original method, modified if in Pocket.</param>
        /// <param name="includeOnlyLifts">Indicates whether only lift positions should be considered.</param>
        /// <returns>
        /// <see langword="false"/> to skip the original method if in Pocket, 
        /// otherwise <see langword="true"/> to let the original method run.
        /// </returns>
        #pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static bool Prefix(Vector3 pos, ref bool __result, bool includeOnlyLifts = false)
        {
            if (Plugin.Instance.Config.RealisticPocket && Room.Get(pos).Type == RoomType.Pocket)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
