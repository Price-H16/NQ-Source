using System;

namespace OpenNos.Data
{
    [Serializable]
    public class MapTypeMapDTO : MappingBaseDTO
    {
        #region Properties

        public short MapId { get; set; }

        public short MapTypeId { get; set; }

        #endregion
    }
}