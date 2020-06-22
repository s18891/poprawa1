using poprawa1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poprawa1.Services
{
    public interface IDbService
    {
        TeamMember GetTeamMember(int id);
        bool DeleteProject(int id);

    }
}
