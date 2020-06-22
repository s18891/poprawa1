using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using poprawa1.Models;


namespace poprawa1.Services
{
    public class SqlDbService : IDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18891;Integrated Security=True";        

        public TeamMember GetTeamMember(int id)
        {

            TeamMember teamMember = null;
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from dbo.TeamMember where TeamMember.IdTeamMember = @IdTeamMember";
                com.Parameters.Clear();
                com.Parameters.AddWithValue("IdTeamMember", id);
                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    teamMember = new TeamMember
                    {
                        IdTeamMember = int.Parse(dr["IdTeamMember"].ToString()),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Email = dr["Email"].ToString()
                    };
                }
                else
                {
                    throw new Exception("Podaj poprawne Id członka zespołu");

                    return null;
                }
                dr.Close();


                var created = new List<Models.Task>();
                com.CommandText = @"select Task.Name, Task.Description, Project.Name, Task.Deadline, TaskType.Name from dbo.Task  
                join dbo.Project on Project.IdTeam = Task.IdTeam
                join dbo.TaskType on TaskType.IdTaskType = Task.IdTaskType
                where Task.IdCreator = @id
                order by Task.Deadline desc";
                com.Parameters.Clear();
                com.Parameters.AddWithValue("id", id);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var t = new Models.Task()
                    {
                        TaskTypeName = dr["TaskType.Name"].ToString(),
                        Deadline = DateTime.Parse(dr["Task.Deadline"].ToString()),
                        Description = dr["Task.Description"].ToString(),
                        ProjectName = dr["Project.Name"].ToString(),
                        TaskName = dr["Task.Name"].ToString()
                    };
                    created.Add(t);
                }
                teamMember.CreatedProjects = created;
                dr.Close();




                var assigned = new List<Models.Task>();
                com.CommandText = @"select Task.Name, Task.Description, Project.Name, Task.Deadline, TaskType.Name from dbo.Task  
                join dbo.Project on Project.IdTeam = Task.IdTeam
                join dbo.TaskType on TaskType.IdTaskType = Task.IdTaskType
                where Task.IdAssignedTo = @id";
                com.Parameters.Clear();
                com.Parameters.AddWithValue("id", id);
                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var t = new Models.Task()
                    {
                        TaskTypeName = dr["TaskType.Name"].ToString(),
                        Deadline = DateTime.Parse(dr["Task.Deadline"].ToString()),
                        Description = dr["Task.Description"].ToString(),
                        ProjectName = dr["Project.Name"].ToString(),
                        TaskName = dr["Task.Name"].ToString()
                    };
                    assigned.Add(t);
                }
                teamMember.AssignedProjects = assigned;
                dr.Close();

                
            }
            return teamMember;
        }



        public bool DeleteProject(int id)
        {
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                try
                {
                    com.Transaction = con.BeginTransaction();

                    com.CommandText = "select * from dbo.Project where IdTeam = @id";
                    com.Parameters.AddWithValue("id", id);
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        throw new Exception("Podaj poprawne Id projektu");
                        return false;
                    }
                    dr.Close();

                    var list = new List<int>();
                    com.CommandText = "select IdTask from dbo.Task where IdTeam = @id;";
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        list.Add(int.Parse(dr["IdTask"].ToString()));
                    }
                    dr.Close();

                    if (list.Any())
                    {
                        foreach (var idTask in list)
                        {
                            com.CommandText = "delete from dbo.Task where idTask = @idTask";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("idTask", idTask);
                            com.ExecuteNonQuery();
                        }
                    }

                    com.CommandText = "delete from dbo.Project where IdTeam = @id";
                    com.Parameters.AddWithValue("id", id);
                    com.ExecuteNonQuery();

                    com.Transaction.Commit();

                    return true;
                }
                catch (SqlException exc)
                {
                    com.Transaction.Rollback();
                    throw new Exception(exc.Message);
                }
            }
        }


    }
}
