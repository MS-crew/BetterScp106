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
            Label Continue = generator.DefineLabel();

            int index = NewCodes.FindIndex(code => code.opcode == OpCodes.Callvirt && code.operand is MethodInfo method && method == AccessTools.Method(typeof(AbilityCooldown), nameof(AbilityCooldown.Trigger)));
            NewCodes[index].labels.Add(Skip);
            NewCodes[index- 1].labels.Add(Continue);

            NewCodes.InsertRange( index - 1,
            [
                //if (!SpecialFeatureUsing)
                //_submergeCooldown.Trigger(5.0)
                //else 
                //_submergeCooldown.Trigger(SpecialFeatureCooldown)
                new(OpCodes.Ldsfld, AccessTools.Field(typeof(EventHandlers), nameof(EventHandlers.SpecialFeatureUsing))),
                new(OpCodes.Brfalse_S, Continue),
                new(OpCodes.Ldsfld, AccessTools.Field(typeof(EventHandlers), nameof(EventHandlers.SpecialFeatureCooldown))),
                new(OpCodes.Br , Skip),
            ]);

            foreach (CodeInstruction code in NewCodes)
                yield return code;

            ListPool<CodeInstruction>.Pool.Return(NewCodes);
        }
    }
}
