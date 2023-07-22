using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IBlockedService
    {
        List<Blocked> GetList();
        List<Blocked> GetListActive();
        List<Blocked> GetListInactive();
        void AddBlocked(Blocked blocked);
        Blocked GetBlocked(int id);
        Blocked GetBlockedByWriterMail(string WriterMail);
        void DeleteBlocked(Blocked blocked);
        void UpdateBlocked(Blocked blocked);

        void CheckExpireDateForEnd();
    }
}
