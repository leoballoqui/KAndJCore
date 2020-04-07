using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Enums
{
    public enum ClaimStatusEnum
    {
        [Display(Name = "Open")]
        Open = 0,

        [Display(Name = "Closed")]
        Closed = 1
    }
}
