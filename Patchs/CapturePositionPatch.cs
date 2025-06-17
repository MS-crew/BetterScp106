// -----------------------------------------------------------------------
// <copyright file="CapturePositionPatch.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using CustomPlayerEffects;
    using Exiled.API.Features.Pools;
    using HarmonyLib;
    using PlayerRoles;

    /// <summary>
    /// This patch replaces the zone of players exiting the pocket dimension with a random zone.
    /// </summary>
    [HarmonyPatch(typeof(PocketCorroding), nameof(PocketCorroding.CapturePosition), MethodType.Getter)]
    public static class CapturePositionPatch
    {
        /// <summary>
        /// Modifies the IL code of the <see cref="PocketCorroding.CapturePosition"/> getter.
        /// Custom logic is injected to check SCP roles and configuration settings before returning a random zone.
        /// </summary>
        /// <param name="instructions">The original IL instructions of the method.</param>
        /// <param name="generator">The IL generator used to create new instructions.</param>
        /// <returns>The modified IL instructions.</returns>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newCodes = ListPool<CodeInstruction>.Pool.Get(instructions);

            Label skip = generator.DefineLabel();

            newCodes[0].labels.Add(skip);

            newCodes.InsertRange(
            0,
            new List<CodeInstruction>
            {
                // this.Hub.IsScp();
                new (OpCodes.Ldarg_0),
                new (OpCodes.Call, AccessTools.PropertyGetter(typeof(StatusEffectBase), nameof(StatusEffectBase.Hub))),
                new (OpCodes.Ldc_I4_1),
                new (OpCodes.Call, AccessTools.Method(typeof(PlayerRolesUtils), nameof(PlayerRolesUtils.IsSCP))),

                // Plugin.Instance.Config.PocketexitRandomZonemode
                new (OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Config))),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.PocketexitRandomZonemode))),

                // if (this.Hub.IsScp() || Plugin.Instance.Config.PocketexitRandomZonemode)
                // return = RandomZone();
                new (OpCodes.Or),
                new (OpCodes.Brfalse_S, skip),

                new (OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RandomZone))),
                new (OpCodes.Ret),
            });

            for (int i = 0; i < newCodes.Count; i++)
            {
                yield return newCodes[i];
            }

            ListPool<CodeInstruction>.Pool.Return(newCodes);
        }
    }
}