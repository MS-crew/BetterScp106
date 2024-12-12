using System.Text;
using System.Linq;
using PlayerRoles;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using UserSettings.ServerSpecific;
using ServerSpecificSyncer.Features;

namespace BetterScp106
{
    public class SettingHandlers
    {
        public static ServerSpecificSettingBase[] Better106Menu()
        {
            return SettingsHelper.GetSettings();
        }
    }
    internal class ServerSettingsSyncer : Menu
    {
        public override ServerSpecificSettingBase[] Settings
        {
            get
            {
                return SettingsHelper.GetSettings();
            }
        }
        public override bool CheckAccess(ReferenceHub hub) => hub.roleManager.CurrentRole.RoleTypeId == RoleTypeId.Scp106;
        public override string Name { get; set; } = "Better 106";
        public override int Id { get; set; } = -106;
    }
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

            AddKeybindSetting(settings, Plugin.C.PocketFeature, Methods.Features.PocketKey, "Pocket Dimension Key", UnityEngine.KeyCode.F, "The button you'll use to go to the pocket");
            AddKeybindSetting(settings, Plugin.C.PocketinFeature, Methods.Features.PocketinKey, "Pocket In Key", UnityEngine.KeyCode.G, "The button you use to bring your friend to the pocket");

            if (Plugin.C.StalkFeature)
            {
                settings.Add(new SSKeybindSetting(Plugin.C.AbilitySettingIds[Methods.Features.StalkKey], "Stalk Key", UnityEngine.KeyCode.H, hint:"The button you want to stalk"));
                settings.Add(new SSTwoButtonsSetting(Plugin.C.AbilitySettingIds[Methods.Features.StalkMode], "Stalk mode", "Distance", "Healt", true, "According to this setting, you either stalk the closest person or the one with the lowest health."));
                settings.Add(new SSSliderSetting(Plugin.C.AbilitySettingIds[Methods.Features.StalkDistanceSlider], "Stalk Distance", 0.0f, Plugin.C.StalkFromEverywhere ? 20000 : Plugin.C.StalkDistance, Plugin.C.StalkFromEverywhere ? 20000 : Plugin.C.StalkDistance, true, "The size of the local radius that can be stalked."));
            }

            if (Plugin.C.TeleportRoomsFeature)
            {
                settings.Add(new SSDropdownSetting(Plugin.C.AbilitySettingIds[Methods.Features.TeleportRoomsList], "Rooms", Plugin.C.Rooms.Select(r => r.ToString()).ToArray()));
                settings.Add(new SSButton(Plugin.C.AbilitySettingIds[Methods.Features.TeleportRooms], "Teleport Room", "Teleport", 2f));
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
