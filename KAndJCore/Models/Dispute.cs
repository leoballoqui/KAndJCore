using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Models
{
    public class Dispute
    {
        public Guid Id { get; set; }

        public Guid ClaimId { get; set; }

        public Guid? AccountId { get; set; }

        public string CompleteReason { get; set; }

        public virtual Account Account { get; set; }
    }
}
