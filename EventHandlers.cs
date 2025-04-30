using PlayerRoles;
using CustomRendering;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Core.UserSettings;

namespace BetterScp106
{
    public class EventHandlers
    {

        public static bool SpecialFeatureUsing = false;

        public static double SpecialFeatureCooldown = 0;

        public static int ScpPullingtoPocket;

        public static bool GetScpPerm = false;

        public void OnStalk(StalkingEventArgs ev)
        {
            if (SpecialFeatureUsing)
                ev.IsAllowed = false;
        }

        public void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {           
            if (ev.Player.IsNoclipPermitted)
                return;

            if (ev.Player.Id != ScpPullingtoPocket)
                return;

            GetScpPerm = true;
        }

        public void On106Attack(AttackingEventArgs ev)
        {
            if (!Plugin.Instance.Config.OneHitPocket)
                return;

            if (ev.Target.GetEffect<Traumatized>().IsEnabled)
                return;

            ev.Target.EnableEffect<PocketCorroding>();
            Log.Debug($"Scp106 drew the {ev.Target.Nickname} directly into the pocket");
        }

        public void OnEscapingPocketDim(EscapingPocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
                return;

            ev.IsAllowed = false;
            Methods.EscapeFromDimension(ev.Player);
            Log.Debug("Scp exit the dimension with find right exit");
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                SpecialFeatureUsing = false;
                SettingBase.Register(SettingsMenu.Better106Menu(), Player => Player == ev.Player);
                SettingBase.SendToPlayer(ev.Player, SettingsMenu.Better106Menu());
                ev.Player.ShowHint(new Hint(Plugin.Instance.Translation.Scp106StartMessage, 10, true));
            }
        }

        public void OnDetonatedWarhead()
        {
            if (!Plugin.Instance.Config.RealisticPocket)
                return;

            foreach (Player ply in Player.List)
                if (ply.CurrentRoom?.Type == RoomType.Pocket)
                    ply.GetEffect<FogControl>()?.SetFogType(FogType.Outside);
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
