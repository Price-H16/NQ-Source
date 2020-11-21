using OpenNos.DAL.DAO;
using OpenNos.DAL.Interface;

namespace OpenNos.DAL
{
    public class DAOFactory
    {
        public static DAOFactory Instance { get; private set; }

        public static void Initialize(DAOFactory dao)
        {
            Instance = dao;
        }

        public DAOFactory(IAccountDAO accountDAO, IBazaarItemDAO bazaarItemDAO, ICardDAO cardDAO, IBoxItemDAO boxItemDAO, IBCardDAO bcardDAO, IRollGeneratedItemDAO rollGeneratedItemDAO,
            ICharacterTitleDAO characterTitleDAO, ICharacterVisitedMapsDAO characterVisitedMapsDAO, ICharacterDAO characterDAO, ICharacterRelationDAO characterRelationDAO, IChatLogDAO chatLogDAO, ICharacterSkillDAO characterskillDAO,
            ICharacterQuestDAO characterQuestDAO, IComboDAO comboDAO, IDropDAO dropDAO, IFamilyCharacterDAO familycharacterDAO, IFamilyDAO familyDAO,
            IFamilyLogDAO familylogDAO, IGeneralLogDAO generallogDAO, IItemDAO itemDAO, IItemInstanceDAO iteminstanceDAO, IFamilyQuestsDAO familyQuestsDAO, IFamilySkillMissionDAO familySkillMissionDAO,
            ILogsCommandsDAO logsCommandsDAO, IBotAuthorityDAO botAuthorityDAO, IMailDAO mailDAO, IMapDAO mapDAO, IMapMonsterDAO mapmonsterDAO, IMapNpcDAO mapnpcDAO, IMapTypeDAO maptypeDAO,
            IMapTypeMapDAO maptypemapDAO, IMateDAO mateDAO, IMinilandObjectDAO minilandobjectDAO, INpcMonsterDAO npcmonsterDAO, INpcMonsterSkillDAO npcmonsterskillDAO, IPenaltyLogDAO penaltylogDAO,
            IPortalDAO portalDAO, IQuestDAO questDAO, IQuestLogDAO questLogDAO, IQuestRewardDAO questRewardDAO, IQuestObjectiveDAO questObjectiveDAO, IQuicklistEntryDAO quicklistDAO,
            IMinigameLogDAO minigameLogDAO, IRecipeDAO recipeDAO, IRecipeItemDAO recipeitemDAO, IRespawnDAO respawnDAO, IRespawnMapTypeDAO respawnMapTypeDAO, IScriptedInstanceDAO scriptedinstanceDAO,
            IShopDAO shopDAO, IShopItemDAO shopitemDAO, IShopSkillDAO shopskillDAO, ISkillDAO skillDAO, IStaticBonusDAO staticBonusDAO, IStaticBuffDAO staticBuffDAO, ITeleporterDAO teleporterDAO,
            ICellonOptionDAO cellonOptionDAO, IMaintenanceLogDAO maintenanceLogDAO, IRuneEffectDAO runeEffectDAO, IShellEffectDAO shellEffectDAO, IPartnerSkillDAO partnerSkillDAO)
        {
            AccountDAO = accountDAO;
            BazaarItemDAO = bazaarItemDAO;
            CardDAO = cardDAO;
            BoxItemDAO = boxItemDAO;
            BCardDAO = bcardDAO;
            RollGeneratedItemDAO = rollGeneratedItemDAO;
            CellonOptionDAO = cellonOptionDAO;
            BotAuthorityDAO = botAuthorityDAO;
            CharacterDAO = characterDAO;
            CharacterRelationDAO = characterRelationDAO;
            CharacterQuestDAO = characterQuestDAO;
            CharacterSkillDAO = characterskillDAO;
            CharacterQuestDAO = characterQuestDAO;
            CharacterTitleDAO = characterTitleDAO;
            CharacterVisitedMapsDAO = characterVisitedMapsDAO;
            ComboDAO = comboDAO;
            DropDAO = dropDAO;
            FamilyQuestsDAO = familyQuestsDAO;
            FamilySkillMissionDAO = familySkillMissionDAO;
            FamilyCharacterDAO = familycharacterDAO;
            FamilyDAO = familyDAO;
            FamilyLogDAO = familylogDAO;
            GeneralLogDAO = generallogDAO;
            ItemDAO = itemDAO;
            IteminstanceDAO = iteminstanceDAO;
            MinigameLogDAO = minigameLogDAO;
            LogsCommandsDAO = logsCommandsDAO;
            MailDAO = mailDAO;
            MapDAO = mapDAO;
            MapMonsterDAO = mapmonsterDAO;
            MapNpcDAO = mapnpcDAO;
            MapTypeDAO = maptypeDAO;
            MapTypeMapDAO = maptypemapDAO;
            MateDAO = mateDAO;
            MinilandObjectDAO = minilandobjectDAO;
            NpcMonsterDAO = npcmonsterDAO;
            NpcMonsterSkillDAO = npcmonsterskillDAO;
            PenaltyLogDAO = penaltylogDAO;
            PortalDAO = portalDAO;
            QuestDAO = questDAO;
            QuestLogDAO = questLogDAO;
            QuestRewardDAO = questRewardDAO;
            QuestObjectiveDAO = questObjectiveDAO;
            QuicklistEntryDAO = quicklistDAO;
            ChatLogDAO = chatLogDAO;
            MaintenanceLogDAO = maintenanceLogDAO;
            RuneEffectDAO = runeEffectDAO;
            ShellEffectDAO = shellEffectDAO;
            PartnerSkillDAO = partnerSkillDAO;
            TeleporterDAO = teleporterDAO;
            RecipeDAO = recipeDAO;
            RecipeItemDAO = recipeitemDAO;
            RespawnDAO = respawnDAO;
            RespawnMapTypeDAO = respawnMapTypeDAO;
            ScriptedInstanceDAO = scriptedinstanceDAO;
            ShopDAO = shopDAO;
            ShopItemDAO = shopitemDAO;
            ShopSkillDAO = shopskillDAO;
            SkillDAO = skillDAO;
            StaticBonusDAO = staticBonusDAO;
            StaticBuffDAO = staticBuffDAO;
            TeleporterDAO = teleporterDAO;
        }


