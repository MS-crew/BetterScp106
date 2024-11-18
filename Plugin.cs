namespace BetterScp106
{
    using Exiled.API.Features;
    using Exiled.Events.Commands.Reload;
    using System;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    public class Plugin : Plugin<Config, Translation>
    {
        public static EventHandlers eventHandlers;
        public static Plugin Instance { get; private set; }
        public override string Author => "ZurnaSever";
        public override string Name => "BetterScp106";
        public override string Prefix => "BetterScp106";
        public override Version RequiredExiledVersion { get; } = new Version(8, 14, 1);
        public override Version Version { get; } = new Version(1, 5, 9);

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);
            PlayerHandlers.Spawned += eventHandlers.OnSpawned;
            PlayerHandlers.Died += eventHandlers.OnDied;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;
            if (Config.RealisticPocket) PlayerHandlers.Hurting += eventHandlers.Warheadkillinhibitor;
            if (Config.CwithPocket) PlayerHandlers.ChangingMoveState += eventHandlers.Tf;
            if (Config.AltwithStalk) PlayerHandlers.TogglingNoClip += eventHandlers.Alt;
            Log.Debug("BetterScp106 is Active");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            PlayerHandlers.Spawned -= eventHandlers.OnSpawned;
            PlayerHandlers.Died -= eventHandlers.OnDied;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.pd;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscape;
            if (Config.RealisticPocket) PlayerHandlers.Hurting -= eventHandlers.Warheadkillinhibitor;
            if (Config.CwithPocket) PlayerHandlers.ChangingMoveState -= eventHandlers.Tf;
            if (Config.AltwithStalk) PlayerHandlers.TogglingNoClip -= eventHandlers.Alt;
            Log.Debug("BetterScp106 is Deactive");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}
