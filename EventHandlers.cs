using MEC;
using UnityEngine;
using PlayerRoles;
using MapGeneration;
using CustomRendering;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using System.Collections.Generic;
using Exiled.Events.EventArgs.Scp106;
using PlayerRoles.FirstPersonControl;
using Exiled.Events.EventArgs.Player;
using PlayerRoles.PlayableScps.Scp106;
using Interactables.Interobjects.DoorUtils;

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
            if (ev.Player.Role != RoleTypeId.Scp106)
                return;

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

            Player target = Stalk.Findtarget(ev.Player);

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
        public void OnTeleport(TeleportingEventArgs ev)
        {
            if (Better106.Using)
                ev.IsAllowed = false;
        }
        private void ShowRandomScp106Hint(Player player)
        {
            int randomIndex = new System.Random().Next(0, 3);
            string hint;

            switch (randomIndex)
            {
                case 0:
                    hint = Plugin.Instance.Translation.Scp106PowersPocket.Replace("$pockethealt", Plugin.config.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.config.PocketdimensionCostVigor.ToString());
                    break;
                case 1:
                    hint = Plugin.Instance.Translation.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.config.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.config.PocketinCostVigor.ToString());
                    break;
                case 2:
                    hint = Plugin.Instance.Translation.Scp106PowersStalk.Replace("$stalkhealt", Plugin.config.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.config.StalkCostVigor.ToString());
                    break;
                default:
                    hint = Plugin.Instance.Translation.Scp106StartMessage;
                    break;
            }
            player.ShowHint(hint, 3);
        }
        public void pd(EscapingPocketDimensionEventArgs ev)
        {
            if (plugin.Config.PocketexitRandomZonemode)
            {
                ev.IsAllowed = false;
                EscapeFromDimension(ev.Player);

                if (ev.Player.Role == RoleTypeId.Scp106 && Plugin.config.Reminders)
                    ShowRandomScp106Hint(ev.Player);

                Log.Debug("Random Zone exit mode is active player exiting with random zone");
                return;
            }
            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                ev.IsAllowed = false;
                EscapeFromDimension(ev.Player);

                if (Plugin.Instance.Config.Reminders)
                    ShowRandomScp106Hint(ev.Player);

                Log.Debug("106 escape the pocket dimension finding the right exit(Random Zone mode is Deactive)");
            }
        }
        public static void EscapeFromDimension(Player player)
        {
            ReferenceHub referenceHub = player.ReferenceHub;
            if (referenceHub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                fpcRole.FpcModule.ServerOverridePosition(!Plugin.config.PocketexitRandomZonemode ? Scp106PocketExitFinder.GetBestExitPosition(fpcRole) : GetBestExitPositionRandomZone(fpcRole), Vector3.zero);

                player.DisableEffect<PocketCorroding>();
                player.DisableEffect<Corroding>();

                if (!player.IsScp)
                {
                    player.EnableEffect<Disabled>(10f, true);
                    player.EnableEffect<Traumatized>();
                }

                PocketDimensionGenerator.RandomizeTeleports();
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
        public static Vector3 GetBestExitPositionRandomZone(IFpcRole role)
        {
            List<Vector3> randompos = new List<Vector3>
            {
                Room.Get(RoomType.Surface).Position
            };

            if (!Warhead.IsDetonated)
            {
                if (!Map.IsLczDecontaminated)
                    randompos.Add(Room.Get(RoomType.Lcz914).Position);
                randompos.Add(Room.Get(RoomType.HczArmory).Position);
                randompos.Add(Room.Get(RoomType.EzCafeteria).Position);
            }

            int Randomzone = new System.Random().Next(randompos.Count);
            Vector3 position = randompos[Randomzone];
            RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPositionRaycasts(position);
            if (roomIdentifier == null)
            {
                Log.Warn($"roomIdentifier was null, room is {Room.Get(position)}, roomIdentifier is {roomIdentifier}, position is {position}, seed is{Map.Seed}");
                return position;
            }

            DoorVariant[] whitelistedDoorsForZone = Scp106PocketExitFinder.GetWhitelistedDoorsForZone(roomIdentifier.Zone);
            return whitelistedDoorsForZone.Length != 0 ? Scp106PocketExitFinder.GetSafePositionForDoor(Scp106PocketExitFinder.GetRandomDoor(whitelistedDoorsForZone), roomIdentifier.Zone == FacilityZone.Surface ? 45f : 11f, role.FpcModule.CharController) : position;
        }
        public void OnFailingEscape(FailingEscapePocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
            {
                Log.Debug("Player failing on escape and will be die.");
                return;
            }
            

            ev.IsAllowed = false;
            EscapeFromDimension(ev.Player);

            if (ev.Player.Role.Type == RoleTypeId.Scp106 && Plugin.Instance.Config.Reminders)
                ShowRandomScp106Hint(ev.Player);

            Log.Debug("Escape failed but player is Scp and successful escape will be made");
        }
    }
}
