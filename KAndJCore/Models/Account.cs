using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Guid AccountTypeId { get; set; }

        public Guid ReasonId { get; set; }

        public string Alias { get; set; }

        public string Owner { get; set; }

        public string Notes { get; set; }

        public int Status { get; set; }

        public virtual Client Client { get; set; }

        public virtual AccountType AccountType { get; set; }

        public virtual Reason Reason { get; set; }
    }
}