        public IAccountDAO AccountDAO { get; }

        public IBazaarItemDAO BazaarItemDAO { get; }

        public ICardDAO CardDAO { get; }

        public IBoxItemDAO BoxItemDAO { get; }

        public ICharacterDAO CharacterDAO { get; }

        public ICellonOptionDAO CellonOptionDAO { get; }

        public ICharacterRelationDAO CharacterRelationDAO { get; }

        public ICharacterSkillDAO CharacterSkillDAO { get; }

        public ICharacterQuestDAO CharacterQuestDAO { get; }

        public IComboDAO ComboDAO { get; }

        public IDropDAO DropDAO { get; }

        public IFamilyCharacterDAO FamilyCharacterDAO { get; }

        public IFamilyDAO FamilyDAO { get; }

        public IFamilyLogDAO FamilyLogDAO { get; }

        public IGeneralLogDAO GeneralLogDAO { get; }

        public IItemDAO ItemDAO { get; }

        public IItemInstanceDAO IteminstanceDAO { get; }

        public IBotAuthorityDAO BotAuthorityDAO { get; }

        public IFamilyQuestsDAO FamilyQuestsDAO { get; }

        public ILogsCommandsDAO LogsCommandsDAO { get; }

        public IMinigameLogDAO MinigameLogDAO { get; }

        public IMailDAO MailDAO { get; }

        public ICharacterTitleDAO CharacterTitleDAO { get; }

        public ICharacterVisitedMapsDAO CharacterVisitedMapsDAO { get; }

        public IChatLogDAO ChatLogDAO { get; }

        public IFamilySkillMissionDAO FamilySkillMissionDAO { get; }

        public IMaintenanceLogDAO MaintenanceLogDAO { get; }

        public IMapDAO MapDAO { get; }

        public IMapMonsterDAO MapMonsterDAO { get; }

        public IMapNpcDAO MapNpcDAO { get; }

        public IMapTypeDAO MapTypeDAO { get; }

        public IMapTypeMapDAO MapTypeMapDAO { get; }

        public IMateDAO MateDAO { get; }

        public IMinilandObjectDAO MinilandObjectDAO { get; }

        public INpcMonsterDAO NpcMonsterDAO { get; }

        public INpcMonsterSkillDAO NpcMonsterSkillDAO { get; }

        public IPenaltyLogDAO PenaltyLogDAO { get; }

        public IPortalDAO PortalDAO { get; }

        public IQuestDAO QuestDAO { get; }

        public IQuestLogDAO QuestLogDAO { get; }

        public IQuestObjectiveDAO QuestObjectiveDAO { get; }

        public IQuestRewardDAO QuestRewardDAO { get; }

        public IQuicklistEntryDAO QuicklistEntryDAO { get; }

        public IRuneEffectDAO RuneEffectDAO { get; }

        public IRecipeDAO RecipeDAO { get; }

        public IRecipeItemDAO RecipeItemDAO { get; }

        public IRespawnDAO RespawnDAO { get; }

        public IRespawnMapTypeDAO RespawnMapTypeDAO { get; }

        public IShopDAO ShopDAO { get; }

        public IShopItemDAO ShopItemDAO { get; }

        public IShopSkillDAO ShopSkillDAO { get; }

        public ISkillDAO SkillDAO { get; }

        public IStaticBonusDAO StaticBonusDAO { get; }

        public IStaticBuffDAO StaticBuffDAO { get; }

        public ITeleporterDAO TeleporterDAO { get; }

        public IScriptedInstanceDAO ScriptedInstanceDAO { get; }

        public IBCardDAO BCardDAO { get; }

        public IRollGeneratedItemDAO RollGeneratedItemDAO { get; }

        public IShellEffectDAO ShellEffectDAO { get; }

        public IPartnerSkillDAO PartnerSkillDAO { get; }
    }
}