using System.Text;
using System.Linq;
using NorthwoodLib.Pools;
using Exiled.API.Features;
using System.Collections.Generic;
using UserSettings.ServerSpecific;

namespace BetterScp106
{
    public class SettingHandlers
    {
        public static ServerSpecificSettingBase[] Better106Menu()
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            if (Plugin.C.PocketFeature)
            {
                stringBuilder.AppendLine(Plugin.T.Scp106PowersPocket.Replace("$pockethealt", Plugin.C.PocketdimensionCostHealt.ToString()).Replace("$pocketvigor", Plugin.C.PocketdimensionCostVigor.ToString()));
                stringBuilder.AppendLine();
            }
            if (Plugin.C.PocketinFeature)
            {
                stringBuilder.AppendLine(Plugin.T.Scp106PowersPocketin.Replace("$pocketinhealt", Plugin.C.PocketinCostHealt.ToString()).Replace("$pocketinvigor", Plugin.C.PocketinCostVigor.ToString()));
                stringBuilder.AppendLine();
            }
            if (Plugin.C.StalkFeature)
            {
                stringBuilder.AppendLine(Plugin.T.Scp106PowersStalk.Replace("$stalkhealt", Plugin.C.StalkCostHealt.ToString()).Replace("$stalkvigor", Plugin.C.StalkCostVigor.ToString()));
                stringBuilder.AppendLine();
            }
            if (Plugin.C.TeleportRoomsFeature)
                stringBuilder.AppendLine(Plugin.T.Scp106PowersTeleport.Replace("$teleporthealt", Plugin.C.TeleportCostHealt.ToString()).Replace("$teleportvigor", Plugin.C.TeleportCostVigor.ToString()));

            List<ServerSpecificSettingBase> mainMenu =
            [
                new SSGroupHeader ("Better Scp-106"),
                new SSTextArea(id: new int?(),StringBuilderPool.Shared.ToStringReturn(stringBuilder), SSTextArea.FoldoutMode.ExtendedByDefault),
 
            ];

            if (Plugin.C.PocketFeature) 
            {  
                mainMenu.Add(new SSKeybindSetting(id: Plugin.C.AbilitySettingIds[Methods.Features.PocketKey],"Pocket Dimension Key", suggestedKey: UnityEngine.KeyCode.F, hint: "The button you'll use to go to the pocket"));
                Log.Debug("Pocket feature is Enabled");
            }

            if (Plugin.C.PocketinFeature)
            {
                mainMenu.Add(new SSKeybindSetting(id: Plugin.C.AbilitySettingIds[Methods.Features.PocketinKey], "Pocket In Key", suggestedKey: UnityEngine.KeyCode.G, hint: "The button you use to bring your friend to the pocket"));
                Log.Debug("Pocket In feature is Enabled");
            }

            if (Plugin.C.StalkFeature)
            { 
                mainMenu.Add(new SSKeybindSetting(id: Plugin.C.AbilitySettingIds[Methods.Features.StalkKey],"Stalk Key", suggestedKey: UnityEngine.KeyCode.H, hint: "The button you want to stalk"));
                mainMenu.Add(new SSTwoButtonsSetting(id: Plugin.C.AbilitySettingIds[Methods.Features.StalkMode], "Stalk mode", "Distance", "Healt", defaultIsB: true, hint: "According to this setting, you either stalk the closest person at first, or more logical according to lore, the person with the lowest health."));
                mainMenu.Add(new SSSliderSetting(id: Plugin.C.AbilitySettingIds[Methods.Features.StalkDistanceSlider], "Stalk Distance", 0.0f, Plugin.C.StalkFromEverywhere ? 20000 : Plugin.C.StalkDistance, defaultValue: Plugin.C.StalkFromEverywhere ? 20000 : Plugin.C.StalkDistance, integer: true, hint: "The size of the local radius that can be stalked (the server owner determines the maximum distance)"));
                Log.Debug("Stalk feature is Enabled");
            }

            if (Plugin.C.TeleportRoomsFeature)
            {    
                mainMenu.Add(new SSDropdownSetting(id: Plugin.C.AbilitySettingIds[Methods.Features.TeleportRoomsList], "Rooms", Plugin.C.Rooms.Select(room => room.ToString()).ToArray()));
                mainMenu.Add(new SSButton(id: Plugin.C.AbilitySettingIds[Methods.Features.TeleportRooms], "Teleport Room", "Teleport", holdTimeSeconds: 2f));
                Log.Debug("Teleport Room feature is Enabled");
            }
            return [.. mainMenu];
        }
    }
}
