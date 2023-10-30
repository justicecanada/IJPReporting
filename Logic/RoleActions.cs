using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using IJPReporting;
using IJPReporting.Models;
using GCWebUsabilityTheme;
using Microsoft.Owin;

namespace IJPReporting.Logic
{
    internal class RoleActions
    {
        internal void AddUserAndRole()
        {
            // Access the application context and create result variables.
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;

            // Create a RoleStore object by using the ApplicationDbContext object.
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object.
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            //Then, you create the "canEdit" role if it doesn't already exist.
            if (!roleMgr.RoleExists("SuperAdmin"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "SuperAdmin" });
            }
            if (!roleMgr.RoleExists("Admin"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Admin" });
            }
            if (!roleMgr.RoleExists("Agent"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Agent" });
            }
            if (!roleMgr.RoleExists("Client"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Client" });
            }

            // Create a UserManager object based on the UserStore object and the ApplicationDbContext
            // object. Note that you can create new objects and use them as parameters in
            // a single line of code, rather than using multiple lines of code, as you did
            // for the RoleManager object.

            //var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            //userMgr.UserValidator = new UserValidator<ApplicationUser>(userMgr)
            //{
            //    AllowOnlyAlphanumericUserNames = false,
            //    RequireUniqueEmail = true
            //};


            //var appUser = new ApplicationUser() 
            //{
            //    UserName = "valerie.hayot-sasson@justice.gc.ca", 
            //    Email = "valerie.hayot-sasson@justice.gc.ca"
            //};
            //IdUserResult = userMgr.Create(appUser, "Pa$$word1");

            //if (IdUserResult.Succeeded)
            //{
            //    // If the new "canEdit" user was successfully created,
            //    // add the "canEdit" user to the "canEdit" role.
            //    if (!userMgr.IsInRole(userMgr.FindByEmail("valerie.hayot-sasson@justice.gc.ca").Id, "SuperAdmin"))
            //    {
            //        IdUserResult = userMgr.AddToRole(userMgr.FindById(appUser.Id).Id, "SuperAdmin");
            //    }
            //}


        }
    }
}