// -----------------------------------------------------------------------
// <copyright file="Methods.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Doors;
    using PlayerRoles;
    using PlayerRoles.FirstPersonControl;
    using PlayerRoles.PlayableScps.Scp106;
    using RelativePositioning;
    using UnityEngine;
    using Map = Exiled.API.Features.Map;
    using Player = Exiled.API.Features.Player;
    using Warhead = Exiled.API.Features.Warhead;

    /// <summary>
    /// Contains utility methods for SCP-106 special features.
    /// </summary>
    public class Methods
    {
        /// <summary>
        /// Enum representing various SCP-106 menu elements.
        /// </summary>
        public enum Features
        {
            PocketKey,
            PocketinKey,
            StalkKey,
            StalkMode,
            StalkDistanceSlider,
            TeleportRooms,
            TeleportRoomsList,
            Description,
        }

        /// <summary>
        /// Selects a random zone for SCP-106 to teleport to, considering game conditions.
        /// </summary>
        /// <returns>A <see cref="RelativePosition"/> representing the selected zone.</returns>
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

            if (Map.DecontaminationState == DecontaminationState.Countdown || Map.DecontaminationState == DecontaminationState.Finish)
            {
                randompos.Remove(RoomType.Lcz914);
            }

            if (randompos.Count == 0)
            {
                Log.Error("Somethings gone wrong, No valid random zones found, defaulting to Surface.");
                return new RelativePosition(Room.Get(RoomType.Surface).Position);
            }

            Vector3 position = Room.Get(randompos.RandomItem()).Position;

            if (position == Vector3.zero)
            {
                Log.Warn("Somethings gone wrong, invalid position detected defaulting to Surface.");
                position = Room.Get(RoomType.Surface).Position;
            }

            RelativePosition relaiveposition = new (position);
            Log.Debug("Random Zones count: " + randompos.Count + " selected random position: " + position + " Random zone mode: " + Plugin.Instance.Config.PocketexitRandomZonemode);

            return relaiveposition;
        }

        /// <summary>
        /// Finds a target player for SCP-106 to stalk based on health or distance.
        /// </summary>
        /// <param name="stalkbyHealt">Whether to prioritize players with lower health.</param>
        /// <param name="distance">The maximum distance to consider for stalking.</param>
        /// <param name="player">The SCP-106 player.</param>
        /// <returns>The target <see cref="Player"/> to stalk, or null if no valid target is found.</returns>
        public static Player Findtarget(bool stalkbyHealt, float distance, Player player)
        {
            IEnumerable<Player> stalkablePlayers = Player.List.Where(
                p =>
                Plugin.Instance.Config.StalkableRoles.Contains(p.Role) &&
                p.Health < Plugin.Instance.Config.StalkTargetmaxHealt &&
                p.CurrentRoom?.Type != RoomType.Pocket);

            if (!Plugin.Instance.Config.StalkFromEverywhere)
            {
                stalkablePlayers = stalkablePlayers.Where(p => Vector3.Distance(p.Position, player.Position) <= distance ||
                    (p.CurrentRoom.Doors?.Any(door => door is ElevatorDoor) == true && Vector3.Distance(p.CurrentRoom.Position, player.Position) <= distance));
            }

            if (stalkbyHealt)
            {
                return stalkablePlayers.OrderBy(p => p.Health).FirstOrDefault();
            }
            else
            {
                return stalkablePlayers.OrderBy(p => Vector3.Distance(p.Position, player.Position)).FirstOrDefault();
            }
        }

        /// <summary>
        /// Finds a nearby SCP teammate for SCP-106.
        /// </summary>
        /// <param name="player">The SCP-106 player.</param>
        /// <returns>The nearest SCP teammate <see cref="Player"/>, or null if none are found.</returns>
        public static Player FindFriend(Player player)
        {
            Player friend = Player.List.Where(p => p != player && p.IsScp && Vector3.Distance(p.Position, player.Position) <= 1.5)
                .OrderBy(p => Vector3.Distance(p.Position, player.Position))
                .FirstOrDefault();

            return friend;
        }

        /// <summary>
        /// Teleports SCP-106 out of the pocket dimension.
        /// </summary>
        /// <param name="player">The SCP-106 player.</param>
        public static void EscapeFromDimension(Player player)
        {
            if (player.Role.Base is not IFpcRole fpcRole)
            {
                Log.Error($"Player {player.Nickname} has an invalid role: {player.Role}");
                return;
            }

            Vector3 exitPos = Scp106PocketExitFinder.GetBestExitPosition(fpcRole);

            if (exitPos == Vector3.zero)
            {
                Log.Error($"EscapeFromDimension: Exit position is invalid for {player.Nickname}.");
                return;
            }

            player.Teleport(exitPos, Vector3.zero);

            if (player.Role == RoleTypeId.Scp106 && Plugin.Instance.Config.Reminders)
            {
                ShowRandomScp106Hint(player);
            }
        }

        /// <summary>
        /// Displays a random hint about SCP-106's abilities to the player.
        /// </summary>
        /// <param name="player">The SCP-106 player.</param>
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
    }
}
