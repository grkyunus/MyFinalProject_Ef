using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// farkl� yap�lara ta��nabilir alt yap�lan� yap�yor 
// Autofac,Ninject,CastleWindsor, StroctoreMap, LinghtInject, DryInject --> IoC Container bu yap� �zerine kullan�labilir.
// AOP
builder.Services.AddControllers();
builder.Services.AddSingleton<IProductService,ProductManager>();
builder.Services.AddSingleton<IProductDal, EfProductDal>();

builder.Services.AddRazorPages();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
