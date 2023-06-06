using AdmissionCampaign.Converters;
using AdmissionCampaign.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security;

namespace AdmissionCampaign.Data
{
    public class DataContext : DbContext
    {
        #region Singleton
        private static readonly DataContext instance = new();

        private DataContext()
        {
            Database.EnsureCreated();
        }

        public static DataContext Instance { get => instance; }
        #endregion

        #region DataBase

        public int SessionUserID { get; set; } = -1;

        #region Tables
        public DbSet<User> Users { get; set; }
        public DbSet<UniversitySpecialityAdmissionCampaigh> UniversitySpecialityAdmissionCampaighs { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Petition> Petitions { get; set; }
        public DbSet<ExamUniversitySpeciality> ExamUniversitySpecialities { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Enrolle> Enrolles { get; set; }
        #endregion

        #region ConfigurationAndSimulation
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=acdb.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UniversitySpecialityAdmissionCampaigh>().ToTable("UniversitySpecialityAdmissionCampaighs");
            modelBuilder.Entity<ExamUniversitySpeciality>().ToTable("ExamUniversitySpecialities");
            modelBuilder.Entity<Speciality>().ToTable("Specialities");
            modelBuilder.Entity<University>().ToTable("Universities");
            modelBuilder.Entity<Petition>().ToTable("Petitions");
            modelBuilder.Entity<Enrolle>().ToTable("Enrolles");
            modelBuilder.Entity<User>().ToTable("Users")
                .HasData(
                new User("admin", SecureStringToHashStringConverter.ConvertSecureStringToString(SecureStringToHashStringConverter.ConvertStringToSecureString("admin")), User.AccountType.Admin) { ID = 1 });
            modelBuilder.Entity<Exam>().ToTable("Exams");
        }
        #endregion

        #region Interaction
        public bool LoginExists(string login) => Users.Any(u => u.Login == login);
        public bool PassportExists(string passport) => Enrolles.Any(e => e.Passport == passport);
        public bool UniversityNameExists(string name) => Universities.Any(u => u.Name == name);

        public User GetUserByLogin(string login)
        {
            IQueryable<User> matches = Users.Where(u => u.Login == login);
            return matches.Any() ? matches.Single() : null;
        }

        #region Registries
        public Enrolle RegisterEnrolle(string login, SecureString password, string name, string surname, string patronymic, string passport)
        {
            User user = new(login, SecureStringToHashStringConverter.ConvertSecureStringToString(password), User.AccountType.Enrolle);
            Users.Add(user);
            SaveChanges();

            Enrolle enrolle = new(name, surname, patronymic, passport, user.ID);
            Enrolles.Add(enrolle);
            SaveChanges();

            return enrolle;
        }

        public University RegisterUniversity(string login, SecureString password, string name)
        {
            User user = new(login, SecureStringToHashStringConverter.ConvertSecureStringToString(password), User.AccountType.University);
            Users.Add(user);
            SaveChanges();

            University university = new(name, user.ID);
            Universities.Add(university);
            SaveChanges();

            return university;
        }
        #endregion

        #endregion

        #endregion
    }
}
