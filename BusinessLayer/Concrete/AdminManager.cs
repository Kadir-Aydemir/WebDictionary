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
    public class AdminManager : IAdminService
    {
        IAdminDal _adminDal;
        public AdminManager(IAdminDal adminDal)
        {
            _adminDal = adminDal;
        }

        public void AddAdmin(Admin admin)
        {
            _adminDal.Insert(admin);
        }

        public Admin ControlAdmin(string AdminUserName, string AdminPassword)
        {
            return _adminDal.Get(x => x.AdminUserName == AdminUserName && x.AdminPassword == AdminPassword && x.AdminStatus == false);
        }

        public void DeleteAdmin(Admin admin)
        {
            _adminDal.Delete(admin);
        }

        public Admin GetAdmin(int id)
        {
            return _adminDal.Get(x => x.AdminId == id);
        }

        public Admin GetAdminForRole(string AdminUserName)
        {
            return _adminDal.Get(x => x.AdminUserName == AdminUserName);
        }

        public List<Admin> GetList()
        {
            return _adminDal.List();
        }

        public Admin IsMailExist(string AdminUserName)
        {
            return _adminDal.Get(x => x.AdminUserName == AdminUserName);
        }

        public void UpdateAdmin(Admin admin)
        {
            _adminDal.Update(admin);
        }
    }
}
