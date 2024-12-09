using System;
using System.Linq;
using UnityEngine;
using PlayerRoles;
using MapGeneration;
using Exiled.API.Enums;
using ProgressiveCulling;
using Exiled.API.Features;
using RelativePositioning;
using BetterScp106.Features;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using UserSettings.ServerSpecific;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using Interactables.Interobjects.DoorUtils;

namespace BetterScp106
{
    public class Methods
    {
        static RoomIdentifier syncroom;
        public static float StalkDistance = Plugin.C.StalkDistance;
        public enum Features
        {
            PocketKey,
            PocketinKey,
            StalkKey,
            StalkMode,
            StalkDistanceSlider,
            TeleportRooms,
            TeleportRoomsList
        }
        public static RelativePosition RandomZone()
        {
            List<Vector3> randompos =
            [
                Room.Get(RoomType.Hcz096).Position,
                Room.Get(RoomType.Lcz914).Position,
                Room.Get(RoomType.EzGateB).Position,
                Room.Get(RoomType.Surface).Position,
            ]; 
            if(Warhead.RealDetonationTimer < 15) 
            { 
                randompos.Remove(Room.Get(RoomType.Lcz914).Position);
                randompos.Remove(Room.Get(RoomType.EzGateB).Position);
                randompos.Remove(Room.Get(RoomType.Hcz096).Position);
            }
            if (Map.IsLczDecontaminated || Map.DecontaminationState == DecontaminationState.Countdown)
                randompos.Remove(Room.Get(RoomType.Lcz914).Position);

            int Randomzone = new System.Random().Next(randompos.Count);
            RelativePosition position = new(randompos[Randomzone]);
            Log.Debug("Random Zones count: " + randompos.Count + "random position " + position);
            return position;
        }
        public static Player Findtarget(Player player)
        {
            bool StalkMode = ServerSpecificSettingsSync.GetSettingOfUser<SSTwoButtonsSetting>(player.ReferenceHub, Plugin.C.AbilitySettingIds[Features.StalkMode]).SyncIsB;
            float StalkDistance = ServerSpecificSettingsSync.GetSettingOfUser<SSSliderSetting>(player.ReferenceHub, Plugin.C.AbilitySettingIds[Features.StalkDistanceSlider]).SyncFloatValue;

            IEnumerable<Player> stalkablePlayers = Player.List.Where
            (
                p =>
                Plugin.C.StalkableRoles.Contains(p.Role) &&
                p.Health < Plugin.C.StalkTargetmaxHealt &&
                p.CurrentRoom?.Type != RoomType.Pocket
            );

            if (!Plugin.C.StalkFromEverywhere)
            {
                stalkablePlayers = stalkablePlayers.Where
                (
                    p =>
                    Vector3.Distance(p.Position, player.Position) <= StalkDistance ||
                    (
                        p.CurrentRoom.Doors?.Any(door => door is ElevatorDoor) == true &&
                        Vector3.Distance(p.CurrentRoom.Position, player.Position) <= StalkDistance
                    )
                );
            }
            if (StalkMode)
            {
                Log.Debug("Stalk Mode: For Healt and Stalk Distance is " + StalkDistance);
                return stalkablePlayers.OrderBy(p => p.Health).FirstOrDefault();
            }
            else
            {
                Log.Debug("Stalk Mode: For Distance and Stalk Distance is" + StalkDistance);
                return stalkablePlayers.OrderBy(p => Vector3.Distance(p.Position, player.Position)).FirstOrDefault();
            }
        }
        public static Player FindFriend(Player player)
        {
            Player friend = Player.List.Where
                            (
                                p => 
                                p != player &&
                                p.IsScp &&
                                Vector3.Distance(p.Position, player.Position) <= 1.5
                            )
                            .OrderBy(p => Vector3.Distance(p.Position, player.Position))
                            .FirstOrDefault();
            return friend;
        }   
        public static void EscapeFromDimension(Player player)
        {
            if (player.ReferenceHub.roleManager.CurrentRole is not IFpcRole fpcRole)
                return;

            player.DisableAllEffects();
            fpcRole.FpcModule.ServerOverridePosition(Scp106PocketExitFinder.GetBestExitPosition(fpcRole), Vector3.zero);
            
            if (player.Role == RoleTypeId.Scp106 && Plugin.C.Reminders)
                ShowRandomScp106Hint(player);
        }
        public static void ShowRandomScp106Hint(Player player)
        {
            int randomIndex = new System.Random().Next(0, 3);
            string hint = randomIndex switch
            {
                0 => Plugin.T.Scp106PowersPocket.Replace("$pockethealt", Plugin.C.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.C.PocketdimensionCostVigor.ToString()),
                1 => Plugin.T.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.C.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.C.PocketinCostVigor.ToString()),
                2 => Plugin.T.Scp106PowersStalk.Replace("$stalkhealt", Plugin.C.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.C.StalkCostVigor.ToString()),
                _ => Plugin.T.Scp106StartMessage,
            };
            player.ShowHint(hint, 3);
        }
        private static bool ValidateDestinationDoor(DoorVariant dv)
        {
            return dv is IScp106PassableDoor scp106PassableDoor && scp106PassableDoor.IsScp106Passable && dv.Rooms.Contains<RoomIdentifier>(syncroom);
        }
        public static Vector3 GetSafePosition(Player scp106, Vector3 targetpos)
        {
            Vector3 safePosition = scp106.Position;
            syncroom = Room.Get(targetpos).Identifier;
            float num1 = float.MaxValue;
            foreach (Pose location in SafeLocationFinder.GetLocations(new Predicate<RoomCullingConnection>(ValidateDestinationConnection), new Predicate<DoorVariant>(ValidateDestinationDoor)))
            {
                if ((double)Mathf.Abs(location.position.y - targetpos.y) <= 50.0)
                {
                    Vector3 vector3 = ClosestDoorPosition(location.position, targetpos);
                    float num2 = (vector3 - targetpos).SqrMagnitudeIgnoreY();
                    if ((double)num2 <= (double)num1)
                    {
                        num1 = num2;
                        safePosition = vector3;
                    }
                }
            }
            return safePosition;
        }
        private static Vector3 ClosestDoorPosition(Vector3 doorPos, Vector3 targetpos)
        {
            Vector3 vector3 = targetpos - doorPos;
            Vector3 dir = new(vector3.x, 0.0f, vector3.z);
            float maxDis = dir.magnitude;
            if ((double)maxDis > 0.0)
                dir /= maxDis;
            float radius = 0.2f;
            float height = 0.5f;
            Vector3 origin = doorPos + Vector3.up * (0.2f + radius);
            Color debugColor = (double)Scp106HuntersAtlasAbility.DebugDuration > 0.0 ? UnityEngine.Random.ColorHSV(0.0f, 1f, 0.5f, 1f, 0.4f, 0.8f) : Color.clear;
            var scp106HuntersAtlasAbility = new Scp106HuntersAtlasAbility();
            Vector3 pos;
            while (!scp106HuntersAtlasAbility.TrySphereCast(debugColor, origin, dir, radius, height, maxDis, out pos))
            {
                maxDis = Mathf.Min(15f, maxDis - radius);
                if ((double)maxDis < (double)radius)
                    return doorPos + Vector3.up * height;
            }
            return pos;
        }
        private static bool ValidateDestinationConnection(RoomCullingConnection connection)
        {
            RoomCullingConnection.RoomLink link = connection.Link;
            if (!link.Valid)
                return false;
            return syncroom == link.RoomA || syncroom == link.RoomB;
        }
        internal static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase settingbase)
        {
            if (sender.roleManager.CurrentRole.RoleTypeId != RoleTypeId.Scp106)
                return;

            if (settingbase is SSButton teleportbuton)
            {
                if (teleportbuton.SettingId == Plugin.C.AbilitySettingIds[Features.TeleportRooms])
                {
                    TeleportRooms.TeleportFeature(Player.Get(sender));
                }
            }

            if (settingbase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                if (keybindSetting.SettingId == Plugin.C.AbilitySettingIds[Features.PocketKey])
                {
                    Pocket.PocketFeature(Player.Get(sender));
                }
                else if (keybindSetting.SettingId == Plugin.C.AbilitySettingIds[Features.PocketinKey])
                {
                    PocketIn.PocketInFeature(Player.Get(sender));
                }
                else if (keybindSetting.SettingId == Plugin.C.AbilitySettingIds[Features.StalkKey])
                {
                    Stalking.StalkFeature(Player.Get(sender));
                }
            }
        }
    }
}
