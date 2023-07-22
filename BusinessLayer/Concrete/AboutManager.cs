using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AboutManager : IAboutService
    {
        IAboutDal _aboutDal;
        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }
        public void AddAbout(About about)
        {
            _aboutDal.Insert(about);
        }

        public void DeleteAbout(About about)
        {
            _aboutDal.Delete(about);
        }

        public About GetAbout(int id)
        {
            return _aboutDal.Get(x => x.ID == id);
        }

        public List<About> GetList()
        {
            return _aboutDal.List();
        }

        public List<About> GetListActive()
        {
            return _aboutDal.List(x => x.AboutRemove == false);
        }

        public void UpdateAbout(About about)
        {
            _aboutDal.Update(about);
        }
    }
}
