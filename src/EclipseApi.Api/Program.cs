using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Domain.Interfaces;
using Infrastructure.Repositories;
using EclipseApi.Application.Commands;
using System.Reflection;
using EclipseApi.Application.Handlers;
using EclipseApi.Domain.Interface;
using EclipseApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddCommentHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTaskHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteProjectHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPerformanceReportHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProjectsQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTasksByProjectQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateTaskHandler).Assembly));


builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Application.Mappings.MappingProfile));

var app = builder.Build();

app.UseMiddleware<ErrorsHandling>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); 
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eclipse API v1"));

    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        await next.Invoke();
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
SeedDatabase(app);
app.Run();

static void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!dbContext.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.Parse("650a8ec4-a12b-409d-8118-5ea31bb5a778"),
                    Name = "Usuário comum",
                    Email = "comum@user.com",
                    Projects = new List<Project>() 
                },
                new User
                {
                    Id = Guid.Parse("dd51de23-ff87-4258-aaf2-5ef506624cd5"),
                    Name = "Usuário gerente",
                    Email = "gerente@user.com",
                    Projects = new List<Project>()
                }
            };

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();

            Console.WriteLine("Usuários padrão criados com sucesso.");
        }
        else
        {
            Console.WriteLine("Usuários já existem no banco. Seeder não será executado.");
        }
    }
}
