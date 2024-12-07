//In Progges
/*using MEC;
using UnityEngine;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using Exiled.API.Features.Roles;
using System.Collections.Generic;

namespace BetterScp106.Features
{
    public class PocketIn
    {
        public static void PocketInFeature(Player player)
        {
            player.Role.Is(out Scp106Role scp106);
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                player.Broadcast(Plugin.T.cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.C.PocketinCostVigor / 100f) || player.Health <= Plugin.C.PocketinCostHealt)
            {
                player.ShowHint("You don't have enough energy or health to carry your henchman!", 3);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                player.Broadcast(Plugin.T.afternuke, shouldClearPrevious: true);
                return;
            }

            Room pocketRoom = Room.Get(RoomType.Pocket);
            if (player.CurrentRoom.Type == RoomType.Pocket)
            {
                player.ShowHint("<color=red>You are already in pocket dimension?</color>", 3);
                return;
            }

            Player friend = Methods.FindFriend(player);

            if (friend == null)
            {
                player.ShowHint("There are no SCPs here or you're not close enough.", 3);
                return;
            }

            Timing.RunCoroutine(GotoPocketInV3(player, friend));
            player.ShowHint("<color=red>You take " + friend.Role.Type + " and go underground and come out in the pocket dimension...</color>", 3);
            return;

        }
        private static IEnumerator<float> GotoPocketInV3(Player player, Player friend)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;

            player.Role.Is(out Scp106Role scp106);
            friend.Broadcast(Plugin.T.scp106ReqFriendinpocket, shouldClearPrevious: true);
            friend.EnableEffect<Flashed>();
            friend.EnableEffect<Ensnared>();
            EventHandlers.GetPocketScp = friend.Id;

            scp106.IsStalking = true;
            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.SubmergeProgress == 1f);

            if (EventHandlers.GetScpPerm == true)
            {
                friend.DisableEffect<Flashed>();
                friend.DisableEffect<Ensnared>();
                player.DisableEffect<Ensnared>();
                player.Broadcast(Plugin.T.scp106friendrefusedlpocketin, true);
                EventHandlers.GetScpPerm = false;
                EventHandlers.GetPocketScp = -1;

                scp106.IsStalking = false;
                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.TargetSubmerged);

                scp106.RemainingSinkholeCooldown = (float)Plugin.C.CanceledPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.C.PocketinCostVigor / 200f);

            }
            else
            {
                player.EnableEffect<PocketCorroding>();
                scp106.IsStalking = false;
                friend.EnableEffect<PocketCorroding>();
                friend.DisableEffect<Ensnared>();
                friend.DisableEffect<Flashed>();
                player.DisableAllEffects();
                player.Broadcast(Plugin.T.scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.T.scp106Friendinpocket, shouldClearPrevious: true);

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.TargetSubmerged);

                scp106.RemainingSinkholeCooldown = (float)Plugin.C.AfterPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.C.PocketinCostVigor / 100f);
                player.Health -= Plugin.C.PocketinCostHealt;
            }
            Better106.Using = false;
        }
    }
}*/
