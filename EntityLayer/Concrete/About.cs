using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class About
    {
        [Key]
        public int ID { get; set; }

        [StringLength(1000)]
        public string Details1 { get; set; }

        [StringLength(1000)]
        public string Details2 { get; set; }

        public bool AboutRemove { get; set; }
    }
}
