using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Models
{
    public class Reason
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        [NotMapped]
        public string Short
        {
            get
            {
                if (String.IsNullOrEmpty(Value))
                    return String.Empty;
                return Value.Length > 40 ? Value.Substring(0, 30) + "..." : Value;
            }
        }
    }
}
