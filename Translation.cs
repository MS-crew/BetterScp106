// -----------------------------------------------------------------------
// <copyright file="Translation.cs" company="Ms-crew">
// Copyright (c) Ms-crew. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BetterScp106
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    /// <summary>
    /// Represents the translation settings for BetterScp106 plugin.
    /// </summary>
    public class Translation : ITranslation
    {
        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 enters the pocket dimension.
        /// </summary>
        [Description("The broadcast when Scp 106 goes to the pocket dimension")]
        public Broadcast Scp106inpocket { get; set; } = new Broadcast("<color=red>Ahh ,Home sweet home...</color>", 4);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 tries to enter the pocket dimension but is already there.
        /// </summary>
        [Description("The broadcast when Scp 106 trying go to pocket dimension but its already pocket")]
        public Broadcast Scp106alreadypocket { get; set; } = new Broadcast("<color=red>You are already in pocket dimension?</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 lacks health or vigor to enter the pocket dimension.
        /// </summary>
        [Description("There is not enough health or vigor to for go to pocket")]
        public Broadcast Scp106cantpocket { get; set; } = new Broadcast("<color=red>You don't have enough energy or health to return to your kingdom!</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 tries to take someone to the pocket dimension.
        /// </summary>
        [Description("Message that will appear when scp 106 tries to take someone to pocket")]
        public Broadcast Scp106ReqFriendinpocket { get; set; } = new Broadcast("<color=red>Scp-106 trying to take you to Pocket, if you don't want it, JUMP! in 3 seconds</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 cannot find another SCP to take to the pocket dimension.
        /// </summary>
        [Description("What Scp 106 will see when it try pocket in but can't find a Scp")]
        public Broadcast Scp106CantFindFriend { get; set; } = new Broadcast("<color=red>There are no SCPs here or you're not close enough.</color>", 4);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 successfully takes another SCP to the pocket dimension.
        /// </summary>
        [Description("The broadcast when The Scp bow that Scp 106 carries goes to the pocket dimension")]
        public Broadcast Scp106Friendinpocket { get; set; } = new Broadcast("<color=red>Welcome to my home friend...</color>", 4);

        /// <summary>
        /// Gets or sets the broadcast shown when another SCP refuses to enter the pocket dimension.
        /// </summary>
        [Description("If the scp that scp106 is trying to throw into the pocket dimension refuses to enter the pocket dimension, the message that will appear to 106")]
        public Broadcast Scp106friendrefusedlpocketin { get; set; } = new Broadcast("<color=red>Your friend refused to go into your pocket!</color>", 3);

        /// <summary>
        /// Gets or sets the warning broadcast shown to the victim of SCP-106 before being stalked.
        /// </summary>
        [Description("Warning message to the victim of Scp106 (Warns the victim the second before set in the Config)")]
        public Broadcast StalkVictimWarn { get; set; } = new Broadcast("<color=red>A black liquid rises from the ground. Coming for you RUN!!!</color>", 4);

        /// <summary>
        /// Gets or sets the broadcast shown to SCP-106 after successfully stalking a victim.
        /// </summary>
        [Description("Broadcast message to Scp 106 after stalking the victim")]
        public Broadcast StalkSuccesfull { get; set; } = new Broadcast("<color=red>Ahhh you smell something... The smell of suffering</color>", 4);

        /// <summary>
        /// Gets or sets the broadcast shown when the victim of stalking dies immediately.
        /// </summary>
        [Description("if the victim to be stalk died immediately")]
        public Broadcast StalkFailed { get; set; } = new Broadcast("<color=red>Your target is no longer alive</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when no target is found for stalking.
        /// </summary>
        [Description("If no suffering target was found in the stalk")]
        public Broadcast StalkNoTarget { get; set; } = new Broadcast("<color=red>You cant find any victim.</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 cannot stalk due to insufficient health, vigor, or cooldown.
        /// </summary>
        [Description("There is not enough health or vigor to stalk or there is a cooldown")]
        public Broadcast StalkCant { get; set; } = new Broadcast("<color=red>You can't stalk right now, you don't have enough energy or health, or you try to do it too often.</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 cannot teleport due to insufficient health or vigor.
        /// </summary>
        [Description("There is not enough health or vigor to Teleport")]
        public Broadcast TeleportCant { get; set; } = new Broadcast("<color=red>You can't Teleport right now, you don't have enough energy or health.</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when the room to teleport to is destroyed or dangerous.
        /// </summary>
        [Description("The room to be teleported was destroyed or Danger")]
        public Broadcast TeleportRoomDanger { get; set; } = new Broadcast("<color=red>You can't teleport to this room, it's being destroyed or terminated.</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when the room to teleport to is null.
        /// </summary>
        [Description("The room to be teleported was null")]
        public Broadcast TeleportRoomNull { get; set; } = new Broadcast("<color=red>You can't teleport to this room, this room lost in a different dimension.</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 must be in the same zone to teleport.
        /// </summary>
        [Description("You must be in the same zone to teleport")]
        public Broadcast TeleportCantforZone { get; set; } = new Broadcast("<color=red>You can't teleport here, you have to be in the same zone.</color>", 3);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 tries to enter the pocket dimension after the warhead explodes.
        /// </summary>
        [Description("The broadcast that appears when trying to go to the pocket dimension after the warhead explodes")]
        public Broadcast Afternuke { get; set; } = new Broadcast("<color=red>The urge inside you pushes you out to hunt...</color>", 4);

        /// <summary>
        /// Gets or sets the broadcast shown when SCP-106 tries to change dimensions too frequently.
        /// </summary>
        [Description("Cooldown warning")]
        public Broadcast Cooldown { get; set; } = new Broadcast("<color=red>You can't change dimension that often! You should wait a bit before changing it again.</color>", 4);

        /// <summary>
        /// Gets or sets the message shown to players when they spawn as SCP-106.
        /// </summary>
        [Description("Message to be shown to player spawn as Scp-106")]
        public string Scp106StartMessage { get; set; } = "<voffset=-9m><color=red>Scp106 has buffed, you can learn its features </color></voffset> \n <voffset=-0.05em><color=blue>'by Server Specific tab'</color> <color=red>on Setting.</color></voffset>";

        /// <summary>
        /// Gets or sets the description of the pocket feature.
        /// </summary>
        [Description("Pocket feature description. ({0} = Healt , {1} = vigor)")]
        public string Scp106PowersPocket { get; set; } = "Pocket : You can go to pocket dimension. It will cost: {0} healt & {1} vigor";

        /// <summary>
        /// Gets or sets the description of the pocket-in feature.
        /// </summary>
        [Description("Pocket in feature description. ({0} = Healt , {1} = vigor)")]
        public string Scp106PowersPocketin { get; set; } = "Pocket in : You can put a SCP in your pocket dimension. It will cost: {0} healt & {1} vigor";

        /// <summary>
        /// Gets or sets the description of the stalk feature.
        /// </summary>
        [Description("Stalk feature description. ({0} = Healt , {1} = vigor)")]
        public string Scp106PowersStalk { get; set; } = "Stalk : You can teleport to a suffering victim. It will cost: {0} healt & {1} vigor";

        /// <summary>
        /// Gets or sets the description of the teleport feature.
        /// </summary>
        [Description("Teleport feature description. ({0} = Healt , {1} = vigor)")]
        public string Scp106PowersTeleport { get; set; } = "Teleport : You can get teleport in the room you want . It will cost: {0} healt & {1} vigor";

        /// <summary>
        /// Gets or sets the server-specific settings for the pocket feature.
        /// </summary>
        [Description("Server Specific Settings Translates")]
        public string[] Pocket { get; set; } =
        [
            "Pocket Dimension Key",
            "The button you'll use to go to the pocket"
        ];

        /// <summary>
        /// Gets or sets the server-specific settings for the pocket-in feature.
        /// </summary>
        public string[] PocketIn { get; set; } =
        [
            "Pocket In Key",
            "The button you use to bring your friend to the pocket"
        ];

        /// <summary>
        /// Gets or sets the server-specific settings for the stalk feature.
        /// </summary>
        public string[] Stalk { get; set; } =
        [
            "Stalk Key",
            "The button you want to stalk",
            "Stalk mode",
            "Distance",
            "Healt",
            "According to this setting, you either stalk the closest person or the one with the lowest health.",
            "Stalk Distance",
            "The size of the local radius that can be stalked."
        ];

        /// <summary>
        /// Gets or sets the server-specific settings for the teleport feature.
        /// </summary>
        public string[] Teleport { get; set; } =
        [
            "Rooms",
            "Teleport Room",
            "Teleport"
        ];

        /// <summary>
        /// Gets or sets the command name for going to the pocket dimension.
        /// </summary>
        [Description("Go to Pocket Command Translations")]
        public string GotoPocketCommand { get; set; } = "GotoPocket";

        /// <summary>
        /// Gets or sets the aliases for the command to go to the pocket dimension.
        /// </summary>
        public string[] GotoPocketCommandAliases { get; set; } =
        [
            "gopd",
            "gopocket"
        ];

        /// <summary>
        /// Gets or sets the description for the command to go to the pocket dimension.
        /// </summary>
        public string GotoPocketCommandDescription { get; set; } = "Go to pocket dimension";

        /// <summary>
        /// Gets or sets the command name for stalking.
        /// </summary>
        [Description("Stalk Command Translations")]
        public string StalkCommand { get; set; } = "Stalk";

        /// <summary>
        /// Gets or sets the aliases for the command to stalk.
        /// </summary>
        public string[] StalkCommandAliases { get; set; } =
        [
            "sk",
            "stalking"
        ];

        /// <summary>
        /// Gets or sets the description for the command to stalk.
        /// </summary>
        public string StalkCommandDescription { get; set; } = "Stalk the closest victim";

        /// <summary>
        /// Gets or sets the command name for taking SCPs to the pocket dimension.
        /// </summary>
        [Description("Take Scps Pocket Command Translations")]
        public string TakeScpsPocketCommand { get; set; } = "Pocketin";

        /// <summary>
        /// Gets or sets the aliases for the command to take SCPs to the pocket dimension.
        /// </summary>
        public string[] TakeScpsPocketCommandAliases { get; set; } =
        [
            "takescp",
            "takepocket",
            "takescpinpocket"
        ];

        /// <summary>
        /// Gets or sets the description for the command to take SCPs to the pocket dimension.
        /// </summary>
        public string TakeScpsPocketCommandDescription { get; set; } = "Take one Scp to pocket dimension with you";

        /// <summary>
        /// Gets or sets the command name for teleporting to a room.
        /// </summary>
        [Description("Teleport Room Command Translations")]
        public string TeleportRoomCommand { get; set; } = "TeleportRoom";

        /// <summary>
        /// Gets or sets the aliases for the command to teleport to a room.
        /// </summary>
        public string[] TeleportRoomCommandAliases { get; set; } =
        [
            "tr",
            "tproom",
        ];

        /// <summary>
        /// Gets or sets the description for the command to teleport to a room.
        /// </summary>
        public string TeleportRoomCommandDescription { get; set; } = "Teleport to the room you want";
    }
}
