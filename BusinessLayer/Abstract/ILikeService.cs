using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ILikeService
    {
        List<Like> GetListByContentID(int id);
        List<Like> GetList();
        void AddLike(Like like);
        void DeleteLike(Like like);
        Like GetLike(int id, int writerId);
    }
}
