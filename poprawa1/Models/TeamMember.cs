using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace poprawa1.Models
{
    public class TeamMember
    {
        public int IdTeamMember { get; set; }
        [MaxLength(100)]

        public string  FirstName { get; set; }
        [MaxLength(100)]

        public string LastName  { get; set; }
        [MaxLength(100)]

        public string Email { get; set; }

        public List<Task> AssignedProjects { get; set; }
        public List<Task> CreatedProjects { get; set; }

    }
}
