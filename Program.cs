using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop;
using Shop.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// use in memory database
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("Database"));
builder.Services.AddScoped<DataContext, DataContext>();

// use sqlServer database
// var Configuration = builder.Configuration;
// System.Console.WriteLine($"ConnectionString: {Configuration.GetConnectionString("connectionString")}");
// builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("connectionString")));
// builder.Services.AddScoped<DataContext, DataContext>();

// Authentication
var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // authentication

app.UseAuthorization();

app.MapControllers();

app.Run();
