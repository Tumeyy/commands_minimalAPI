using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region database
var sqlConnection = new SqlConnectionStringBuilder();
sqlConnection.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
sqlConnection.UserID = builder.Configuration["UserId"];
sqlConnection.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConnection.ConnectionString));
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
