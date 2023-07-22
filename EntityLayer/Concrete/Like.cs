using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Like
    {
        [Key]
        public int LikeID { get; set; }

        public int ContentID { get; set; }
        public virtual Content Content { get; set; }

        public int? WriterID { get; set; }
        public virtual Writer Writer { get; set; }

        public DateTime LikeDate { get; set; }

    }
}
