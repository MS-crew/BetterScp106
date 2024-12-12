using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterScp106
{
    using Exiled.API.Features;
    public class Translation : ITranslation
    {
        [Description("The broadcast when Scp 106 goes to the pocket dimension")]
        public Broadcast Scp106inpocket { get; set; } = new Broadcast("<color=red>Ahh ,Home sweet home...</color>", 4);

        [Description("The broadcast when Scp 106 trying go to pocket dimension but its already pocket")]
        public Broadcast Scp106alreadypocket { get; set; } = new Broadcast("<color=red>You are already in pocket dimension?</color>", 3);

        [Description("There is not enough health or vigor to for go to pocket")]
        public Broadcast Scp106cantpocket { get; set; } = new Broadcast("<color=red>You don't have enough energy or health to return to your kingdom!</color>", 3);

        [Description("Message that will appear when scp 106 tries to take someone to pocket")]
        public Broadcast Scp106ReqFriendinpocket { get; set; } = new Broadcast("<color=red>Scp-106 trying to take you to Pocket, if you don't want it, JUMP! in 3 seconds</color>", 3);

        [Description("What Scp 106 will see when it try pocket in but can't find a Scp")]
        public Broadcast Scp106CantFindFriend { get; set; } = new Broadcast("<color=red>There are no SCPs here or you're not close enough.</color>", 4);

        [Description("The broadcast when The Scp bow that Scp 106 carries goes to the pocket dimension")]
        public Broadcast Scp106Friendinpocket { get; set; } = new Broadcast("<color=red>Welcome to my home friend...</color>", 4);

        [Description("If the scp that scp106 is trying to throw into the pocket dimension refuses to enter the pocket dimension, the message that will appear to 106")]
        public Broadcast Scp106friendrefusedlpocketin { get; set; } = new Broadcast("<color=red>Your friend refused to go into your pocket!</color>", 3);

        [Description("Warning message to the victim of Scp106 (Warns the victim the second before set in the Config)")]
        public Broadcast StalkVictimWarn { get; set; } = new Broadcast("<color=red>A black liquid rises from the ground. Coming for you RUN!!!</color>", 4);

        [Description("Broadcast message to Scp 106 after stalking the victim")]
        public Broadcast StalkSuccesfull { get; set; } = new Broadcast("<color=red>Ahhh you smell something... The smell of suffering</color>", 4);

        [Description("if the victim to be stalk died immediately")]
        public Broadcast StalkFailed { get; set; } = new Broadcast("<color=red>Your target is no longer alive</color>", 3);

        [Description("If no suffering target was found in the stalk")]
        public Broadcast StalkNoTarget { get; set; } = new Broadcast("<color=red>You cant find any victim.</color>", 3);

        [Description("There is not enough health or vigor to stalk or there is a cooldown")]
        public Broadcast StalkCant { get; set; } = new Broadcast("<color=red>You can't stalk right now, you don't have enough energy or health, or you try to do it too often.</color>", 3);

        [Description("There is not enough health or vigor to Teleport")]
        public Broadcast TeleportCant { get; set; } = new Broadcast("<color=red>You cant't Teleport right now, you don't have enough energy or health.</color>", 3);

        [Description("The room to be teleported was destroyed or Danger")]
        public Broadcast TeleportRoomDanger { get; set; } = new Broadcast("<color=red>You can't teleport to this room, it's being destroyed or terminated.</color>", 3);

        [Description("You must be in the same zone to teleport")]
        public Broadcast TeleportCantforZone { get; set; } = new Broadcast("<color=red>You can't teleport here, you have to be in the same zone.</color>", 3);

        [Description("The broadcast that appears when trying to go to the pocket dimension after the warhead explodes")]
        public Broadcast Afternuke { get; set; } = new Broadcast("<color=red>The urge inside you pushes you out to hunt...</color>", 4);

        [Description("Cooldown warning")]
        public Broadcast Cooldown { get; set; } = new Broadcast("<color=red>You can't change dimension that often! You should wait a bit before changing it again.</color>", 4);

        [Description("Message to be shown to player spawn as Scp-106")]
        public string Scp106StartMessage { get; set; } = "<voffset=-8em><color=red>Scp106 has buffed, you can learn its features </color></voffset> \n <voffset=-0.05em><color=blue>'by Server Specific tab'</color> <color=red>on Setting.</color></voffset>";

        [Description("Pocket feature description")]
        public string Scp106PowersPocket { get; set; } = "Pocket : You can go to pocket dimension. It will cost: $pockethealt healt & $pocketvigor vigor";

        [Description("Pocket in feature description")]
        public string Scp106PowersPocketin { get; set; } = "Pocket in : You can put a SCP in your pocket dimension. It will cost: $pocketinhealt healt & $pocketinvigor vigor";

        [Description("Stalk feature description")]
        public string Scp106PowersStalk { get; set; } = "Stalk : You can teleport to a suffering victim. It will cost: $stalkhealt healt & $stalkvigor vigor";

        [Description("Teleport feature description")]
        public string Scp106PowersTeleport { get; set; } = "Teleport : You can get teleport in the room you want . It will cost: $teleporthealt healt & $teleportvigor vigor";

    }
}
