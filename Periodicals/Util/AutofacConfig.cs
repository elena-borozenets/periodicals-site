using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Repositories;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

namespace Periodicals.Util
{
    public class AutofacConfig 
    {

        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем контроллер в текущей сборке
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // регистрируем споставление типов
            builder.RegisterType<EditionRepository>().As<IRepository<Edition>>();
            builder.RegisterType<TopicRepository>().As<IRepository<Topic>>();
            builder.RegisterType<ReviewRepository>().As<IRepository<Review>>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));




            // установка сопоставителя зависимостей
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}