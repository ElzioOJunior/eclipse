using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Handlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IMapper _mapper;
        public CreateProjectHandler(IRepository<Project> projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
    
            var project = _mapper.Map<Project>(request);

            project.Id = Guid.NewGuid();

            await _projectRepository.AddAsync(project);

            return _mapper.Map<ProjectDto>(project);
        }

    }

}
