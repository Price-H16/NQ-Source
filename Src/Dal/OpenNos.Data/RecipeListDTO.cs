using System;

namespace OpenNos.Data
{
    [Serializable]
    public class RecipeListDTO : MappingBaseDTO
    {
        #region Properties

        public short? ItemVNum { get; set; }

        public int? MapNpcId { get; set; }

        public short RecipeId { get; set; }

        public int RecipeListId { get; set; }

        #endregion
    }
}