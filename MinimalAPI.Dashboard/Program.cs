using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dashboard.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// conecto a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
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

app.MapGet("/api/dashboard", async (AppDbContext context) =>
{
    var totalGames = await context.Games.CountAsync();

    var mostPopular = await context.Games
        .OrderByDescending(g => g.TimesAdded)
        .Take(5)
        .Select(g => new
        {
            g.Name,
            g.TimesAdded
        })
        .ToListAsync();

    var recent = await context.Games
        .OrderByDescending(g => g.CreatedAt)
        .Take(5)
        .Select(g => new
        {
            g.Name,
            g.CreatedAt
        })
        .ToListAsync();

    return Results.Ok(new
    {
        totalGames,
        mostPopular,
        recent
    });
});

app.Run();


