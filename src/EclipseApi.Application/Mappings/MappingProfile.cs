using AutoMapper;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Domain.Contracts;
using EclipseApi.Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProjectCommand, Project>()
               .ForMember(dest => dest.Tasks, opt => opt.Ignore()); 

            CreateMap<Project, ProjectDto>().ReverseMap();

            CreateMap<EclipseApi.Domain.Entities.Task, TaskDto>().ReverseMap();

            CreateMap<CreateTaskCommand, EclipseApi.Domain.Entities.Task>();

            CreateMap<UpdateTaskCommand, EclipseApi.Domain.Entities.Task>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UserTaskCompletionData, PerformanceReportDto>();

            CreateMap<TaskHistory, TaskHistoryDto>(); 
            CreateMap<Comment, CommentDto>();

            CreateMap<EclipseApi.Domain.Entities.Task, TaskDto>()
                   .ForMember(dest => dest.Histories, opt => opt.MapFrom(src => src.Histories))
                   .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<AddCommentCommand, Comment>()
           .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId))
           .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId))
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)); 
        }
    }
}
