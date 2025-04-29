using MEC;
using UnityEngine;
using PlayerStatsSystem;
using CustomPlayerEffects;
using Exiled.API.Features;
using SSMenuSystem.Features;
using System.Collections.Generic;
using Exiled.API.Features.Roles;
using CommandSystem.Commands.RemoteAdmin;
using UserSettings.ServerSpecific;

namespace BetterScp106.Features
{
    public class Stalking
    {
        public static void StalkFeature(Scp106Role scp106)
        {
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown,true);
                return;
            }

            if (EventHandlers.SpecialFeatureUsing || scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.StalkCostHealt)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkCant, true);
                return;
            }
            bool Stalkmode = scp106.Owner.ReferenceHub.GetParameter<SettingsMenu.ServerSettingsSyncer, SSTwoButtonsSetting>(Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkMode]).SyncIsB;
            int Stalkdistance = scp106.Owner.ReferenceHub.GetParameter<SettingsMenu.ServerSettingsSyncer, SSSliderSetting>(Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkDistanceSlider]).SyncIntValue;
            Player target = Methods.Findtarget(Stalkmode, Stalkdistance, scp106.Owner);

            if (target == null)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkNoTarget, true);
                return;
            }

            Timing.RunCoroutine(StalkV3(scp106, target));
        }
        public static IEnumerator<float> StalkV3(Scp106Role scp106, Player target)
        {
            if (EventHandlers.SpecialFeatureUsing)
                yield break;

            EventHandlers.SpecialFeatureUsing = true;
            EventHandlers.SpecialFeatureCooldown = Plugin.Instance.Config.AfterStalkCooldown;

            if (Plugin.Instance.Config.StalkWarning) 
            {
                yield return Timing.WaitForSeconds(Plugin.Instance.Config.StalkWarningBefore);
                target.Broadcast(Plugin.Instance.Translation.StalkVictimWarn, shouldClearPrevious: true);
            }

            if (!target.IsAlive)
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkFailed, shouldClearPrevious: true);
                EventHandlers.SpecialFeatureUsing = false;
                Log.Debug("Stalk victim die before stalk");
            }

            else
            {
                scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkSuccesfull, shouldClearPrevious: true);
                Log.Debug("Stalk teleport starting");

                Vector3 tp = target.Position;
                if (!Physics.Raycast(target.Position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2))
                   tp = target.CurrentRoom.Position;

                scp106.IsSubmerged = true;
                scp106.Owner.EnableEffect<Ensnared>();

                yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);
                scp106.IsSubmerged = false;
                Log.Debug("SCP-106 is ground'.");

                if (target.Lift == null)
                    scp106.Owner.Teleport(tp);
                else
                    scp106.Owner.Teleport(target.Lift.Position + Vector3.up * ElevatorTeleportCommand.PositionOffset);

                scp106.Owner.DisableEffect<Ensnared>();
                scp106.Vigor -= Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f);
                scp106.Owner.Hurt(new CustomReasonDamageHandler("Using Shadow Realm Forces", Plugin.Instance.Config.StalkCostHealt, null));

                yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
                EventHandlers.SpecialFeatureUsing = false;
            }    
        }
    }
}