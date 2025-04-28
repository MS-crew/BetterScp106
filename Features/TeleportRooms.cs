using MEC;
using UnityEngine;
using Exiled.API.Enums;
using PlayerStatsSystem;
using Exiled.API.Features;
using SSMenuSystem.Features;
using System.Collections.Generic;
using UserSettings.ServerSpecific;
using Scp106Role = Exiled.API.Features.Roles.Scp106Role;

namespace BetterScp106.Features
{
    public class TeleportRooms
    {

        public static void TeleportFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0 || scp106.IsStalking || scp106.IsSubmerged)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.TeleportCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.TeleportCostHealt)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportCant);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }
            int TargetRoomIndex = scp106.Owner.ReferenceHub.GetParameter<SettingsMenu.ServerSettingsSyncer,SSDropdownSetting>((int)Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRoomsList]).SyncSelectionIndexRaw;
            Room targetRoom = Room.Get(Plugin.Instance.Config.Rooms[TargetRoomIndex]);

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

            bool flaglcz = Map.IsLczDecontaminated && targetRoom.Zone == ZoneType.LightContainment;
            bool flagSite = Warhead.IsDetonated && targetRoom.Zone != ZoneType.Surface;

            if (flaglcz || flagSite)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportRoomDanger, shouldClearPrevious: true);
                return;
            }

            Timing.RunCoroutine(TeleportRoom(scp106, targetRoom.Position));
            
        }
        private static IEnumerator<float> TeleportRoom(Scp106Role scp106, Vector3 TargetRoomPos)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            EventHandlers.SpecialFeatureUsing = true;

            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.TeleportCooldown;

            scp106.UsePortal(TargetRoomPos, Mathf.Clamp01(Plugin.Instance.Config.TeleportCostVigor / 100f));
            scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.TeleportCostHealt, null));

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);
            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);

            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}
