<h1 align="center">Better Scp-106</h1>
<div align="center">
<a href="https://github.com/MS-crew/BetterScp106"><img src="https://img.shields.io/github/actions/workflow/status/Exiled-Team/EXILED/main.yml?style=for-the-badge&logo=githubactions&label=build" href="https://github.com/MS-crew/BetterScp106" alt="GitHub Source Code"></a>
<a href="https://github.com/MS-crew/BetterScp106/releases"><img src="https://img.shields.io/badge/Build-1.5.7-brightgreen?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/BetterScp106/releases" alt="GitHub Releases"></a>
<a href="https://github.com/MS-crew/BetterScp106/blob/master/LICENSE"><img src="https://img.shields.io/badge/Licence-GNU_3.0-blue?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/BetterScp106/blob/master/LICENSE" alt="General Public License v3.0"></a>
<a href="https://github.com/ExMod-Team/EXILED"><img src="https://img.shields.io/badge/Exiled-8.12.2-red?style=for-the-badge&logo=gitbook" href="https://github.com/ExMod-Team/EXILED" alt="GitHub Exiled"></a>


This add-on for SCP: Secret Laboratory brings new features closer to the lore for the existing SCP-106. The plugin provides a number of customizable features to enhance your server's gaming or RP experience.
</div>

## Commands Features

- **Pocket:** With this command, 106 can return to her own pocket dimension (unless the warhead has exploded in order not to spoil the game experience/and now you can do it with the [C]/sneak walk key).
- **Pocket in:** With this command, 106 can pull a scp he goes near to his pocket dimension (Scp trying to be taken to pocket can reject it by pressing the [ALT] key).
- **Stalk:** The most important feature is that if there is an injured victim, you can teleport to a place where he is with the stalk feature (the victim is warned 1 second in advance/and now you can do it with the [ALT] key).
- **Better106:** It tells you all the features of the 106 and how much they will cost.
- **Flexible management:** With the config file, you can determine which feature will cost how much vigor or healt, how many meters away the victim can be teleported to the nearby victim with Stalk, or how long the cooldown time will be given to Scp-106 by which feature.

## Experimental feature

- **Pocket exit random zone mode:** When players successfully exit the pocket dimension, they exit from a random zone depending on chance instead of the same zone.
- **Realistic Pocket mode:** Pocket dimension is not affected by warhead explosion and effect.

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
Is_enabled: true
debug: false
# Should players exit a random zone when they exit the pocket dimension?
pocketexit_random_zonemode: true
# Pocket dimension is not affected by warhead explosion and effect
realistic_pocket: true
# Should you be reminded of your 106 powers every time you leave your pocket?
reminders: true
# Is it possible to stalk with the Alt key?
altwith_stalk: true
# How much health does it cost to go to pocket dimension?
pocketdimension_cost_healt: 50
# How much Vigor/106 energy does it cost to go to pocket dimension?
pocketdimension_cost_vigor: 100
# How much cooldown should be after the Pocketdim?
after_pocketdimension_cooldown: 30
# How much health does it cost pocketing a scp?
pocketin_cost_healt: 200
# How much Vigor/106 energy does it cost pocketing a scp?
pocketin_cost_vigor: 100
# How much cooldown should be after Pocketing a scp?
after_pocketing_scp_cooldown: 60
# How much cooldown should be after canceling Pocketing a scp?
canceled_pocketing_scp_cooldown: 15
# How far can you go to victims with Stalk. If you want everywhere you can enter crazy numbers like 99999
stalk_distance: 200
# How low should the health of the target to be stalked be? 106 tracks moribund targets, so the target to be stalked will be the one with the lowest health and the one you set. (if you want him to be able to stalk everyone, you can just write 101)
stalk_targetmax_healt: 90
# How much healt would a successful stalk cost?
stalk_cost_healt: 150
# How much Vigor/106 energy would a successful stalk cost?
stalk_cost_vigor: 25
# How much cooldown should be after Stalk?
after_stalk_cooldown: 90
```
