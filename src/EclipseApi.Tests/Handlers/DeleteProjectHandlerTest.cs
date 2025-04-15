using Application.Exceptions;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Handlers;
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
    public class DeleteProjectHandlerTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenProjectDoesNotExist()
        {
            // Arrange
            var command = new DeleteProjectCommand
            {
                Id = Guid.NewGuid()
            };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(
                command.Id,
                It.IsAny<Expression<Func<Project, object>>>()))
                .ReturnsAsync((Project)null);

            var handler = new DeleteProjectHandler(projectRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenProjectHasPendingTasks()
        {
            // Arrange
            var command = new DeleteProjectCommand
            {
                Id = Guid.NewGuid()
            };

            var project = new Project
            {
                Id = command.Id,
                Tasks = new List<Domain.Entities.Task>
                {
                    new Domain.Entities.Task { Status = Domain.Enum.TaskStatus.Pendente } 
                }
            };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(
                command.Id,
                It.IsAny<Expression<Func<Project, object>>>()))
                .ReturnsAsync(project);

            var handler = new DeleteProjectHandler(projectRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldDeleteProject_WhenProjectIsValid()
        {
            // Arrange
            var command = new DeleteProjectCommand
            {
                Id = Guid.NewGuid()
            };

            var project = new Project
            {
                Id = command.Id,
                Tasks = new List<Domain.Entities.Task> 
                {
                    new Domain.Entities.Task { Status = Domain.Enum.TaskStatus.Concluida }
                }
            };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.GetByIdWithIncludesAsync(
                command.Id,
                It.IsAny<Expression<Func<Project, object>>>()))
                .ReturnsAsync(project);

            projectRepositoryMock.Setup(r => r.DeleteAsync(project)).Returns(System.Threading.Tasks.Task.CompletedTask);

            var handler = new DeleteProjectHandler(projectRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            projectRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Project>()), Times.Once);
        }


    }
}
