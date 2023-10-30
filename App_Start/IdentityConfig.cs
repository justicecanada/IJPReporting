using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using IJPReporting.Models;


public class EmailService : IIdentityMessageService
{
    public Task SendAsync(IdentityMessage message)
    {
        // Plug in your email service here to send an email.
        // Plug in your email service here to send an email.
        // Credentials:
        var credentialUserName = ConfigurationManager.AppSettings["emailServiceUserName"];
        var sentFrom = ConfigurationManager.AppSettings["emailServiceSentFrom"];
        var pwd = ConfigurationManager.AppSettings["emailServicePassword"];
        bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["emailServiceEnableSsl"]);

        // Configure the client:
        System.Net.Mail.SmtpClient client =
            new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["emailServiceSetting"]);

        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["emailServicePort"])) client.Port = Int32.Parse(ConfigurationManager.AppSettings["emailServicePort"]);

        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;

        // Creatte the credentials:
        System.Net.NetworkCredential credentials;
        if (String.IsNullOrEmpty(credentialUserName) || (String.IsNullOrEmpty(pwd)))
        {
            credentials = new System.Net.NetworkCredential();
        }
        else
        {
            credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
        }

        client.EnableSsl = enableSsl;
        client.Credentials = credentials;


        // Create the message:
        var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);

        mail.IsBodyHtml = true;
        mail.Subject = message.Subject;
        mail.Body = message.Body;

        // Send:
        return client.SendMailAsync(mail);



        // return Task.FromResult(0);
    }
}

public class SmsService : IIdentityMessageService
{
    public Task SendAsync(IdentityMessage message)
    {
        // Plug in your SMS service here to send a text message.
        return Task.FromResult(0);
    }
}

// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
        : base(store)
    {
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
        // Configure validation logic for usernames
        manager.UserValidator = new UserValidator<ApplicationUser>(manager)
        {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
        };

        // Configure validation logic for passwords
        manager.PasswordValidator = new PasswordValidator
        {
            RequiredLength = 8,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
        };
        // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        // You can write your own provider and plug it in here.
        manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
        {
            MessageFormat = "Your security code is {0}"
        });
        manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
        {
            Subject = "Security Code",
            BodyFormat = "Your security code is {0}"
        });

        // Configure user lockout defaults
        manager.UserLockoutEnabledByDefault = true;
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        manager.EmailService = new EmailService();
        manager.SmsService = new SmsService();

        var dataProtectionProvider = options.DataProtectionProvider;
        if (dataProtectionProvider != null)
        {
            manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }
        return manager;
    }
}

public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
{
    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
        base(userManager, authenticationManager)
    { }

    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    {
        return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    }

    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    {
        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }
}

public class ApplicationRoleManager : RoleManager<IdentityRole>
{
    public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
        : base(roleStore)
    {
    }

    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    {
        return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
    }
}

