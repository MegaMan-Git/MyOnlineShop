using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Persistence.Context;
using Infrastructure.Identity;


var builder = WebApplication.CreateBuilder(args);

#region Add services
// Add services to the container.

builder.Services.AddControllers();

#region Add connection string
builder.Services.AddDbContext<OnlineShopContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion

#region Add Identity service
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<OnlineShopContext>()
    .AddDefaultTokenProviders();
#endregion

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
