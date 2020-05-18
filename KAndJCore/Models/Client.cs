using KAndJCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        //public int FileNumber { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string SSN { get; set; }

        [Display(Name = "Status")]
        public ClientStatusEnum CurrentStatus { get; set; }

        public DateTime Created { get; set; }

        [Display(Name = "Client Since")]
        public DateTime ClientSince { get; set; }

        [Required]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Other Phone")]
        public string OtherPhone { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        public string Fax { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }

        [Required]
        public string Address { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Previous Address")]
        public string PreviousAddress { get; set; }

        public string Notes { get; set; }

        public virtual List<Document> Documents { get; set; }

        [NotMapped]
        public string FullName { get {
                if(!String.IsNullOrEmpty(MiddleName))
                    return ($"{Name} {MiddleName} {LastName}");
                return ($"{Name} {LastName}");

            } 
        }

        [NotMapped]
        public string RedactedSSN
        {
            get
            {
                return (!String.IsNullOrEmpty(this.SSN) && this.SSN.Length >= 4) ? String.Format("XXX-XX-{0}", this.SSN.Substring(this.SSN.Length - 4, 4)) : "XXX-XX-XXXX";
            }
        }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return (String.IsNullOrEmpty(AddressLine2)) ? Address : $"{Address}, {AddressLine2}";
            }
        }

        public Client()
        {
            DOB = DateTime.Now.AddYears(-40);
            Created = DateTime.Now;
            ClientSince = DateTime.Now;
        }

    }
}
