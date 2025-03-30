using MEC;
using UnityEngine;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using System.Collections.Generic;

namespace BetterScp106.Features
{
    public class TakeScpsPocket
    {
        public static void PocketInFeature(Player player)
        {
            player.Role.Is(out Scp106Role scp106);
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                player.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f) || player.Health <= Plugin.Instance.Config.PocketinCostHealt)
            {
                player.Broadcast(Plugin.Instance.Translation.Scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                player.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (player.CurrentRoom.Type == RoomType.Pocket)
            {
                player.Broadcast(Plugin.Instance.Translation.Scp106alreadypocket, shouldClearPrevious: true);
                return;
            }

            Player friend = Methods.FindFriend(player);

            if (friend == null)
            {
                player.Broadcast(Plugin.Instance.Translation.Scp106CantFindFriend, shouldClearPrevious: true);
                return;
            }

            Timing.RunCoroutine(GotoPocketInV3(player, friend));
        }
        private static IEnumerator<float> GotoPocketInV3(Player player, Player friend)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            player.Role.Is(out Scp106Role scp106);

            EventHandlers.SpecialFeatureUsing = true;

            friend.Broadcast(Plugin.Instance.Translation.Scp106ReqFriendinpocket, shouldClearPrevious: true);
            friend.EnableEffect<Flashed>();
            friend.EnableEffect<Ensnared>();
            player.EnableEffect<Ensnared>();

            EventHandlers.ScpPullingtoPocket = friend.Id;

            scp106.IsSubmerged = true;

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

            if (EventHandlers.GetScpPerm == true)
            {
                friend.DisableEffect<Flashed>();
                friend.DisableEffect<Ensnared>();
                player.DisableEffect<Ensnared>();
                player.Broadcast(Plugin.Instance.Translation.Scp106friendrefusedlpocketin, true);

                EventHandlers.GetScpPerm = false;
                EventHandlers.ScpPullingtoPocket = -1;

                player.DisableEffect<Ensnared>();
                scp106.RemainingSinkholeCooldown = (float)Plugin.Instance.Config.CanceledPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 200f);
            }

            else
            {
                player.EnableEffect<PocketCorroding>();
                friend.EnableEffect<PocketCorroding>();
                friend.DisableEffect<Ensnared>();
                friend.DisableEffect<Flashed>();

                player.Broadcast(Plugin.Instance.Translation.Scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.Instance.Translation.Scp106Friendinpocket, shouldClearPrevious: true);

                player.DisableAllEffects();
                scp106.RemainingSinkholeCooldown = Plugin.Instance.Config.AfterPocketingScpCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f);
                player.Health -= Plugin.Instance.Config.PocketinCostHealt;
            }

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.SubmergeProgress >= 1);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}