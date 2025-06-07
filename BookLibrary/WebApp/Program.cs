using DAL.Models;
using DAL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(DAL.AutoMapper.MappingProfile));
builder.Services.AddAutoMapper(typeof(WebApp.AutoMapper.ViewModelMappingProfile));
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IEntityService<Genre>, GenreService>();
builder.Services.AddScoped<IEntityService<Location>, LocationService>();

builder.Services.AddDbContext<BookLibraryContext>(options => {
    options.UseSqlServer("name=ConnectionStrings:BookLibrary");
});

builder.Services.AddAuthentication()
    .AddCookie(options => {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/User/Forbidden";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
