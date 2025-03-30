using PlayerRoles;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System.ComponentModel;
using System.Collections.Generic;

namespace BetterScp106
{
    public class Config : IConfig
    {
        public bool IsEnabled { get ; set ; } = true;

        public bool Debug { get; set; } = false;

        [Description("What features should be turned on??")]
        public bool PocketFeature { get; set; } = true;
        public bool PocketinFeature { get; set; } = true;
        public bool StalkFeature { get; set; } = true;
        public bool TeleportRoomsFeature { get; set; } = true;
        public bool OneHitPocket { get; set; } = false;

        [Description("Button setting ids of features")]
        public Dictionary<Methods.Features, int> AbilitySettingIds { get; set; } = new Dictionary<Methods.Features, int>
        {
            {Methods.Features.PocketKey,          106},
            {Methods.Features.PocketinKey,        107},
            {Methods.Features.StalkKey,           108},
            {Methods.Features.StalkMode,          109},
            {Methods.Features.StalkDistanceSlider,110},
            {Methods.Features.TeleportRoomsList,  111},
            {Methods.Features.TeleportRooms,      112},
            {Methods.Features.Description,        113},
        };

        [Description("Should players exit a random zone when they exit the pocket dimension?")]
        public bool PocketexitRandomZonemode { get; set; } = true;

        [Description("Pocket dimension is not affected by warhead explosion and effect")]
        public bool RealisticPocket { get; set; } = false;

        [Description("Should you be reminded of your 106 powers every time you leave your pocket?")]
        public bool Reminders { get; set; } = true;

        [Description("How much health does it cost to go to pocket dimension?")]
        public int PocketdimensionCostHealt { get; set; } = 50;

        [Description("How much Vigor/106 energy does it cost to go to pocket dimension?")]
        public int PocketdimensionCostVigor { get; set; } = 25;

        [Description("How much cooldown should be after the Pocketdim?")]
        public int AfterPocketdimensionCooldown { get; set; } = 30;

        [Description("How much health does it cost pocketing a scp?")]
        public int PocketinCostHealt { get; set; } = 150;

        [Description("How much Vigor/106 energy does it cost pocketing a scp?")]
        public int PocketinCostVigor { get; set; } = 100;

        [Description("How much cooldown should be after Pocketing a scp?")]
        public int AfterPocketingScpCooldown { get; set; } = 60;

        [Description("How much cooldown should be after canceling Pocketing a scp?")]
        public int CanceledPocketingScpCooldown { get; set; } = 15;

        [Description("How much healt would a successful stalk cost?")]
        public int StalkCostHealt { get; set; } = 150;

        [Description("How much Vigor/106 energy would a successful stalk cost?")]
        public int StalkCostVigor { get; set; } = 25;

        [Description("How much cooldown should be after Stalk?")]
        public int AfterStalkCooldown { get; set; } = 90;

        [Description("How far can you go to victims with Stalk.")]
        public int StalkDistance { get; set; } = 200;

        [Description("Stalk from any distance.")]
        public bool StalkFromEverywhere { get; set; } = false; 

        [Description("How low should the health of the target to be stalked be? 106 tracks moribund targets, so the target to be stalked will be the one with the lowest health and the one you set. (if you want him to be able to stalk everyone, you can just write 101)")]
        public int StalkTargetmaxHealt { get; set; } = 90;

        [Description("Warn victim before Stalk.")]
        public bool StalkWarning { get; set; } = true;

        [Description("How many seconds before target warning in Stalks (If the stalk warning is on)")]
        public float StalkWarningBefore { get; set; } = 1;

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

        [Description("Teleport Mode.")]
        public bool TeleportOnlySameZone { get; set; } = false;

        [Description("How much healt would a successful Teleport cost?")]
        public int TeleportCostHealt { get; set; } = 25;

        [Description("How much Vigor/106 energy would a successful Teleport cost?")]
        public int TeleportCostVigor { get; set; } = 25;

        [Description("How much cooldown should be after Teleport?")]
        public int TeleportCooldown { get; set; } = 50;

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
            RoomType.Surface,
        ];
    }
}
