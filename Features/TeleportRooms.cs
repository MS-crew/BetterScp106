using MEC;
using UnityEngine;
using Exiled.API.Features;
using CustomPlayerEffects;
using System.Collections.Generic;
using UserSettings.ServerSpecific;
using Scp106Role = Exiled.API.Features.Roles.Scp106Role;
using Exiled.API.Enums;

namespace BetterScp106.Features
{
    public class TeleportRooms
    {
        public static void TeleportFeature(Player player)
        {
            player.Role.Is(out Scp106Role scp106);
            if (scp106.RemainingSinkholeCooldown > 0|| scp106.IsStalking || scp106.IsSubmerged)
            {
                player.Broadcast(Plugin.T.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.C.TeleportCostVigor / 100f) || player.Health <= Plugin.C.TeleportCostHealt)
            {
                player.Broadcast(Plugin.T.TeleportCant);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                player.Broadcast(Plugin.T.Afternuke, shouldClearPrevious: true);
                return;
            }

            int TargetRoomIndex = ServerSpecificSettingsSync.GetSettingOfUser<SSDropdownSetting>
                (player.ReferenceHub, Plugin.C.AbilitySettingIds[Methods.Features.TeleportRoomsList])
                .SyncSelectionIndexRaw;

            Room targetRoom = Room.Get(Plugin.C.Rooms[TargetRoomIndex]);
           
            if (Plugin.C.TeleportOnlySameZone && player.Zone != targetRoom.Zone) 
            {
                player.Broadcast(Plugin.T.TeleportCantforZone, shouldClearPrevious: true);
                return;
            }

            bool flaglcz = Map.IsLczDecontaminated && targetRoom.Zone == ZoneType.LightContainment;
            bool flagSite = Warhead.IsDetonated && targetRoom.Zone != ZoneType.Surface;

            if (flaglcz || flagSite)
            {
                player.Broadcast(Plugin.T.TeleportRoomDanger, shouldClearPrevious: true);
                return;
            }

            Vector3 TargetRoomPos = Methods.GetSafePosition(player, targetRoom.Position);
            Timing.RunCoroutine(TeleportRoom(player, TargetRoomPos));
        }
        private static IEnumerator<float> TeleportRoom(Player player, Vector3 TargetRoomPos)
        {
            if (Plugin.Using)
                yield break;

            Plugin.Using = true;

            player.Role.Is(out Scp106Role scp106);

            scp106.IsSubmerged = true;
            player.EnableEffect<Ensnared>();

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

            scp106.Owner.Teleport(TargetRoomPos);
            Log.Debug("SCP-106 is ground'.");

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
            Log.Debug("SCP-106 exiting ground.");

            player.DisableEffect<Ensnared>();

            scp106.RemainingSinkholeCooldown = Plugin.C.TeleportCooldown;
            player.Health -= Plugin.C.TeleportCostHealt;
            scp106.Vigor -= Mathf.Clamp01(Plugin.C.TeleportCostVigor / 100f);

            Plugin.Using = false;
        }
    }
}
