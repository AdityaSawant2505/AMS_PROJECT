using BLL.Implementations;
using BLL.Interfaces;
using DLL.Implementations;
using DLL.Interfaces;
using DLL.Models.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DAL
builder.Services.AddScoped<IUsersService,UsersService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<PasswordHelper,PasswordHelper>();


//BAL
builder.Services.AddScoped<IUsersManagerService,UsersManagerService>();
builder.Services.AddScoped<IAuthManagerService,AuthManagerService>();
builder.Services.AddScoped<IEmailManagerService,EmailManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
