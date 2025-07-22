// -----------------------------------------------------------------------
// <copyright file="GotoPocket.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Features
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Features.Roles;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;

    /// <summary>
    /// Provides functionality for SCP-106 to teleport to the Pocket Dimension.
    /// </summary>
    public class GotoPocket
    {
        /// <summary>
        /// Handles the teleportation of SCP-106 to the Pocket Dimension.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        public static void PocketFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.PocketdimensionCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.PocketdimensionCostHealt)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (scp106.Owner.CurrentRoom.Type == RoomType.Pocket)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106alreadypocket);
                return;
            }

            Timing.RunCoroutine(GoPocket(scp106));
        }

        /// <summary>
        /// Coroutine that handles the teleportation process to the Pocket Dimension.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        private static IEnumerator<float> GoPocket(Scp106Role scp106)
        {
            if (EventHandlers.SpecialFeatureUsing)
            {
                yield break;
            }

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterPocketdimensionCooldown;

            scp106.IsSubmerged = true;

            yield return Timing.WaitUntilTrue(() => scp106.IsSinkholeHidden);

            scp106.Owner.EnableEffect<PocketCorroding>();
            scp106.Owner.DisableEffect(EffectType.Corroding);

            scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketdimensionCostVigor / 100f);
            scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.PocketdimensionCostHealt, null));
            scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106inpocket, shouldClearPrevious: true);

            yield return Timing.WaitUntilFalse(() => scp106.IsSinkholeHidden);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}