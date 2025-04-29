using System;
using CommandSystem;
using Exiled.API.Features;
using BetterScp106.Features;
using Exiled.API.Features.Roles;

namespace BetterScp106.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class GotoPocketCommand : ICommand
    {
        public string Command => Plugin.Instance.Translation.GotoPocketCommand;
        public string[] Aliases => Plugin.Instance.Translation.GotoPocketCommandAliases;
        public string Description => Plugin.Instance.Translation.GotoPocketCommandDescription;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.PocketFeature)
            {
                response = "This feature is disabled";
                return false;
            }

            Player player = Player.Get(sender);

            if (player.Role.Is<Scp106Role>(out Scp106Role scp106))
            {
                GotoPocket.PocketFeature(scp106);
                response = string.Empty;
                return false;
            }

            response = "You can`t use this command";
            return false;
        }
    }
}
