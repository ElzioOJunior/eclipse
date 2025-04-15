using EclipseApi.Api.Helpers;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Controllers
{
    public class TasksControllerTest
    {
        [Fact]
        public async Task GetTasks_ShouldReturnApiResponseWithListOfTasks_WhenProjectHasTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<TaskDto>
            {
                new TaskDto { Id = Guid.NewGuid(), Title = "Task 1" },
                new TaskDto { Id = Guid.NewGuid(), Title = "Task 2" }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetTasksByProjectQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(tasks);

            var controller = new TasksController(mediatorMock.Object);

            // Act
            var result = await controller.GetTasks(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message);
            Assert.Equal(tasks, apiResponse.Data);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnApiResponseWithCreatedTask_WhenTaskIsCreatedSuccessfully()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                Title = "New Task",
                Description = "Task Description",
                UserId = Guid.NewGuid()
            };

            var task = new TaskDto { Id = Guid.NewGuid(), Title = "New Task", Description = "Task Description" };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(task);

            var controller = new TasksController(mediatorMock.Object);

            // Act
            var result = await controller.CreateTask(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message);
            Assert.Equal(task, apiResponse.Data);
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnApiResponseWithUpdatedTask_WhenTaskIsUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UpdateTaskDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                UpdatedByUserId = Guid.NewGuid(),
                Status = Domain.Enum.TaskStatus.EmAndamento,
                UserId = Guid.NewGuid()
            };

            var command = new UpdateTaskCommand(id, dto.Title, dto.Description, dto.UpdatedByUserId, dto.Status, dto.UserId);

            var updatedTask = new TaskDto { Id = id, Title = "Updated Title", Description = "Updated Description" };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(updatedTask);

            var controller = new TasksController(mediatorMock.Object);

            // Act
            var result = await controller.UpdateTask(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); 
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value); 
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message); 
            Assert.Equal(updatedTask, apiResponse.Data); 
        }


        [Fact]
        public async Task AddComment_ShouldReturnApiResponseWithComment_WhenCommentIsAddedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new AddCommentDto
            {
                Content = "This is a comment",
                UserId = Guid.NewGuid()
            };

            var command = new AddCommentCommand(id, dto.Content, dto.UserId);

            var comment = new CommentDto { Content = "This is a comment" };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<AddCommentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(comment); 

            var controller = new TasksController(mediatorMock.Object);

            // Act
            var result = await controller.AddComment(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); 
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value); 
            Assert.Equal("success", apiResponse.Status); 
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message); 
            Assert.Equal(comment, apiResponse.Data); 
        }

    }
}
