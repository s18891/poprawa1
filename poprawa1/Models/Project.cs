using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace poprawa1.Models
{
    public class Project
    {
        public int IdTeam { get; set; }
        [MaxLength(100)]

        public string Name { get; set; }

        public DateTime date { get; set; }

    }
}
