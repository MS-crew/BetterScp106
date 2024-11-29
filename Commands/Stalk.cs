using MEC;
using System;
using PlayerRoles;
using UnityEngine;
using CommandSystem;
using Exiled.API.Features;
using BetterScp106.Commands;
using System.Collections.Generic;
using CustomPlayerEffects;


namespace BetterScp106
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Stalk : ICommand
    {
        public string Command => "stalk";
        public string[] Aliases { get; } = { "sk" };
        public string Description => "Sends Scp-106 to someone injured";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.C.StalkFeature)
            {
                response = "This feature is closed by server owner.";
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
            
            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                response = "You can't Stalk that often! Wait a cooldown before Stalk again.";
                return false;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.C.StalkCostVigor / 100f) || player.Health <= Plugin.C.StalkCostHealt)
            {
                response = "You don't have enough energy or health to stalk anybody!";
                return false;
            }
    
            #nullable enable
            Player? target = Methods.Findtarget(player);
            #nullable disable
            if (target == null)
            {
                response = "You cant find any victim.";
                return false;
            }

            Timing.RunCoroutine(StalkV2(player, target));
            response = "Yesssss the fragrance of suffering";
            return true;
        }

        public static IEnumerator<float> StalkV2(Player player, Player target)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;

            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            target.Broadcast(Plugin.T.StalkVictimWarn, shouldClearPrevious: true);

            yield return Timing.WaitForSeconds(Plugin.C.StalkWarningBefore);

            if (!target.IsAlive)
            {
                player.Broadcast(Plugin.T.StalkFailed, shouldClearPrevious: true);
                Log.Debug("Cant find any player for stalk");
            }
            else
            {
                player.Broadcast(Plugin.T.StalkSuccesfull, shouldClearPrevious: true);
                Log.Debug("Stalk teleport starting");

                Vector3 tp = target.Position;
                if (!Physics.Raycast(target.Position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2))
                    tp = target.CurrentRoom.Position;

                scp106.IsSubmerged = true;
                player.EnableEffect<Ensnared>();

                yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

                scp106.Owner.Teleport(tp);
                Log.Debug("SCP-106 is ground'.");

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
                Log.Debug("SCP-106 exiting ground.");

                player.DisableEffect<Ensnared>();
                player.Health -= Plugin.C.StalkCostHealt;
                scp106.RemainingSinkholeCooldown = (float)Plugin.C.AfterStalkCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.C.StalkCostVigor / 100f);
                Log.Debug("cooldown is added and health and vigor are reduced");
            }

            Better106.Using = false;
        }
    }
}
