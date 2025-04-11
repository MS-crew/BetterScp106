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
    public class GotoPocket
    {
        public static void PocketFeature(Player sender)
        {
            sender.Role.Is(out Scp106Role scp106);

            if (scp106.RemainingSinkholeCooldown > 0)
            {
                sender.Broadcast(Plugin.Instance.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.PocketdimensionCostVigor / 100f) || sender.Health <= Plugin.Instance.Config.PocketdimensionCostHealt)
            {
                sender.Broadcast(Plugin.Instance.Translation.Scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                sender.Broadcast(Plugin.Instance.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (sender.CurrentRoom.Type == RoomType.Pocket)
            {
                sender.Broadcast(Plugin.Instance.Translation.Scp106alreadypocket);
                return;
            }

            Timing.RunCoroutine(GoPocketV3(sender));
        }

        private static IEnumerator<float> GoPocketV3(Player player)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterPocketdimensionCooldown;

            player.Role.Is(out Scp106Role scp106);

            scp106.IsSubmerged = true;
            player.EnableEffect<Ensnared>();

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

            player.EnableEffect<PocketCorroding>();
            player.DisableAllEffects();

            scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.PocketdimensionCostVigor / 100f);
            scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.PocketdimensionCostHealt, null));

            player.Broadcast(Plugin.Instance.Translation.Scp106inpocket, shouldClearPrevious: true);

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}