using Abp.Domain.Entities.Auditing;
using CatchApp.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class Member : AuditedEntity<long>
    {
        public const int MaxLengthName = 50;

        [MaxLength(MaxLengthName)]
        public string UserName { get; set; }

        [MaxLength(MaxLengthName)]
        public string Name { get; set; }

        [MaxLength(MaxLengthName)]
        public string Surname { get; set; }
        public bool IsMale { get; set; }
        public bool IsSingle { get; set; }

        public virtual MemberImage MemberImage { get; set; }
        public IList<ClubVisit> Visits { get; set; }
        public IList<Member> Friends { get; set; }

        public Member()
        {
            Visits = new List<ClubVisit>();
            Friends = new List<Member>();
        }
    }
}
