using BLL.Services.Implimentation;
using BLL.Services.Interface;
using DAL.Implimentation;
using DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
