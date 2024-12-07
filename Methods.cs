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
        public enum Features
        {
            PocketKey,
            PocketinKey,
            StalkKey
        } 
        public static RelativePosition RandomZone()
        {
            List<Vector3> randompos =
            [
                Room.Get(RoomType.Surface).Position
            ];
            if (!Warhead.IsDetonated)
            {
                if(!Map.IsLczDecontaminated)
                    randompos.Add(Room.Get(RoomType.Lcz914).Position);
                randompos.Add(Room.Get(RoomType.EzGateB).Position);
                randompos.Add(Room.Get(RoomType.Hcz096).Position);
            }

            int Randomzone = new System.Random().Next(randompos.Count);
            Log.Debug( randompos.Count + Randomzone);
            RelativePosition position = new(randompos[Randomzone]);
            Log.Debug(position);
            return position;
        }
        public static Player Findtarget(Player player)
        {
            Player target;
            if (Plugin.C.StalkFromEverywhere)
            {
                target = Player.List
                .Where(p =>
                    Plugin.C.SalkingRoles.Contains(p.Role) &&
                    p.Health < Plugin.C.StalkTargetmaxHealt &&
                    p.CurrentRoom != null &&
                    p.CurrentRoom.Type != RoomType.Pocket
                )
                .OrderBy(p => p.Health)
                .FirstOrDefault();
            }
            else
            {
                target = Player.List
                .Where(p =>
                    Plugin.C.SalkingRoles.Contains(p.Role) &&
                    p.Health < Plugin.C.StalkTargetmaxHealt &&
                    p.CurrentRoom != null &&
                    p.CurrentRoom.Type != RoomType.Pocket &&
                    (
                        Vector3.Distance(p.Position, player.Position) <= Plugin.C.StalkDistance ||
                        (
                         p.CurrentRoom.Doors != null &&
                         p.CurrentRoom.Doors.Any(door => door is ElevatorDoor) &&
                         Vector3.Distance(p.CurrentRoom.Position, player.Position) <= Plugin.C.StalkDistance
                         )
                    )
                )
                .OrderBy(p => p.Health)
                .FirstOrDefault();
            }
            return target;
        }
        public static Player FindFriend(Player player)
        {
            Player friend = Player.List.Where
                            (p => p != player &&
                                      p.IsScp &&
                                      Vector3.Distance(p.Position, player.Position) <= 1.5
                            )
                            .OrderBy(p => Vector3.Distance(p.Position, player.Position))
                            .FirstOrDefault();
            return friend;
        }   
        public static void EscapeFromDimension(Player player)
        {
            ReferenceHub referenceHub = player.ReferenceHub;
            if (referenceHub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                fpcRole.FpcModule.ServerOverridePosition(Scp106PocketExitFinder.GetBestExitPosition(fpcRole), Vector3.zero);

                player.DisableAllEffects();

                if (player.Role == RoleTypeId.Scp106 && Plugin.C.Reminders)
                    Methods.ShowRandomScp106Hint(player);
            }
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
        public static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase setting)
        {
            switch ((Features) setting.SettingId)
            {
                case Features.PocketKey
                when setting is SSKeybindSetting keybind:
                    {
                        if (keybind.SyncIsPressed)
                            Pocket.PocketFeature(Player.Get(sender));
                    }
                    break;
                case Features.PocketinKey
                when setting is SSKeybindSetting keybind:
                    {
                        if (keybind.SyncIsPressed)
                            PocketIn.PocketInV3(Player.Get(sender));
                    }
                    break;
            }
        }  
    }
}
