using MEC;
using System;
using UnityEngine;
using CommandSystem;
using Exiled.API.Features;
using BetterScp106.Features;
using Exiled.API.Features.Roles;

namespace BetterScp106.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class StalkCommand : ICommand
    {
        public string Command => Plugin.Instance.Translation.StalkCommand;
        public string[] Aliases => Plugin.Instance.Translation.StalkCommandAliases;
        public string Description => Plugin.Instance.Translation.StalkCommandDescription;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.StalkFeature)
            {
                response = "This feature is disabled";
                return false;
            }

            Player player = Player.Get(sender);

            if (player.Role.Is<Scp106Role>(out Scp106Role scp106))
            {
                response = string.Empty;
                if (scp106.RemainingSinkholeCooldown > 0)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.Cooldown, true);
                    return false;
                }

                if (EventHandlers.SpecialFeatureUsing || scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.StalkCostHealt)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkCant, true);
                    return false;
                }

                Player target = Methods.Findtarget(true, Plugin.Instance.Config.StalkDistance, scp106.Owner);

                if (target == null)
                {
                    scp106.Owner.Broadcast(Plugin.Instance.Translation.StalkNoTarget, true);
                    return false;
                }

                Timing.RunCoroutine(Stalking.StalkV3(scp106, target));
                return true;
            }

            response = "You can`t use this command";
            return false;
        }
    }
}
