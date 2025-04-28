using MEC;
using UnityEngine;
using Exiled.API.Enums;
using PlayerStatsSystem;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using System.Collections.Generic;

namespace BetterScp106.Features
{
    public class TakeScpsPocket
    {
        public static void PocketInFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.PocketinCostHealt)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (scp106.Owner.CurrentRoom.Type == RoomType.Pocket)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106alreadypocket, shouldClearPrevious: true);
                return;
            }

            Player friend = Methods.FindFriend(scp106.Owner);

            if (friend == null)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106CantFindFriend, shouldClearPrevious: true);
                return;
            }

            Timing.RunCoroutine(GotoPocketInV3(scp106, friend));
        }
        private static IEnumerator<float> GotoPocketInV3(Scp106Role scp106, Player friend)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterPocketingScpCooldown;

            friend.Broadcast(Plugin.Instance.Translation.Scp106ReqFriendinpocket, shouldClearPrevious: true);
            friend.EnableEffect<Flashed>();
            friend.EnableEffect<Ensnared>();
            scp106.Owner.EnableEffect<Ensnared>();

            EventHandlers.ScpPullingtoPocket = friend.Id;

            scp106.IsSubmerged = true;

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

            if (EventHandlers.GetScpPerm == true)
            {
                EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.CanceledPocketingScpCooldown;

                friend.DisableEffect<Flashed>();
                friend.DisableEffect<Ensnared>();
                scp106.Owner.DisableEffect<Ensnared>();
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106friendrefusedlpocketin, true);

                EventHandlers.GetScpPerm = false;
                EventHandlers.ScpPullingtoPocket = -1;

                scp106.Owner.DisableEffect<Ensnared>();
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 200f);
            }

            else
            {
                scp106.Owner.EnableEffect<PocketCorroding>();
                friend.EnableEffect<PocketCorroding>();

                scp106.Owner.DisableAllEffects();

                friend.DisableEffect<Ensnared>();
                friend.DisableEffect<Flashed>();

                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106inpocket, shouldClearPrevious: true);
                friend.Broadcast(Plugin.Instance.Translation.Scp106Friendinpocket, shouldClearPrevious: true);

                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketinCostVigor / 100f);
                scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.PocketinCostHealt, null));
            }

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}