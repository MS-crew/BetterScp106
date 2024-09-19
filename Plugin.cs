
namespace BetterScp106
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using System;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using ServerHandlers = Exiled.Events.Handlers.Server;
    public class Plugin : Plugin<Config, Translation>
    {
        private EventHandlers eventHandlers;
        public static Plugin Instance { get; private set; }

        public override string Author => "ZurnaSever";

        public override string Name => "BetterScp106";

        public override string Prefix => "BetterScp106";

        public override Version RequiredExiledVersion { get; } = new Version(8, 11, 0);

        public override Version Version { get; } = new Version(1, 1, 1);

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);
            PlayerHandlers.Spawned += eventHandlers.OnSpawned;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.pd;
            Log.Debug("BetterScp106 is Active");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            PlayerHandlers.Spawned -= eventHandlers.OnSpawned;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscape;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.pd;
            Log.Debug("BetterScp106 is Deactive");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
