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

            Label Skip = generator.DefineLabel();

            NewCodes[0].labels.Add(Skip);

            NewCodes.InsertRange(0,
            [
                    // this.Hub.IsScp();
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Call, AccessTools.PropertyGetter(typeof(StatusEffectBase), nameof(StatusEffectBase.Hub))),
                    new(OpCodes.Ldc_I4_1),
                    new(OpCodes.Call, AccessTools.Method(typeof(PlayerRolesUtils), nameof(PlayerRolesUtils.IsSCP))),

                    // Plugin.Instance.Config.PocketexitRandomZonemode
                    new(OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                    new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Config))),
                    new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.PocketexitRandomZonemode))),

                    // if ( this.Hub.IsScp() || Plugin.Instance.Config.PocketexitRandomZonemode )
                    // return = RandomZone();
                    new(OpCodes.Or),
                    new(OpCodes.Brfalse_S, Skip),

                    new(OpCodes.Call, AccessTools.Method(typeof(Methods), nameof(Methods.RandomZone))),
                    new(OpCodes.Ret),
            ]);

            for (int i = 0; i < NewCodes.Count; i++)
                yield return NewCodes[i];

            ListPool<CodeInstruction>.Pool.Return(NewCodes);
        }
    }
}
