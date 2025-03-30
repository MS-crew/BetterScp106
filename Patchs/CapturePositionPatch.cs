using HarmonyLib;
using PlayerRoles;
using CustomPlayerEffects;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;
using System.Collections.Generic;

namespace BetterScp106
{
    [HarmonyPatch(typeof(PocketCorroding), nameof(PocketCorroding.CapturePosition), MethodType.Getter)]
    public static class CapturePositionPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> NewCodes = ListPool<CodeInstruction>.Pool.Get(instructions);
            LocalBuilder IsScp = generator.DeclareLocal(typeof(bool));

            Label Skip = generator.DefineLabel();
            NewCodes[0].labels.Add(Skip);

            NewCodes.InsertRange(0, new List<CodeInstruction>
            {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(StatusEffectBase), nameof(StatusEffectBase.Hub))),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerRolesUtils), nameof(PlayerRolesUtils.IsSCP))),
                    new CodeInstruction(OpCodes.Stloc, IsScp.LocalIndex),

                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.PocketexitRandomZonemode))),
                    new CodeInstruction(OpCodes.Ldloc, IsScp.LocalIndex),
                    new CodeInstruction(OpCodes.Or),
                    new CodeInstruction(OpCodes.Brfalse_S, Skip),

                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RandomZone))),
                    new CodeInstruction(OpCodes.Ret),
            });

            for (int z = 0; z < NewCodes.Count; z++)
                yield return NewCodes[z];

            ListPool<CodeInstruction>.Pool.Return(NewCodes);
        }
    }
}
