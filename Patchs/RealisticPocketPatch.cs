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
            // Retrieve the original instructions and prepare for modification.
            List<CodeInstruction> newCodes = ListPool<CodeInstruction>.Pool.Get(instructions);

            // Define a label for branching logic.
            Label @continue = generator.DefineLabel();

            // Add the label to the first instruction.
            newCodes[0].labels.Add(@continue);

            // Insert custom logic at the beginning of the instruction list.
            newCodes.InsertRange(
            0,
            new List<CodeInstruction>
            {
                // Check if the realistic pocket mode is enabled and the current room is of type Pocket.
                new (OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Config))),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.RealisticPocket))),
                new (OpCodes.Brfalse_S, @continue),

                new (OpCodes.Ldarg_0),
                new (OpCodes.Call, AccessTools.Method(typeof(Room), nameof(Room.Get), new[] { typeof(Vector3) })),
                new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Room), nameof(Room.Type))),
                new (OpCodes.Ldc_I4_S, (int)RoomType.Pocket),
                new (OpCodes.Ceq),
                new (OpCodes.Brfalse_S, @continue),

                // Return false to prevent detonation.
                new (OpCodes.Ldc_I4_0),
                new (OpCodes.Ret),
            });

            // Yield the modified instructions.
            for (int i = 0; i < newCodes.Count; i++)
            {
                yield return newCodes[i];
            }

            // Return the instruction list to the pool.
            ListPool<CodeInstruction>.Pool.Return(newCodes);
        }
    }
}