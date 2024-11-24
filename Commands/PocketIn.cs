using MEC;
using System;
using System.Linq;
using PlayerRoles;
using UnityEngine;
using CommandSystem;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using System.Collections.Generic;

namespace BetterScp106.Commands
{
    public class PocketIn : ICommand
    {
        public string Command { get; } = "in";

        public string[] Aliases { get; } = { "i" };

        public string Description { get; } = "Host your friends at your home";
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

            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (scp106.Vigor < Mathf.Clamp01(Plugin.config.PocketinCostVigor / 100f) || player.Health <= Plugin.config.PocketinCostHealt)
            {
                response = "You don't have enough energy or health to carry your henchman!";
                return false;
            }

            if (AlphaWarheadController.Detonated)
            {
                if (scp106.RemainingSinkholeCooldown <= 0f)
                    scp106.IsSubmerged = true;
                
                player.Broadcast(Plugin.Instance.Translation.afternuke, shouldClearPrevious: true);
                response = "You can't go to your pocket dimension after Warhead explodes!";
                return false;
            }


            if (scp106.RemainingSinkholeCooldown > 0)
            {  
                player.Broadcast(Plugin.Instance.Translation.cooldown, shouldClearPrevious: true);
                response = "You can't change dimension that often! Wait a cooldown before changing it again.";
                return false;
            }

            Room pocketRoom = Room.Get(RoomType.Pocket);
            if (player.CurrentRoom.Type == RoomType.Pocket)
            {
                response = "<color=red>You are already in pocket dimension?</color>";
                return false;
            }
            #nullable enable
            Player? friend = Methods.FindFriend(player);
            #nullable disable

            if (friend == null)
            {
                response = "There are no SCPs here or you're not close enough.";
                return false;
            }

            Timing.RunCoroutine(GotoPocketInV2(player, friend));
            response = "<color=red>You take " + friend.Role.Type + " and go underground and come out in the pocket dimension...</color>";
            return true;

        }
        public static IEnumerator<float> GotoPocketInV2(Player player, Player friend)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;

            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            friend.Broadcast(Plugin.Instance.Translation.scp106ReqFriendinpocket, shouldClearPrevious: true);
            friend.EnableEffect<Flashed>();
            friend.EnableEffect<Ensnared>();
            EventHandlers.GetPocketScp = friend.Id;

            scp106.HuntersAtlasAbility.SetSubmerged(true);
            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.NormalizedState == 1.0f);

            if (EventHandlers.GetScpPerm == true)
            {
                friend.DisableEffect<Flashed>();
                friend.DisableEffect<Ensnared>();
                player.Broadcast(Plugin.Instance.Translation.scp106friendrefusedlpocketin, true);
                EventHandlers.GetScpPerm = false;
                EventHandlers.GetPocketScp = -1;

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.NormalizedState == 1.0f);

                scp106.RemainingSinkholeCooldown = (float)Plugin.config.CanceledPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.config.PocketinCostVigor / 200f);
                
            }
            else
            {
                player.EnableEffect<PocketCorroding>();
                friend.EnableEffect<PocketCorroding>();
                friend.DisableEffect<Ensnared>();
                friend.DisableEffect<Flashed>();
                player.DisableAllEffects();
                player.Broadcast(Plugin.Instance.Translation.scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.Instance.Translation.scp106Friendinpocket, shouldClearPrevious: true);

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.NormalizedState == 1.0f);

                scp106.RemainingSinkholeCooldown = (float)Plugin.Instance.Config.AfterPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f);
                player.Health -= Plugin.Instance.Config.PocketinCostHealt;     
            }
            Better106.Using = false;
        }
    }
}
