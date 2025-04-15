using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Dtos
{
    public class PerformanceReportDto
    {
        public Guid UserId { get; set; }
        public int CompletedTasksCount { get; set; }
        public double AverageCompletedTasks { get; set; }
    }

}
