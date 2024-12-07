﻿//In Progges
/*using MEC;
using UnityEngine;
using Exiled.API.Enums;
using CustomPlayerEffects;
using Exiled.API.Features;
using BetterScp106.Commands;
using Exiled.API.Features.Roles;
using System.Collections.Generic;

namespace BetterScp106.Features
{
    public class Pocket
    {
        public static void PocketFeature(Player sender)
        {
            sender.Role.Is(out Scp106Role scp106);
            if (scp106.RemainingSinkholeCooldown > 0)
            {
                sender.Broadcast(Plugin.T.cooldown, shouldClearPrevious: true);
                return;
            }

            if (scp106.Vigor < Mathf.Clamp01(Plugin.C.PocketdimensionCostVigor / 100f) || sender.Health <= Plugin.C.PocketdimensionCostHealt)
            {
                sender.Broadcast(Plugin.T.scp106cantpocket);
                return;
            }

            if (AlphaWarheadController.Detonated)
            {
                scp106.IsSubmerged = true;
                sender.Broadcast(Plugin.T.afternuke, shouldClearPrevious: true);
                return;
            }

            Room pocketRoom = Room.Get(RoomType.Pocket);
            if (sender.CurrentRoom.Type == RoomType.Pocket)
            {
                sender.Broadcast(Plugin.T.scp106alreadypocket);
                return;
            }
            Timing.RunCoroutine(GoPocketV3(sender));
        }
        private static IEnumerator<float> GoPocketV3(Player player)
        {
            if (Better106.Using)
                yield break;

            Better106.Using = true;
            var scp106 = player.Role as Scp106Role;

            scp106.IsStalking = true;

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.SubmergeProgress == 1f);

            player.EnableEffect<PocketCorroding>();
            scp106.IsStalking = false;

            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.TargetSubmerged);

            player.DisableAllEffects();
            scp106.RemainingSinkholeCooldown = Plugin.C.AfterPocketdimensionCooldown;
            player.Health -= Plugin.C.PocketdimensionCostHealt;
            scp106.Vigor -= Mathf.Clamp01(Plugin.C.PocketdimensionCostVigor / 100f);
            player.Broadcast(Plugin.T.scp106inpocket, shouldClearPrevious: true);
            Better106.Using = false;
        }
    }
}*/