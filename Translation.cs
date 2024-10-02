namespace BetterScp106
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    public class Translation : ITranslation
    {
        [Description("The broadcast when Scp 106 goes to the pocket dimension")]
        public Broadcast scp106inpocket { get; set; } = new Broadcast("<color=red>Ahh ,Home sweet home...</color>", 4);

        [Description("The broadcast when Scp 106 trying go to pocket dimension but its already pocket")]
        public Broadcast scp106alreadypocket { get; set; } = new Broadcast("<color=red>You are already in pocket dimension?</color>", 3);

        [Description("There is not enough health or vigor to stalk for go to pocket")]
        public Broadcast scp106cantpocket { get; set; } = new Broadcast("<color=red>You don't have enough energy or health to return to your kingdom!</color>", 3);

        [Description("Message that will appear when scp 106 tries to take someone to pocket")]
        public Broadcast scp106ReqFriendinpocket { get; set; } = new Broadcast("<color=red>Scp-106 trying to take you to Pocket, if you don't want it, JUMP! in 3 seconds</color>", 3);

        [Description("The broadcast when The Scp bow that Scp 106 carries goes to the pocket dimension")]
        public Broadcast scp106Friendinpocket { get; set; } = new Broadcast("<color=red>Welcome to my home friend...</color>", 4);

        [Description("If the scp that scp106 is trying to throw into the pocket dimension refuses to enter the pocket dimension, the message that will appear to 106")]
        public Broadcast scp106friendrefusedlpocketin { get; set; } = new Broadcast("<color=red>Your friend refused to go into your pocket!</color>", 3);

        [Description("Warning message to the victim of Scp106 (It warns the victim 1 second before, you can just leave it blank if you do not want it)")]
        public Broadcast StalkVictimWarn { get; set; } = new Broadcast("<color=red>A black liquid rises from the ground. Coming for you RUN!!!</color>", 4);

        [Description("Broadcast message to Scp 106 after stalking the victim")]
        public Broadcast StalkSuccesfull { get; set; } = new Broadcast("<color=red>Ahhh you smell something... The smell of suffering</color>", 4);

        [Description("if the victim to be stalk died immediately")]
        public Broadcast StalkFailed { get; set; } = new Broadcast("<color=red>Your target is no longer alive</color>", 3);

        [Description("If no suffering target was found in the stalk")]
        public Broadcast StalkNoTarget { get; set; } = new Broadcast("<color=red>You cant find any victim.</color>", 3);

        [Description("There is not enough health or vigor to stalk or there is a cooldown")]
        public Broadcast StalkCant { get; set; } = new Broadcast("<color=red>You can't stalk right now, you don't have enough energy or health, or you try to do it too often.</color>", 3);

        [Description("The broadcast that appears when trying to go to the pocket dimension after the warhead explodes")]
        public Broadcast afternuke { get; set; } = new Broadcast("<color=red>The urge inside you pushes you out to hunt...</color>", 4);

        [Description("Cooldown warning")]
        public Broadcast cooldown { get; set; } = new Broadcast("You can't change dimension that often! You should wait a bit before changing it again.", 4);

        [Description("Message to be shown to player spawn as Scp-106")]
        public string Scp106StartMessage { get; set; } = "<voffset=-8em><color=red>Scp106 has buffed, you can learn its features by typing</color></voffset> \n <voffset=-0.05em><color=blue>'.Better106'</color><color=red> or</color> <color=blue>'.106'</color> <color=red>on the console.</color></voffset>";

        [Description("Pocket feature description")]
        public string Scp106PowersPocket { get; set; } = "Pocket: You can go to pocket dimension with [C]/sneak key or '.pocket' or '.pd' command. It will cost: $pockethealt healt & $pocketvigor vigor";

        [Description("Pocket in feature description")]
        public string Scp106PowersPocketin { get; set; } = "Pocket in: You can put a SCP in your pocket dimension with '.pocket in' or '.pd in' command. It will cost: $pocketinhealt healt & $pocketinvigor vigor";

        [Description("Stalk feature description")]
        public string Scp106PowersStalk { get; set; } = "Stalk: You can teleport to a suffering victim with '.stalk' or '.sk' command. It will cost: $stalkhealt healt & $stalkvigor vigor";
    }
}
