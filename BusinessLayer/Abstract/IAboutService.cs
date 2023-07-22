using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAboutService
    {
        List<About> GetList();
        List<About> GetListActive();
        void AddAbout(About about);
        About GetAbout(int id);
        void DeleteAbout(About about);
        void UpdateAbout(About about);
    }
}
