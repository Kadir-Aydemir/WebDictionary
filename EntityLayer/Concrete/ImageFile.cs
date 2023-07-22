using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ImageFile
    {
        [Key]
        public int ImageId { get; set; }

        [StringLength(100)]
        public string ImageName { get; set; }

        public byte[] ImagePath { get; set; }

        public bool ImageRemove { get; set; }
    }
}
