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
    public class ReportsControllerTest
    {
        [Fact]
        public async Task GetPerformanceReport_ShouldReturnApiResponse_WhenRoleIsManager()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var role = "gerente";
            var performanceReport = new List<PerformanceReportDto>
            {
                new PerformanceReportDto { UserId = userId,  AverageCompletedTasks = 5 },
                new PerformanceReportDto { UserId = Guid.NewGuid(), AverageCompletedTasks = 8 }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetPerformanceReportQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(performanceReport);

            var controller = new ReportsController(mediatorMock.Object);

            // Act
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                Request = {Headers = { ["Role"] = role }}
            };

            var result = await controller.GetPerformanceReport(userId, role);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("success", apiResponse.Status);
            Assert.Equal("Operação concluída com sucesso", apiResponse.Message);
            Assert.Equal(performanceReport, apiResponse.Data);
        }

        [Fact]
        public async Task GetPerformanceReport_ShouldReturnForbidden_WhenRoleIsNotManager()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var role = "funcionario";

            var mediatorMock = new Mock<IMediator>();
            var controller = new ReportsController(mediatorMock.Object);

            // Act
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                Request = { Headers = { ["Role"] = role } }
            };

            var result = await controller.GetPerformanceReport(userId, role);

            // Assert
            var forbiddenResult = Assert.IsType<ForbidResult>(result);
            Assert.NotNull(forbiddenResult);
        }

        [Fact]
        public async Task GetPerformanceReport_ShouldReturnForbidden_WhenRoleIsMissing()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mediatorMock = new Mock<IMediator>();
            var controller = new ReportsController(mediatorMock.Object);

            // Act
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = await controller.GetPerformanceReport(userId, null);

            // Assert
            var forbiddenResult = Assert.IsType<ForbidResult>(result);
            Assert.NotNull(forbiddenResult);
        }

    }
}
