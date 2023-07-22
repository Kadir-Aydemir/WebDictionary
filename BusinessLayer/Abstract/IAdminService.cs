using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAdminService
    {
        List<Admin> GetList();
        void AddAdmin(Admin admin);
        Admin GetAdmin(int id);
        Admin GetAdminForRole(string AdminUserName);
        Admin ControlAdmin(string AdminUserName, string AdminPassword);
        Admin IsMailExist(string AdminUserName);
        void DeleteAdmin(Admin admin);
        void UpdateAdmin(Admin admin);
    }
}
