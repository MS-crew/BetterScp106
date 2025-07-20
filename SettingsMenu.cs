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
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using NorthwoodLib.Pools;
    using PlayerRoles;
    using SSMenuSystem.Features;
    using SSMenuSystem.Features.Wrappers;
    using UserSettings.ServerSpecific;

    /// <summary>
    /// Represents the settings menu for Better SCP-106 features.
    /// </summary>
    public class SettingsMenu
    {
        /// <summary>
        /// Generates a list of settings for the Better SCP-106 menu.
        /// </summary>
        /// <returns>A list of <see cref="ServerSpecificSettingBase"/> objects representing the menu settings.</returns>
        public static ServerSpecificSettingBase[] Better106Menu()
        {
            List<ServerSpecificSettingBase> settings = new ();
            StringBuilder descriptionBuilder = StringBuilderPool.Shared.Rent();

            if (Plugin.Instance.Config.PocketFeature)
            {
                settings.Add(new Keybind(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.PocketKey],
                    label: Plugin.Instance.Translation.Pocket[0],
                    suggestedKey: UnityEngine.KeyCode.F,
                    hint: Plugin.Instance.Translation.Pocket[1],
                    allowSpectatorTrigger: false,
                    onUsed: (hub, ispressed) =>
                    {
                        if (ispressed && Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                        {
                            GotoPocket.PocketFeature(scp106);
                        }
                    }));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersPocket, Plugin.Instance.Config.PocketdimensionCostHealt, Plugin.Instance.Config.PocketdimensionCostVigor));
            }

            if (Plugin.Instance.Config.PocketinFeature)
            {
                settings.Add(new Keybind(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.PocketinKey],
                    label: Plugin.Instance.Translation.PocketIn[0],
                    suggestedKey: UnityEngine.KeyCode.G,
                    hint: Plugin.Instance.Translation.PocketIn[1],
                    allowSpectatorTrigger: false,
                    onUsed: (hub, ispressed) =>
                    {
                        if (ispressed && Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                        {
                            TakeScpsPocket.PocketInFeature(scp106);
                        }
                    }));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersPocketin, Plugin.Instance.Config.PocketinCostHealt, Plugin.Instance.Config.PocketinCostVigor));
            }

            if (Plugin.Instance.Config.StalkFeature)
            {
                settings.Add(new Keybind(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.StalkKey],
                    label: Plugin.Instance.Translation.Stalk[0],
                    suggestedKey: UnityEngine.KeyCode.H,
                    hint: Plugin.Instance.Translation.Stalk[1],
                    allowSpectatorTrigger: false,
                    onUsed: (hub, ispressed) =>
                    {
                        if (ispressed && Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                        {
                            Stalking.StalkFeature(scp106);
                        }
                    }));

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

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersStalk, Plugin.Instance.Config.StalkCostHealt, Plugin.Instance.Config.StalkCostVigor));
            }

            if (Plugin.Instance.Config.TeleportRoomsFeature)
            {
                settings.Add(new SSDropdownSetting(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRoomsList],
                    label: Plugin.Instance.Translation.Teleport[0],
                    options: Plugin.Instance.Config.Rooms.Select(r => r.ToString()).ToArray()));

                settings.Add(new Button(
                    id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.TeleportRooms],
                    label: Plugin.Instance.Translation.Teleport[0],
                    buttonText: Plugin.Instance.Translation.Teleport[1],
                    hint: Plugin.Instance.Translation.Teleport[2],
                    holdTimeSeconds: 2f,
                    onClick: (hub, setting) =>
                    {
                        if (Player.Get(hub).Role.Is<Scp106Role>(out Scp106Role scp106))
                        {
                            TeleportRooms.TeleportFeature(scp106);
                        }
                    }));

                descriptionBuilder.AppendLine(string.Format(Plugin.Instance.Translation.Scp106PowersTeleport, Plugin.Instance.Config.TeleportCostHealt, Plugin.Instance.Config.TeleportCostVigor));
            }

            settings.Insert(0, new SSTextArea(
               id: Plugin.Instance.Config.AbilitySettingIds[Methods.Features.Description],
               content: StringBuilderPool.Shared.ToStringReturn(descriptionBuilder),
               foldoutMode: SSTextArea.FoldoutMode.ExtendedByDefault));

            return settings.ToArray();
        }

        /// <summary>
        /// Represents the synchronization of server settings for Better SCP-106 features.
        /// </summary>
        public class ServerSettingsSyncer : Menu
        {
            /// <summary>
            /// Gets or sets the unique identifier for the menu.
            /// </summary>
            public override int Id { get; set; } = -106;

            /// <summary>
            /// Gets or sets the name of the menu.
            /// </summary>
            public override string Name { get; set; } = "Better 106";

            /// <summary>
            /// Gets the settings for the menu.
            /// </summary>
            public override ServerSpecificSettingBase[] Settings => Better106Menu().Cast<ServerSpecificSettingBase>().ToArray();

            /// <summary>
            /// Checks whether the specified hub has access to the menu.
            /// </summary>
            /// <param name="hub">The reference hub to check access for.</param>
            /// <returns>True if the hub has access; otherwise, false.</returns>
            public override bool CheckAccess(ReferenceHub hub) => hub.GetRoleId() == RoleTypeId.Scp106;
        }
    }
}
