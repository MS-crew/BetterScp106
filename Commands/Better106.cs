﻿using CommandSystem;
using Exiled.API.Features;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using System;
using System.Text;
using UnityEngine;

namespace BetterScp106.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Better106 : ICommand
    {
        public string Command => "Better106";

        public string[] Aliases { get; } = { "106" };

        public string Description => "Features of 106";

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
            var config = Plugin.Instance.Config;
            var trans = Plugin.Instance.Translation;
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Better Scp-106:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(trans.Scp106PowersPocket.Replace("$pockethealt", config.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", config.PocketdimensionCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(trans.Scp106PowersPocketin.Replace("$pocketinhealt", config.PocketinCostHealt.ToString()).Replace("$pocketinvigor", config.PocketinCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(trans.Scp106PowersStalk.Replace("$stalkhealt", config.StalkCostHealt.ToString()).Replace("$stalkvigor", config.StalkCostVigor.ToString()));
            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            return true;
        }
    }
}
