using PlayerRoles;
using CustomRendering;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using PlayerRoles.FirstPersonControl;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Core.UserSettings;


namespace BetterScp106
{
    public class EventHandlers
    {
        public readonly Plugin plugin;

        public static bool SpecialFeatureUsing = false;

        public static int ScpPullingtoPocket;

        public static bool GetScpPerm = false;

        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        public void OnStalk(StalkingEventArgs ev)
        {
            if (EventHandlers.SpecialFeatureUsing)
                ev.IsAllowed = false;
        }

        public void Alt(TogglingNoClipEventArgs ev)
        {
            if (FpcNoclip.IsPermitted(ev.Player.ReferenceHub))
                return;

            if (ev.Player.Id == ScpPullingtoPocket)
            {
                GetScpPerm = true;
                return;
            }
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
            if (EventHandlers.SpecialFeatureUsing)
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

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleTypeId.Scp106)
            {
                SettingBase.SendToPlayer(ev.Player, SettingsMenu.Better106Menu());
                ev.Player.ShowHint(new Hint(plugin.Translation.Scp106StartMessage, 10, true));
            }
        }

        public void Warheadkillinhibitor(HurtingEventArgs ev)
        {
            if (ev.DamageHandler.Type != DamageType.Warhead)
                return;

            if (ev.Player.CurrentRoom.Type != RoomType.Pocket)
                return;

            ev.IsAllowed = false;
            ev.Player.GetEffect<FogControl>()?.SetFogType(FogType.Outside);
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
