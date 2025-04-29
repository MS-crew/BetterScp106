using HarmonyLib;
using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Features;
using System.Reflection.Emit;
using Exiled.API.Features.Pools;
using System.Collections.Generic;

namespace BetterScp106.Patchs
{
    [HarmonyPatch(typeof(AlphaWarheadController), nameof(AlphaWarheadController.CanBeDetonated))]
    public static class RealisticPocketPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> NewCodes = ListPool<CodeInstruction>.Pool.Get(instructions);

            Label Continue = generator.DefineLabel();

            NewCodes[0].labels.Add(Continue);

            NewCodes.InsertRange(0,
            [
                // if (Plugin.Instance.Config.PocketexitRandomZonemode && Room.Get(this.position) == Roomtype.Pocket)
                // return false

                new(OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Config))),
                new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Config), nameof(Config.RealisticPocket))),
                new(OpCodes.Brfalse_S, Continue),

                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, AccessTools.Method(typeof(Room), nameof(Room.Get), [typeof(Vector3)])),
                new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Room), nameof(Room.Type))),
                new(OpCodes.Ldc_I4_S, (int)RoomType.Pocket),
                new(OpCodes.Ceq),
                new(OpCodes.Brfalse_S, Continue),

                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Ret),
            ]);

            for (int i = 0; i < NewCodes.Count; i++)
                yield return NewCodes[i];

            ListPool<CodeInstruction>.Pool.Return(NewCodes);
        }
    }
} 
