using System.Text;
using System.Linq;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using UserSettings.ServerSpecific;

namespace BetterScp106
{
    public class SettingHandlers
    {
        public static ServerSpecificSettingBase[] Better106Menu() => SettingsHelper.GetSettings();
    }
    /*public class ServerSettingsSyncer : Menu
    {
        public override int Id { get; set; } = -106;
        public override string Name { get; set; } = "Better 106";
        public override bool CheckAccess(ReferenceHub hub) => hub;
        public override ServerSpecificSettingBase[] Settings => SettingsHelper.GetSettings();
        public override void OnInput(ReferenceHub hub, ServerSpecificSettingBase setting) => Methods.ProcessUserInput(hub, setting);
    }*/
    public class SettingsHelper
    {
        public static ServerSpecificSettingBase[] GetSettings()
        {
            List<ServerSpecificSettingBase> settings = [];
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();

            AddFeatureDescription(Plugin.C.PocketFeature, Plugin.T.Scp106PowersPocket, stringBuilder,
                "$pockethealt", Plugin.C.PocketdimensionCostHealt.ToString(),
                "$pocketvigor", Plugin.C.PocketdimensionCostVigor.ToString());

            AddFeatureDescription(Plugin.C.PocketinFeature, Plugin.T.Scp106PowersPocketin, stringBuilder,
                "$pocketinhealt", Plugin.C.PocketinCostHealt.ToString(),
                "$pocketinvigor", Plugin.C.PocketinCostVigor.ToString());

            AddFeatureDescription(Plugin.C.StalkFeature, Plugin.T.Scp106PowersStalk, stringBuilder,
                "$stalkhealt", Plugin.C.StalkCostHealt.ToString(),
                "$stalkvigor", Plugin.C.StalkCostVigor.ToString());

            AddFeatureDescription(Plugin.C.TeleportRoomsFeature, Plugin.T.Scp106PowersTeleport, stringBuilder,
                "$teleporthealt", Plugin.C.TeleportCostHealt.ToString(),
                "$teleportvigor", Plugin.C.TeleportCostVigor.ToString());

            settings.Add(new SSGroupHeader("Better Scp-106"));
            settings.Add(new SSTextArea(null, StringBuilderPool.Shared.ToStringReturn(stringBuilder), SSTextArea.FoldoutMode.ExtendedByDefault));

            AddKeybindSetting(settings, Plugin.C.PocketFeature, Methods.Features.PocketKey, Plugin.T.Pocket[0], UnityEngine.KeyCode.F, Plugin.T.Pocket[1]);
            AddKeybindSetting(settings, Plugin.C.PocketinFeature, Methods.Features.PocketinKey, Plugin.T.PocketIn[0], UnityEngine.KeyCode.G, Plugin.T.PocketIn[1]);

            if (Plugin.C.StalkFeature)
            {
                settings.Add(new SSKeybindSetting(Plugin.C.AbilitySettingIds[Methods.Features.StalkKey], Plugin.T.Stalk[0], UnityEngine.KeyCode.H, hint: Plugin.T.Stalk[1]));
                settings.Add(new SSTwoButtonsSetting(Plugin.C.AbilitySettingIds[Methods.Features.StalkMode], Plugin.T.Stalk[2], Plugin.T.Stalk[3], Plugin.T.Stalk[4], true, Plugin.T.Stalk[5]));
                settings.Add(new SSSliderSetting(Plugin.C.AbilitySettingIds[Methods.Features.StalkDistanceSlider], Plugin.T.Stalk[6], 0.0f, Plugin.C.StalkFromEverywhere ? 20000 : Plugin.C.StalkDistance, Plugin.C.StalkFromEverywhere ? 20000 : Plugin.C.StalkDistance, true, Plugin.T.Stalk[7]));
            }

            if (Plugin.C.TeleportRoomsFeature)
            {
                settings.Add(new SSDropdownSetting(Plugin.C.AbilitySettingIds[Methods.Features.TeleportRoomsList], Plugin.T.Teleport[0], Plugin.C.Rooms.Select(r => r.ToString()).ToArray()));
                settings.Add(new SSButton(Plugin.C.AbilitySettingIds[Methods.Features.TeleportRooms], Plugin.T.Teleport[1], Plugin.T.Teleport[2], 2f));
            }

            return [.. settings];
        }
        private static void AddFeatureDescription(bool isEnabled, string template, StringBuilder stringBuilder, string placeholder1, string value1, string placeholder2, string value2)
        {
            if (isEnabled)
            {
                stringBuilder.AppendLine(template.Replace(placeholder1, value1).Replace(placeholder2, value2));
                stringBuilder.AppendLine();
            }
        }
        private static void AddKeybindSetting(List<ServerSpecificSettingBase> settings, bool isEnabled, Methods.Features feature, string label, UnityEngine.KeyCode suggestedKey, string hint)
        {
            if (isEnabled)
            {
                settings.Add(new SSKeybindSetting(Plugin.C.AbilitySettingIds[feature], label, suggestedKey, hint: hint));
            }
        }
    }

}
