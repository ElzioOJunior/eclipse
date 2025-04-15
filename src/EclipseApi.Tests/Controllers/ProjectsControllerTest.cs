using Application.Exceptions;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Controllers
{
    public class ProjectsControllerTest
    {
        [Fact]
        public async Task GetProjects_ShouldReturnOkWithListOfProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projects = new List<ProjectDto>
            {
                new ProjectDto { Id = Guid.NewGuid(), Name = "Project A" },
                new ProjectDto { Id = Guid.NewGuid(), Name = "Project B" }
            };

            var apiResponse = new ApiResponse
            {
                Data = projects,
                Status = "success",
                Message = "Operação concluída com sucesso"
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetProjectsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(projects);

            var controller = new ProjectsController(mediatorMock.Object);

            // Act
            var result = await controller.GetProjects(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var actualResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(apiResponse.Status, actualResponse.Status);
            Assert.Equal(apiResponse.Message, actualResponse.Message);
            Assert.Equal(projects, actualResponse.Data);
        }

        [Fact]
        public async Task GetProjects_ShouldReturnApiResponseWithEmptyList_WhenNoProjectsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projects = new List<ProjectDto>();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetProjectsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(projects);

            var controller = new ProjectsController(mediatorMock.Object);

            // Act
            var result = await controller.GetProjects(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message);
            Assert.Empty((IEnumerable<ProjectDto>)apiResponse.Data);
        }


        [Fact]
        public async Task CreateProject_ShouldReturnApiResponseWithCreatedProject_WhenProjectIsCreatedSuccessfully()
        {
            // Arrange
            var command = new CreateProjectCommand { Name = "New Project", UserId = Guid.NewGuid() };
            var project = new ProjectDto { Id = Guid.NewGuid(), Name = "New Project" };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(project);

            var controller = new ProjectsController(mediatorMock.Object);

            // Act
            var result = await controller.CreateProject(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message);
            Assert.Equal(project, apiResponse.Data);
        }


        [Fact]
        public async Task DeleteProject_ShouldReturnApiResponseWithSuccessMessage_WhenProjectIsDeleted()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var isDeleted = true;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProjectCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(isDeleted);

            var controller = new ProjectsController(mediatorMock.Object);

            // Act
            var result = await controller.DeleteProject(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message);
            Assert.Equal(isDeleted, apiResponse.Data);
        }



    }
}
