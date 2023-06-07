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
        private DataContext()
        {
            _ = Database.EnsureCreated();
        }

        public static DataContext Instance { get; } = new();
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
            _ = optionsBuilder.UseSqlite("Filename=acdb.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<UniversitySpecialityAdmissionCampaigh>().ToTable("UniversitySpecialityAdmissionCampaighs");
            _ = modelBuilder.Entity<ExamUniversitySpeciality>().ToTable("ExamUniversitySpecialities");
            _ = modelBuilder.Entity<Speciality>().ToTable("Specialities");
            _ = modelBuilder.Entity<University>().ToTable("Universities");
            _ = modelBuilder.Entity<Petition>().ToTable("Petitions");
            _ = modelBuilder.Entity<Enrolle>().ToTable("Enrolles");
            _ = modelBuilder.Entity<User>().ToTable("Users")
                .HasData(
                new User("admin", SecureStringToHashStringConverter.ConvertSecureStringToString(SecureStringToHashStringConverter.ConvertStringToSecureString("admin")), User.AccountType.Admin) { ID = 1 });
            _ = modelBuilder.Entity<Exam>().ToTable("Exams");
        }
        #endregion

        #region Interaction
        public bool LoginExists(string login)
        {
            return Users.Any(u => u.Login == login);
        }

        public bool PassportExists(string passport)
        {
            return Enrolles.Any(e => e.Passport == passport);
        }

        public bool UniversityNameExists(string name)
        {
            return Universities.Any(u => u.Name == name);
        }

        public bool SpecialitiesNameExists(string name)
        {
            return Specialities.Any(s => s.Name == name);
        }

        public bool ExamsNameExists(string name)
        {
            return Exams.Any(e => e.Name == name);
        }

        public bool SpecialityNameExists(string name)
        {
            return Specialities.Any(s => s.Name == name);
        }

        public bool ExamNameExists(string name)
        {
            return Exams.Any(e => e.Name == name);
        }

        public bool SpecialityCodeExists(string code)
        {
            return Specialities.Any(s => s.Code == code);
        }

        public bool SpecialitiesExamExists(int ID)
        {
            return Specialities.Any(s => s.Exam1ID == ID || s.Exam2ID == ID || s.Exam3ID == ID);
        }

        public User GetUserByLogin(string login)
        {
            IQueryable<User> matches = Users.Where(u => u.Login == login);
            return matches.Any() ? matches.Single() : null;
        }

        #region Registries
        public Enrolle RegisterEnrolle(string login, SecureString password, string name, string surname, string patronymic, string passport)
        {
            User user = new(login, SecureStringToHashStringConverter.ConvertSecureStringToString(password), User.AccountType.Enrolle);
            _ = Users.Add(user);
            _ = SaveChanges();

            Enrolle enrolle = new(name, surname, patronymic, passport, user.ID);
            _ = Enrolles.Add(enrolle);
            _ = SaveChanges();

            return enrolle;
        }

        public University RegisterUniversity(string login, SecureString password, string name)
        {
            User user = new(login, SecureStringToHashStringConverter.ConvertSecureStringToString(password), User.AccountType.University);
            _ = Users.Add(user);
            _ = SaveChanges();

            University university = new(name, user.ID);
            _ = Universities.Add(university);
            _ = SaveChanges();

            return university;
        }

        public Speciality RegisterSpeciality(string name, string code)
        {
            Speciality speciality = new(name, code);
            _ = Specialities.Add(speciality);
            _ = SaveChanges();

            return speciality;
        }

        public Exam RegisterExam(string name)
        {
            Exam exam = new(name);
            _ = Exams.Add(exam);
            _ = SaveChanges();

            return exam;
        }
        #endregion

        #region Removes
        public void RemoveUniversity(int ID)
        {
            University university = Universities.Where(u => u.ID == ID).Single();
            _ = Users.Where(u => u.ID == university.UserID).ExecuteDelete();
            _ = Universities.Where(u => u.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }

        public void RemoveSpeciality(int ID)
        {
            _ = Specialities.Where(u => u.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }

        public void RemoveExam(int ID)
        {
            _ = Exams.Where(e => e.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }
        #endregion

        #endregion

        #endregion
    }
}
