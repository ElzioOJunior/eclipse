using System;

namespace EclipseApi.Api.Helpers
{
    public class AddCommentDto
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}
