using Application.Exceptions;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Domain.Entities;
using EclipseApi.Domain.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Handlers
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IRepository<Project> _projectRepository;

        public DeleteProjectHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await ValidateProjectExistsAsync(request.Id);

            if (project.Tasks.Any())
            {
                ValidatePendingTasks(project);
            }

            await _projectRepository.DeleteAsync(project);

            return true;
        }

        private async Task<Project> ValidateProjectExistsAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdWithIncludesAsync(projectId, project=> project.Tasks);
            if (project == null)
            {
                throw new CustomException(
                    type: "InvalidData",
                    message: "Project not found",
                    detail: $"The project with ID {projectId} was not found in the system."
                );
            }
            return project;
        }

        private void ValidatePendingTasks(Project project)
        {
            var hasPendingTasks = project.Tasks.Any(t => t.Status != Domain.Enum.TaskStatus.Concluida);
            if (hasPendingTasks)
            {
                throw new CustomException(
                    type: "InvalidData",
                    message: "The project has pending tasks",
                    detail: "The project has pending tasks. Complete or remove the tasks before deleting the project."
                );
            }
        }

    }

}
