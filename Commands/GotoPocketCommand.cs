// -----------------------------------------------------------------------
// <copyright file="GotoPocketCommand.cs" company="Ms-crew">
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

    /// <summary>
    /// Represents the command that allows SCP-106 to teleport to the pocket dimension.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GotoPocketCommand : ICommand
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Command => Plugin.Instance.Translation.GotoPocketCommand;

        /// <summary>
        /// Gets the aliases for the command.
        /// </summary>
        public string[] Aliases => Plugin.Instance.Translation.GotoPocketCommandAliases;

        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        public string Description => Plugin.Instance.Translation.GotoPocketCommandDescription;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="arguments">The arguments passed to the command.</param>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="response">The response message to be sent back to the sender.</param>
        /// <returns>True if the command executed successfully; otherwise, false.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.PocketFeature)
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

            GotoPocket.PocketFeature(scp106);
            response = "Pocket Ability used";
            return true;
        }
    }
}
