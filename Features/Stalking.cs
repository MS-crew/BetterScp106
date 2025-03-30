using MEC;
using UnityEngine;
using CustomPlayerEffects;
using Exiled.API.Features;
using System.Collections.Generic;
using Exiled.API.Features.Roles;

namespace BetterScp106.Features
{
    public class Stalking
    {
        public static void StalkFeature(Player player)
        {
            player.Role.Is(out Scp106Role scp106);

            if (scp106.RemainingSinkholeCooldown > 0)
            {
                player.Broadcast(Plugin.Instance.Translation.Cooldown,true);
                return;
            }

            if (EventHandlers.SpecialFeatureUsing || scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f) || player.Health <= Plugin.Instance.Config.StalkCostHealt)
            {
                player.Broadcast(Plugin.Instance.Translation.StalkCant, true);
                return;
            }

            Player target = Methods.Findtarget(player);

            if (target == null)
            {
                player.Broadcast(Plugin.Instance.Translation.StalkNoTarget, true);
                return;
            }

            Timing.RunCoroutine(StalkV3(player, target));
        }
        public static IEnumerator<float> StalkV3(Player player, Player target)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            EventHandlers.SpecialFeatureUsing = true;

            if (Plugin.Instance.Config.StalkWarning) 
            {
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.StalkWarningBefore);
                target.Broadcast(Plugin.Instance.Translation.StalkVictimWarn, shouldClearPrevious: true);
            }

            player.Role.Is(out Scp106Role scp106);

            if (!target.IsAlive)
            {
                player.Broadcast(Plugin.Instance.Translation.StalkFailed, shouldClearPrevious: true);
                Log.Debug("Stalk victim die before stalk");
            }

            else
            {
                player.Broadcast(Plugin.Instance.Translation.StalkSuccesfull, shouldClearPrevious: true);
                Log.Debug("Stalk teleport starting");

                Vector3 tp = target.Position;
                if (!Physics.Raycast(target.Position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2))
                   tp = target.CurrentRoom.Position;

                scp106.IsSubmerged = true;
                player.EnableEffect<Ensnared>();

                yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

                scp106.Owner.Teleport(tp);
                Log.Debug("SCP-106 is ground'.");

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
                Log.Debug("SCP-106 exiting ground.");

                player.DisableEffect<Ensnared>();
                player.Health -= Plugin.Instance.Config.StalkCostHealt;
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f); 
                scp106.RemainingSinkholeCooldown = (float)Plugin.Instance.Config.AfterStalkCooldown;
            }

            EventHandlers.SpecialFeatureUsing = false;
        }
    }
}