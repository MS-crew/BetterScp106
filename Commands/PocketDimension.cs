using MEC;
using System;
using PlayerRoles;
using UnityEngine;
using CommandSystem;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using System.Collections.Generic;
using Player = Exiled.API.Features.Player;
using Scp106Role = Exiled.API.Features.Roles.Scp106Role;

namespace BetterScp106
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PocketDimension : ParentCommand
    {
        public override string Command => "pocket";
        public override string[] Aliases { get; } = { "pd" };
        public override string Description => "Sends Scp-106 to pocket dimension";
        public PocketDimension() => LoadGeneratedCommands();
        public override void LoadGeneratedCommands()
        {
            if (Plugin.C.PocketinFeature)
                RegisterCommand(new PocketIn());
        }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.C.PocketFeature)
            {
                response = "This features closed by server owner.";
                return false;
            }

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

            player.Role.Is(out Scp106Role scp106);

            if (scp106.RemainingSinkholeCooldown > 0)
            {
                response = "You can't change dimension that often! Wait a cooldown before changing it again.";
                player.Broadcast(Plugin.T.cooldown, shouldClearPrevious: true);
                return false;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.C.PocketdimensionCostVigor / 100f) || player.Health <= Plugin.C.PocketdimensionCostHealt)
            {
                player.Broadcast(Plugin.T.scp106cantpocket);
                response = "You don't have enough energy or health to return to your kingdom!";
                return false;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                response = "You can't go to your pocket dimension after Warhead explodes!";
                player.Broadcast(Plugin.T.afternuke, shouldClearPrevious: true);
                return false;
            }

            Room pocketRoom = Room.Get(RoomType.Pocket);
            if (player.CurrentRoom.Type == RoomType.Pocket)
            {
                player.Broadcast(Plugin.T.scp106alreadypocket);
                response = "<color=red>You are already in pocket dimension?</color>";
                return false;
            }

            Timing.RunCoroutine(GoPocketV2(player));
            response = "<color=red>You go underground and come out in the pocket dimension...</color>";
            return true;
        }
        public static IEnumerator<float> GoPocketV2(Player player)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;
            var scp106 = player.Role as Scp106Role;

            scp106.IsStalking = true;

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.SubmergeProgress == 1f); 

            player.EnableEffect<PocketCorroding>();
            scp106.IsStalking = false;

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.TargetSubmerged); 

            player.DisableAllEffects();
            scp106.RemainingSinkholeCooldown = Plugin.C.AfterPocketdimensionCooldown;
            player.Health -= Plugin.C.PocketdimensionCostHealt;
            scp106.Vigor -= Mathf.Clamp01(Plugin.C.PocketdimensionCostVigor / 100f);
            player.Broadcast(Plugin.T.scp106inpocket, shouldClearPrevious: true);
            Better106.Using = false;
        }

    }
}
