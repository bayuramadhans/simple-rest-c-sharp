using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply automatic migrations
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Check if database exists, if not create it
        if (!await dbContext.Database.CanConnectAsync())
        {
            await dbContext.Database.EnsureCreatedAsync();
            Console.WriteLine("Database created successfully");
        }
        
        // Check for pending migrations
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            Console.WriteLine($"Found {pendingMigrations.Count()} pending migrations to apply");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("All migrations applied successfully");
        }
        else
        {
            Console.WriteLine("Database is up to date - no migrations needed");
        }
        
        // Final connection test
        if (await dbContext.Database.CanConnectAsync())
        {
            Console.WriteLine("Successfully connected to the database");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error during database migration: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    throw; // Re-throw to prevent application startup if database is not available
}

// GET all tasks
app.MapGet("/api/tasks", async (ApplicationDbContext db) =>
{
    var tasks = await db.Tasks.ToListAsync();
    return Results.Ok(tasks);
})
.WithName("GetAllTasks");

// GET task by ID
app.MapGet("/api/tasks/{id}", async (int id, ApplicationDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
})
.WithName("GetTaskById");

// POST create new task
app.MapPost("/api/tasks", async (TaskItem newTask, ApplicationDbContext db) =>
{
    db.Tasks.Add(newTask);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{newTask.Id}", newTask);
})
.WithName("CreateTask");

// PUT update task
app.MapPut("/api/tasks/{id}", async (int id, TaskItem updatedTask, ApplicationDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();
    
    task.Title = updatedTask.Title;
    task.IsCompleted = updatedTask.IsCompleted;
    await db.SaveChangesAsync();
    return Results.Ok(task);
})
.WithName("UpdateTask");

// DELETE task
app.MapDelete("/api/tasks/{id}", async (int id, ApplicationDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();
    
    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteTask");

app.Run();

// DbContext
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; }
}

// Model
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}