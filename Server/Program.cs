using Azure.Storage.Blobs;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using OnlineStore.Server;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Jobs;
using OnlineStore.Server.Middleware;
using OnlineStore.Server.Options;
using OnlineStore.Server.Services;
using OnlineStore.Server.Services.Email;
using OnlineStore.Server.Validators;
using OnlineStore.Shared.Models;

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
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OnlineStoreDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineStoreDbConnection")));

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<StoreSeeder>();
builder.Services.AddScoped<IBlobStorage, AzureStorage>();
builder.Services.AddSingleton<IClock, Clock>();
builder.Services.AddMemoryCache();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSwaggerGen();

builder.Services.RegisterQuartzJobs();
builder.Services.RegisterEmailServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", p =>
        p.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            // .WithOrigins(builder.Configuration["AllowedOrigins"],
            //              builder.Configuration["ClientAddress"])
    );
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbSeeder = scope.ServiceProvider.GetRequiredService<StoreSeeder>();
    dbSeeder.Seed();

    var templateSeeder = scope.ServiceProvider.GetRequiredService<EmailTemplateSeeder>();
    await templateSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineStoreApi API"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("FrontEndClient");

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