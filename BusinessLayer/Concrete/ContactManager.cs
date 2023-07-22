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
    public class ContactManager : IContactService
    {
        IContactDal _contactDal;
        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        public void AddContact(Contact contact)
        {
            _contactDal.Insert(contact);
        }

        public void DeleteContact(Contact contact)
        {
            _contactDal.Delete(contact);
        }

        public Contact GetContact(int id)
        {
            return _contactDal.Get(x => x.ID == id);
        }

        public List<Contact> GetList()
        {
            return _contactDal.List(x => x.Remove == false, x => x.ContactDate);
        }

        public List<Contact> GetListNotRead()
        {
            return _contactDal.List(x => x.IsRead == false && x.Remove == false);
        }

        public List<Contact> GetListRemoved()
        {
            return _contactDal.List(x => x.Remove == true);
        }

        public void UpdateContact(Contact contact)
        {
            _contactDal.Update(contact);
        }
    }
}
