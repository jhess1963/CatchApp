using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class ClubVisit : AuditedEntity
    {
        public DateTime EntryDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public bool HasLeft { get; set; }

        public int ClubId { get; set; }

        [ForeignKey("ClubId")]
        public Club Club { get; set; }

        public long MemberId { get; set; }
        [ForeignKey("MemberId")]

        public Member Member { get; set; }
    }
}
