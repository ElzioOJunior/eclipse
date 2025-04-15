using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Handlers
{
    public class RemoveTaskHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new RemoveTaskCommand
            {
                Id = Guid.NewGuid()
            };

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                              .ReturnsAsync((EclipseApi.Domain.Entities.Task)null);

            var mapperMock = new Mock<IMapper>();

            var handler = new RemoveTaskHandler(taskRepositoryMock.Object, mapperMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldRemoveTaskAndReturnTrue_WhenTaskExists()
        {
            // Arrange
            var command = new RemoveTaskCommand
            {
                Id = Guid.NewGuid()
            };

            var task = new EclipseApi.Domain.Entities.Task { Id = command.Id };

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                              .ReturnsAsync(task);
            taskRepositoryMock.Setup(r => r.DeleteAsync(task))
                              .Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();

            var handler = new RemoveTaskHandler(taskRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            taskRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            taskRepositoryMock.Verify(r => r.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithCorrectTaskId()
        {
            // Arrange
            var command = new RemoveTaskCommand
            {
                Id = Guid.NewGuid()
            };

            var task = new EclipseApi.Domain.Entities.Task { Id = command.Id };

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                              .ReturnsAsync(task);
            taskRepositoryMock.Setup(r => r.DeleteAsync(task))
                              .Returns(Task.CompletedTask);

            var mapperMock = new Mock<IMapper>();

            var handler = new RemoveTaskHandler(taskRepositoryMock.Object, mapperMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            taskRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            taskRepositoryMock.Verify(r => r.DeleteAsync(task), Times.Once);
        }

    }
}
