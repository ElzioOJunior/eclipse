using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Domain.Interface;
using MediatR;

public class RemoveTaskHandler : IRequestHandler<RemoveTaskCommand, bool>
{
    private readonly IRepository<EclipseApi.Domain.Entities.Task> _taskRepository;
    private readonly IMapper _mapper;

    public RemoveTaskHandler(IRepository<EclipseApi.Domain.Entities.Task> taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RemoveTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "Task not found",
                detail: $"Task with ID {request.Id} does not exist."
            );
        }

         await _taskRepository.DeleteAsync(task);

        return true;
    }
}
