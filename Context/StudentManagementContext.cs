using Microsoft.EntityFrameworkCore;
using API_UP2.Models;
using System;
using Newtonsoft.Json.Linq;

namespace API_UP2.Context
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext(DbContextOptions<StudentManagementContext> options)
            : base(options)
        {
        }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<EducationStudent> EducationStudent { get; set; }
        public DbSet<Financy> Financy { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Orphans> Orphans { get; set; }
        public DbSet<RoomsHostel> RoomsHostel { get; set; }
        public DbSet<SocialPayout> SocialPayout { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentDisabledPeople> StudentDisabledPeople { get; set; }
        public DbSet<StudentHostel> StudentHostel { get; set; }
        public DbSet<StudentOVZ> StudentOVZ { get; set; }
        public DbSet<StudentSOP> StudentSOP { get; set; }
        public DbSet<StudentSPPP> StudentSPPP { get; set; }
        public DbSet<StudentSVO> StudentSVO { get; set; }
        public DbSet<TypeOfDisability> TypeOfDisability { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // **Вот исправленная конфигурация для AuditLog:**
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TableName).HasMaxLength(100);
                entity.Property(e => e.Action).HasMaxLength(50);
                entity.Property(e => e.RecordId).IsRequired();
                entity.Property(e => e.ChangedBy).IsRequired();
                entity.Property(e => e.ChangedAt).IsRequired();

                // Игнорируем свойства JObject, чтобы EF не пытался их маппить
                entity.Ignore(e => e.OldData);
                entity.Ignore(e => e.NewData);


            });

            // Остальные конфигурации остаются без изменений
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Education).HasMaxLength(50);
                entity.Property(e => e.Education_Group).HasMaxLength(50);
                entity.Property(e => e.Financy).HasMaxLength(50);
                entity.Property(e => e.DepartmentId).HasMaxLength(50);
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.ToTable("departament");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(100);
                entity.Property(e => e.RoleId).HasMaxLength(50);
            });

            modelBuilder.Entity<Orphans>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StatusAssignmentOrder).HasMaxLength(100);
                entity.Property(e => e.Note).HasColumnType("text");
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<StudentDisabledPeople>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StatusAssignmentOrder).HasMaxLength(100);
                entity.Property(e => e.Note).HasColumnType("text");
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<StudentOVZ>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StatusAssignmentOrder).HasMaxLength(100);
                entity.Property(e => e.Note).HasColumnType("text");
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<StudentSVO>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StatusAssignmentOrder).HasMaxLength(100);
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<StudentSOP>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BasisDelivery).HasColumnType("text");
                entity.Property(e => e.BasisDeRegistration).HasColumnType("text");
                entity.Property(e => e.ReasonDelivery).HasColumnType("text");
                entity.Property(e => e.ReasonDeRegistration).HasColumnType("text");
                entity.Property(e => e.Note).HasColumnType("text");
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<StudentSPPP>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BasisChallenge).HasColumnType("text");
                entity.Property(e => e.StaffPresent).HasColumnType("text");
                entity.Property(e => e.ManagerPresent).HasColumnType("text");
                entity.Property(e => e.ReasonCalling).HasColumnType("text");
                entity.Property(e => e.Decision).HasColumnType("text");
                entity.Property(e => e.Note).HasColumnType("text");
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<SocialPayout>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StatusAssignmentOrder).HasMaxLength(100);
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<StudentHostel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Note).HasColumnType("text");
                entity.Property(e => e.FilePath).HasMaxLength(500);
            });

            modelBuilder.Entity<RoomsHostel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameRoom).HasMaxLength(100);
            });

            modelBuilder.Entity<EducationStudent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameEducationStudent).HasMaxLength(100);
            });

            modelBuilder.Entity<Financy>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameFinancy).HasMaxLength(100);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameGender).HasMaxLength(50);
            });

            modelBuilder.Entity<TypeOfDisability>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameType).HasMaxLength(100);
            });
        }
    }
}