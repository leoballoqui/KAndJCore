using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Enums
{
    public enum ClientStatusEnum
    {
        [Display(Name = "Active")]
        Open = 0,

        [Display(Name = "On Hold")]
        OnHold = 1,

        [Display(Name = "Closed")]
        Closed = 2,

        [Display(Name = "Credit Repaired")]
        Completed = 3
    }
}
