using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Handlers
{
    public class CreateTaskHandlerTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenProjectDoesNotExist()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now,
                Priority = Domain.Enum.TaskPriority.Media
            };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(command.ProjectId,It.IsAny<Expression<Func<Project, object>>>())).ReturnsAsync((Project)null);

            var userRepositoryMock = new Mock<IRepository<User>>();
            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new CreateTaskHandler(
                taskRepositoryMock.Object,
                projectRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now,
                Priority = Domain.Enum.TaskPriority.Media
            };

            var project = new Project { Id = command.ProjectId, Tasks = new List<EclipseApi.Domain.Entities.Task>() };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(
                command.ProjectId,
                It.IsAny<Expression<Func<Project, object>>>()))
                .ReturnsAsync(project);

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new CreateTaskHandler(
                taskRepositoryMock.Object,
                projectRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }


        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenTaskLimitReached()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now,
                Priority = Domain.Enum.TaskPriority.Media
            };

            var project = new Project
            {
                Id = command.ProjectId,
                Tasks = new List<EclipseApi.Domain.Entities.Task>(new EclipseApi.Domain.Entities.Task[20]) 
            };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(
                command.ProjectId,
                It.IsAny<Expression<Func<Project, object>>>()))
                .ReturnsAsync(project);

            var userRepositoryMock = new Mock<IRepository<User>>();
            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new CreateTaskHandler(
                taskRepositoryMock.Object,
                projectRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }


        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCreateTaskAndReturnDto_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now,
                Priority = Domain.Enum.TaskPriority.Media
            };

            var project = new Project { Id = command.ProjectId, Tasks = new List<EclipseApi.Domain.Entities.Task>() };
            var user = new User { Id = command.UserId };
            var task = new EclipseApi.Domain.Entities.Task { Id = Guid.NewGuid(), Title = command.Title, UserId = user.Id };
            var taskDto = new TaskDto { Title = command.Title };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(
                command.ProjectId,
                It.IsAny<Expression<Func<Project, object>>>()))
                .ReturnsAsync(project);

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync(user);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.AddAsync(It.IsAny<EclipseApi.Domain.Entities.Task>()))
                              .Callback<EclipseApi.Domain.Entities.Task>(t => t.Id = task.Id);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<EclipseApi.Domain.Entities.Task>(command)).Returns(task);
            mapperMock.Setup(m => m.Map<TaskDto>(task)).Returns(taskDto);

            var handler = new CreateTaskHandler(
                taskRepositoryMock.Object,
                projectRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            taskRepositoryMock.Verify(r => r.AddAsync(It.IsAny<EclipseApi.Domain.Entities.Task>()), Times.Once);
            mapperMock.Verify(m => m.Map<TaskDto>(It.IsAny<EclipseApi.Domain.Entities.Task>()), Times.Once);
        }


    }
}
