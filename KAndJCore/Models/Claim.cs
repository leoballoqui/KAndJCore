using KAndJCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Models
{
    public class Claim
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Guid BuroId { get; set; }

        public Guid TemplateId { get; set; }

        public string ClaimNumber { get; set; }

        public DateTime Created { get; set; }

        public DateTime NextRevision { get; set; }

        public int CurrentIteration { get; set; }

        public ClaimStatusEnum Status { get; set; }

        public string Notes { get; set; }

        public virtual Client Client { get; set; }

        public virtual Buro Buro { get; set; }

        public virtual Template Template { get; set; }

        public virtual List<Dispute> Disputes { get; set; }

        [NotMapped]
        public string DisputedAccounts
        {
            get
            {
                string disputes = "";
                if (Disputes != null)
                    foreach (var item in Disputes.OrderBy(d => d.AccountId).ToList())
                        disputes += item.Account?.Alias + ", ";
                return !String.IsNullOrEmpty(disputes) ? disputes.Remove(disputes.Length - 2, 2) : disputes;
            }
        }

    }
}
