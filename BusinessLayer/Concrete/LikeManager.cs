using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class LikeManager : ILikeService
    {
        ILikeDal _likeDal;

        public LikeManager(ILikeDal likeDal)
        {
            _likeDal = likeDal;
        }

        public void AddLike(Like like)
        {
            _likeDal.Insert(like);
        }

        public void DeleteLike(Like like)
        {
            _likeDal.Delete(like);
        }

        public Like GetLike(int id, int writerId)
        {
            return _likeDal.Get(x => x.ContentID == id && x.WriterID == writerId);
        }

        public List<Like> GetList()
        {
            return _likeDal.List();
        }

        public List<Like> GetListByContentID(int id)
        {
            return _likeDal.List(x => x.ContentID == id);
        }
    }
}
