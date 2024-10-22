using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Kutsung;
using Kutsung.Models;

public class AutofacConfig
{
    public static void Configure()
    {
        var builder = new ContainerBuilder();

        builder.RegisterControllers(typeof(MvcApplication).Assembly);

        builder.RegisterType<GuestContext>().AsSelf().InstancePerRequest();
        builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();

        var container = builder.Build();

        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    }
}