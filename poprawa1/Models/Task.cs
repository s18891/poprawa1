using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace poprawa1.Models
{
    public class Task
    {
       
        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string ProjectName { get; set; }
        public DateTime Deadline { get; set; }

        [MaxLength(100)]
        public string TaskTypeName { get; set; }

        [MaxLength(100)]
        public string TaskName { get; set; }


    }
}
