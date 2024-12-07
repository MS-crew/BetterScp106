using MEC;
using UnityEngine;
using PlayerRoles;
using CustomRendering;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using UserSettings.ServerSpecific;
using Exiled.Events.EventArgs.Scp106;
using PlayerRoles.FirstPersonControl;
using Exiled.Events.EventArgs.Player;



namespace BetterScp106
{
    public class EventHandlers
    {
        public readonly Plugin plugin;

        public static int GetPocketScp;

        public static bool GetScpPerm = false;
        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
        }
        public void OnStalk(StalkingEventArgs ev)
        {
            if (Better106.Using)
                ev.IsAllowed = false;
        }
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role != RoleTypeId.Scp106)
                return;

          /*ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub, SettingHandlers.Better106Menu());
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += Methods.ProcessUserInput;*/
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

            if (ev.Player.Role.Type != RoleTypeId.Scp106)
                return;

            if (!plugin.Config.StalkFeature || !plugin.Config.AltwithStalk)
                return;

            ev.Player.Role.Is(out Exiled.API.Features.Roles.Scp106Role scp106);
            if (scp106.Vigor < Mathf.Clamp01(plugin.Config.StalkCostVigor / 100f) || ev.Player.Health <= plugin.Config.StalkCostHealt || scp106.RemainingSinkholeCooldown > 0)
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
            if (scp106.RemainingSinkholeCooldown > 0f)
            {
                ev.Player.Broadcast(plugin.Translation.Cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(plugin.Config.PocketdimensionCostVigor / 100f) || ev.Player.Health <= plugin.Config.PocketdimensionCostHealt)
            {
                ev.Player.Broadcast(plugin.Translation.Scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                ev.Player.Broadcast(plugin.Translation.Afternuke, shouldClearPrevious: true);
                return;
            }

            if (ev.Player.CurrentRoom.Type == RoomType.Pocket)
            {
                ev.Player.Broadcast(plugin.Translation.Scp106alreadypocket);
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
        public void Pd(EscapingPocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
                return;

            ev.IsAllowed = false;
            Methods.EscapeFromDimension(ev.Player);
            Log.Debug("Scp exit the dimension with find right exit");
        }
        public void Warheadkillinhibitor(HurtingEventArgs ev)
        {
            if (ev.DamageHandler.Type != DamageType.Warhead)
                return;

            if (ev.Player.CurrentRoom.Type != RoomType.Pocket)
                return;

            ev.IsAllowed = false;
            FogControl fogControl = ev.Player.ReferenceHub.playerEffectsController.GetEffect<FogControl>();
            fogControl?.SetFogType(FogType.Outside);
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
            Log.Debug("Escape failed but player is Scp and successful escape will be made");
        }
    }
}
