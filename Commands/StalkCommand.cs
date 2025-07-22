// -----------------------------------------------------------------------
// <copyright file="StalkCommand.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Commands
{
    using System;
    using BetterScp106.Features;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Represents the command used by SCP-106 to stalk a target.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class StalkCommand : ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Command => Plugin.Instance.Translation.StalkCommand;

        /// <summary>
        /// Gets the aliases for the command.
        /// </summary>
        public string[] Aliases => Plugin.Instance.Translation.StalkCommandAliases;

        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        public string Description => Plugin.Instance.Translation.StalkCommandDescription;

        /// <summary>
        /// Executes the stalk command.
        /// </summary>
        /// <param name="arguments">The arguments passed to the command.</param>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="response">The response message to be sent back to the sender.</param>
        /// <returns>True if the command executed successfully; otherwise, false.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.StalkFeature)
            {
                response = "This feature is disabled";
                return false;
            }

            Player player = Player.Get(sender);

            if (!player.Role.Is<Scp106Role>(out Scp106Role scp106))
            {
                response = "You can`t use this command";
                return false;
            }

            if (scp106.RemainingSinkholeCooldown > 0)
            {
                player.Broadcast(Plugin.Instance.Translation.Cooldown, true);
                response = Plugin.Instance.Translation.Cooldown.Content;
                return false;
            }

            if (Plugin.EventHandlers.SpecialFeatureUsing || scp106.Vigor < Mathf.Clamp01(Plugin.Instance.Config.StalkCostVigor / 100f) || scp106.Owner.Health <= Plugin.Instance.Config.StalkCostHealt)
            {
                player.Broadcast(Plugin.Instance.Translation.StalkCant, true);
                response = Plugin.Instance.Translation.StalkCant.Content;
                return false;
            }

            Player target = Methods.FindTarget(true, Plugin.Instance.Config.StalkDistance, scp106.Owner);

            if (target == null)
            {
                player.Broadcast(Plugin.Instance.Translation.StalkNoTarget, true);
                response = Plugin.Instance.Translation.StalkNoTarget.Content;
                return false;
            }

            Timing.RunCoroutine(Stalking.Stalk(scp106, target));
            response = "Stalk Ability used";
            return true;
        }
    }
}
