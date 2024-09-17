using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using System.Linq;
using UnityEngine;

namespace BetterScp106
{
    public class EventHandlers
    {
        private readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp106)
            { 
                ev.Player.ShowHint(new Hint(plugin.Translation.Scp106StartMessage,15,true));
            }
        }
        public void OnFailingEscape(FailingEscapePocketDimensionEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                Log.Debug("Escape failed but player is Scp and successful escape will be made");
                ev.IsAllowed = false;
                EscapeFromDimension(ev.Player);
                if(ev.Player.Role.Type==RoleTypeId.Scp106)
                    ev.Player.ShowHint(plugin.Translation.Scp106StartMessage, 5);
            }
        }
        public void pd(EscapingPocketDimensionEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.Player.ShowHint(plugin.Translation.Scp106StartMessage, 5);
                Log.Debug("106 escape the pocket dimension finding the right exit");
                Timing.CallDelayed(1f, () =>
                {
                    ev.Player.DisableAllEffects();
                });
            }
        }
        public static void EscapeFromDimension(Player player)
        {
            ReferenceHub referenceHub = player.ReferenceHub;
            if (referenceHub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                fpcRole.FpcModule.ServerOverridePosition(Scp106PocketExitFinder.GetBestExitPosition(fpcRole), Vector3.zero);
                player.DisableEffect<PocketCorroding>();
                player.DisableEffect<Corroding>();
                player.DisableEffect<Traumatized>();
                PocketDimensionGenerator.RandomizeTeleports();
            }
        }

    }
}
