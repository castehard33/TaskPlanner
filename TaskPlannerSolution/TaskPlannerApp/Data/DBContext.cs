using Microsoft.EntityFrameworkCore;
using TaskPlannerApp.Models;

namespace TaskPlannerApp.Data
{
    public class TaskPlannerContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;AttachDbFileName=C:\Users\bmedr\TaskPlannerDB.mdf;Database=TaskPlannerDB;Trusted_Connection=True;");
        }

    }   
}