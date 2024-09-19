using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using System;
using System.Collections.Generic;
using System.Security.Policy;
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
                ev.Player.ShowHint(new Hint(plugin.Translation.Scp106StartMessage, 15, true));
            }
        }
        public void OnFailingEscape(FailingEscapePocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
            {
                Log.Debug("Player failing on escape and will be die.");
                return;
            }
            Log.Debug("Escape failed but player is Scp and successful escape will be made");
            ev.IsAllowed = false;
            EscapeFromDimension(ev.Player);
            if (ev.Player.Role.Type == RoleTypeId.Scp106)
                ev.Player.ShowHint(plugin.Translation.Scp106StartMessage, 5);
        }
        public void pd(EscapingPocketDimensionEventArgs ev)
        {
            if(plugin.Config.PocketexitRandomZonemode && ev.Player.Role.Type!=RoleTypeId.Scp106)
            {
                ev.IsAllowed = false;
                EscapeFromDimension(ev.Player);
                Log.Debug("Random Zone exit mode is active player exiting with random zone");
            }
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.IsAllowed = false;
                EscapeFromDimension(ev.Player);
                ev.Player.ShowHint(plugin.Translation.Scp106StartMessage, 5);
                Log.Debug("106 escape the pocket dimension finding the right exit");
            }
        }
        public static void EscapeFromDimension(Player player)
        {
            ReferenceHub referenceHub = player.ReferenceHub;
            if (referenceHub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                fpcRole.FpcModule.ServerOverridePosition(!Plugin.Instance.Config.PocketexitRandomZonemode ? Scp106PocketExitFinder.GetBestExitPosition(fpcRole) :GetBestExitPositionRandomZone(fpcRole),Vector3.zero);

                player.DisableEffect<PocketCorroding>();
                player.DisableEffect<Corroding>();

                if(!player.IsScp)
                {
                    player.EnableEffect<Disabled>(10f, true);
                    player.EnableEffect<Traumatized>();
                }

                PocketDimensionGenerator.RandomizeTeleports();
            }
        }
        public static Vector3 GetBestExitPositionRandomZone(IFpcRole role)
        {
            if (!NetworkServer.active)
                throw new InvalidOperationException("Scp106PocketExitFinder.GetBestExitPosition is a server-side only method!");
            ReferenceHub hub;
            if (!(role is PlayerRoleBase playerRoleBase) || !playerRoleBase.TryGetOwner(out hub))
                throw new InvalidOperationException("Scp106PocketExitFinder.GetBestExitPosition provided with non-compatible role!");
            List<Vector3> randompos = new List<Vector3>
            {
                Room.Get(RoomType.Surface).Position
            };

            if (!Map.IsLczDecontaminated)
                randompos.Add(Room.Get(RoomType.Lcz914).Position);

            if (!Warhead.IsDetonated)
            {
                randompos.Add(Room.Get(RoomType.HczArmory).Position);
                randompos.Add(Room.Get(RoomType.EzCafeteria).Position);
            }

            int Randomzone = new System.Random().Next(randompos.Count);
            Vector3 position = randompos[Randomzone];
            RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPositionRaycasts(position);
            if ((UnityEngine.Object)roomIdentifier == (UnityEngine.Object)null)
                return position;
            DoorVariant[] whitelistedDoorsForZone = Scp106PocketExitFinder.GetWhitelistedDoorsForZone(roomIdentifier.Zone);
            return whitelistedDoorsForZone.Length != 0 ? Scp106PocketExitFinder.GetSafePositionForDoor(Scp106PocketExitFinder.GetRandomDoor(whitelistedDoorsForZone), roomIdentifier.Zone == FacilityZone.Surface ? 45f : 11f, role.FpcModule.CharController) : position;
        }
    }
}
