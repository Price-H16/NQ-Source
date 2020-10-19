using System;
using System.Collections.Generic;
using OpenNos.Data;
using OpenNos.Data.Enums;

namespace OpenNos.DAL.Interface
{
    public interface IPartnerSkillDAO
    {
        #region Methods

        PartnerSkillDTO Insert(PartnerSkillDTO partnerSkillDTO);

        List<PartnerSkillDTO> LoadByEquipmentSerialId(Guid equipmentSerialId);

        DeleteResult Remove(long partnerSkillId);

        #endregion
    }
}