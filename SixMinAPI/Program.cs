using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinAPI.Data;
using SixMinAPI.Dtos;
using SixMinAPI.Models;

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

app.MapGet("api/v1/commands", async (ICommandRepository repository, IMapper mapper) =>
{
    var commands = await repository.GetAllCommandsAsync();
    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});

app.MapGet("api/v1/commands/{id}", async ([FromRoute] int id, ICommandRepository repository, IMapper mapper) =>
{
    var command = await repository.GetCommandByIdAsync(id);
    if (command != null)
    {
        return Results.Ok(mapper.Map<CommandReadDto>(command));
    }
    return Results.NotFound();
});

app.MapPost("api/v1/commands", async (ICommandRepository repository, IMapper mapper, [FromBody] CommandCreateDto command) =>
{
    var commandModel = mapper.Map<Command>(command);
    await repository.CreateCommandAsync(commandModel);
    await repository.SaveChangesAsync();
    var commandReadDto = mapper.Map<CommandReadDto>(commandModel);
    return Results.Created($"/api/v1/commands/{commandReadDto.Id}", commandReadDto);
});

app.MapPut("api/v1/commands/{id}", async (ICommandRepository repository, IMapper mapper, [FromRoute] int id, [FromBody] CommandUpdateDto commandUpdateDto) =>
{
    var command = await repository.GetCommandByIdAsync(id);
    if (command == null)
    {
        return Results.NotFound();
    }
    mapper.Map(commandUpdateDto, command);
    await repository.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("api/v1/commands/{id}", async (ICommandRepository repository, IMapper mapper, [FromRoute] int id) =>
{
    var command = await repository.GetCommandByIdAsync(id);
    if (command == null)
    {
        return Results.NotFound();
    }
    repository.DeleteCommand(command);
    await repository.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
