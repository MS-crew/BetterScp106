﻿using System.Linq;
using UnityEngine;
using PlayerRoles;
using Exiled.API.Enums;
using Exiled.API.Features;
using RelativePositioning;
//using BetterScp106.Features;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
//using UserSettings.ServerSpecific;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;

namespace BetterScp106
{
    public class Methods
    {
      /*public enum Features
        {
            PocketKey,
            PocketinKey,
            StalkKey
        } */
        public static RelativePosition RandomZone()
        {
            List<Vector3> randompos =
            [
                Room.Get(RoomType.Hcz096).Position,
                Room.Get(RoomType.Lcz914).Position,
                Room.Get(RoomType.EzGateB).Position,
                Room.Get(RoomType.Surface).Position,
            ]; 
            if (Map.IsLczDecontaminated)
                randompos.Remove(Room.Get(RoomType.Lcz914).Position);

            int Randomzone = new System.Random().Next(randompos.Count);
            RelativePosition position = new(randompos[Randomzone]);
            Log.Debug("Random Zones count: " + randompos.Count + "random position " + position);
            return position;
        }
        public static Player Findtarget(Player player)
        {
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
                    Vector3.Distance(p.Position, player.Position) <= Plugin.C.StalkDistance ||
                    (
                        p.CurrentRoom.Doors?.Any(door => door is ElevatorDoor) ==true &&
                        Vector3.Distance(p.CurrentRoom.Position, player.Position) <= Plugin.C.StalkDistance
                    )
                );
            }

            return stalkablePlayers.OrderBy(p => p.Health).FirstOrDefault();
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
      /*public static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase setting)
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
                            PocketIn.PocketInFeature(Player.Get(sender));
                    }
                    break;
                case Features.StalkKey
                when setting is SSKeybindSetting keybind:
                    {
                        if (keybind.SyncIsPressed)
                            Stalking.StalkFeature(Player.Get(sender));
                    }
                    break;
            }
        }*/
    }
}
