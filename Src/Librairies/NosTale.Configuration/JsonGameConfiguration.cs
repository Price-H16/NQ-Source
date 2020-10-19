using NosTale.Configuration.Configuration.Server;

namespace NosTale.Configuration
{
    public class JsonGameConfiguration
    {
        // Here some Config ↓
        public ServerConfiguration Server { get; set; } = new ServerConfiguration
        {
            //Auth
            AuthentificationServiceAuthKey = "llTestServerAuth14031941",
            MasterAuthKey = "llMasterAuthzzz14031941",
            LogKey = "LogAuth147", // this isn't being used as far as i know

            // Config ip/port
            LogerPort = 6970,
            IPAddress = "89.203.249.171",
            MasterIP = "127.0.0.1",
            Act4Port = 5100,
            LoginPort = 4001,
            MasterPort = 6969,
            WorldPort = 5000,

            // Config Srv
            ServerGroupS1 = "NosQuest",
            Language = "uk",
            SessionLimit = 150,
            LagMode = false,
            SceneOnCreate = false,
            UseOldCrypto = false,
            WorldInformation = true
        };

        public RateConfiguration Rate { get; set; } = new RateConfiguration
        {
            CylloanPercentRate = 10,
            GlacernonPercentRatePvm = 5,
            GlacernonPercentRatePvp = 1,
            RateXP = 3,
            PartnerSpXp = 2,
            QuestDropRate = 1,
            RateDrop = 35,
            RateFairyXP = 10,
            RateGold = 35,
            RateGoldDrop = 2,
            RateHeroicXP = 10,
            RateReputation = 1
        };

        public EventConfiguration Event { get; set; } = new EventConfiguration
        {
            ChristmasEvent = false,
            HalloweenEvent = false
        };

        public MaxConfiguration Max { get; set; } = new MaxConfiguration
        {
            HeroicStartLevel = 88,
            MaxGold = int.MaxValue,
            MaxGoldBank = 100000000000,
            MaxHeroLevel = 60,
            MaxJobLevel = 80,
            MaxLevel = 99,
            MaxSPLevel = 99,
            MaxUpgrade = 10
        };
    }
}