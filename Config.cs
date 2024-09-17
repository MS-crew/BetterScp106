using Exiled.API.Interfaces;
using System.ComponentModel;

namespace BetterScp106
{
    public class Config : IConfig
    {
        public bool IsEnabled { get ; set ; } = true;
        public bool Debug { get; set; } = false;

        [Description("How much health does it cost to go to pocket dimension??")]
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

        [Description("How far can you go to victims with Stalk. If you want everywhere you can enter crazy numbers like 99999")]
        public int StalkDistance { get; set; } = 200;

        [Description("How low should the health of the target to be stalked be? 106 tracks moribund targets, so the target to be stalked will be the one with the lowest health and the one you set. (if you want him to be able to stalk everyone, you can just write 101)")]
        public int StalkTargetmaxHealt { get; set; } = 90;

        [Description("How much healt would a successful stalk cost?")]
        public int StalkCostHealt { get; set; } = 150;

        [Description("How much Vigor/106 energy would a successful stalk cost?")]
        public int StalkCostVigor { get; set; } = 25;

        [Description("How much cooldown should be after Stalk?")]
        public int AfterStalkCooldown { get; set; } = 90;

    }
}
