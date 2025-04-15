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
    public class GetProjectsQueryHandlerTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnEmptyList_WhenNoProjectsFound()
        {
            // Arrange
            var query = new GetProjectsQuery
            {
                UserId = Guid.NewGuid()
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(r => r.GetProjectsByUserIdAsync(query.UserId))
                                 .ReturnsAsync(new List<Project>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ProjectDto>>(It.IsAny<IEnumerable<Project>>()))
                      .Returns(new List<ProjectDto>());

            var handler = new GetProjectsQueryHandler(projectRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            projectRepositoryMock.Verify(r => r.GetProjectsByUserIdAsync(query.UserId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ProjectDto>>(It.IsAny<IEnumerable<Project>>()), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnMappedProjects_WhenProjectsAreFound()
        {
            // Arrange
            var query = new GetProjectsQuery
            {
                UserId = Guid.NewGuid()
            };

            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Projeto A" },
                new Project { Id = Guid.NewGuid(), Name = "Projeto B" }
            };

            var mappedProjects = new List<ProjectDto>
            {
                new ProjectDto { Id = projects[0].Id, Name = "Projeto A" },
                new ProjectDto { Id = projects[1].Id, Name = "Projeto B" }
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(r => r.GetProjectsByUserIdAsync(query.UserId))
                                 .ReturnsAsync(projects);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ProjectDto>>(projects))
                      .Returns(mappedProjects);

            var handler = new GetProjectsQueryHandler(projectRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Projeto A", result.First().Name);
            projectRepositoryMock.Verify(r => r.GetProjectsByUserIdAsync(query.UserId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ProjectDto>>(projects), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCallRepositoryWithCorrectUserId()
        {
            // Arrange
            var query = new GetProjectsQuery
            {
                UserId = Guid.NewGuid()
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(r => r.GetProjectsByUserIdAsync(query.UserId))
                                 .ReturnsAsync(new List<Project>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<ProjectDto>>(It.IsAny<IEnumerable<Project>>()))
                      .Returns(new List<ProjectDto>());

            var handler = new GetProjectsQueryHandler(projectRepositoryMock.Object, mapperMock.Object);

            // Act
            await handler.Handle(query, CancellationToken.None);

            // Assert
            projectRepositoryMock.Verify(r => r.GetProjectsByUserIdAsync(query.UserId), Times.Once);
        }

    }
}
