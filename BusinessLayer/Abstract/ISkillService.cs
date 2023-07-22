using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ISkillService
    {
        List<Skill> GetList();
        List<Skill> GetListActive();
        void AddSkill(Skill skill);
        Skill GetSkill(int id);
        void DeleteSkill(Skill skill);
        void UpdateSkill(Skill skill);
    }
}
