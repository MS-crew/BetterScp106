// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using CustomRendering;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Core.UserSettings;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Scp106;
    using PlayerRoles;

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
        public static int ScpPullingToPocket;

        /// <summary>
        /// Indicates whether SCP permission has been granted.
        /// </summary>
        public static bool GetScpPerm = false;

        /// <summary>
        /// Cache for the Better SCP-106 menu settings.
        /// </summary>
        private static readonly List<SettingBase> Better106MenuCache = SettingsMenu.Better106Menu();

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

            if (ev.Player.Id != ScpPullingToPocket)
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

            ev.Scp106.CapturePlayer(ev.Target);
            Log.Debug($"Scp106 drew the {ev.Target.Nickname} directly into the pocket");
        }

        /// <summary>
        /// Handles the event when a player escapes the pocket dimension.
        /// </summary>
        /// <param name="ev">The event arguments for escaping the pocket dimension.</param>
        public void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (!ev.Player.IsScp)
            {
                return;
            }

            ev.IsAllowed = false;
            Methods.EscapeFromDimension(ev.Player, ev.TeleportPosition);
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
                SettingBase.Unregister(ev.Player, Better106MenuCache);
                Log.Debug($"Player {ev.Player.Nickname} is no longer SCP-106, removing menu from the list.");
            }

            if (ev.Player.Role == RoleTypeId.Scp106)
            {
                SpecialFeatureUsing = false;
                SettingBase.Register(ev.Player, Better106MenuCache);
                ev.Player.ShowHint(new Hint(Plugin.Instance.Translation.Scp106StartMessage, 10, true));
                Log.Debug($"Player {ev.Player.Nickname} is now SCP-106, adding menu to the list.");
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
