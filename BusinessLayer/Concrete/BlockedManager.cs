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
    public class BlockedManager : IBlockedService
    {
        IBlockedDal _blockedDal;
        public BlockedManager(IBlockedDal blockedDal)
        {
            _blockedDal = blockedDal;
        }

        public void AddBlocked(Blocked blocked)
        {
            _blockedDal.Insert(blocked);
        }

        public void CheckExpireDateForEnd()
        {
            var currentDate = DateTime.Today;

            var expiredItems = _blockedDal.List(x => x.ExpireDate < currentDate && !x.Situation);

            foreach (var item in expiredItems)
            {
                item.Situation = true;
                _blockedDal.Update(item);
            }         
        }

        public void DeleteBlocked(Blocked blocked)
        {
            _blockedDal.Delete(blocked);
        }

        public Blocked GetBlocked(int id)
        {
            return _blockedDal.Get(x => x.BlockID == id);
        }

        public Blocked GetBlockedByWriterMail(string WriterMail)
        {
            return _blockedDal.Get(x => x.Writer.WriterMail == WriterMail && x.Situation == false);
        }

        public List<Blocked> GetList()
        {
            return _blockedDal.List();
        }

        public List<Blocked> GetListActive()
        {
            return _blockedDal.List(x => x.Situation == false);
        }

        public List<Blocked> GetListInactive()
        {
            return _blockedDal.List(x => x.Situation == true);
        }

        public void UpdateBlocked(Blocked blocked)
        {
            _blockedDal.Update(blocked);
        }
    }
}
