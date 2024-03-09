using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filter;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Repository;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Hata sonucu response(mvc->sayfa) dönme:Hazýr model Use Exception handler middleware exception fýrlatýldýðýnda bu hatayý yakalar

//Middlewware:Ara katmanlar(istemcinin requestinin gelmesinden response oluþup clienta geri gidene kadar geçtiði yerler)(request ve response arasý yapýlacak iþlemlerdir)

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;   //Bunu mvc tarafýnda yapmaya gerek yok.API default kullanýyor mvc kullanmýyor.Çünkü sayfa sonuç dönüyor hangi sayfayý döneceðini bilmiyor
});  //api de response dönüyor
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped(typeof(NotFoundFilter<>));

builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>  //Db baðlantý connection stringi alýnýyor
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);   //Burda AppDbContext in bulunduðu Assemblyi buluyoruz.Migrationlar burada oluþacak
    });
});
//AutoFac
//Inversal Controle Container-DI Container(Dependency Injection Container):Oluþturulacak olan nesnelerin yaþam döngülerinin yönetilmesidir.
//Nesnelerin uygulama boyuncaki yaþam döngüsünden sorumludur.

//ASP.NET CORE da built-in DI Container olarak bu gelir.Constructor injection, metot injection

//builder.Services.AddScoped<IProductService, ProductService>()
//program.cs de herhangi bir classýn constructorýnda kullanacaðýmýz interface ve buna karþýlýk gelen class ý ekliyoruz

//AutoFac default gelen DI containerdan daha yetenekli

//AutoFac        				 DI containerdan 
//Metot, constructor, property               metot, constructor     injection
//dinamik service ekleme var               yok (AddScoped(sonu service ile biten interfaceleri ekle, sonu service ile biten sýnýflarý ekle)

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilde => containerBuilde.RegisterModule(new RepoServiceModule()));

builder.Services.AddMemoryCache();

var app = builder.Build();

//Bunlarda middlewarelardýr.Ýlk çalýþtýðýnda buraya gelir
//startup.cs dosyasýnýn kalkmasýyla oradaki kodlar buraya geldi
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  //yönlendirmeyi yapsýn.Http yi https dönüþtürsün

app.UseCustomException();  //HATA OLDUÐU ÝÇÝN YUKARDA TANIMLANIR

app.UseAuthorization();

app.MapControllers();

app.Run();
