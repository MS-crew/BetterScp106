namespace BetterScp106
{   
    using System;
    using HarmonyLib;
    using System.Linq;
    using Exiled.Loader;
    using Exiled.API.Features;
    using UserSettings.ServerSpecific;
    using ServerSpecificSyncer.Features;
    using Scp106 = Exiled.Events.Handlers.Scp106;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    public class Plugin : Plugin<Config, Translation>
    {
        private Harmony harmony;

        public static bool Using = false;
        public static bool Sssisactive = false;
        public static EventHandlers eventHandlers;
        public static Config C => Plugin.Instance?.Config;
        public override string Author => "ZurnaSever";
        public override string Name => "BetterScp106";
        public override string Prefix => "BetterScp106"; 
        public static Plugin Instance { get; private set; }
        public static Translation T => Plugin.Instance?.Translation;
        public override Version Version { get; } = new Version(2, 5, 5);
        public override Version RequiredExiledVersion { get; } = new Version(9, 2, 1);
        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);

            Scp106.Stalking += eventHandlers.OnStalk;
            Scp106.Teleporting += eventHandlers.OnTeleport;
            PlayerHandlers.TogglingNoClip += eventHandlers.Alt;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.Pd;
            PlayerHandlers.ChangingRole += eventHandlers.OnChangingRole;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += Methods.ProcessUserInput;
            
            if (C.OneHitPocket) Scp106.Attacking += eventHandlers.On106Attack;
            if (C.RealisticPocket) PlayerHandlers.Hurting += eventHandlers.Warheadkillinhibitor;

            /*Sssisactive = Loader.Plugins.Any(p => p.Name.Contains("ServerSpecificSyncer")&& p.Config.IsEnabled);
            if (Sssisactive)
            {
                Menu.RegisterAll();
                Log.Debug("ServerSpecificSyncer is present and active, subscribing to the main menu...");
            }*/

            harmony = new Harmony("Better106RandomZoneMode");
            harmony.PatchAll();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Scp106.Stalking -= eventHandlers.OnStalk;
            Scp106.Teleporting -= eventHandlers.OnTeleport;
            PlayerHandlers.TogglingNoClip -= eventHandlers.Alt;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.Pd;
            PlayerHandlers.ChangingRole -= eventHandlers.OnChangingRole;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscape;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= Methods.ProcessUserInput;

            if (C.OneHitPocket) Scp106.Attacking -= eventHandlers.On106Attack;
            if (C.RealisticPocket) PlayerHandlers.Hurting -= eventHandlers.Warheadkillinhibitor;

            Sssisactive = Loader.Plugins.Any(p => p.Name == "ServerSpecificSyncer" && p.Config.IsEnabled);

            if (Sssisactive) 
            {
                Menu.UnregisterAll();
                Log.Debug("ServerSpecificSyncer is present and active,but Better 106 is deactive Unsubscribing to the main menu...");
            }

            harmony.UnpatchAll(harmonyID: "Better106RandomZoneMode");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
