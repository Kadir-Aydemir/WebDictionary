using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Blocked
    {
        [Key]
        public int BlockID { get; set; }

        public int? WriterID { get; set; }
        public virtual Writer Writer { get; set; }

        public DateTime BlockedDate { get; set; }
        public DateTime ExpireDate { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        public bool Situation { get; set; }
    }
}
