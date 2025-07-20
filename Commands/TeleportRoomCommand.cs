// -----------------------------------------------------------------------
// <copyright file="TeleportRoomCommand.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Commands
{
    using System;
    using BetterScp106.Features;
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Represents the command used to teleport SCP-106 to a specific room.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeleportRoomCommand : ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Command => Plugin.Instance.Translation.TeleportRoomCommand;

        /// <summary>
        /// Gets the command aliases.
        /// </summary>
        public string[] Aliases => Plugin.Instance.Translation.TeleportRoomCommandAliases;

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => Plugin.Instance.Translation.TeleportRoomCommandDescription;

        /// <summary>
        /// Executes the teleport room command.
        /// </summary>
        /// <param name="arguments">The arguments provided with the command.</param>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="response">The response message to be sent back to the sender.</param>
        /// <returns>True if the command executed successfully; otherwise, false.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.TeleportRoomsFeature)
            {
                response = "This feature is disabled";
                return false;
            }

            Player player = Player.Get(sender);

            if (!player.Role.Is<Scp106Role>(out Scp106Role scp106))
            {
                response = "You can`t use this command";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = $"Usage: .{Plugin.Instance.Translation.TeleportRoomCommand} <RoomType> for roomtypes .{Plugin.Instance.Translation.TeleportRoomCommand} rooms";
                return false;
            }

            if (arguments.Array[1].ToLower() == "rooms")
            {
                response = "Rooms:\n" + string.Join("\n", Plugin.Instance.Config.Rooms);
                return false;
            }

            if (!Enum.TryParse(arguments.Array[1], true, out RoomType roomType) || !Plugin.Instance.Config.Rooms.Contains(roomType))
            {
                response = $"'{arguments.Array[1]}' is not a valid RoomType, for roomtypes .{Plugin.Instance.Translation.TeleportRoomCommand} rooms";
                return false;
            }

            if (scp106.RemainingSinkholeCooldown > 0 || scp106.IsStalking || scp106.IsSubmerged)
            {
                player.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                response = Plugin.Instance.Translation.Cooldown.Content;
                return false;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.TeleportCostVigor / 100f) || player.Health <= Plugin.Instance.Config.TeleportCostHealt)
            {
                player.Broadcast(Plugin.Instance.Translation.TeleportCant, shouldClearPrevious: true);
                response = Plugin.Instance.Translation.TeleportCant.Content;
                return false;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                player.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                response = Plugin.Instance.Translation.Afternuke.Content;
                return false;
            }

            Room targetRoom = Room.Get(roomType);

            if (targetRoom == null)
            {
                player.Broadcast(Plugin.Instance.Translation.TeleportRoomNull, shouldClearPrevious: true);
                response = Plugin.Instance.Translation.TeleportRoomNull.Content;
                return false;
            }

            if (Plugin.Instance.Config.TeleportOnlySameZone && player.Zone != targetRoom.Zone)
            {
                player.Broadcast(Plugin.Instance.Translation.TeleportCantforZone, shouldClearPrevious: true);
                response = Plugin.Instance.Translation.TeleportCantforZone.Content;
                return false;
            }

            bool flagLcz = Map.IsLczDecontaminated && targetRoom.Zone == ZoneType.LightContainment;
            bool flagSite = Warhead.IsDetonated && targetRoom.Zone != ZoneType.Surface;

            if (flagLcz || flagSite)
            {
                player.Broadcast(Plugin.Instance.Translation.TeleportRoomDanger, shouldClearPrevious: true);
                response = Plugin.Instance.Translation.TeleportRoomDanger.Content;
                return false;
            }

            Timing.RunCoroutine(TeleportRooms.TeleportRoom(scp106, targetRoom.Position));
            response = "Teleport-Room Ability used";
            return true;
        }
    }
}