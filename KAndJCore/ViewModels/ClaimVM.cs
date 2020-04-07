using KAndJCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.ViewModels
{
    public class ClaimVM
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public string ClaimNumber { get; set; }

        [Required]
        public Guid TemplateId { get; set; }
        
        [Required(ErrorMessage = "Please select at least one buro.")]
        public string IncludedBuros { get; set; }
        
        [Required(ErrorMessage = "Please select at least one account to dispute.")]
        public string IncludedAccounts { get; set; }

        public string Notes { get; set; }

        public List<DisputeVM> DisputeAccounts { get; set; }

        public ClaimVM() { }

        public ClaimVM(Claim claim) {

            this.ClientId = claim.ClientId;
            this.TemplateId = claim.TemplateId;
            this.ClaimNumber = claim.ClaimNumber;
            this.Notes = claim.Notes;

            this.DisputeAccounts = new List<DisputeVM>();
            foreach (var item in claim.Disputes)
            {
                this.DisputeAccounts.Add(new DisputeVM
                {
                    Id = item.Id,
                    CompleteReason = item.CompleteReason,
                });
            }

        }
    }
}
