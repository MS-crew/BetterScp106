<h1 align="center">Better Scp-106</h1>
<div align="center">
<a href="https://github.com/MS-crew/BetterScp106/releases"><img src="https://img.shields.io/github/downloads/MS-crew/BetterScp106/total?style=for-the-badge&logo=githubactions&label=Downloads" href="https://github.com/MS-crew/BetterScp106/releases" alt="GitHub Release Download"></a>
<a href="https://github.com/MS-crew/BetterScp106/releases"><img src="https://img.shields.io/badge/Build-2.6.4-brightgreen?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/BetterScp106/releases" alt="GitHub Releases"></a>
<a href="https://github.com/MS-crew/BetterScp106/blob/Ss-Menu/LICENSE.txt"><img src="https://img.shields.io/badge/Licence-GNU_3.0-blue?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/BetterScp106/blob/Ss-Menu/LICENSE.txt" alt="General Public License v3.0"></a>
<a href="https://github.com/ExMod-Team/EXILED"><img src="https://img.shields.io/badge/Exiled-9.6.0-green?style=for-the-badge&logo=gitbook" href="https://github.com/ExMod-Team/EXILED" alt="GitHub Exiled"></a>


This add-on for SCP: Secret Laboratory introduces new features and enhancements for the existing SCP-106 character, aligning them more closely with the game's core lore. Better SCP-106 offers a range of customizable functionalities designed to elevate your server's gaming or role-playing (RP) experience.
</div>

## Features
Empower SCP-106 with exciting new abilities using Better SCP-106:
- **Pocket Dimension (Pocket):** SCP-106 can return to its own pocket dimension. This feature is disabled when the warhead has detonated, ensuring it doesn't spoil the game experience.
- **Pocket-in:** SCP-106 can pull an SCP in its vicinity into its pocket dimension. The targeted SCP can reject being pulled by pressing the [ALT] key.
- **Stalk:** If there's an injured victim, SCP-106 can teleport to their location using the stalk feature. The victim is default warned 1 second in advance. This is a critical feature for strategic hunting and surprise attacks.
- **Teleport Room:** SCP-106 can teleport to any desired room. If the "same zone only" setting is enabled in the configuration, teleportation is restricted to rooms within the current zone.
- **Flexible management:** Through the provided configuration file (config.yml), you can precisely determine the cost (health or SCP-106 energy) and cooldown for each feature. Additionally, certain features can be optionally enabled or disabled, providing full control to suit your server's specific needs.

## Features Commands 
- **Pocket:** `.GotoPocket`
- **Pocket in:** `.Pocketin`
- **Stalk:** `.Stalk`
- **Teleport Room:**
   - To see available rooms: .TeleportRoom rooms
   - To teleport to a specific room: .TeleportRoom {RoomName} (Example: .TeleportRoom Lcz914)

## Experimental feature
These features are currently under development and provided for testing purposes. Your feedback is highly valuable!

- **Pocket Exit Random Zone Mode:** When players successfully exit the pocket dimension, they will exit from a random zone based on chance, instead of always the same zone.
- **Realistic Pocket Mode:**  The pocket dimension is unaffected by warhead explosions and their effects. This allows SCP-106 to retreat to a safer zone with its victims.
- **One Hit Pocket:** SCP-106 sends its victims directly to the pocket dimension with a single hit. Ideal for a more aggressive playstyle.

## Dependicies
- SSMenuSystem [here](https://github.com/skyfr0676/SSMenuSystem)

## Installation

1. Download the release file from the GitHub page [here](https://github.com/MS-crew/BetterScp106/releases).
2. Extract the contents into your `\AppData\Roaming\EXILED\Plugins` directory.
3. Restart your server to generate the configuration and translation files.
4. Configure the plugin according to your server's needs using the provided settings in the config.yml file located at AppData\Roaming\EXILED\Plugins\BetterScp106.
5. Restart your server once more to apply all changes.

## Feedback and Issues

This is the initial release of the plugin. We welcome any feedback, bug reports, or suggestions for improvements. Your contributions help us make the plugin even better!

- **Report Issues:** [Issues Page](https://github.com/MS-crew/BetterScp106/issues)
- **Contact:** [discerrahidenetim@gmail.com](mailto:discerrahidenetim@gmail.com)

Thank you for using our plugin and helping us improve it!
## Default Config
Below is an example of the plugin's default config.yml file. You can modify these values to suit your server's specific requirements.
```yml
is_enabled: true
debug: false
# What features should be turned on??
pocket_feature: true
# Pocket-in feature enabled
pocketin_feature: true
# Stalk feature enabled
stalk_feature: true
# Teleport rooms feature enabled
teleport_rooms_feature: true
# One-hit pocket feature enabled
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
  Description: 113
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
# How much healt does it cost pocketing a scp?
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
teleport_cooldown: 50
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
