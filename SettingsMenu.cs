using System.Text;
using System.Linq;
using PlayerRoles;
using NorthwoodLib.Pools;
using Exiled.API.Features;
using SSMenuSystem.Features;
using BetterScp106.Features;
using System.Collections.Generic;
using UserSettings.ServerSpecific;
using SSMenuSystem.Features.Wrappers;
using Exiled.API.Features.Roles;

namespace BetterScp106
{
    public class SettingsMenu
    {
        public class ServerSettingsSyncer : Menu
        {
            public override int Id { get; set; } = -106;

            public override string Name { get; set; } = "Better 106";

            public override bool CheckAccess(ReferenceHub hub) => hub;

            public override ServerSpecificSettingBase[] Settings => [.. Better106Menu().Cast<ServerSpecificSettingBase>()];

        }
        public static ServerSpecificSettingBase[] Better106Menu()
        {
            List<ServerSpecificSettingBase> settings = [];
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();

            AddFeatureDescription(Plugin.Instance.Config.PocketFeature, Plugin.Instance.Translation.Scp106PowersPocket, stringBuilder,
                "$pockethealt", Plugin.Instance.Config.PocketdimensionCostHealt.ToString(),
                "$pocketvigor", Plugin.Instance.Config.PocketdimensionCostVigor.ToString());

            AddFeatureDescription(Plugin.Instance.Config.PocketinFeature, Plugin.Instance.Translation.Scp106PowersPocketin, stringBuilder,
                "$pocketinhealt", Plugin.Instance.Config.PocketinCostHealt.ToString(),
                "$pocketinvigor", Plugin.Instance.Config.PocketinCostVigor.ToString());

            AddFeatureDescription(Plugin.Instance.Config.StalkFeature, Plugin.Instance.Translation.Scp106PowersStalk, stringBuilder,
                "$stalkhealt", Plugin.Instance.Config.StalkCostHealt.ToString(),
                "$stalkvigor", Plugin.Instance.Config.StalkCostVigor.ToString());

            AddFeatureDescription(Plugin.Instance.Config.TeleportRoomsFeature, Plugin.Instance.Translation.Scp106PowersTeleport, stringBuilder,
                "$teleporthealt", Plugin.Instance.Config.TeleportCostHealt.ToString(),
                "$teleportvigor", Plugin.Instance.Config.TeleportCostVigor.ToString());

            settings.Add(new SSTextArea(
                id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.Description],
                content : StringBuilderPool.Shared.ToStringReturn(stringBuilder),
                foldoutMode: SSTextArea.FoldoutMode.ExtendedByDefault));

            if (Plugin.Instance.Config.PocketFeature)
            {
                settings.Add(new Keybind(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.PocketKey],
                    label: Plugin.Instance.Translation.Pocket[0],
                    suggestedKey: UnityEngine.KeyCode.F,
                    hint: Plugin.Instance.Translation.Pocket[1],
                    onUsed: (hub, ispressed) => { 
                        if(ispressed && Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                            GotoPocket.PocketFeature(scp106); 
                    }
                    ));
            }

            if (Plugin.Instance.Config.PocketinFeature)
            {
                settings.Add(new Keybind(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.PocketinKey],
                    label: Plugin.Instance.Translation.PocketIn[0],
                    suggestedKey: UnityEngine.KeyCode.G,
                    hint: Plugin.Instance.Translation.PocketIn[1],
                    onUsed: (hub, ispressed) => {
                        if (ispressed && Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                            TakeScpsPocket.PocketInFeature(scp106);
                    }
                    ));
            }

            if (Plugin.Instance.Config.StalkFeature)
            {
                settings.Add(new Keybind(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkKey],
                    label: Plugin.Instance.Translation.Stalk[0],
                    suggestedKey: UnityEngine.KeyCode.H,
                    hint: Plugin.Instance.Translation.Stalk[1],
                    onUsed: (hub, ispressed) => { 
                        if(ispressed && Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                            Stalking.StalkFeature(scp106); 
                    }
                    ));

                settings.Add(new SSTwoButtonsSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkMode],
                    label: Plugin.Instance.Translation.Stalk[2],
                    optionA: Plugin.Instance.Translation.Stalk[3],
                    optionB: Plugin.Instance.Translation.Stalk[4],
                    defaultIsB: true,
                    hint: Plugin.Instance.Translation.Stalk[5]));

                settings.Add(new SSSliderSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkDistanceSlider],
                    label: Plugin.Instance.Translation.Stalk[6],
                    minValue: 0.0f,
                    maxValue: Plugin.Instance.Config.StalkFromEverywhere ? 20000 : Plugin.Instance.Config.StalkDistance,
                    defaultValue: Plugin.Instance.Config.StalkFromEverywhere ? 20000 : Plugin.Instance.Config.StalkDistance,
                    integer: true,
                    hint: Plugin.Instance.Translation.Stalk[7]));
            }

            if (Plugin.Instance.Config.TeleportRoomsFeature)
            {
                settings.Add(new SSDropdownSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRoomsList],
                    label: Plugin.Instance.Translation.Teleport[0],
                    options: [.. Plugin.Instance.Config.Rooms.Select(r => r.ToString())]));

                settings.Add(new Button(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRooms],
                    label: Plugin.Instance.Translation.Teleport[0],
                    buttonText: Plugin.Instance.Translation.Teleport[1],
                    hint: Plugin.Instance.Translation.Teleport[2],
                    holdTimeSeconds: 2f,
                    onClick: (hub, setting) => {
                        if (Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                            TeleportRooms.TeleportFeature(scp106); 
                    }
                    ));

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
    }

}
