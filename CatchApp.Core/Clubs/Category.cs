using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class Category : AuditedEntity
    {
        public const int MaxLengthName = 50;

        [MaxLength(MaxLengthName)]
        public string Name { get; set; }

        public IList<Club> Clubs { get; set; }

        public Category()
        {
            Clubs = new List<Club>();
        }
    }
}
