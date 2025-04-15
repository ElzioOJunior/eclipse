using EclipseApi.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Commands
{
    public class AddCommentCommand : IRequest<CommentDto>
    {
        public Guid TaskId { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }

        public AddCommentCommand(Guid taskId, string content, Guid userId)
        {
            TaskId = taskId;
            Content = content;
            UserId = userId;
        }
    }

}
