using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class HeadingManager : IHeadingService
    {
        IHeadingDal _headingDal;
        ICategoryDal _categoryDal;
        public HeadingManager(IHeadingDal headingDal, ICategoryDal categoryDal)
        {
            _headingDal = headingDal;
            _categoryDal = categoryDal;
        }

        public void AddHeading(Heading heading)
        {
            _headingDal.Insert(heading);
        }

        public void DeleteHeading(Heading heading)
        {
            _headingDal.Delete(heading);
        }

        public Heading GetHeading(int id)
        {
            return _headingDal.Get(x => x.HeadingID == id);
        }

        public Heading GetHeadingActive(int id)
        {
            return _headingDal.Get(x => x.HeadingID == id && x.HeadingRemove == false);
        }

        public Heading GetHeadingByName(string HeadingName)
        {
            return _headingDal.Get(x => x.HeadingName == HeadingName && x.HeadingRemove==false);
        }

        public List<Heading> GetList()
        {
            return _headingDal.List();
        }

        public List<Heading> GetListActive()
        {
            return _headingDal.List(x => x.HeadingRemove == false);
        }

        public List<Heading> GetListAgenda()
        {
            return _headingDal.List(x => x.HeadingRemove == false, x => x.HeadingDate);
        }

        public List<Heading> GetListByCategory(int id)
        {
            return _headingDal.List(x => x.CategoryID == id);
        }

        public List<Heading> GetListByWriter(int id)
        {
            return _headingDal.List(x => x.WriterID == id);
        }

        public List<Heading> GetListByWriterActive(int id)
        {
            return _headingDal.List(x => x.WriterID == id && x.HeadingRemove == false);
        }

        public List<CategoryHeadingChart> ListCategoryHeading()
        {
            var result = _headingDal.ListChart(x => x.CategoryID, x => new CategoryHeadingChart
            {
                CategoryID = x.Key,
                Count = x.Count()
            });

            var categoryIDs = result.Select(x => x.CategoryID).ToList();
            var categoryNames = _categoryDal.List(c => categoryIDs.Contains(c.CategoryID))
                                        .ToDictionary(c => c.CategoryID, c => c.CategoryName);

            foreach (var item in result)
            {
                if (categoryNames.ContainsKey(item.CategoryID))
                {
                    item.CategoryName = categoryNames[item.CategoryID];
                }
            }
            return result;
        }

        public void UpdateHeading(Heading heading)
        {
            _headingDal.Update(heading);
        }
    }
}
