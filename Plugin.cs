using System;
using HarmonyLib;
using Exiled.API.Features;
using Scp106 = Exiled.Events.Handlers.Scp106;
using Warhead = Exiled.Events.Handlers.Warhead;
using PlayerHandlers = Exiled.Events.Handlers.Player;

namespace BetterScp106
{   
    public class Plugin : Plugin<Config, Translation>
    {
        private Harmony harmony;
        
        public static EventHandlers eventHandlers;

        public override string Author => "ZurnaSever";

        public override string Name => "BetterScp106";

        public override string Prefix => "BetterScp106"; 

        public static Plugin Instance {get; private set;}

        public override Version Version { get; } = new Version(2, 6, 3);

        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers();

            Scp106.Stalking += eventHandlers.OnStalk; 
            Scp106.Attacking += eventHandlers.On106Attack;

            Warhead.Detonated += eventHandlers.WarheadFogDisabler;

            PlayerHandlers.Spawned += eventHandlers.OnSpawned;
            PlayerHandlers.TogglingNoClip += eventHandlers.Alt;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.Pd;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscape;

            harmony = new Harmony("Better106Patchs"+ DateTime.Now.Ticks);
            harmony.PatchAll();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Scp106.Stalking -= eventHandlers.OnStalk;
            Scp106.Attacking -= eventHandlers.On106Attack;

            Warhead.Detonated -= eventHandlers.WarheadFogDisabler;

            PlayerHandlers.Spawned -= eventHandlers.OnSpawned;
            PlayerHandlers.TogglingNoClip -= eventHandlers.Alt;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.Pd;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscape;

            harmony.UnpatchAll(harmonyID: "Better106Patchs");
            eventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }    
    }
}
