// -----------------------------------------------------------------------
// <copyright file="TakeScpsPocket.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Features
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;

    /// <summary>
    /// Provides functionality for SCP-106 to interact with the Pocket Dimension.
    /// </summary>
    public class TakeScpsPocket
    {
        /// <summary>
        /// Handles the logic for SCP-106 entering the Pocket Dimension with a friend.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        public static void PocketInFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.PocketinCostHealt)
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
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106alreadypocket, shouldClearPrevious: true);
                return;
            }

            Player friend = Methods.FindFriend(scp106.Owner);

            if (friend == null)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106CantFindFriend, shouldClearPrevious: true);
                return;
            }

            Timing.RunCoroutine(GotoPocketInV3(scp106, friend));
        }

        /// <summary>
        /// Coroutine for handling SCP-106 pulling a friend into the Pocket Dimension.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        /// <param name="friend">The friend player to pull into the Pocket Dimension.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        private static IEnumerator<float> GotoPocketInV3(Scp106Role scp106, Player friend)
        {
            if (EventHandlers.SpecialFeatureUsing)
            {
                yield break;
            }

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterPocketingScpCooldown;

            friend.Broadcast(Plugin.Instance.Translation.Scp106ReqFriendinpocket, shouldClearPrevious: true);
            friend.EnableEffects([EffectType.Flashed, EffectType.Ensnared]);
            scp106.Owner.EnableEffect<Ensnared>();

            EventHandlers.ScpPullingtoPocket = friend.Id;

            scp106.IsSubmerged = true;

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

            if (EventHandlers.GetScpPerm == true)
            {
                EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.CanceledPocketingScpCooldown;
                scp106.IsSubmerged = false;

                friend.DisableEffects([EffectType.Flashed, EffectType.Ensnared]);
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106friendrefusedlpocketin, true);

                EventHandlers.GetScpPerm = false;
                EventHandlers.ScpPullingtoPocket = -1;

                scp106.Owner.DisableEffect<Ensnared>();
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 200f);
            }
            else
            {
                scp106.IsSubmerged = false;

                scp106.Owner.EnableEffect<PocketCorroding>();
                friend.EnableEffect<PocketCorroding>();

                scp106.Owner.DisableEffects([EffectType.Ensnared, EffectType.Corroding]);
                friend.DisableEffects([EffectType.Flashed, EffectType.Ensnared]);

                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.Instance.Translation.Scp106Friendinpocket, shouldClearPrevious: true);

                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f);
                scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.PocketinCostHealt, null));
            }

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}