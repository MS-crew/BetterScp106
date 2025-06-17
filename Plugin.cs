// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using SSMenuSystem.Features;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using Scp106 = Exiled.Events.Handlers.Scp106;
    using Warhead = Exiled.Events.Handlers.Warhead;

    /// <summary>
    /// Represents the main plugin class for BetterScp106.
    /// </summary>
    public class Plugin : Plugin<Config, Translation>
    {
        /// <summary>
        /// Gets or sets the event handlers for the plugin.
        /// </summary>
        public static EventHandlers EventHandlers;

        private Harmony harmony;

        /// <summary>
        /// Gets the singleton instance of the plugin.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public override string Author => "ZurnaSever";

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public override string Name => "BetterScp106-SSmenu";

        /// <summary>
        /// Gets the prefix used for the plugin.
        /// </summary>
        public override string Prefix => "BetterScp106-SSmenu";

        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        public override Version Version { get; } = new Version(2, 6, 4);

        /// <summary>
        /// Gets the required Exiled version for the plugin.
        /// </summary>
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);

        /// <summary>
        /// Called when the plugin is enabled.
        /// </summary>
        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers();

            Scp106.Stalking += EventHandlers.OnStalk;
            Scp106.Attacking += EventHandlers.On106Attack;

            Warhead.Detonated += EventHandlers.OnDetonatedWarhead;

            PlayerHandlers.Spawned += EventHandlers.OnSpawned;
            PlayerHandlers.TogglingNoClip += EventHandlers.OnTogglingNoClip;
            PlayerHandlers.EscapingPocketDimension += EventHandlers.OnEscapingPocketDim;
            PlayerHandlers.FailingEscapePocketDimension += EventHandlers.OnFailingEscape;

            Menu.RegisterAll();

            this.harmony = new Harmony("Better106Patchs" + DateTime.Now.Ticks);
            this.harmony.PatchAll();
            base.OnEnabled();
        }

        /// <summary>
        /// Called when the plugin is disabled.
        /// </summary>
        public override void OnDisabled()
        {
            Scp106.Stalking -= EventHandlers.OnStalk;
            Scp106.Attacking -= EventHandlers.On106Attack;

            Warhead.Detonated -= EventHandlers.OnDetonatedWarhead;

            PlayerHandlers.Spawned -= EventHandlers.OnSpawned;
            PlayerHandlers.TogglingNoClip -= EventHandlers.OnTogglingNoClip;
            PlayerHandlers.EscapingPocketDimension -= EventHandlers.OnEscapingPocketDim;
            PlayerHandlers.FailingEscapePocketDimension -= EventHandlers.OnFailingEscape;

            this.harmony.UnpatchAll(harmonyID: "Better106Patchs");
            EventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
