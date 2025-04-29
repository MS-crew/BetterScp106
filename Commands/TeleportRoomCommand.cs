using MEC;
using System;
using UnityEngine;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using BetterScp106.Features;
using Exiled.API.Features.Roles;

namespace BetterScp106.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TeleportRoomCommand : ICommand
    {
        public string Command => Plugin.Instance.Translation.TeleportRoomCommand;
        public string[] Aliases => Plugin.Instance.Translation.TeleportRoomCommandAliases;
        public string Description => Plugin.Instance.Translation.TeleportRoomCommandDescription;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.TeleportRoomsFeature)
            {
                response = "This feature is disabled";
                return false;
            }

            Player player = Player.Get(sender);

            if (player.Role.Is<Scp106Role>(out Scp106Role scp106))
            {
                response = string.Empty;

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
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                    return false;
                }

                if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.TeleportCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.TeleportCostHealt)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportCant);
                    return false;
                }

                if (AlphaWarheadController.Detonated)
                {
                    scp106.IsSubmerged = true;
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                    return false;
                }

                Room targetRoom = Room.Get(roomType);

                if (targetRoom == null)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportRoomNull, shouldClearPrevious: true);
                    return false;
                }

                if (Plugin.Instance.Config.TeleportOnlySameZone && scp106.Owner.Zone != targetRoom.Zone)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportCantforZone, shouldClearPrevious: true);
                    return false;
                }

                bool flaglcz = Map.IsLczDecontaminated && targetRoom.Zone == ZoneType.LightContainment;
                bool flagSite = Warhead.IsDetonated && targetRoom.Zone != ZoneType.Surface;

                if (flaglcz || flagSite)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.TeleportRoomDanger, shouldClearPrevious: true);
                    return false;
                }

                Timing.RunCoroutine(TeleportRooms.TeleportRoom(scp106, targetRoom.Position));
                return true;
            }

            response = "You can`t use this command";
            return false;
        }
    }
}
