using PlayerRoles;
using CustomRendering;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Events.EventArgs.Player;

namespace BetterScp106
{
    public class EventHandlers
    {

        public static bool SpecialFeatureUsing = false;

        public static float SpecialFeatureCooldown = 0;

        public static int ScpPullingtoPocket;

        public static bool GetScpPerm = false;

        public void OnStalk(StalkingEventArgs ev)
        {
            if (SpecialFeatureUsing)
                ev.IsAllowed = false;

        }

        public void Alt(TogglingNoClipEventArgs ev)
        {           
            if (ev.Player.IsNoclipPermitted)
                return;

            if (ev.Player.Id == ScpPullingtoPocket)
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

        public void Pd(EscapingPocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
                return;

            ev.IsAllowed = false;
            Methods.EscapeFromDimension(ev.Player);
            Log.Debug("Scp exit the dimension with find right exit");
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Type == RoleTypeId.Scp106)
            {
                SpecialFeatureUsing = false;
                ev.Player.ShowHint(new Hint(Plugin.Instance.Translation.Scp106StartMessage, 10, true));
            }
        }

        public void Warheadkillinhibitor(HurtingEventArgs ev)
        {
            if (!Plugin.Instance.Config.RealisticPocket)
                return; 

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
