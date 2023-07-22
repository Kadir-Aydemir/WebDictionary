using EntityLayer;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IHeadingService
    {
        List<Heading> GetList();
        List<Heading> GetListActive();
        List<Heading> GetListByWriter(int id);
        List<Heading> GetListByWriterActive(int id);
        List<Heading> GetListByCategory(int id);
        List<Heading> GetListAgenda();
        void AddHeading(Heading heading);
        Heading GetHeading(int id);
        Heading GetHeadingActive(int id);
        Heading GetHeadingByName(string HeadingName);
        void DeleteHeading(Heading heading);
        void UpdateHeading(Heading heading);
        List<CategoryHeadingChart> ListCategoryHeading();
        
    }
}
