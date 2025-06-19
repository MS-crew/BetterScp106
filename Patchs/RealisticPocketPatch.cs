// -----------------------------------------------------------------------
// <copyright file="RealisticPocketPatch.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Patchs
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Pools;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// A patch for modifying the behavior of the <see cref="AlphaWarheadController.CanBeDetonated"/> method.
    /// This patch is used to prevent the realistic pocket feature that comes with the add-on from harming players in pockket.
    /// </summary>
    [HarmonyPatch(typeof(AlphaWarheadController), nameof(AlphaWarheadController.CanBeDetonated))]
    public static class RealisticPocketPatch
    {
        /// <summary>
        /// Transpiler method to inject custom logic into the Alpha Warhead detonation process.
        /// </summary>
        /// <param name="instructions">The original IL instructions.</param>
        /// <param name="generator">The IL generator for creating new instructions.</param>
        /// <returns>The modified IL instructions.</returns>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newCodes = ListPool<CodeInstruction>.Pool.Get(instructions);

            Label @continue = generator.DefineLabel();

            newCodes[0].labels.Add(@continue);

            newCodes.InsertRange(
            0,
            new List<CodeInstruction>
            {
                // if (!Plugin.Instance.Config.RealisticPocket)
                // continue base method
                new (OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Config))),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.RealisticPocket))),
                new (OpCodes.Brfalse_S, @continue),

                // if (Room.Get(this.pos).Type == RoomType.Pocket)
                // return false;
                // else
                // continue base method
                new (OpCodes.Ldarg_0),
                new (OpCodes.Call, AccessTools.Method(typeof(Room), nameof(Room.Get), new[] { typeof(Vector3) })),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Room), nameof(Room.Type))),
                new (OpCodes.Ldc_I4_S, (int)RoomType.Pocket),
                new (OpCodes.Ceq),
                new (OpCodes.Brfalse_S, @continue),

                new (OpCodes.Ldc_I4_0),
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