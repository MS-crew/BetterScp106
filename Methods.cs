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
using Map = Exiled.API.Features.Map;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using Player = Exiled.API.Features.Player;
using Warhead = Exiled.API.Features.Warhead;
using Exiled.API.Features.Core.UserSettings;

namespace BetterScp106
{
    public class Methods
    {

        public enum Features
        {
            PocketKey,
            PocketinKey,
            StalkKey,
            StalkMode,
            StalkDistanceSlider,
            TeleportRooms,
            TeleportRoomsList,
            Description
        }

        public static RelativePosition RandomZone()
        {
            List<RoomType> randompos =
            [
                RoomType.Lcz914,
                RoomType.Hcz096,
                RoomType.EzGateB,
                RoomType.Surface,
            ];

            if (Warhead.RealDetonationTimer < 15 || Warhead.IsDetonated)
            {
                randompos.Remove(RoomType.Lcz914);
                randompos.Remove(RoomType.Hcz096);
                randompos.Remove(RoomType.EzGateB);
            }

            if (Map.DecontaminationState == DecontaminationState.Countdown|| Map.DecontaminationState == DecontaminationState.Finish) 
                randompos.Remove(RoomType.Lcz914);
                
            if (randompos.Count == 0)
            {
                Log.Error("Somethings gone wrong,No valid random zones found, defaulting to Surface.");
                return new RelativePosition(Room.Get(RoomType.Surface).Position);
            }

            Vector3 position = Room.Get(randompos.RandomItem()).Position;

            if (position == Vector3.zero)
            {
                Log.Warn("Somethings gone wrong, invalid position detected defaulting to Surface.");
                position = Room.Get(RoomType.Surface).Position;
            }

            RelativePosition Relaiveposition = new(position);
            Log.Debug("Random Zones count: " + randompos.Count + " selected random position: " + position + " Random zone mode: " + Plugin.Instance.Config.PocketexitRandomZonemode);

            return Relaiveposition;
        }

        public static Player Findtarget(Player player)
        {
            if (!SettingBase.TryGetSetting<TwoButtonsSetting>(player, Plugin.Instance.Config.AbilitySettingIds[Features.StalkMode], out TwoButtonsSetting stalkmode) || !SettingBase.TryGetSetting<SliderSetting>(player, Plugin.Instance.Config.AbilitySettingIds[Features.StalkDistanceSlider], out SliderSetting Slider))
                return null;

            IEnumerable<Player> stalkablePlayers = Player.List.Where
            (
                p =>
                Plugin.Instance.Config.StalkableRoles.Contains(p.Role) &&
                p.Health < Plugin.Instance.Config.StalkTargetmaxHealt &&
                p.CurrentRoom?.Type != RoomType.Pocket
            );

            if (!Plugin.Instance.Config.StalkFromEverywhere)
            {
                stalkablePlayers = stalkablePlayers.Where
                (
                    p =>
                    Vector3.Distance(p.Position, player.Position) <= Slider.SliderValue ||
                    (
                        p.CurrentRoom.Doors?.Any(door => door is ElevatorDoor) == true &&
                        Vector3.Distance(p.CurrentRoom.Position, player.Position) <= Slider.SliderValue
                    )
                );
            }

            if (stalkmode.IsSecond)
                return stalkablePlayers.OrderBy(p => p.Health).FirstOrDefault();
            else
                return stalkablePlayers.OrderBy(p => Vector3.Distance(p.Position, player.Position)).FirstOrDefault();
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

            player.Teleport(Scp106PocketExitFinder.GetBestExitPosition(fpcRole), Vector3.zero);

            if (player.Role == RoleTypeId.Scp106 && Plugin.Instance.Config.Reminders)
                ShowRandomScp106Hint(player);
        }

        public static void ShowRandomScp106Hint(Player player)
        {
            int randomIndex = Random.Range(0, 3);
            string hint = randomIndex switch
            {
                0 => Plugin.Instance.Translation.Scp106PowersPocket.Replace("$pockethealt", Plugin.Instance.Config.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.Instance.Config.PocketdimensionCostVigor.ToString()),
                1 => Plugin.Instance.Translation.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.Instance.Config.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.Instance.Config.PocketinCostVigor.ToString()),
                2 => Plugin.Instance.Translation.Scp106PowersStalk.Replace("$stalkhealt", Plugin.Instance.Config.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.Instance.Config.StalkCostVigor.ToString()),
                _ => Plugin.Instance.Translation.Scp106StartMessage,
            };

            player.ShowHint(hint, 3);
        }

        public static void ProcessUserInput(Player sender, SettingBase settingbase)
        {
            if (sender.Role.Type != RoleTypeId.Scp106 || EventHandlers.SpecialFeatureUsing)
                return;

            if (settingbase is ButtonSetting teleportbuton)
            {

                if (teleportbuton.Id == Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRooms])
                    TeleportRooms.TeleportFeature(sender);
            }

            if (settingbase is KeybindSetting keybindSetting && keybindSetting.IsPressed)
            {

                if (keybindSetting.Id == Plugin.Instance.Config.AbilitySettingIds[Methods.Features.PocketKey])
                    GotoPocket.PocketFeature(sender);

                else if (keybindSetting.Id == Plugin.Instance.Config.AbilitySettingIds[Methods.Features.PocketinKey])
                    TakeScpsPocket.PocketInFeature(sender);

                else if (keybindSetting.Id == Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkKey])
                    Stalking.StalkFeature(sender);
            }
        }
    }
}
