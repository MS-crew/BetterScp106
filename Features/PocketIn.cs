using MEC;
using UnityEngine;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
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
                player.Broadcast(Plugin.T.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.C.PocketinCostVigor / 100f) || player.Health <= Plugin.C.PocketinCostHealt)
            {
                player.Broadcast(Plugin.T.Scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                player.Broadcast(Plugin.T.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (player.CurrentRoom.Type == RoomType.Pocket)
            {
                player.Broadcast(Plugin.T.Scp106alreadypocket, shouldClearPrevious: true);
                return;
            }

            Player friend = Methods.FindFriend(player);

            if (friend == null)
            {
                player.Broadcast(Plugin.T.Scp106CantFindFriend, shouldClearPrevious: true);
                return;
            }

            Timing.RunCoroutine(GotoPocketInV3(player, friend));
        }
        private static IEnumerator<float> GotoPocketInV3(Player player, Player friend)
        {
            if (Plugin.Using)
                yield break;

            Plugin.Using = true;

            player.Role.Is(out Scp106Role scp106);
            friend.Broadcast(Plugin.T.Scp106ReqFriendinpocket, shouldClearPrevious: true);
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
                player.Broadcast(Plugin.T.Scp106friendrefusedlpocketin, true);
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
                player.Broadcast(Plugin.T.Scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.T.Scp106Friendinpocket, shouldClearPrevious: true);

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.TargetSubmerged);

                scp106.RemainingSinkholeCooldown = (float)Plugin.C.AfterPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.C.PocketinCostVigor / 100f);
                player.Health -= Plugin.C.PocketinCostHealt;
            }
            Plugin.Using = false;
        }
    }
}