using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.ViewModels
{
    public class DisputeVM
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }

        public string CompleteReason { get; set; }
    }
}
