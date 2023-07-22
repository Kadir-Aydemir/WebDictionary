using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Contact
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Mail { get; set; }

        [StringLength(50)]
        public string Subject { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }

        public DateTime ContactDate { get; set; }

        public bool Remove { get; set; }

        public bool IsRead { get; set; }
    }
}
