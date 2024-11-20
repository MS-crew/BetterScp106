using MEC;
using System;
using PlayerRoles;
using UnityEngine;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using BetterScp106.Commands;
using System.Collections.Generic;
using Exiled.API.Features.Doors;
using PlayerRoles.FirstPersonControl;
using Exiled.API.Features.Roles;


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
            if (!Plugin.config.StalkFeature)
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
            if (scp106.Vigor < Mathf.Clamp01(Plugin.config.StalkCostVigor / 100f) || player.Health <= Plugin.config.StalkCostHealt)
            {
                response = "You don't have enough energy or health to stalk anybody!";
                return false;
            }

            if (scp106.RemainingSinkholeCooldown > 0)
            {
                response = "You can't Stalk that often! Wait a cooldown before Stalk again.";
                return false;
            }

            Player target = Findtarget(player);
            if (target == null)
            {
                response = "You cant find any victim.";
                return false;
            }

            Timing.RunCoroutine(StalkV2(player, target));
            response = "Yesssss the fragrance of suffering";
            return true;
        }

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

        public static IEnumerator<float> StalkV2(Player player, Player target)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;

            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            target.Broadcast(Plugin.Instance.Translation.StalkVictimWarn, shouldClearPrevious: true);

            yield return Timing.WaitForSeconds(Plugin.config.StalkWarningBefore);

            if (!target.IsAlive)
            {
                player.Broadcast(Plugin.Instance.Translation.StalkFailed, shouldClearPrevious: true);
                Log.Debug("Cant find any player for stalk");
            }
            else
            {
                player.Broadcast(Plugin.Instance.Translation.StalkSuccesfull, shouldClearPrevious: true);
                Log.Debug("Stalk teleport starting");

                Vector3 tp = target.Position;
                if (!Physics.Raycast(target.Position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2))
                    tp = target.CurrentRoom.Position;

                scp106.HuntersAtlasAbility.SetSubmerged(true);

                yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.NormalizedState == 1.0f);

                scp106.Owner.Teleport(tp);
                Log.Debug("SCP-106 is ground'.");

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.NormalizedState == 1.0f);
                Log.Debug("SCP-106 exiting ground.");
                player.Health -= Plugin.config.StalkCostHealt;
                scp106.RemainingSinkholeCooldown = (float)Plugin.config.AfterStalkCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f);
                Log.Debug("cooldown is added and health and vigor are reduced");
            }

            Better106.Using = false;
        }
    }
}
