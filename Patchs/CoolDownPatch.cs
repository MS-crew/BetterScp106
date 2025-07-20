// -----------------------------------------------------------------------
// <copyright file="CoolDownPatch.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106.Patchs
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.API.Features.Pools;
    using HarmonyLib;
    using PlayerRoles.PlayableScps.Scp106;
    using PlayerRoles.Subroutines;

    /// <summary>
    /// A patch for modifying the behavior of the <see cref="Scp106SinkholeController.ServerSetSubmerged"/> method.
    /// This patch is used to adjust the cooldowns of the abilities that come with the plugin.
    /// </summary>
    [HarmonyPatch(typeof(Scp106SinkholeController), nameof(Scp106SinkholeController.ServerSetSubmerged))]
    public static class CoolDownPatch
    {
        /// <summary>
        /// Transpiler method to inject custom logic into the IL code of the target method.
        /// </summary>
        /// <param name="instructions">The original IL instructions of the target method.</param>
        /// <param name="generator">The IL generator used to create new instructions.</param>
        /// <returns>The modified IL instructions.</returns>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newCodes = ListPool<CodeInstruction>.Pool.Get(instructions);

            Label skip = generator.DefineLabel();
            Label @continue = generator.DefineLabel();

            int index = newCodes.FindIndex(code => code.opcode == OpCodes.Callvirt && (object)code.operand == AccessTools.Method(typeof(AbilityCooldown), nameof(AbilityCooldown.Trigger)));

            newCodes[index].labels.Add(skip);
            newCodes[index - 1].labels.Add(@continue);

            newCodes.InsertRange(
            index - 1,
            new List<CodeInstruction>
            {
                // if (EventHandlers.SpecialFeatureUsing)
                // _submergeCooldown = EventHandlers.SpecialFeatureCooldown;
                new (OpCodes.Ldsfld, AccessTools.Field(typeof(EventHandlers), nameof(EventHandlers.SpecialFeatureUsing))),
                new (OpCodes.Brfalse_S, @continue),
                new (OpCodes.Ldsfld, AccessTools.Field(typeof(EventHandlers), nameof(EventHandlers.SpecialFeatureCooldown))),
                new (OpCodes.Br, skip),
            });

            for (int i = 0; i < newCodes.Count; i++)
            {
                yield return newCodes[i];
            }

            ListPool<CodeInstruction>.Pool.Return(newCodes);
        }
    }
}