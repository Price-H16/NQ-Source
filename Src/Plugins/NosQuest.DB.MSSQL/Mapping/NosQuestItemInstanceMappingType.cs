// WingsEmu
// 
// Developed by NosWings Team

using OpenNos.DAL.DAO;
using OpenNos.DAL.EF;
using System;
using System.Collections.Generic;


namespace WingsEmu.Plugins.DB.MSSQL.Mapping
{
    public class NosQuestItemInstanceMappingType : ItemInstanceDAO.IItemInstanceMappingTypes
    {
        public List<(Type, Type)> Types { get; } = new List<(Type, Type)>
        {
            (typeof(OpenNos.GameObject.ItemInstance), typeof(ItemInstance)),
        };
    }
}