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
//Hata sonucu response(mvc->sayfa) d�nme:Haz�r model Use Exception handler middleware exception f�rlat�ld���nda bu hatay� yakalar

//Middlewware:Ara katmanlar(istemcinin requestinin gelmesinden response olu�up clienta geri gidene kadar ge�ti�i yerler)(request ve response aras� yap�lacak i�lemlerdir)

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;   //Bunu mvc taraf�nda yapmaya gerek yok.API default kullan�yor mvc kullanm�yor.��nk� sayfa sonu� d�n�yor hangi sayfay� d�nece�ini bilmiyor
});  //api de response d�n�yor
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped(typeof(NotFoundFilter<>));

builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>  //Db ba�lant� connection stringi al�n�yor
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);   //Burda AppDbContext in bulundu�u Assemblyi buluyoruz.Migrationlar burada olu�acak
    });
});
//AutoFac
//Inversal Controle Container-DI Container(Dependency Injection Container):Olu�turulacak olan nesnelerin ya�am d�ng�lerinin y�netilmesidir.
//Nesnelerin uygulama boyuncaki ya�am d�ng�s�nden sorumludur.

//ASP.NET CORE da built-in DI Container olarak bu gelir.Constructor injection, metot injection

//builder.Services.AddScoped<IProductService, ProductService>()
//program.cs de herhangi bir class�n constructor�nda kullanaca��m�z interface ve buna kar��l�k gelen class � ekliyoruz

//AutoFac default gelen DI containerdan daha yetenekli

//AutoFac        				 DI containerdan 
//Metot, constructor, property               metot, constructor     injection
//dinamik service ekleme var               yok (AddScoped(sonu service ile biten interfaceleri ekle, sonu service ile biten s�n�flar� ekle)

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilde => containerBuilde.RegisterModule(new RepoServiceModule()));

builder.Services.AddMemoryCache();

var app = builder.Build();

//Bunlarda middlewarelard�r.�lk �al��t���nda buraya gelir
//startup.cs dosyas�n�n kalkmas�yla oradaki kodlar buraya geldi
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  //y�nlendirmeyi yaps�n.Http yi https d�n��t�rs�n

app.UseCustomException();  //HATA OLDU�U ���N YUKARDA TANIMLANIR

app.UseAuthorization();

app.MapControllers();

app.Run();
