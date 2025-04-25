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
using OnlineStore.Server.Features.Accounts.RegisterUser;
using OnlineStore.Server.Features.Accounts.Repositories;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Features.Accounts.Strategies;
using OnlineStore.Server.Features.Orders.CreateOrder;
using OnlineStore.Server.Features.Orders.Repository;
using OnlineStore.Server.Features.Products.CreateProduct;
using OnlineStore.Server.Features.Products.Repository;
using OnlineStore.Server.Features.Products.Services;
using OnlineStore.Server.Features.Products.UpdateProduct;
using OnlineStore.Server.Features.ShoppingCart;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Jobs;
using OnlineStore.Server.Middleware;
using OnlineStore.Server.Options;
using OnlineStore.Server.Services;
using OnlineStore.Server.Services.Email;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;
using OnlineStore.Shared.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add authentication
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddRazorPages();

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

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));   
});
builder.Services.AddValidatorsFromAssembly(typeof(AbstractValidator<>).Assembly);

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterAdmin>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<CreateProductsBatch>, CreateProductsBatchValidator>();
builder.Services.AddScoped<IValidator<UpdateProduct>, UpdateProductValidator>();
builder.Services.AddScoped<IValidator<CreateOrder>, CreateOrderValidator>();
builder.Services.AddScoped<IValidator<CreateOrder.CreateOrderItem>, CreateOrderItemValidator>();
builder.Services.AddScoped<StoreSeeder>();
builder.Services.AddScoped<IBlobStorage, AzureStorage>();
builder.Services.AddSingleton<IClock, Clock>();
builder.Services.AddScoped<IResultPaginator, ResultPaginator>();
builder.Services.AddTransient<IUserFactory<RegisterAdmin>, AdminFactory>();
builder.Services.AddTransient<IUserFactory<RegisterClient>, ClientFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<ILoggedUserService, LoggedUserService>();
builder.Services.AddScoped<IShoppingCartCookieService, ShoppingCartService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession();
builder.Services.AddMemoryCache();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection("BlobStorage"));
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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();