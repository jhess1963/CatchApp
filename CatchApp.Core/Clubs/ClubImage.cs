using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class ClubImage : AuditedEntity
    {
        public virtual Club Club { get; set; }
        public byte[] Image { get; set; }
    }
}
