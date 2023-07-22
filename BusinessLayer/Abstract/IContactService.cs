using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IContactService
    {
        List<Contact> GetList();
        List<Contact> GetListRemoved();
        List<Contact> GetListNotRead();
        void AddContact(Contact contact);
        Contact GetContact(int id);
        void DeleteContact(Contact contact);
        void UpdateContact(Contact contact);
    }
}
