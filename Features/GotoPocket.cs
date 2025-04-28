using MEC;
using UnityEngine;
using Exiled.API.Enums;
using PlayerStatsSystem;
using CustomPlayerEffects;
using Exiled.API.Features.Roles;
using System.Collections.Generic;

namespace BetterScp106.Features
{
    public class GotoPocket
    {
        public static void PocketFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.PocketdimensionCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.PocketdimensionCostHealt)
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
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106alreadypocket);
                return;
            }

            Timing.RunCoroutine(GoPocketV3(scp106));
        }

        private static IEnumerator<float> GoPocketV3(Scp106Role scp106)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterPocketdimensionCooldown;

            scp106.IsSubmerged = true;
            scp106.Owner.EnableEffect<Ensnared>();

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden); 

            scp106.Owner.EnableEffect<PocketCorroding>();
            scp106.Owner.DisableAllEffects();
            
            scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketdimensionCostVigor / 100f);
            scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.PocketdimensionCostHealt, null));

            scp106.Owner.Broadcast(Plugin.Instance.Translation.Scp106inpocket, shouldClearPrevious: true);

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}