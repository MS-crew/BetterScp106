using MEC;
using UnityEngine;
using PlayerRoles;
using CustomRendering;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using Exiled.Events.EventArgs.Scp106;
using PlayerRoles.FirstPersonControl;
using Exiled.Events.EventArgs.Player;

namespace BetterScp106
{
    public class EventHandlers
    {
        private readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        public static int GetPocketScp;

        public static bool GetScpPerm = false;
        public void OnStalk(StalkingEventArgs ev)
        {
            if (Better106.Using)
                ev.IsAllowed = false;
        }
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp106)
                ev.Player.ShowHint(new Hint(plugin.Translation.Scp106StartMessage, 10, true));   
        } 
        public void Alt(TogglingNoClipEventArgs ev)
        {
            if (FpcNoclip.IsPermitted(ev.Player.ReferenceHub))
                return;

            if (ev.Player.Id == GetPocketScp)
            {
                GetScpPerm = true;
                return;
            }

            if (!Plugin.config.StalkFeature)
                return;

            if (!Plugin.config.AltwithStalk)
                return;

            if (ev.Player.Role.Type != RoleTypeId.Scp106)
                return;

            ev.Player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);

            if (scp106.Vigor < Mathf.Clamp01(Plugin.config.StalkCostVigor / 100f) || ev.Player.Health <= Plugin.config.StalkCostHealt || scp106.RemainingSinkholeCooldown > 0)
            {
                ev.Player.Broadcast(Plugin.Instance.Translation.StalkCant);
                return;
            }

            Player target = Methods.Findtarget(ev.Player);

            if (target == null)
            {
                ev.Player.Broadcast(Plugin.Instance.Translation.StalkNoTarget);
                return;
            }

            Timing.RunCoroutine(Stalk.StalkV2(ev.Player, target));
        }
        public void Tf(ChangingMoveStateEventArgs ev)
        {
            if (ev.Player.Role.Type != RoleTypeId.Scp106)
                return;

            if (ev.NewState != PlayerMovementState.Sneaking)
                return;

            ev.Player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);

            if (AlphaWarheadController.Detonated)
            {
                if (scp106.RemainingSinkholeCooldown <= 0f)
                {
                    scp106.IsSubmerged = true;
                }
                ev.Player.Broadcast(Plugin.Instance.Translation.afternuke, shouldClearPrevious: true);
                return;
            }

            if (scp106.RemainingSinkholeCooldown > 0)
            {
                ev.Player.Broadcast(Plugin.Instance.Translation.cooldown, shouldClearPrevious: true);
                return;
            }

            Room pocketRoom = Room.Get(RoomType.Pocket);
            if (ev.Player.CurrentRoom.Type == RoomType.Pocket)
            {
                ev.Player.Broadcast(plugin.Translation.scp106alreadypocket);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.config.PocketdimensionCostVigor / 100f) || ev.Player.Health <= Plugin.config.PocketdimensionCostHealt)
            {
                ev.Player.Broadcast(plugin.Translation.scp106cantpocket);
                return;
            }

            Timing.RunCoroutine(PocketDimension.GoPocketV2(ev.Player));
        }
        public void On106Attack(AttackingEventArgs ev)
        {
            if (ev.Target.GetEffect<Traumatized>().IsEnabled)
                return;

            ev.Target.EnableEffect<PocketCorroding>();
            Log.Debug($"Scp106 drew the {ev.Target.Nickname} directly into the pocket");
        }
        public void OnTeleport(TeleportingEventArgs ev)
        {
            if (Better106.Using)
                ev.IsAllowed = false;
        }
        public void pd(EscapingPocketDimensionEventArgs ev)
        {
            if (plugin.Config.PocketexitRandomZonemode)
            {
                ev.IsAllowed = false;
                Methods.EscapeFromDimension(ev.Player);

                if (ev.Player.Role == RoleTypeId.Scp106 && Plugin.config.Reminders)
                    Methods.ShowRandomScp106Hint(ev.Player);

                Log.Debug("Random Zone exit mode is active player exiting with random zone");
                return;
            }
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.IsAllowed = false;
                Methods.EscapeFromDimension(ev.Player);

                if (Plugin.Instance.Config.Reminders)
                    Methods.ShowRandomScp106Hint(ev.Player);

                Log.Debug("106 escape the pocket dimension finding the right exit(Random Zone mode is Deactive)");
            }
        }
        public void Warheadkillinhibitor(HurtingEventArgs ev)
        {
            if (ev.DamageHandler.Type != DamageType.Warhead)
                return;

            if (ev.Player.CurrentRoom.Type == RoomType.Pocket)
            {
                ev.IsAllowed = false;
                FogControl fogControl = ev.Player.ReferenceHub.playerEffectsController.GetEffect<FogControl>();
                fogControl?.SetFogType(FogType.Outside);
            }
        } 
        public void OnFailingEscape(FailingEscapePocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
            {
                Log.Debug("Player failing on escape and will be die.");
                return;
            }
            
            ev.IsAllowed = false;
            Methods.EscapeFromDimension(ev.Player);

            if (ev.Player.Role.Type == RoleTypeId.Scp106 && Plugin.Instance.Config.Reminders)
                Methods.ShowRandomScp106Hint(ev.Player);

            Log.Debug("Escape failed but player is Scp and successful escape will be made");
        }
    }
}
