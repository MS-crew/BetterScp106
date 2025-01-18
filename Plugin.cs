namespace BetterScp106
{   
    using System;
    using HarmonyLib;
    using Exiled.API.Features;
    using UserSettings.ServerSpecific;
    using Scp106 = Exiled.Events.Handlers.Scp106;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    public class Plugin : Plugin<Config, Translation>
    {
        private Harmony harmony;
        public static bool Using = false;
        public static EventHandlers eventHandlers;
        public static Config C => Plugin.Instance?.Config;
        public override string Author => "ZurnaSever";
        public override string Name => "BetterScp106";
        public override string Prefix => "BetterScp106"; 
        public static Plugin Instance { get; private set; }
        public static Translation T => Plugin.Instance?.Translation;
        public override Version Version { get; } = new Version(2, 5, 6);
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

            harmony.UnpatchAll(harmonyID: "Better106RandomZoneMode");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
