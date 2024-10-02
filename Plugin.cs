namespace BetterScp106
{
    using Exiled.API.Features;
    using System;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    public class Plugin : Plugin<Config, Translation>
    {
        private EventHandlers eventHandlers;
        public static Plugin Instance { get; private set; }
        public override string Author => "ZurnaSever";
        public override string Name => "BetterScp106";
        public override string Prefix => "BetterScp106";
        public override Version RequiredExiledVersion { get; } = new Version(8, 11, 0);
        public override Version Version { get; } = new Version(1, 5, 7);

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);
            PlayerHandlers.Spawned += eventHandlers.OnSpawned;
            PlayerHandlers.TogglingNoClip += eventHandlers.Alt;
            PlayerHandlers.ChangingMoveState += eventHandlers.Tf;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;
            if (Config.RealisticPocket) PlayerHandlers.Hurting += eventHandlers.Warheadkillinhibitor;
            Log.Debug("BetterScp106 is Active");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            PlayerHandlers.Spawned -= eventHandlers.OnSpawned;
            PlayerHandlers.TogglingNoClip -= eventHandlers.Alt;
            PlayerHandlers.ChangingMoveState -= eventHandlers.Tf;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscape;
            if (Config.RealisticPocket) PlayerHandlers.Hurting -= eventHandlers.Warheadkillinhibitor;
            Log.Debug("BetterScp106 is Deactive");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
