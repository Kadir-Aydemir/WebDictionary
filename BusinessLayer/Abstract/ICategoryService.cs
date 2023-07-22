using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        List<Category> GetListAll();
        List<Category> GetList();
        void AddCategory(Category category);
        Category GetCategory(int id);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
    }
}
