using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using PlayerRoles.Subroutines;
using Exiled.API.Features.Pools;
using System.Collections.Generic;
using PlayerRoles.PlayableScps.Scp106;

namespace BetterScp106.Patchs
{
    [HarmonyPatch(typeof(Scp106SinkholeController), nameof(Scp106SinkholeController.ServerSetSubmerged))]
    public static class CoolDownPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> NewCodes = ListPool<CodeInstruction>.Pool.Get(instructions);

            Label Skip = generator.DefineLabel();
            int index = NewCodes.FindIndex(code => code.opcode == OpCodes.Callvirt && code.operand is MethodInfo method && method == AccessTools.Method(typeof(AbilityCooldown), nameof(AbilityCooldown.Trigger)));
            NewCodes[index + 1].labels.Add(Skip);

            int insertindex = NewCodes.FindIndex(code => code.opcode == OpCodes.Brtrue_S);
            NewCodes.InsertRange( insertindex + 1,
            [
                    new(OpCodes.Ldsfld, AccessTools.Field(typeof(EventHandlers), nameof(EventHandlers.SpecialFeatureUsing))),
                    new(OpCodes.Brtrue_S, Skip),
            ]);

            foreach (CodeInstruction code in NewCodes)
                yield return code;

            ListPool<CodeInstruction>.Pool.Return(NewCodes);
        }
    }
}
