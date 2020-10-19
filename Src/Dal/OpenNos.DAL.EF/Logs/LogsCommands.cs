﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.DAL.EF
{
    public class LogCommands
    {
        #region Properties

        public virtual Character Character { get; set; }

        public long? CharacterId { get; set; }

        public string Command { get; set; }

        [Key] public long CommandId { get; set; }

        public string Data { get; set; }

        [MaxLength(255)] public string IpAddress { get; set; }

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        #endregion
    }
}
