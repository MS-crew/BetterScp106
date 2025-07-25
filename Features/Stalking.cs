﻿// -----------------------------------------------------------------------
// <copyright file="Stalking.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Features
{
    using System.Collections.Generic;
    using CommandSystem.Commands.RemoteAdmin;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.API.Features.Core.UserSettings;
    using Exiled.API.Features.Roles;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;

    /// <summary>
    /// Provides functionality for SCP-106's stalking ability.
    /// </summary>
    public class Stalking
    {
        /// <summary>
        /// Initiates the stalking feature for SCP-106.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        public static void StalkFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, true);
                return;
            }

            if (EventHandlers.SpecialFeatureUsing || scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.StalkCostHealt)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkCant, true);
                return;
            }

            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(scp106.Owner, Plugin.Instance.Config.AbilitySettingIds[SettingsMenu.Features.StalkMode], out TwoButtonsSetting stalkMode) || !SettingBase.TryGetSetting<SliderSetting>(scp106.Owner, Plugin.Instance.Config.AbilitySettingIds[SettingsMenu.Features.StalkDistanceSlider], out SliderSetting slider))
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkNoTarget, true);
                return;
            }

            Player target = Methods.FindTarget(stalkMode.IsSecond, slider.SliderValue, scp106.Owner);

            if (target == null)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkNoTarget, true);
                return;
            }

            Timing.RunCoroutine(Stalk(scp106, target));
        }

        /// <summary>
        /// Executes the stalking process for SCP-106, teleporting it to the target player.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        /// <param name="target">The target player to stalk.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        public static IEnumerator<float> Stalk(Scp106Role scp106, Player target)
        {
            if (EventHandlers.SpecialFeatureUsing)
            {
                yield break;
            }

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterStalkCooldown;

            if (Plugin.Instance.Config.StalkWarning)
            {
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.StalkWarningBefore);
                target.Broadcast(Plugin.Instance.Translation.StalkVictimWarn, shouldClearPrevious: true);
            }

            if (!target.IsAlive)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkFailed, shouldClearPrevious: true);
                EventHandlers.SpecialFeatureUsing = false;
                Log.Debug("Stalk victim died before stalk.");
                yield break;
            }

            scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkSuccesfull, shouldClearPrevious: true);
            Log.Debug("Stalk teleport starting.");

            Vector3 tp = target.Position;
            if (!Physics.Raycast(target.Position + (Vector3.up * 0.5f), Vector3.down, out RaycastHit hit, 2))
            {
                tp = target.CurrentRoom.Position;
            }

            scp106.IsSubmerged = true;

            yield return Timing.WaitUntilTrue(() => scp106.IsSinkholeHidden);

            if (target.Lift == null)
            {
                scp106.Owner.Teleport(tp);
            }
            else
            {
                scp106.Owner.Teleport(target.Lift.Position + (Vector3.up * ElevatorTeleportCommand.PositionOffset));
            }

            scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f);
            scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.StalkCostHealt, null));

            yield return Timing.WaitUntilFalse(() => scp106.IsSinkholeHidden);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}