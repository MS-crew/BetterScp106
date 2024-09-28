using System;
using System.Linq;
using CommandSystem;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace BetterScp106.Commands
{
    public class PocketIn : ICommand
    {
        public string Command { get;} = "in";

        public string[] Aliases { get;} = { "i" };

        public string Description { get;} = "Host your friends at your home";
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
                response = "This command can only be used for SCP-106!";
                return false;
            }
            var config = Plugin.Instance.Config;
            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (scp106.Vigor < Mathf.Clamp01(config.PocketinCostVigor / 100f) || player.Health <= config.PocketinCostHealt)
            {
                response = "You don't have enough energy or health to carry your henchman!";
                return false;
            }

            if (AlphaWarheadController.Detonated)
            {
                if (scp106.RemainingSinkholeCooldown <= 0f)
                    scp106.IsSubmerged = true;
                response = "You can't go to your pocket dimension after Warhead explodes!";
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

                Player friend = Player.List
                .Where(p => p != player && p.IsScp && Vector3.Distance(p.Position, player.Position) <= 1.5)
                .OrderBy(p => Vector3.Distance(p.Position, player.Position))
                .FirstOrDefault();

                if (friend == null) 
                {
                    response = "There are no SCPs here or you're not close enough.";
                    return false;
                }

                response = "<color=red>You take "+ friend.Role.Type +" and go underground and come out in the pocket dimension...</color>";
                scp106.StalkAbility.IsActive = true;
                friend.Broadcast(Plugin.Instance.Translation.scp106ReqFriendinpocket, shouldClearPrevious: true);
                friend.EnableEffect<Flashed>();
                friend.EnableEffect<Ensnared>();
                EventHandlers.GetPocketScp = friend.Id;
                Timing.CallDelayed(3f, () =>
                {
                    if (EventHandlers.GetScpPerm == true)
                    {
                        friend.DisableEffect<Flashed>();
                        friend.DisableEffect<Ensnared>();
                        scp106.StalkAbility.IsActive = false;
                        player.Broadcast(Plugin.Instance.Translation.scp106friendrefusedlpocketin, true);
                        scp106.Vigor -= Mathf.Clamp01(config.PocketinCostVigor / 200f);
                        EventHandlers.GetScpPerm = false;
                        EventHandlers.GetPocketScp = -1;
                    }
                    else 
                    { 
                    player.EnableEffect<PocketCorroding>();
                    scp106.StalkAbility.IsActive = false;
                    friend.EnableEffect<PocketCorroding>();
                    friend.DisableEffect<Flashed>();
                    friend.DisableEffect<Ensnared>();
                    player.DisableAllEffects();
                    }
                });

                if (EventHandlers.GetScpPerm == true)
                {   
                    Timing.CallDelayed(3.5f, () => scp106.RemainingSinkholeCooldown = (float)config.CanceledPocketingScpCooldown);
                    return false;
                }
                player.Health -= config.PocketinCostHealt;
                scp106.Vigor -= Mathf.Clamp01(config.PocketinCostVigor / 100f);
                player.Broadcast(Plugin.Instance.Translation.scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.Instance.Translation.scp106Friendinpocket, shouldClearPrevious: true);

                Timing.CallDelayed(3.5f, () => scp106.RemainingSinkholeCooldown = (float)config.AfterPocketingScpCooldown);
                return true;
            }
            else
            {
                response = "You can't change dimension that often! Wait a cooldown before changing it again.";
                player.Broadcast(Plugin.Instance.Translation.cooldown, shouldClearPrevious: true);
                return false;
            }
        }
    }
}
