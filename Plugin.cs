namespace BetterScp106
{   
    using System;
    using Exiled.API.Features;
    using Scp106 = Exiled.Events.Handlers.Scp106;
    using PlayerHandlers = Exiled.Events.Handlers.Player; 

    public class Plugin : Plugin<Config, Translation>
    {
        public static EventHandlers eventHandlers;
        public static Plugin Instance { get; private set; }
        public override string Author => "ZurnaSever";
        public override string Name => "BetterScp106";
        public override string Prefix => "BetterScp106"; 
        public static Config config => Instance?.Config;
        public override Version Version { get; } = new Version(1, 6, 7);
        public override Version RequiredExiledVersion { get; } = new Version(8, 14, 1);
        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);
            Scp106.Stalking += eventHandlers.OnStalk;
            Scp106.Teleporting += eventHandlers.OnTeleport;
            PlayerHandlers.Spawned += eventHandlers.OnSpawned;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;

            if (Config.OneHitPocket) Scp106.Attacking += eventHandlers.On106Attack;
            if (Config.AltwithStalk) PlayerHandlers.TogglingNoClip += eventHandlers.Alt;
            if (Config.CwithPocket) PlayerHandlers.ChangingMoveState += eventHandlers.Tf;
            if (Config.RealisticPocket) PlayerHandlers.Hurting += eventHandlers.Warheadkillinhibitor;
                        
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

            if (Config.OneHitPocket) Scp106.Attacking -= eventHandlers.On106Attack;
            if (Config.AltwithStalk) PlayerHandlers.TogglingNoClip -= eventHandlers.Alt;
            if (Config.CwithPocket) PlayerHandlers.ChangingMoveState -= eventHandlers.Tf;
            if (Config.RealisticPocket) PlayerHandlers.Hurting -= eventHandlers.Warheadkillinhibitor;

            Log.Debug("BetterScp106 is Deactive");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
