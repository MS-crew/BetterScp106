namespace BetterScp106
{   
    using System;
    using Exiled.API.Features;
    using Scp106 = Exiled.Events.Handlers.Scp106;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using HarmonyLib;

    public class Plugin : Plugin<Config, Translation>
    {
        private Harmony harmony;
        public static Config C => Instance?.Config;
        public static Translation T => Instance?.Translation;

        public static EventHandlers eventHandlers;
        public static Plugin Instance { get; private set; }
        public override string Author => "ZurnaSever";
        public override string Name => "BetterScp106";
        public override string Prefix => "BetterScp106"; 
        public override Version Version { get; } = new Version(2, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(9, 0, 0);
        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);

            Scp106.Stalking += eventHandlers.OnStalk;
            Scp106.Teleporting += eventHandlers.OnTeleport;
            PlayerHandlers.Spawned += eventHandlers.OnSpawned;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;

            if (C.OneHitPocket) Scp106.Attacking += eventHandlers.On106Attack;
            if (C.AltwithStalk) PlayerHandlers.TogglingNoClip += eventHandlers.Alt;
            if (C.CwithPocket) PlayerHandlers.ChangingMoveState += eventHandlers.Tf;
            if (C.RealisticPocket) PlayerHandlers.Hurting += eventHandlers.Warheadkillinhibitor;

            harmony = new Harmony("Better106RandomZoneMode");
            harmony.PatchAll();          

            Log.Debug("BetterScp106 is Active");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Scp106.Stalking -= eventHandlers.OnStalk;
            Scp106.Teleporting -= eventHandlers.OnTeleport;
            PlayerHandlers.Spawned -= eventHandlers.OnSpawned;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscape;

            if (C.OneHitPocket) Scp106.Attacking -= eventHandlers.On106Attack;
            if (C.AltwithStalk) PlayerHandlers.TogglingNoClip -= eventHandlers.Alt;
            if (C.CwithPocket) PlayerHandlers.ChangingMoveState -= eventHandlers.Tf;
            if (C.RealisticPocket) PlayerHandlers.Hurting -= eventHandlers.Warheadkillinhibitor;

            harmony.UnpatchAll(harmonyID: "Better106RandomZoneMode");

            Log.Debug("BetterScp106 is Deactive");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
