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
using System.Xml.Linq;

namespace EclipseApi.Tests.Handlers
{
    public class AddCommentHandlerTest
    {

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCreateCommentAndReturnDto_WhenDataIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var command = new AddCommentCommand(taskId, "teste", userId);

            var user = new User { Id = userId };
            var task = new EclipseApi.Domain.Entities.Task { Id = taskId };
            var comment = new Comment { Id = Guid.NewGuid(), Content = command.Content, TaskId = taskId };
            var commentDto = new CommentDto { Content = command.Content };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            commentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Comment>()))
                                 .Callback<Comment>(c => c.Id = Guid.NewGuid());


            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();
            taskHistoryRepositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskHistory>()))
                                     .Verifiable();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Comment>(command)).Returns(comment);
            mapperMock.Setup(m => m.Map<CommentDto>(comment)).Returns(commentDto);

            var handler = new AddCommentHandler(
                commentRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                taskRepositoryMock.Object,
                mapperMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Content, result.Content);
            commentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Once);
            taskHistoryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskHistory>()), Times.Once);
        }


        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new AddCommentCommand(Guid.NewGuid(), "teste", Guid.NewGuid());

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync(new User());

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.TaskId)).ReturnsAsync((EclipseApi.Domain.Entities.Task)null);

            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new AddCommentHandler(
                commentRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                taskRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new AddCommentCommand(Guid.NewGuid(), "teste", Guid.NewGuid());

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);

            var taskRepositoryMock = new Mock<IRepository<EclipseApi.Domain.Entities.Task>>();
            taskRepositoryMock.Setup(r => r.GetByIdAsync(command.TaskId)).ReturnsAsync(new EclipseApi.Domain.Entities.Task());

            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var taskHistoryRepositoryMock = new Mock<IRepository<TaskHistory>>();
            var mapperMock = new Mock<IMapper>();

            var handler = new AddCommentHandler(
                commentRepositoryMock.Object,
                taskHistoryRepositoryMock.Object,
                userRepositoryMock.Object,
                taskRepositoryMock.Object,
                mapperMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }


    }


}
