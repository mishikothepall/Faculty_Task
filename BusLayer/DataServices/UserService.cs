using DataLayer.Factories;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Owin.Security;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataServices
{
    interface IUserService {
        IEnumerable<string> RolesList();
        IEnumerable<AppUser> UsersList();
        bool CreateUser(string name, string email, string password);
        string Login(string username, string password, IAuthenticationManager manager);
    }

    public class UserService : IUserService
    {
        private AppUserFactory uf = new AppUserFactory();

        private UnitOfWork unit = new UnitOfWork();

        public bool CreateUser(string name, string email, string password)
        {
            var user = new AppUser {
                UserName = name,
                Email = email,
                Password = password
            };
            
            return uf.users.Create().CreateUser(user, unit.context);
        }

        public string Login(string username, string password, IAuthenticationManager manager)
        {
            var user = new AppUser {
                UserName = username,
                Password = password
            };
            return uf.users.Enter().LoginUser(user, manager, unit.context);
        }

        public IEnumerable<string> RolesList()
        {
            return uf.users.RoleList().RolesList(unit.context);
        }

        public IEnumerable<AppUser> UsersList()
        {
            return uf.users.GetUsers().UserList(unit.context);
        }
    }
}
