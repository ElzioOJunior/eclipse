using EclipseApi.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Dtos
{
    public class TaskHistoryDto
    {
        public Guid Id { get; set; }                
        public string ChangedField { get; set; }   
        public string PreviousValue { get; set; }   
        public string NewValue { get; set; }        
        public DateTime ChangedDate { get; set; }   
        public Guid ChangedBy { get; set; }         
    }

}

