using CarvedRock.Admin.Data;
using CarvedRock.Admin.Logic;
using CarvedRock.Admin.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CarvedRock.Admin.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AdminContext>();

builder.Services.AddDefaultIdentity<AdminUser>(options =>
  {
    options.SignIn.RequireConfirmedAccount = false;   
  }
).AddEntityFrameworkStores<AdminContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

builder.Services.AddDbContext<ProductContext>();

builder.Services.AddScoped<ICarvedRockRepository, CarvedRockRepository>();
builder.Services.AddScoped<IProductLogic, ProductLogic>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var ctx = services.GetRequiredService<ProductContext>();
  ctx.Database.Migrate();

  var userCtx = services.GetRequiredService<AdminContext>();
  userCtx.Database.Migrate();

  if (app.Environment.IsDevelopment())
  {
    ctx.SeedInitialData();
  }
}

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
