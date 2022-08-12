using System.Reflection;
using System.Text;
using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using SneakersBase.Client.Configurations;
using SneakersBase.Server;
using SneakersBase.Server.Authentication;
using SneakersBase.Server.Entities;
using SneakersBase.Server.Middleware;
using SneakersBase.Server.Services;
using SneakersBase.Server.Validators;
using SneakersBase.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// NLog: Setup NLog for Dependency injection
//builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add authentication
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SneakersDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SneakersDbConnection")));

builder.Services.AddScoped<SneakersSeeder>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); 
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

builder.Services.AddScoped<IAzureStorage, AzureStorage>();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", p =>
    
        p.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"], 
                         builder.Configuration["ClientAddress"])
    );
});



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sneakers API"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var scope = app.Services.CreateScope();

app.UseCors("FrontEndClient");

var seeder = scope.ServiceProvider.GetRequiredService<SneakersSeeder>();
seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
