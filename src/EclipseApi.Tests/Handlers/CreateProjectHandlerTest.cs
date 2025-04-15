using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Handlers;
using EclipseApi.Domain.Entities;
using Moq;

namespace EclipseApi.Tests.Handlers
{
    public class CreateProjectHandlerTest
    {

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCreateProjectAndReturnDto_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                Name = "Teste criação projeto",
                UserId = Guid.NewGuid()
            };

            var project = new Project { Name = command.Name, UserId = command.UserId, Id = Guid.NewGuid() };
            var projectDto = new ProjectDto { Name = command.Name, Id = project.Id };

            var projectRepositoryMock = new Mock<IRepository<Project>>();
            projectRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Project>())).Callback<Project>(p => p.Id = project.Id);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Project>(command)).Returns(project);
            mapperMock.Setup(m => m.Map<ProjectDto>(project)).Returns(projectDto);

            var handler = new CreateProjectHandler(projectRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            projectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
            mapperMock.Verify(m => m.Map<ProjectDto>(It.IsAny<Project>()), Times.Once);
        }
    }
}
