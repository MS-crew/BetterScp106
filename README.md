<h1 align="center">Better Scp-106</h1>
<div align="center">
<a href="https://github.com/MS-crew/BetterScp106/releases"><img src="https://img.shields.io/github/downloads/MS-crew/BetterScp106/total?style=for-the-badge&logo=githubactions&label=Downloads" href="https://github.com/MS-crew/BetterScp106/releases" alt="GitHub Release Download"></a>
<a href="https://github.com/MS-crew/BetterScp106/releases"><img src="https://img.shields.io/badge/Build-2.5.9-brightgreen?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/BetterScp106/releases" alt="GitHub Releases"></a>
<a href="https://github.com/MS-crew/BetterScp106/blob/master/LICENSE"><img src="https://img.shields.io/badge/Licence-GNU_3.0-blue?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/BetterScp106/blob/master/LICENSE" alt="General Public License v3.0"></a>
<a href="https://github.com/ExMod-Team/EXILED"><img src="https://img.shields.io/badge/Exiled-9.4.0-red?style=for-the-badge&logo=gitbook" href="https://github.com/ExMod-Team/EXILED" alt="GitHub Exiled"></a>


This add-on for SCP: Secret Laboratory brings new features closer to the lore for the existing SCP-106. The plugin provides a number of customizable features to enhance your server's gaming or RP experience.
</div>

## Features

- **Pocket:** 106 can return to her own pocket dimension (unless the warhead has exploded in order not to spoil the game experience).
- **Pocket in:** 106 can pull a scp he goes near to his pocket dimension (Scp trying to be taken to pocket can reject it by pressing the [ALT] key).
- **Stalk:** The most important feature is that if there is an injured victim, you can teleport to a place where he is with the stalk feature (the victim is warned 1 second in advance).
- **Teleport :** You can teleport to any room you want (you can only go to rooms in the same zone if only the same zone setting in the config is on).
- **Flexible management:** With the config file, you can determine which feature will cost how much vigor or healt, how many meters away the victim can be teleported to the nearby victim with Stalk, or how long the cooldown time will be given to Scp-106 by which feature. Additionally, some features can be turned on or off optionally.

## Experimental feature

- **Pocket exit random zone mode:** When players successfully exit the pocket dimension, they exit from a random zone depending on chance instead of the same zone.
- **Realistic Pocket mode:** Pocket dimension is not affected by warhead explosion and effect.
- **One Hit Pocket:** 106 sends its victims directly to the pocket in one hit.

## Installation

1. Download the release file from the GitHub page [here](https://github.com/MS-crew/BetterScp106/releases).
2. Extract the contents into your `\AppData\Roaming\EXILED\Plugins` directory.
3. Configure the plugin according to your server’s needs using the provided settings.
4. Restart your server to apply the changes.

## Feedback and Issues

This is the initial release of the plugin. We welcome any feedback, bug reports, or suggestions for improvements.

- **Report Issues:** [Issues Page](https://github.com/MS-crew/BetterScp106/issues)
- **Contact:** [discerrahidenetim@gmail.com](mailto:discerrahidenetim@gmail.com)

Thank you for using our plugin and helping us improve it!
## Default Config
```yml
is_enabled: true
debug: false
# What features should be turned on??
pocket_feature: true
pocketin_feature: true
stalk_feature: true
teleport_rooms_feature: true
one_hit_pocket: false
# Button setting ids of features
ability_setting_ids:
  PocketKey: 106
  PocketinKey: 107
  StalkKey: 108
  StalkMode: 109
  StalkDistanceSlider: 110
  TeleportRoomsList: 111
  TeleportRooms: 112
# Should players exit a random zone when they exit the pocket dimension?
pocketexit_random_zonemode: true
# Pocket dimension is not affected by warhead explosion and effect
realistic_pocket: false
# Should you be reminded of your 106 powers every time you leave your pocket?
reminders: true
# How much health does it cost to go to pocket dimension?
pocketdimension_cost_healt: 50
# How much Vigor/106 energy does it cost to go to pocket dimension?
pocketdimension_cost_vigor: 25
# How much cooldown should be after the Pocketdim?
after_pocketdimension_cooldown: 30
# How much health does it cost pocketing a scp?
pocketin_cost_healt: 150
# How much Vigor/106 energy does it cost pocketing a scp?
pocketin_cost_vigor: 100
# How much cooldown should be after Pocketing a scp?
after_pocketing_scp_cooldown: 60
# How much cooldown should be after canceling Pocketing a scp?
canceled_pocketing_scp_cooldown: 15
# How much healt would a successful stalk cost?
stalk_cost_healt: 150
# How much Vigor/106 energy would a successful stalk cost?
stalk_cost_vigor: 25
# How much cooldown should be after Stalk?
after_stalk_cooldown: 90
# How far can you go to victims with Stalk.
stalk_distance: 200
# Stalk from any distance.
stalk_from_everywhere: false
# How low should the health of the target to be stalked be? 106 tracks moribund targets, so the target to be stalked will be the one with the lowest health and the one you set. (if you want him to be able to stalk everyone, you can just write 101)
stalk_targetmax_healt: 90
# Warn victim before Stalk.
stalk_warning: true
# How many seconds before target warning in Stalks (If the stalk warning is on)
stalk_warning_before: 1
# Which roles can be Stalked?
stalkable_roles:
- ClassD
- Scientist
- FacilityGuard
- ChaosConscript
- ChaosMarauder
- ChaosRepressor
- ChaosRifleman
- NtfCaptain
- NtfPrivate
- NtfSergeant
- NtfSpecialist
# Teleport Mode.
teleport_only_same_zone: false
# How much healt would a successful Teleport cost?
teleport_cost_healt: 25
# How much Vigor/106 energy would a successful Teleport cost?
teleport_cost_vigor: 25
# How much cooldown should be after Teleport?
teleport_cooldown: 5
# Rooms to teleport to?
rooms:
- LczArmory
- Lcz914
- LczCafe
- LczPlants
- LczToilets
- Lcz173
- LczClassDSpawn
- LczGlassBox
- Lcz330
- LczCheckpointB
- LczCheckpointA
- Hcz079
- HczEzCheckpointA
- HczEzCheckpointB
- HczArmory
- Hcz939
- HczHid
- Hcz049
- Hcz106
- HczNuke
- Hcz096
- HczTestRoom
- HczElevatorA
- HczElevatorB
- HczCrossRoomWater
- EzIntercom
- EzGateA
- EzGateB
- EzDownstairsPcs
- EzPcs
- EzConference
- EzChef
- EzCafeteria
- EzUpstairsPcs
- Surface
```
