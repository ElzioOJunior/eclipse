using AutoMapper;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Domain.Contracts;
using EclipseApi.Domain.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Tests.Handlers
{
    public class GetPerformanceReportHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoDataFound()
        {
            // Arrange
            var query = new GetPerformanceReportQuery
            {
                UserId = Guid.NewGuid()
            };

            var reportRepositoryMock = new Mock<IReportRepository>();
            reportRepositoryMock.Setup(r => r.GetAverageCompletedTasksPerUserAsync(query.UserId))
                                .ReturnsAsync(new List<UserTaskCompletionData>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<PerformanceReportDto>>(It.IsAny<IEnumerable<UserTaskCompletionData>>()))
                      .Returns(new List<PerformanceReportDto>());

            var handler = new global::GetPerformanceReportHandler(reportRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            reportRepositoryMock.Verify(r => r.GetAverageCompletedTasksPerUserAsync(query.UserId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<PerformanceReportDto>>(It.IsAny<IEnumerable<UserTaskCompletionData>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedData_WhenDataIsFound()
        {
            // Arrange
            var query = new GetPerformanceReportQuery
            {
                UserId = Guid.NewGuid()
            };

            var reportData = new List<UserTaskCompletionData>
            {
                new UserTaskCompletionData { UserId = query.UserId,  AverageCompletedTasks = 5 }
            };

                    var mappedData = new List<PerformanceReportDto>
            {
                new PerformanceReportDto { UserId = query.UserId,  AverageCompletedTasks = 5 }
            };

            var reportRepositoryMock = new Mock<IReportRepository>();
            reportRepositoryMock.Setup(r => r.GetAverageCompletedTasksPerUserAsync(query.UserId))
                                .ReturnsAsync(reportData);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<PerformanceReportDto>>(reportData))
                      .Returns(mappedData);

            var handler = new global::GetPerformanceReportHandler(reportRepositoryMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(5, result.First().AverageCompletedTasks);
            reportRepositoryMock.Verify(r => r.GetAverageCompletedTasksPerUserAsync(query.UserId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<PerformanceReportDto>>(reportData), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithCorrectUserId()
        {
            // Arrange
            var query = new GetPerformanceReportQuery
            {
                UserId = Guid.NewGuid()
            };

            var reportRepositoryMock = new Mock<IReportRepository>();
            reportRepositoryMock.Setup(r => r.GetAverageCompletedTasksPerUserAsync(query.UserId))
                                .ReturnsAsync(new List<UserTaskCompletionData>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<PerformanceReportDto>>(It.IsAny<IEnumerable<UserTaskCompletionData>>()))
                      .Returns(new List<PerformanceReportDto>());

            var handler = new global::GetPerformanceReportHandler(reportRepositoryMock.Object, mapperMock.Object);

            // Act
            await handler.Handle(query, CancellationToken.None);

            // Assert
            reportRepositoryMock.Verify(r => r.GetAverageCompletedTasksPerUserAsync(query.UserId), Times.Once);
        }


    }
}
