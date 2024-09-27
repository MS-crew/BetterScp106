using System;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;
using Exiled.API.Enums;
using Player = Exiled.API.Features.Player;
using Exiled.API.Features.Roles;
using MEC;
using CustomPlayerEffects;
using BetterScp106.Commands;
using PlayerRoles.PlayableScps.Scp106;
using Scp106Role = Exiled.API.Features.Roles.Scp106Role;
using Mirror;
using DeathAnimations;
using System.Collections.Generic;

namespace BetterScp106
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PocketDimension : ParentCommand
    {
        public override string Command => "pocket";

        public override string[] Aliases { get; } = { "pd"};

        public override string Description => "Sends Scp-106 to pocket dimension";

        public PocketDimension() => LoadGeneratedCommands();
        protected override  bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "This command must be executed in-game.";
                return false;
            }

            if (player.Role != RoleTypeId.Scp106)
            {
                response = "This command only for Scp-106";
                return false;
            }

            if (!(player.Role is Scp106Role scp106))
            {
                response = "Role error!";
                return false;
            }


            if (AlphaWarheadController.Detonated)
            {
                if (scp106.RemainingSinkholeCooldown <= 0f)
                {
                    scp106.IsSubmerged = true;
                }
                response= "You can't go to your pocket dimension after Warhead explodes!";
                player.Broadcast(Plugin.Instance.Translation.afternuke, shouldClearPrevious: true);
                return false;
            }


            if (scp106.RemainingSinkholeCooldown <= 0)
            {
                Room pocketRoom = Room.Get(RoomType.Pocket);

                if (player.CurrentRoom.Type == RoomType.Pocket)
                {
                    response = "<color=red>You are already in pocket dimension?</color>";
                    return false;
                }
                var config = Plugin.Instance.Config;
                if (scp106.Vigor < Mathf.Clamp01(config.PocketdimensionCostVigor / 100f) || player.Health <= config.PocketdimensionCostHealt)
                {
                    response = "You don't have enough energy or health to return to your kingdom!";
                    return false;
                }
                response = "<color=red>You go underground and come out in the pocket dimension...</color>";

                scp106.StalkAbility.IsActive = true;
                Timing.CallDelayed(3f, () =>
                {
                    player.EnableEffect<PocketCorroding>();
                    scp106.StalkAbility.IsActive = false;
                    player.DisableEffect<Corroding>();
                });

                player.Health -= config.PocketdimensionCostHealt;
                scp106.Vigor -= Mathf.Clamp01(config.PocketdimensionCostVigor / 100f);
                scp106.RemainingSinkholeCooldown = (float)config.AfterPocketdimensionCooldown;
                player.Broadcast(Plugin.Instance.Translation.scp106inpocket, shouldClearPrevious: true);         
                Timing.CallDelayed(3.5f, () => scp106.RemainingSinkholeCooldown = (float)config.AfterPocketdimensionCooldown);
                return true;
            }
            else
            {
                response = "You can't change dimension that often! Wait a cooldown before changing it again.";
                player.Broadcast(Plugin.Instance.Translation.cooldown, shouldClearPrevious: true);
                return false;
            }
        }
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new PocketIn());
        }
    }
}
