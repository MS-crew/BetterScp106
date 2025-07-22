// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;
    using PlayerRoles;

    /// <summary>
    /// Represents the configuration for the BetterScp106 plugin.
    /// </summary>
    public class Config : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the plugin is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug mode is enabled.
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the pocket feature is enabled.
        /// </summary>
        [Description("What features should be turned on??")]
        public bool PocketFeature { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the pocket-in feature is enabled.
        /// </summary>
        [Description("Pocket-in feature enabled")]
        public bool PocketinFeature { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the stalk feature is enabled.
        /// </summary>
        [Description("Stalk feature enabled")]
        public bool StalkFeature { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the teleport rooms feature is enabled.
        /// </summary>
        [Description("Teleport rooms feature enabled")]
        public bool TeleportRoomsFeature { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the one-hit pocket feature is enabled.
        /// </summary>
        [Description("One-hit pocket feature enabled")]
        public bool OneHitPocket { get; set; } = false;

        /// <summary>
        /// Gets or sets the button setting IDs for various features.
        /// </summary>
        [Description("Button setting ids of features")]
        public Dictionary<SettingsMenu.Features, int> AbilitySettingIds { get; set; } = new Dictionary<SettingsMenu.Features, int>
        {
            { SettingsMenu.Features.PocketKey, 106 },
            { SettingsMenu.Features.PocketinKey, 107 },
            { SettingsMenu.Features.StalkKey, 108 },
            { SettingsMenu.Features.StalkMode, 109 },
            { SettingsMenu.Features.StalkDistanceSlider, 110 },
            { SettingsMenu.Features.TeleportRoomsList, 111 },
            { SettingsMenu.Features.TeleportRooms, 112 },
            { SettingsMenu.Features.Description, 113 },
        };

        /// <summary>
        /// Gets or sets a value indicating whether players exit a random zone when leaving the pocket dimension.
        /// </summary>
        [Description("Should players exit a random zone when they exit the pocket dimension?")]
        public bool PocketexitRandomZonemode { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the pocket dimension is unaffected by warhead explosions and effects.
        /// </summary>
        [Description("Pocket dimension is not affected by warhead explosion and effect")]
        public bool RealisticPocket { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether players are reminded of their SCP-106 powers upon leaving the pocket dimension.
        /// </summary>
        [Description("Should you be reminded of your 106 powers every time you leave your pocket?")]
        public bool Reminders { get; set; } = true;

        /// <summary>
        /// Gets or sets the health cost for entering the pocket dimension.
        /// </summary>
        [Description("How much health does it cost to go to pocket dimension?")]
        public int PocketdimensionCostHealt { get; set; } = 50;

        /// <summary>
        /// Gets or sets the vigor cost for entering the pocket dimension.
        /// </summary>
        [Description("How much Vigor/106 energy does it cost to go to pocket dimension?")]
        public int PocketdimensionCostVigor { get; set; } = 25;

        /// <summary>
        /// Gets or sets the cooldown duration after entering the pocket dimension.
        /// </summary>
        [Description("How much cooldown should be after the Pocketdim?")]
        public double AfterPocketdimensionCooldown { get; set; } = 30;

        /// <summary>
        /// Gets or sets the health cost for pocketing a SCP.
        /// </summary>
        [Description("How much healt does it cost pocketing a scp?")]
        public int PocketinCostHealt { get; set; } = 150;

        /// <summary>
        /// Gets or sets the vigor cost for pocketing a SCP.
        /// </summary>
        [Description("How much Vigor/106 energy does it cost pocketing a scp?")]
        public int PocketinCostVigor { get; set; } = 100;

        /// <summary>
        /// Gets or sets the cooldown after pocketing a SCP.
        /// </summary>
        [Description("How much cooldown should be after Pocketing a scp?")]
        public double AfterPocketingScpCooldown { get; set; } = 60;

        /// <summary>
        /// Gets or sets the cooldown after canceling pocketing a SCP.
        /// </summary>
        [Description("How much cooldown should be after canceling Pocketing a scp?")]
        public double CanceledPocketingScpCooldown { get; set; } = 15;

        /// <summary>
        /// Gets or sets the health cost for a successful stalk.
        /// </summary>
        [Description("How much healt would a successful stalk cost?")]
        public int StalkCostHealt { get; set; } = 150;

        /// <summary>
        /// Gets or sets the vigor cost for a successful stalk.
        /// </summary>
        [Description("How much Vigor/106 energy would a successful stalk cost?")]
        public int StalkCostVigor { get; set; } = 25;

        /// <summary>
        /// Gets or sets the cooldown after a stalk.
        /// </summary>
        [Description("How much cooldown should be after Stalk?")]
        public double AfterStalkCooldown { get; set; } = 90;

        /// <summary>
        /// Gets or sets the maximum stalk distance.
        /// </summary>
        [Description("How far can you go to victims with Stalk.")]
        public int StalkDistance { get; set; } = 200;

        /// <summary>
        /// Gets or sets a value indicating whether stalk can be performed from any distance.
        /// </summary>
        [Description("Stalk from any distance.")]
        public bool StalkFromEverywhere { get; set; } = false;

        /// <summary>
        /// Gets or sets the maximum health of the target to be stalked.
        /// </summary>
        [Description("How low should the health of the target to be stalked be? 106 tracks moribund targets, so the target to be stalked will be the one with the lowest health and the one you set. (if you want him to be able to stalk everyone, you can just write 101)")]
        public int StalkTargetmaxHealt { get; set; } = 90;

        /// <summary>
        /// Gets or sets a value indicating whether to warn the victim before stalk.
        /// </summary>
        [Description("Warn victim before Stalk.")]
        public bool StalkWarning { get; set; } = true;

        /// <summary>
        /// Gets or sets the seconds before target warning in stalks.
        /// </summary>
        [Description("How many seconds before target warning in Stalks (If the stalk warning is on)")]
        public float StalkWarningBefore { get; set; } = 1;

        /// <summary>
        /// Gets or sets the roles that can be stalked.
        /// </summary>
        [Description("Which roles can be Stalked?")]
        public RoleTypeId[] StalkableRoles { get; set; } =
        [
            RoleTypeId.ClassD,
            RoleTypeId.Scientist,
            RoleTypeId.FacilityGuard,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman,
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfSpecialist
        ];

        /// <summary>
        /// Gets or sets a value indicating whether teleport is only allowed in the same zone.
        /// </summary>
        [Description("Teleport Mode.")]
        public bool TeleportOnlySameZone { get; set; } = false;

        /// <summary>
        /// Gets or sets the health cost for a successful teleport.
        /// </summary>
        [Description("How much healt would a successful Teleport cost?")]
        public int TeleportCostHealt { get; set; } = 25;

        /// <summary>
        /// Gets or sets the vigor cost for a successful teleport.
        /// </summary>
        [Description("How much Vigor/106 energy would a successful Teleport cost?")]
        public int TeleportCostVigor { get; set; } = 25;

        /// <summary>
        /// Gets or sets the cooldown after teleport.
        /// </summary>
        [Description("How much cooldown should be after Teleport?")]
        public double TeleportCooldown { get; set; } = 50;

        /// <summary>
        /// Gets or sets the rooms to teleport to.
        /// </summary>
        [Description("Rooms to teleport to?")]
        public RoomType[] Rooms { get; set; } =
        [
            RoomType.LczArmory,
            RoomType.Lcz914,
            RoomType.LczCafe,
            RoomType.LczPlants,
            RoomType.LczToilets,
            RoomType.Lcz173,
            RoomType.LczClassDSpawn,
            RoomType.LczGlassBox,
            RoomType.Lcz330,
            RoomType.LczCheckpointB,
            RoomType.LczCheckpointA,
            RoomType.Hcz079,
            RoomType.HczEzCheckpointA,
            RoomType.HczEzCheckpointB,
            RoomType.HczArmory,
            RoomType.Hcz939,
            RoomType.HczHid,
            RoomType.Hcz049,
            RoomType.Hcz106,
            RoomType.HczNuke,
            RoomType.Hcz096,
            RoomType.HczTestRoom,
            RoomType.HczElevatorA,
            RoomType.HczElevatorB,
            RoomType.HczCrossRoomWater,
            RoomType.EzIntercom,
            RoomType.EzGateA,
            RoomType.EzGateB,
            RoomType.EzDownstairsPcs,
            RoomType.EzPcs,
            RoomType.EzConference,
            RoomType.EzChef,
            RoomType.EzCafeteria,
            RoomType.EzUpstairsPcs,
            RoomType.Surface
        ];
    }
}
