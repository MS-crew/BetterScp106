using System.Linq;
using UnityEngine;
using PlayerRoles;
using Exiled.API.Enums;
using Exiled.API.Features;
using RelativePositioning;
using BetterScp106.Features;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using UserSettings.ServerSpecific;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;

namespace BetterScp106
{
    public class Methods
    {
        public static float StalkDistance = Plugin.C.StalkDistance;
        public enum Features
        {
            PocketKey,
            PocketinKey,
            StalkKey,
            StalkMode,
            StalkDistanceSlider
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
        internal static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase settingbase)
        {
            if (settingbase is SSKeybindSetting keybindSetting && sender.roleManager.CurrentRole.RoleTypeId == RoleTypeId.Scp106 && keybindSetting.SyncIsPressed)
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
