using System;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;
using System.Linq;
using MEC;
using static UnityEngine.GraphicsBuffer;

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

            var config = Plugin.Instance.Config;
            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (scp106.Vigor < Mathf.Clamp01(config.StalkCostVigor / 100f) || player.Health <= config.StalkCostHealt|| scp106.RemainingSinkholeCooldown > 0)
            {
                response = "You don't have enough energy or health to stalk anybody!";
                return false;
            }
            
            Player target = Findtarget(player);

            if (target == null)
            {
               response = "You cant find any victim.";
               return false;
            }

            response = "Yesssss the fragrance of suffering";
            stalk(player,target);
            return true;
        }

        public static bool IsSafePosition(Vector3 position)
        {
            if (Physics.Raycast(position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, 2f))
            {
                //if (!Physics.CheckCapsule(hit.point, hit.point + Vector3.up * 1.8f, 0.5f))
                //{
                    Log.Debug("Position is not safe");
                    return true;
                //}
                //return false;
            }
            Log.Debug("Position is Safe");
            return false;
        }
        public static Player Findtarget(Player player)
        {   
            Player target = Player.List
            .Where(p => p.IsHuman && p.Health < Plugin.Instance.Config.StalkTargetmaxHealt
            && ((Vector3.Distance(p.Position, player.Position) <= Plugin.Instance.Config.StalkDistance
            || (p.CurrentRoom?.NearestRooms.FirstOrDefault()?.Position != null
            && Vector3.Distance(p.CurrentRoom.NearestRooms.FirstOrDefault().Position, player.Position) <= Plugin.Instance.Config.StalkDistance))))
            .OrderBy(p => p.Health)
            .FirstOrDefault();
            return target;
        }
        public static void stalk(Player player,Player target)
        {
            var config = Plugin.Instance.Config;
            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            target.Broadcast(Plugin.Instance.Translation.StalkVictimWarn, shouldClearPrevious: true);
            scp106.Vigor -= Mathf.Clamp01(config.StalkCostVigor / 100f);
            scp106.RemainingSinkholeCooldown = (float)config.AfterStalkCooldown;
            player.Health -= config.StalkCostHealt;

            Timing.CallDelayed(1f, () =>
            {
                if (target.IsAlive)
                {
                    player.Broadcast(Plugin.Instance.Translation.StalkSuccesfull, shouldClearPrevious: true);
                    Log.Debug("Portal is being used");
                    Room room = Room.Get(target.Position);
                    Vector3 tp = target.Position;
                    Log.Debug("Target victim room: " + room.name);

                    if (!IsSafePosition(target.Position))
                    {
                        tp = room.Position;
                    }
                    scp106.UsePortal(tp + Vector3.up, 0f);              
                    Timing.CallDelayed(3.5f, () =>
                    {
                        scp106.RemainingSinkholeCooldown = 0f;
                        scp106.RemainingSinkholeCooldown = (float)config.AfterStalkCooldown;
                    });
                    Log.Debug("cooldown is added and health and vigor are reduced");
                    return;
                }
                player.Broadcast(Plugin.Instance.Translation.StalkFailed, shouldClearPrevious: true);
            });
        }
    }
}
