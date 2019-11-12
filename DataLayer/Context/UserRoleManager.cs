using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    class UserRoleManager : RoleManager<UserRoles>, IDisposable
    {
        public UserRoleManager(IRoleStore<UserRoles, string> store) : base(store)
        {
        }

        public static UserRoleManager Create(IdentityFactoryOptions<UserRoleManager> options,
            IOwinContext context) {

            return new UserRoleManager(new RoleStore<UserRoles>(context.Get<AppIdentityDbContext>()));

        }
    }
}
