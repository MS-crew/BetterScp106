// -----------------------------------------------------------------------
// <copyright file="SettingsMenu.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BetterScp106.Features;
    using Exiled.API.Features.Core.UserSettings;
    using Exiled.API.Features.Roles;
    using NorthwoodLib.Pools;
    using UserSettings.ServerSpecific;

    /// <summary>
    /// Represents the settings menu for Better SCP-106 features.
    /// </summary>
    public class SettingsMenu
    {
        /// <summary>
        /// Enum representing various SCP-106 menu elements.
        /// </summary>
        public enum Features
        {
            Header,
            PocketKey,
            PocketinKey,
            StalkKey,
            StalkMode,
            StalkDistanceSlider,
            TeleportRooms,
            TeleportRoomsList,
            Description,
        }

        /// <summary>
        /// Gets cache for the Better SCP-106 menu settings.
        /// </summary>
        public static List<SettingBase> Better106MenuCache { get; } = Better106Menu();

        /// <summary>
        /// Generates a list of settings for the Better SCP-106 menu.
        /// </summary>
        /// <returns>A list of <see cref="SettingBase"/> objects representing the menu settings.</returns>
        public static List<SettingBase> Better106Menu()
        {
            List<SettingBase> settings = new ();
            StringBuilder descriptionBuilder = StringBuilderPool.Shared.Rent();

            settings.Add(new HeaderSetting(Plugin.Instance.Config.AbilitySettingIds[Features.Header] ,"Better Scp-106"));

            if (Plugin.Instance.Config.PocketFeature)
            {
                settings.Add(new KeybindSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.PocketKey],
                    label: Plugin.Instance.Translation.Pocket[0],
                    suggested: UnityEngine.KeyCode.F,
                    hintDescription: Plugin.Instance.Translation.Pocket[1],
                    onChanged: (player, setting) =>
                    {
                        if (player.Role.Is<Scp106Role>(out Scp106Role scp106) && setting.As<KeybindSetting>().IsPressed)
                        {
                            GotoPocket.PocketFeature(scp106);
                        }
                    }));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersPocket, Plugin.Instance.Config.PocketdimensionCostHealt, Plugin.Instance.Config.PocketdimensionCostVigor));
            }

            if (Plugin.Instance.Config.PocketinFeature)
            {
                settings.Add(new KeybindSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.PocketinKey],
                    label: Plugin.Instance.Translation.PocketIn[0],
                    suggested: UnityEngine.KeyCode.G,
                    hintDescription: Plugin.Instance.Translation.PocketIn[1],
                    onChanged: (player, setting) =>
                    {
                        if (player.Role.Is<Scp106Role>(out Scp106Role scp106) && setting.As<KeybindSetting>().IsPressed)
                        {
                            TakeScpsPocket.PocketInFeature(scp106);
                        }
                    }));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersPocketin, Plugin.Instance.Config.PocketinCostHealt, Plugin.Instance.Config.PocketinCostVigor));
            }

            if (Plugin.Instance.Config.StalkFeature)
            {
                settings.Add(new KeybindSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.StalkKey],
                    label: Plugin.Instance.Translation.Stalk[0],
                    suggested: UnityEngine.KeyCode.H,
                    hintDescription: Plugin.Instance.Translation.Stalk[1],
                    onChanged: (player, setting) =>
                    {
                        if (player.Role.Is<Scp106Role>(out Scp106Role scp106) && setting.As<KeybindSetting>().IsPressed)
                        {
                            Stalking.StalkFeature(scp106);
                        }
                    }));

                settings.Add(new TwoButtonsSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.StalkMode],
                    label: Plugin.Instance.Translation.Stalk[2],
                    firstOption: Plugin.Instance.Translation.Stalk[3],
                    secondOption: Plugin.Instance.Translation.Stalk[4],
                    defaultIsSecond: true,
                    hintDescription: Plugin.Instance.Translation.Stalk[5]));

                settings.Add(new SliderSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.StalkDistanceSlider],
                    label: Plugin.Instance.Translation.Stalk[6],
                    minValue: 0.0f,
                    maxValue: Plugin.Instance.Config.StalkFromEverywhere ? 20000 : Plugin.Instance.Config.StalkDistance,
                    defaultValue: Plugin.Instance.Config.StalkFromEverywhere ? 20000 : Plugin.Instance.Config.StalkDistance,
                    isInteger: true,
                    hintDescription: Plugin.Instance.Translation.Stalk[7]));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersStalk, Plugin.Instance.Config.StalkCostHealt, Plugin.Instance.Config.StalkCostVigor));
            }

            if (Plugin.Instance.Config.TeleportRoomsFeature)
            {
                IEnumerable<string> teleportRoomList = Plugin.Instance.Config.Rooms.Select(r => r.ToString());
                settings.Add(new DropdownSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.TeleportRoomsList],
                    label: Plugin.Instance.Translation.Teleport[0],
                    options: teleportRoomList));

                settings.Add(new ButtonSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Features.TeleportRooms],
                    label: Plugin.Instance.Translation.Teleport[0],
                    buttonText: Plugin.Instance.Translation.Teleport[1],
                    hintDescription: Plugin.Instance.Translation.Teleport[2],
                    holdTime: 2f,
                    onChanged: (player, setting) =>
                    {
                        if (player.Role.Is<Scp106Role>(out Scp106Role scp106))
                        {
                            TeleportRooms.TeleportFeature(scp106);
                        }
                    }));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersTeleport, Plugin.Instance.Config.TeleportCostHealt, Plugin.Instance.Config.TeleportCostVigor));
            }

            settings.Insert(1, new TextInputSetting(
                id: Plugin.Instance.Config.AbilitySettingIds[Features.Description],
                label: StringBuilderPool.Shared.ToStringReturn(descriptionBuilder),
                foldoutMode: SSTextArea.FoldoutMode.ExtendedByDefault));

            return settings;
        }
    }
}