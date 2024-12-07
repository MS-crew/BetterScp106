using MEC;
using UnityEngine;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using System.Collections.Generic;

namespace BetterScp106.Features
{
    public class Stalking
    {
        public static void StalkFeature(Player player)
        {
            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                player.Broadcast(Plugin.T.cooldown,true);
                return;
            }

            if (Better106.Using || scp106.Vigor < Mathf.Clamp01(Plugin.C.StalkCostVigor / 100f) || player.Health <= Plugin.C.StalkCostHealt)
            {
                player.Broadcast(Plugin.T.StalkCant, true);
                return;
            }

            Player target = Methods.Findtarget(player);

            if (target == null)
            {
                player.Broadcast(Plugin.T.StalkNoTarget, true);
                return;
            }

            Timing.RunCoroutine(StalkV3(player, target));
        }
        public static IEnumerator<float> StalkV3(Player player, Player target)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;

            if (Plugin.C.StalkWarning) 
            {
                yield return Timing.WaitForSeconds(Plugin.C.StalkWarningBefore);
                target.Broadcast(Plugin.T.StalkVictimWarn, shouldClearPrevious: true);
            }

            player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (!target.IsAlive)
            {
                player.Broadcast(Plugin.T.StalkFailed, shouldClearPrevious: true);
                Log.Debug("Stalk victim die before stalk");
            }
            else
            {
                player.Broadcast(Plugin.T.StalkSuccesfull, shouldClearPrevious: true);
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
                player.Health -= Plugin.C.StalkCostHealt;
                scp106.RemainingSinkholeCooldown = (float)Plugin.C.AfterStalkCooldown;
                scp106.Vigor -= Mathf.Clamp01(Plugin.C.StalkCostVigor / 100f);
                Log.Debug("cooldown is added and health and vigor are reduced");
            }

            Better106.Using = false;
        }
    }
}
