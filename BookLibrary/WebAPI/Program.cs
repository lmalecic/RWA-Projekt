using DAL.AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IEntityService<Book>, BookService>();
builder.Services.AddScoped<IEntityService<Genre>, GenreService>();
builder.Services.AddScoped<IEntityService<Location>, LocationService>();
builder.Services.AddScoped<IAssociationService<BookLocation>, BookLocationService>();

builder.Services.AddDbContext<BookLibraryContext>(options => {
    options.UseSqlServer("name=ConnectionStrings:BookLibrary");
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => {
    option.SwaggerDoc("v1",
        new OpenApiInfo { Title = "RWA Web Library API", Version = "v1" });

    option.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme {
            In = ParameterLocation.Header,
            Description = "Please enter valid JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        });
});

string? secureKey = builder.Configuration["JWT:SecureKey"];
if (secureKey == null)
    throw new Exception("JWT SecureKey is not set in appsettings.json!");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        var Key = Encoding.UTF8.GetBytes(secureKey);
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
        };
    });

builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
