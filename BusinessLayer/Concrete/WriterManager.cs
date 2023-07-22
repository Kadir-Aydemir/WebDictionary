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
    public class WriterManager : IWriterService
    {
        IWriterDal _writerDal;
        public WriterManager(IWriterDal writerDal)
        {
            _writerDal = writerDal;
        }

        public void AddWriter(Writer writer)
        {
            _writerDal.Insert(writer);
        }

        public Writer ControlWriter(string WriterMail, string WriterPassword)
        {
            return _writerDal.Get(x => x.WriterMail == WriterMail && x.WriterPassword == WriterPassword);
        }

        public void DeleteWriter(Writer writer)
        {
            _writerDal.Delete(writer);
        }

        public List<Writer> GetList()
        {
            return _writerDal.List();
        }

        public Writer GetWriter(int id)
        {
            return _writerDal.Get(x => x.WriterID == id);
        }

        public Writer GetWriterByMailControl(string WriterMail)
        {
            return _writerDal.Get(x => x.WriterMail == WriterMail && x.WriterRemove == false);
        }

        public Writer GetWriterIdByMail(string WriterMail)
        {
            return _writerDal.Get(x => x.WriterMail == WriterMail);
        }

        public void UpdateWriter(Writer writer)
        {
            _writerDal.Update(writer);
        }
    }
}
