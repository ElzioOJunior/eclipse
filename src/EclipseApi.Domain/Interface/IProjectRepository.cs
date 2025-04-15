using EclipseApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Domain.Interface
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId);
    }

}
