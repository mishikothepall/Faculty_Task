using DataLayer.Context;
using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{
    public interface IUserManager {
        ILogin Enter();
        IUserList GetUsers();
        IUserCreator Create();
        IRoleList RoleList();
    }

    public interface IRoleList {
        IEnumerable<string> RolesList(AppIdentityDbContext db);
    }

    public interface IUserList {
        IEnumerable<AppUser> UserList(AppIdentityDbContext db);
    }

    public interface IUserCreator {
         bool CreateUser(AppUser user, AppIdentityDbContext db);
    }

    public interface ILogin {
        string LoginUser(AppUser guest, IAuthenticationManager manager, AppIdentityDbContext db);
    }

    class ConcreteAppUserFactory : IUserManager
    {
        public IUserCreator Create()
        {
            return new UserCreator();
        }

        public ILogin Enter()
        {
            return new Login();
        }

        public IUserList GetUsers()
        {
            return new Users();
        }

        public IRoleList RoleList()
        {
            return new Roles();
        }
    }

    class Login : ILogin {
        AppIdentityDbContext db = new AppIdentityDbContext();

        public string LoginUser(AppUser guest, IAuthenticationManager manager, AppIdentityDbContext db)
        {

            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            AppUser user = userMgr.Find(guest.UserName, guest.Password);
            
            if (user == null)
            {
                return "Некорректное имя или пароль";
            }
            else {
                ClaimsIdentity identity = userMgr.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                manager.SignOut();
                manager.SignIn(new AuthenticationProperties {
                    IsPersistent = false
                }, identity);
                return string.Empty;
            }
        }

    }

    class Roles : IRoleList
    {

        public IEnumerable<string> RolesList(AppIdentityDbContext db)
        {
            return db.Roles.Where(n=>n.Name!="admin").Select(r=>r.Name);
        }
    }

    class Users : IUserList
    {

        public IEnumerable<AppUser> UserList(AppIdentityDbContext db)
        {
            return db.Users.ToList();
        }
    }


    class UserCreator : IUserCreator
    {
        
        public bool CreateUser(AppUser user, AppIdentityDbContext db)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));

            IdentityResult res =  userMgr.Create(user, user.Password);

            if (res.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

  
   public class AppUserFactory
    {

       public IUserManager users = new ConcreteAppUserFactory();

    }
}
