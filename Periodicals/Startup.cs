using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Periodicals.Startup))]

namespace Periodicals
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            /*var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);
            var user = new ApplicationUser() { Email = "informatyka4444@wp.pl", UserName = "informatyka4444@wp.pl" };
            manager.Create(user, "TestPass44!");*/

            /*
            app.CreatePerOwinContext(() => new BlogDbContext());
            //app.CreatePerOwinContext<UserManager(UserManager.Create);
            //app.CreatePerOwinContext<RoleManager<AdminRole>>((options, context) =>
            //    new RoleManager<AdminRole>(
            //        new RoleStore<AdminRole>(context.Get<BlogDbContext>())));
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync($"Hello world!:{DateTime.UtcNow}");
            });
            */
        }
    }
}
