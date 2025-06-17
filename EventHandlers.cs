// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using CustomPlayerEffects;
    using CustomRendering;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Scp106;
    using PlayerRoles;
    using SSMenuSystem.Features;
    using static BetterScp106.SettingsMenu;
    using Log = Exiled.API.Features.Log;

    /// <summary>
    /// Handles various events related to SCP-106 and player interactions.
    /// </summary>
    public class EventHandlers
    {
        /// <summary>
        /// Indicates whether the special feature is currently being used.
        /// </summary>
        public static bool SpecialFeatureUsing = false;

        /// <summary>
        /// Represents the cooldown duration for the special feature.
        /// </summary>
        public static double SpecialFeatureCooldown = 0;

        /// <summary>
        /// Stores the ID of the player being pulled into the pocket dimension.
        /// </summary>
        public static int ScpPullingtoPocket;

        /// <summary>
        /// Indicates whether SCP permission has been granted.
        /// </summary>
        public static bool GetScpPerm = false;

        /// <summary>
        /// Handles the stalking event for SCP-106.
        /// </summary>
        /// <param name="ev">The event arguments for stalking.</param>
        public void OnStalk(StalkingEventArgs ev)
        {
            if (SpecialFeatureUsing)
            {
                ev.IsAllowed = false;
            }
        }

        /// <summary>
        /// Handles the toggling of noclip for players.
        /// </summary>
        /// <param name="ev">The event arguments for toggling noclip.</param>
        public void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (ev.Player.IsNoclipPermitted)
            {
                return;
            }

            if (ev.Player.Id != ScpPullingtoPocket)
            {
                return;
            }

            GetScpPerm = true;
        }

        /// <summary>
        /// Handles SCP-106's attack event.
        /// </summary>
        /// <param name="ev">The event arguments for attacking.</param>
        public void On106Attack(AttackingEventArgs ev)
        {
            if (!Plugin.Instance.Config.OneHitPocket)
            {
                return;
            }

            if (ev.Target.GetEffect<Traumatized>().IsEnabled)
            {
                return;
            }

            ev.Target.EnableEffect<PocketCorroding>();
            Log.Debug($"Scp106 drew the {ev.Target.Nickname} directly into the pocket");
        }

        /// <summary>
        /// Handles the event when a player escapes the pocket dimension.
        /// </summary>
        /// <param name="ev">The event arguments for escaping the pocket dimension.</param>
        public void OnEscapingPocketDim(EscapingPocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
            {
                return;
            }

            ev.IsAllowed = false;
            Methods.EscapeFromDimension(ev.Player);
            Log.Debug("Scp exit the dimension with find right exit");
        }

        /// <summary>
        /// Handles the event when a Scp106 spawn.
        /// </summary>
        /// <param name="ev">The event arguments for spawning.</param>
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.OldRole == RoleTypeId.Scp106)
            {
                Menu.LoadForPlayer(ev.Player.ReferenceHub, null);
            }

            if (ev.Player.Role.Type == RoleTypeId.Scp106)
            {
                SpecialFeatureUsing = false;
                Menu.LoadForPlayer(ev.Player.ReferenceHub, Menu.GetMenu(typeof(ServerSettingsSyncer)));
                ev.Player.ShowHint(new Hint(Plugin.Instance.Translation.Scp106StartMessage, 10, true));
            }
        }

        /// <summary>
        /// Handles the event when the warhead is detonated.
        /// </summary>
        public void OnDetonatedWarhead()
        {
            if (!Plugin.Instance.Config.RealisticPocket)
            {
                return;
            }

            foreach (Player ply in Player.List)
            {
                if (ply.CurrentRoom?.Type != RoomType.Pocket)
                {
                    continue;
                }

                ply.GetEffect<FogControl>()?.SetFogType(FogType.Outside);
            }
        }

        /// <summary>
        /// Handles the event when a player fails to escape the pocket dimension.
        /// </summary>
        /// <param name="ev">The event arguments for failing to escape the pocket dimension.</param>
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
