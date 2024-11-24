using System.Linq;
using UnityEngine;
using MapGeneration;
using Exiled.API.Enums;
using Exiled.API.Features;
using CustomPlayerEffects;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using Interactables.Interobjects.DoorUtils;

namespace BetterScp106
{
    public class Methods
    {
        public static Player Findtarget(Player player)
        {
            Player target = Player.List
                .Where(p =>
                    p.IsHuman &&
                    p.CurrentRoom != null &&
                    p.Health < Plugin.config.StalkTargetmaxHealt &&
                    (
                        Vector3.Distance(p.Position, player.Position) <= Plugin.config.StalkDistance
                        ||
                        (p.CurrentRoom.Doors != null &&
                         p.CurrentRoom.Doors.Any(door => door is ElevatorDoor) &&
                         Vector3.Distance(p.CurrentRoom.Position, player.Position) <= Plugin.config.StalkDistance)
                    )
                )
                .OrderBy(p => p.Health)
                .FirstOrDefault();
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
                fpcRole.FpcModule.ServerOverridePosition(!Plugin.config.PocketexitRandomZonemode ? Scp106PocketExitFinder.GetBestExitPosition(fpcRole) : Methods.GetBestExitPositionRandomZone(fpcRole), Vector3.zero);

                player.DisableEffect<PocketCorroding>();
                player.DisableEffect<Corroding>();

                if (!player.IsScp)
                {
                    player.EnableEffect<Disabled>(10f, true);
                    player.EnableEffect<Traumatized>();
                }

                PocketDimensionGenerator.RandomizeTeleports();
            }
        }
        public static void ShowRandomScp106Hint(Player player)
        {
            int randomIndex = new System.Random().Next(0, 3);
            string hint = randomIndex switch
            {
                0 => Plugin.Instance.Translation.Scp106PowersPocket.Replace("$pockethealt", Plugin.config.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.config.PocketdimensionCostVigor.ToString()),
                1 => Plugin.Instance.Translation.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.config.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.config.PocketinCostVigor.ToString()),
                2 => Plugin.Instance.Translation.Scp106PowersStalk.Replace("$stalkhealt", Plugin.config.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.config.StalkCostVigor.ToString()),
                _ => Plugin.Instance.Translation.Scp106StartMessage,
            };
            player.ShowHint(hint, 3);
        }
        public static Vector3 GetBestExitPositionRandomZone(IFpcRole role)
        {
            List<Vector3> randompos = new List<Vector3>
            {
                Room.Get(RoomType.Surface).Position
            };

            if (!Warhead.IsDetonated)
            {
                if (!Map.IsLczDecontaminated)
                    randompos.Add(Room.Get(RoomType.Lcz914).Position);
                randompos.Add(Room.Get(RoomType.HczArmory).Position);
                randompos.Add(Room.Get(RoomType.EzCafeteria).Position);
            }

            int Randomzone = new System.Random().Next(randompos.Count);
            Vector3 position = randompos[Randomzone];
            RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPositionRaycasts(position);
            if (roomIdentifier == null)
            {
                Log.Warn($"roomIdentifier was null, room is {Room.Get(position)}, roomIdentifier is {roomIdentifier}, position is {position}, seed is{Map.Seed}");
                return position;
            }

            DoorVariant[] whitelistedDoorsForZone = Scp106PocketExitFinder.GetWhitelistedDoorsForZone(roomIdentifier.Zone);
            return whitelistedDoorsForZone.Length != 0 ? Scp106PocketExitFinder.GetSafePositionForDoor(Scp106PocketExitFinder.GetRandomDoor(whitelistedDoorsForZone), roomIdentifier.Zone == FacilityZone.Surface ? 45f : 11f, role.FpcModule.CharController) : position;
        }
    }
}
