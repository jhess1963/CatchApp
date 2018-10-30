using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class MemberImage : AuditedEntity<long>
    {
        public virtual Member Member { get; set; }
        public byte[] Image { get; set; }
    }
}
