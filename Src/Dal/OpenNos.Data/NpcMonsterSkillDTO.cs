using System;

namespace OpenNos.Data
{
    [Serializable]
    public class NpcMonsterSkillDTO : MappingBaseDTO
    {
        #region Properties

        public long NpcMonsterSkillId { get; set; }

        public short NpcMonsterVNum { get; set; }

        public short Rate { get; set; }

        public short SkillVNum { get; set; }

        #endregion
    }
}