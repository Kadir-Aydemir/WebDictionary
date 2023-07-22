using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IWriterService
    {
        List<Writer> GetList();
        void AddWriter(Writer writer);
        Writer GetWriter(int id);
        Writer GetWriterIdByMail(string WriterMail);
        Writer GetWriterByMailControl(string WriterMail);
        Writer ControlWriter(string WriterMail, string WriterPassword);
        void DeleteWriter(Writer writer);
        void UpdateWriter(Writer writer);
    }
}
