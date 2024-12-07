using System.Text;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using UserSettings.ServerSpecific;

namespace BetterScp106
{
    public class SettingHandlers
    {
        public static ServerSpecificSettingBase[] Better106Menu()
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Better Scp-106:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersPocket.Replace("$pockethealt", Plugin.C.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.C.PocketdimensionCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.C.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.C.PocketinCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersStalk.Replace("$stalkhealt", Plugin.C.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.C.StalkCostVigor.ToString()));

            List<ServerSpecificSettingBase> mainMenu =
            [
                new SSGroupHeader("Better Scp-106"),
                new SSTextArea         (107, StringBuilderPool.Shared.ToStringReturn(stringBuilder), SSTextArea.FoldoutMode.ExtendedByDefault),
                new SSTwoButtonsSetting(108, "Stalk mode", "Healt", "Distance",defaultIsB: false,hint: "According to this setting, you either stalk the closest person at first, or more logical according to lore, the person with the lowest health."),
                new SSSliderSetting    (109, "SliderSetting", 0.0f, Plugin.C.StalkFromEverywhere? 20000: Plugin.C.StalkDistance, defaultValue: Plugin.C.StalkFromEverywhere? 20000: Plugin.C.StalkDistance, integer:true, hint:"The size of the local radius that can be stalked (the server owner determines the maximum distance)"),
                new SSKeybindSetting   ((int)Methods.Features.PocketKey, "Pocket Dimension Key", suggestedKey: UnityEngine.KeyCode.F),
                new SSKeybindSetting   ((int)Methods.Features.PocketinKey, "Pocket In Key", suggestedKey: UnityEngine.KeyCode.G),
                new SSKeybindSetting   ((int)Methods.Features.StalkKey, "Stalk Key", suggestedKey: UnityEngine.KeyCode.H),
            ];
            return [.. mainMenu];
        }
    }
}

