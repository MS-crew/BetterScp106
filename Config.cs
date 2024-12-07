using PlayerRoles;
using Exiled.API.Interfaces;
using System.ComponentModel;

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
        public bool OneHitPocket { get; set; } = false;

        [Description("Should players exit a random zone when they exit the pocket dimension?")]
        public bool PocketexitRandomZonemode { get; set; } = true;

        [Description("Pocket dimension is not affected by warhead explosion and effect")]
        public bool RealisticPocket { get; set; } = false;

        [Description("Should you be reminded of your 106 powers every time you leave your pocket?")]
        public bool Reminders { get; set; } = true;

        [Description("Is it possible to stalk with the Alt key?")]
        public bool AltwithStalk { get; set; } = true;

        [Description("Is it possible to go to pocket dimension with the  [C]/sneak key?")]
        public bool CwithPocket { get; set; } = true;

        [Description("How much health does it cost to go to pocket dimension?")]
        public int PocketdimensionCostHealt { get; set; } = 50;

        [Description("How much Vigor/106 energy does it cost to go to pocket dimension?")]
        public int PocketdimensionCostVigor { get; set; } = 100;

        [Description("How much cooldown should be after the Pocketdim?")]
        public int AfterPocketdimensionCooldown { get; set; } = 30;

        [Description("How much health does it cost pocketing a scp?")]
        public int PocketinCostHealt { get; set; } = 200;

        [Description("How much Vigor/106 energy does it cost pocketing a scp?")]
        public int PocketinCostVigor { get; set; } = 100;

        [Description("How much cooldown should be after Pocketing a scp?")]
        public int AfterPocketingScpCooldown { get; set; } = 60;

        [Description("How much cooldown should be after canceling Pocketing a scp?")]
        public int CanceledPocketingScpCooldown { get; set; } = 15;

        [Description("How far can you go to victims with Stalk.")]
        public int StalkDistance { get; set; } = 200;

        [Description("Stalk from any distance.")]
        public bool StalkFromEverywhere { get; set; } = false; 

        [Description("How low should the health of the target to be stalked be? 106 tracks moribund targets, so the target to be stalked will be the one with the lowest health and the one you set. (if you want him to be able to stalk everyone, you can just write 101)")]
        public int StalkTargetmaxHealt { get; set; } = 90;

        [Description("How much healt would a successful stalk cost?")]
        public int StalkCostHealt { get; set; } = 150;

        [Description("How much Vigor/106 energy would a successful stalk cost?")]
        public int StalkCostVigor { get; set; } = 25;

        [Description("Warn victim before Stalk.")]
        public bool StalkWarning { get; set; } = true;

        [Description("How many seconds before target warning in Stalks (If the stalk warning is on)")]
        public float StalkWarningBefore { get; set; } = 1;

        [Description("How much cooldown should be after Stalk?")]
        public int AfterStalkCooldown { get; set; } = 90;

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

    }
}
