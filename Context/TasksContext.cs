using API_UP2.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_UP2.Context
{
    public class TasksContext : DbContext
    {
        public DbSet<Tasks> Tasks { get; set; }
        public TasksContext()
        {
            Database.EnsureCreated(); 
            Tasks.Load(); 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=127.0.0.1;" +
                "uid=root;" +
                "pwd=;" +
                "database=UP",
                new MySqlServerVersion(new Version(8, 0, 11)));
        }  
    }
}