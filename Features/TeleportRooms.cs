// -----------------------------------------------------------------------
// <copyright file="TeleportRooms.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Features
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Core.UserSettings;
    using MEC;
    using PlayerStatsSystem;
    using UnityEngine;
    using Scp106Role = Exiled.API.Features.Roles.Scp106Role;

    /// <summary>
    /// Provides teleportation functionality for SCP-106 to specific rooms.
    /// </summary>
    public class TeleportRooms
    {
        /// <summary>
        /// Handles the teleportation feature for SCP-106.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        public static void TeleportFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0 || scp106.IsStalking || scp106.IsSubmerged)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.TeleportCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.TeleportCostHealt)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportCant, shouldClearPrevious: true);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (!SettingBase.TryGetSetting<DropdownSetting>(scp106.Owner, Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRoomsList], out DropdownSetting dropDown))
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportCant, shouldClearPrevious: true);
                return;
            }

            int targetRoomIndex = dropDown.SelectedIndex;
            Room targetRoom = Room.Get(Plugin.Instance.Config.Rooms[targetRoomIndex]);

            if (targetRoom == null)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportRoomNull, shouldClearPrevious: true);
                return;
            }

            if (Plugin.Instance.Config.TeleportOnlySameZone && scp106.Owner.Zone != targetRoom.Zone)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportCantforZone, shouldClearPrevious: true);
                return;
            }

            bool flagLcz = Map.IsLczDecontaminated && targetRoom.Zone == ZoneType.LightContainment;
            bool flagSite = Warhead.IsDetonated && targetRoom.Zone != ZoneType.Surface;

            if (flagLcz || flagSite)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportRoomDanger, shouldClearPrevious: true);
                return;
            }

            Timing.RunCoroutine(TeleportRoom(scp106, targetRoom.Position));
        }

        /// <summary>
        /// Teleports SCP-106 to the specified room position.
        /// </summary>
        /// <param name="scp106">The SCP-106 role instance.</param>
        /// <param name="targetRoomPos">The target room position.</param>
        /// <returns>An enumerator for the teleportation coroutine.</returns>
        public static IEnumerator<float> TeleportRoom(Scp106Role scp106, Vector3 targetRoomPos)
        {
            if (EventHandlers.SpecialFeatureUsing)
            {
                yield break;
            }

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.TeleportCooldown;

            scp106.UsePortal(targetRoomPos, Mathf.Clamp01(Plugin.Instance.Config.TeleportCostVigor / 100f));
            scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.TeleportCostHealt, null));

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);
            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);

            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}
