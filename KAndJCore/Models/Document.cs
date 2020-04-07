using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Models
{
    public class Document
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Guid DocumentTypeId { get; set; }

        public string FileFullName { get; set; }

        public virtual Client Client { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        [NotMapped]
        public string ThumbnailFullName { get {
                return FileFullName.ToLower().EndsWith(".pdf") ? "pdf.png" : FileFullName;
            } 
        }
    }
}
