
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

#region Add services
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<OnlineShopContext>(option =>
    option.UseSqlServer(builder.Configuration["connectionstrings:defaultconnection"])
);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
