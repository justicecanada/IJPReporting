using Microsoft.Owin;
using Owin;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(IJPReporting.Startup))]
namespace IJPReporting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //if (!roleManager.RoleExists("SuperAdmin"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "SuperAdmin";
            //    roleManager.Create(role);
            //}
            //if (!roleManager.RoleExists("Admin"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Admin";
            //    roleManager.Create(role);
            //}

            //if (!roleManager.RoleExists("Client"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Client";
            //    roleManager.Create(role);
            //}
        }
    }
}
