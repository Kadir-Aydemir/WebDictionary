using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }

        [StringLength(100)]
        public string SkillName { get; set; }

        [StringLength(3)]
        public string SkillValue { get; set; }

        [StringLength(25)]
        public string SkillColor { get; set; }

        public bool SkillRemove { get; set; }
    }
}
