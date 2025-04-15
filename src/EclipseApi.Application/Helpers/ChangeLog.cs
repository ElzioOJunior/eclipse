using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Helpers
{
    public class ChangeLog
    {
        public required string FieldName { get; set; }
        public required string PreviousValue { get; set; }
        public required string NewValue { get; set; }
    }
}

