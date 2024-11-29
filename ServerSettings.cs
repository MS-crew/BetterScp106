using System.Text;
using NorthwoodLib.Pools;
using UserSettings.ServerSpecific;
using UserSettings.ServerSpecific.Examples;

namespace BetterScp106
{
    /*    In Progges
    public class ServerSettings : SSExampleImplementationBase
    {
        public override string Name => "Better 106 by Zurna Sever";
        public override void Activate()
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Better Scp-106:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersPocket.Replace("$pockethealt", Plugin.C.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.C.PocketdimensionCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.C.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.C.PocketinCostVigor.ToString()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Plugin.T.Scp106PowersStalk.Replace("$stalkhealt", Plugin.C.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.C.StalkCostVigor.ToString()));
            StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            ServerSpecificSettingsSync.DefinedSettings =
            [
                new SSGroupHeader("Better Scp-106"),
                new SSTwoButtonsSetting(106, "Stalk mode", "Healt", "Distance",defaultIsB: false,hint: "According to this setting, you either stalk the closest person at first, or more logical according to lore, the person with the lowest health."),
                new SSTextArea         (new int?(), StringBuilderPool.Shared.ToStringReturn(stringBuilder), SSTextArea.FoldoutMode.ExtendedByDefault),
                new SSSliderSetting    (107, "SliderSetting", 0.0f, Plugin.C.StalkDistance, defaultValue: Plugin.C.StalkDistance, integer:true, hint:"The size of the local radius that can be stalked (the server owner determines the maximum distance)"),
                new SSKeybindSetting   (108, "Pocket Dimension Key",suggestedKey: UnityEngine.KeyCode.F),
                new SSKeybindSetting   (109, "Pocket In Key",suggestedKey: UnityEngine.KeyCode.G),
                new SSKeybindSetting   (110, "Stalk Key",suggestedKey: UnityEngine.KeyCode.H),
            ];
            ServerSpecificSettingsSync.SendToAll();
        }

        public override void Deactivate()
        {
        }
    }*/
}
