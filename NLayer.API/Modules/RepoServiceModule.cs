using Autofac;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;
namespace NLayer.API.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ServiceWithDto<,>)).As(typeof(IServiceWithDto<,>)).InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
            builder.RegisterType<ProductServiceWithDto>().As(typeof(IProductServiceWithDto)).InstancePerLifetimeScope();


            var apiAssembly = Assembly.GetExecutingAssembly();  //Üzerinde çalıştığı Assembly  API
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(ProductService));
            var coreAssembly = Assembly.GetAssembly(typeof(IUnitOfWork));

            builder.RegisterAssemblyTypes(repoAssembly, serviceAssembly, coreAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //autofacdeki bu metot AsImplementedInterfaces->.net core scope:bir requeest başladıktan sonra bitene kadar aynı instance kullanılır 
            //InstancePerDependcy=>.net core transit:Herhangi bir classın constructorında o interface nerde geçririldiyse her seferinde yeni bir instance oluşturur.

            builder.RegisterAssemblyTypes(repoAssembly, serviceAssembly, coreAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
           // builder.RegisterType<ProductServiceWithCaching>().As(typeof(IProductService)).InstancePerLifetimeScope();
        }
    }
}
