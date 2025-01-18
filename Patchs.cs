using HarmonyLib;
using PlayerRoles;
using CustomPlayerEffects;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;
using System.Collections.Generic;

namespace BetterScp106
{
    [HarmonyPatch(typeof(PocketCorroding), "get_CapturePosition")]
    public static class CapturePositionPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> Yenikodlar = ListPool<CodeInstruction>.Pool.Get(instructions);
            LocalBuilder Scpmi = generator.DeclareLocal(typeof(bool));

            Label Atla = generator.DefineLabel();
            Yenikodlar[0].labels.Add(Atla);
            Yenikodlar.InsertRange(0, new List<CodeInstruction>
            {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StatusEffectBase), "get_Hub")),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerRolesUtils), nameof(PlayerRolesUtils.IsSCP))),
                    new CodeInstruction(OpCodes.Stloc, Scpmi.LocalIndex),

                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.C))),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.PocketexitRandomZonemode))),
                    new CodeInstruction(OpCodes.Ldloc, Scpmi.LocalIndex),
                    new CodeInstruction(OpCodes.Or),
                    new CodeInstruction(OpCodes.Brfalse_S, Atla),

                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RandomZone))),
                    new CodeInstruction(OpCodes.Ret),
            });

            for (int z = 0; z < Yenikodlar.Count; z++)
                yield return Yenikodlar[z];

            ListPool<CodeInstruction>.Pool.Return(Yenikodlar);
        }
    }
}
