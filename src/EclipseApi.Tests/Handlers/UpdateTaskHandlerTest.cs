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
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Handlers
{
    public class UpdateTaskHandlerTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new UpdateTaskCommand(Guid.NewGuid(), "teste", "descrição", Guid.NewGuid(), Domain.Enum.TaskStatus.EmAndamento, Guid.NewGuid());

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new UpdateTaskHandler(
                taskRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new UpdateTaskCommand(Guid.NewGuid(), "teste", "descrição", Guid.NewGuid(), Domain.Enum.TaskStatus.EmAndamento, Guid.NewGuid());

            var user = new User { Id = command.UserId };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId))
                              .ReturnsAsync(user);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((EclipseApi.Domain.Entities.Task)null);

            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new UpdateTaskHandler(
                taskRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldUpdateTaskAndReturnDto_WhenDataIsValid()
        {
            // Arrange
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
                "Updated Task",
                "Updated Description",
                Guid.NewGuid(),
                Domain.Enum.TaskStatus.EmAndamento,
                Guid.NewGuid()
            );

            var user = new User { Id = command.UserId };
            var originalTask = new EclipseApi.Domain.Entities.Task
            {
                Id = command.Id,
                Title = "Original Task",
                Description = "Original Description",
                Status = Domain.Enum.TaskStatus.Pendente
            };

            var updatedTask = new EclipseApi.Domain.Entities.Task
            {
                Id = command.Id,
                Title = command.Title,
                Description = command.Description,
                Status = command.Status
            };

            var updatedTaskDto = new TaskDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                Description = updatedTask.Description
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId))
                              .ReturnsAsync(user);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                              .ReturnsAsync(originalTask);
            taskRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<EclipseApi.Domain.Entities.Task>()))
                              .Returns(System.Threading.Tasks.Task.CompletedTask);

            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map(command, originalTask))
                      .Callback<UpdateTaskCommand, EclipseApi.Domain.Entities.Task>((cmd, task) =>
                      {
                          task.Title = cmd.Title;
                          task.Description = cmd.Description;
                          task.Status = cmd.Status;
                      });
            mapperMock.Setup(m => m.Map<TaskDto>(originalTask))
                      .Returns(updatedTaskDto);

            var handler = new UpdateTaskHandler(
                taskRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);

            taskRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<EclipseApi.Domain.Entities.Task>()), Times.Once);

            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.Is<TaskHistory>(
                h => h.ChangedField == "Title" &&
                     h.PreviousValue == "Original Task" &&
                     h.NewValue == "Updated Task"
            )), Times.Once);

            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.Is<TaskHistory>(
                h => h.ChangedField == "Description" &&
                     h.PreviousValue == "Original Description" &&
                     h.NewValue == "Updated Description"
            )), Times.Once);

            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.Is<TaskHistory>(
                h => h.ChangedField == "Status" &&
                     h.PreviousValue == Domain.Enum.TaskStatus.Pendente.ToString() &&
                     h.NewValue == Domain.Enum.TaskStatus.EmAndamento.ToString()
            )), Times.Once);

            mapperMock.Verify(m => m.Map<TaskDto>(originalTask), Times.Once);
        }


        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldRegisterChangesInHistory_WhenTaskIsUpdated()
        {
            // Arrange
            var command = new UpdateTaskCommand(
                Guid.NewGuid(),
                "Updated Task",
                "Updated Description",
                Guid.NewGuid(),
                Domain.Enum.TaskStatus.EmAndamento,
                Guid.NewGuid()
            );

            var user = new User { Id = command.UserId };
            var originalTask = new EclipseApi.Domain.Entities.Task
            {
                Id = command.Id,
                Title = "Original Task",
                Description = "Original Description",
                Status = Domain.Enum.TaskStatus.Pendente
            };

            var updatedTask = new EclipseApi.Domain.Entities.Task
            {
                Id = command.Id,
                Title = command.Title,
                Description = command.Description,
                Status = command.Status
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId))
                              .ReturnsAsync(user);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                              .ReturnsAsync(originalTask);
            taskRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<EclipseApi.Domain.Entities.Task>()))
                              .Returns(System.Threading.Tasks.Task.CompletedTask);

            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map(command, originalTask))
                      .Callback<UpdateTaskCommand, EclipseApi.Domain.Entities.Task>((cmd, task) =>
                      {
                          task.Title = cmd.Title;
                          task.Description = cmd.Description;
                          task.Status = cmd.Status;
                      });

            var handler = new UpdateTaskHandler(
                taskRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                mapperMock.Object
            );

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.Is<TaskHistory>(
                h => h.ChangedField == "Title" &&
                     h.PreviousValue == "Original Task" &&
                     h.NewValue == "Updated Task"
            )), Times.Once);

            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.Is<TaskHistory>(
                h => h.ChangedField == "Description" &&
                     h.PreviousValue == "Original Description" &&
                     h.NewValue == "Updated Description"
            )), Times.Once);

            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.Is<TaskHistory>(
                h => h.ChangedField == "Status" &&
                     h.PreviousValue == Domain.Enum.TaskStatus.Pendente.ToString() &&
                     h.NewValue == Domain.Enum.TaskStatus.EmAndamento.ToString()
            )), Times.Once);

            taskRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<EclipseApi.Domain.Entities.Task>()), Times.Once);
        }

    }
}
