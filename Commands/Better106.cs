using System;
using System.Text;
using PlayerRoles;
using CommandSystem;
using Exiled.API.Features;
using NorthwoodLib.Pools;

namespace BetterScp106.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Better106 : ICommand
    {
        public string Command => "Better106";
        public string[] Aliases { get; } = { "106" };
        public string Description => "Features of 106";

        public static bool Using = false;
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "This command must be executed in-game.";
                return false;
            }

            if (player.Role != RoleTypeId.Scp106)
            {
                response = "This command only for Scp-106";
                return false;
            }

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Better Scp-106:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersPocket.Replace("$pockethealt", Plugin.C.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.C.PocketdimensionCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.C.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.C.PocketinCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersStalk.Replace("$stalkhealt", Plugin.C.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.C.StalkCostVigor.ToString()));
            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            return true;
        }
    }
}
