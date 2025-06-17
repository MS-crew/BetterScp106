// -----------------------------------------------------------------------
// <copyright file="TakeScpsPocketCommand.cs" company="Ms-crew">
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
    /// Represents the command that allows players to interact with SCP-106's pocket dimension.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TakeScpsPocketCommand : ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Command => Plugin.Instance.Translation.TakeScpsPocketCommand;

        /// <summary>
        /// Gets the command aliases.
        /// </summary>
        public string[] Aliases => Plugin.Instance.Translation.TakeScpsPocketCommandAliases;

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => Plugin.Instance.Translation.TakeScpsPocketCommandDescription;

        /// <summary>
        /// Executes the command logic.
        /// </summary>
        /// <param name="arguments">The command arguments.</param>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="response">The response message to be sent back to the sender.</param>
        /// <returns>True if the command executed successfully; otherwise, false.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.PocketinFeature)
            {
                response = "This feature is disabled";
                return false;
            }

            Player player = Player.Get(sender);

            if (player.Role.Is<Scp106Role>(out Scp106Role scp106))
            {
                TakeScpsPocket.PocketInFeature(scp106);
                response = string.Empty;
                return false;
            }

            response = "You can`t use this command";
            return false;
        }
    }
}
