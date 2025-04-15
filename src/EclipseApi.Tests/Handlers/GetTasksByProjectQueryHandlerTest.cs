using AutoMapper;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Domain.Entities;
using EclipseApi.Domain.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Handlers
{
    public class GetTasksByProjectQueryHandlerTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnEmptyList_WhenNoTasksFound()
        {
            // Arrange
            var query = new GetTasksByProjectQuery
            {
                ProjectId = Guid.NewGuid()
            };

            var taskRepositoryMock = new Mock<ITaskRepository>();
            taskRepositoryMock.Setup(r => r.GetTasksByProjectIdAsync(query.ProjectId))
                              .ReturnsAsync(new List<EclipseApi.Domain.Entities.Task>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<TaskDto>>(It.IsAny<IEnumerable<EclipseApi.Domain.Entities.Task>>()))
                      .Returns(new List<TaskDto>());

            var handler = new GetTasksByProjectQueryHandler(taskRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            taskRepositoryMock.Verify(r => r.GetTasksByProjectIdAsync(query.ProjectId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<TaskDto>>(It.IsAny<IEnumerable<EclipseApi.Domain.Entities.Task>>()), Times.Once);
        }


        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnMappedTasks_WhenTasksAreFound()
        {
            // Arrange
            var query = new GetTasksByProjectQuery
            {
                ProjectId = Guid.NewGuid()
            };

            var tasks = new List<EclipseApi.Domain.Entities.Task>
            {
                new EclipseApi.Domain.Entities.Task { Id = Guid.NewGuid(), Title = "Task 1" },
                new EclipseApi.Domain.Entities.Task { Id = Guid.NewGuid(), Title = "Task 2" }
            };

                    var mappedTasks = new List<TaskDto>
            {
                new TaskDto { Id = tasks[0].Id, Title = "Task 1" },
                new TaskDto { Id = tasks[1].Id, Title = "Task 2" }
            };

            var taskRepositoryMock = new Mock<ITaskRepository>();
            taskRepositoryMock.Setup(r => r.GetTasksByProjectIdAsync(query.ProjectId))
                              .ReturnsAsync(tasks);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<TaskDto>>(tasks))
                      .Returns(mappedTasks);

            var handler = new GetTasksByProjectQueryHandler(taskRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Title);
            taskRepositoryMock.Verify(r => r.GetTasksByProjectIdAsync(query.ProjectId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<TaskDto>>(tasks), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCallRepositoryWithCorrectProjectId()
        {
            // Arrange
            var query = new GetTasksByProjectQuery
            {
                ProjectId = Guid.NewGuid()
            };

            var taskRepositoryMock = new Mock<ITaskRepository>();
            taskRepositoryMock.Setup(r => r.GetTasksByProjectIdAsync(query.ProjectId))
                              .ReturnsAsync(new List<EclipseApi.Domain.Entities.Task>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<TaskDto>>(It.IsAny<IEnumerable<EclipseApi.Domain.Entities.Task>>()))
                      .Returns(new List<TaskDto>());

            var handler = new GetTasksByProjectQueryHandler(taskRepositoryMock.Object, mapperMock.Object);

            // Act
            await handler.Handle(query, CancellationToken.None);

            // Assert
            taskRepositoryMock.Verify(r => r.GetTasksByProjectIdAsync(query.ProjectId), Times.Once);
        }

    }
}
