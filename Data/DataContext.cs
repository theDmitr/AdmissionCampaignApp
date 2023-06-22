using AdmissionCampaign.Converters;
using AdmissionCampaign.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;

namespace AdmissionCampaign.Data
{
    /// <summary>
    /// Класс, проводящий работу с ORM (EntityFramework)
    /// </summary>
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
        public int SessionUserID { get; set; } = -1; // ID пользователя, находящегося в текущей сессии

        #region Tables
        public DbSet<User> Users { get; set; }
        public DbSet<UniversitySpecialityAdmissionCampaigh> UniversitySpecialityAdmissionCampaigns { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<UniversitySpeciality> UniversitySpecialities { get; set; }
        public DbSet<Petition> Petitions { get; set; }
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
            _ = modelBuilder.Entity<Speciality>().ToTable("Specialities");
            _ = modelBuilder.Entity<UniversitySpeciality>().ToTable("UniversitySpecialities");
            _ = modelBuilder.Entity<University>().ToTable("Universities");
            _ = modelBuilder.Entity<Petition>().ToTable("Petitions");
            _ = modelBuilder.Entity<Enrolle>().ToTable("Enrolles");
            _ = modelBuilder.Entity<User>().ToTable("Users")
                .HasData(
                new User("admin", SecureStringToHashStringConverter.ConvertSecureStringToString(SecureStringToHashStringConverter.ConvertStringToSecureString("admin")), User.AccountType.Admin) { ID = 1 });
            _ = modelBuilder.Entity<Exam>().ToTable("Exams");
        }
        #endregion

        #region Getters
        /// <summary>
        /// Получение University из текущей сессии
        /// </summary>
        public University GetUniversityFromSession => GetUserByID(SessionUserID).AcountType == User.AccountType.University
                ? Universities.Where(u => u.UserID == SessionUserID).Single() : null;

        /// <summary>
        /// Получение Enrolle из текущей сессии
        /// </summary>
        public Enrolle GetEnrolleFromSession => GetUserByID(SessionUserID).AcountType == User.AccountType.Enrolle
            ? Enrolles.Where(e => e.UserID == SessionUserID).Single() : null;

        /// <summary>
        /// Получание Specialities которые используется в UniversitySpecialities в конкретном University
        /// </summary>
        /// <param name="universityID"></param>
        /// <returns></returns>
        public ObservableCollection<Speciality> GetUniversitySpecialitiesAsSpecialities(int universityID)
        {
            return new(UniversitySpecialities
            .Where(us => us.UniversityID == universityID)
            .Join(Specialities, us => us.SpecialityID, s => s.ID, (us, s) => s));
        }

        public ObservableCollection<UniversitySpeciality> GetUniversitySpecialities(int universityID)
        {
            return new(UniversitySpecialities.Where(us => us.UniversityID == universityID));
        }

        public ObservableCollection<UniversitySpecialityAdmissionCampaigh> GetUniversitySpecialityAdmissionCampaigns(int universityID)
        {
            return new(UniversitySpecialityAdmissionCampaigns
            .Where(ac => UniversitySpecialities.Where(us => us.ID == ac.UniversitySpecialityID).Single().UniversityID == universityID));
        }

        /// <summary>
        /// Получение всех заявок в конкретный ВУЗ
        /// </summary>
        /// <param name="universityID"></param>
        /// <returns></returns>
        public ObservableCollection<Petition> GetUniversityPetitions(int universityID)
        {
            return new(Petitions
                .Where(p => universityID == UniversitySpecialities
                .Where(us => us.ID == UniversitySpecialityAdmissionCampaigns
                .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID)
                .Single().UniversitySpecialityID)
                .Single().UniversityID));
        }

        /// <summary>
        /// Получает User по ID из параметра
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public User GetUserByID(int ID)
        {
            return Users.Where(u => u.ID == ID).Single();
        }

        /// <summary>
        /// Получает User по логину из параметра
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public User GetUserByLogin(string login)
        {
            IQueryable<User> matches = Users.Where(u => u.Login == login);
            return matches.Any() ? matches.Single() : null;
        }

        /// <summary>
        /// Получение специальности по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Speciality GetSpeciality(int ID)
        {
            return Specialities.Where(s => s.ID == ID).Single();
        }

        /// <summary>
        /// Получение специальности ВУЗа по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public UniversitySpeciality GetUniversitySpeciality(int ID)
        {
            return UniversitySpecialities.Where(us => us.ID == ID).Single();
        }

        /// <summary>
        /// Получение специальности ВУЗа по специальности
        /// </summary>
        /// <param name="universityID"></param>
        /// <param name="specialityID"></param>
        /// <returns></returns>
        public UniversitySpeciality GetUniversitySpecialityBySpeciality(int universityID, int specialityID)
        {
            return UniversitySpecialities.Where(us => us.UniversityID == universityID && us.SpecialityID == specialityID).Single();
        }

        /// <summary>
        /// Получение предмета по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Exam GetExam(int ID)
        {
            return Exams.Where(e => e.ID == ID).Single();
        }
        #endregion

        #region Existes
        /// <summary>
        /// Проверяет используется ли логин из параметра в Users
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool IsLoginExists(string login)
        {
            return Users.Any(u => u.Login == login);
        }

        /// <summary>
        /// Проверяет используется ли паспорт из параметра в Enrolles
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        public bool IsPassportExists(string passport)
        {
            return Enrolles.Any(e => e.Passport == passport);
        }

        /// <summary>
        /// Проверяет используется ли имя из параметра в Universities
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsUniversityNameExists(string name)
        {
            return Universities.Any(u => u.Name == name);
        }

        /// <summary>
        /// Проверяет существование Speciality по имени из параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsSpecialityNameExists(string name)
        {
            return Specialities.Any(s => s.Name == name);
        }

        /// <summary>
        /// Проверяет существование UniversitySpeciality по ID в конкретном University
        /// </summary>
        /// <param name="universityID"></param>
        /// <param name="SpecialityID"></param>
        /// <returns></returns>
        public bool IsUniversitySpecialityExists(int universityID, int SpecialityID)
        {
            return GetUniversitySpecialities(universityID).Any(s => s.SpecialityID == SpecialityID);
        }

        /// <summary>
        /// Проверяет существование Exam с именем из параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExamNameExists(string name)
        {
            return Exams.Any(e => e.Name == name);
        }

        /// <summary>
        /// Проверяет существование кода специальности
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsSpecialityCodeExists(string code)
        {
            return Specialities.Any(s => s.Code == code);
        }

        /// <summary>
        /// Проверяет используется ли в данный момент Exam по ID (Если удалить Exam, который где-то используется, то к хорошему это не приведёт)
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool IsUniversitySpecialityAdmissionCampaighExamExists(int ID)
        {
            return UniversitySpecialityAdmissionCampaigns.Any(s => s.Exam1ID == ID || s.Exam2ID == ID || s.Exam3ID == ID);
        }

        /// <summary>
        /// Проверят существование приёмной кампании в конкретном University по Speciality и Year
        /// </summary>
        /// <param name="universityID"></param>
        /// <param name="specialityID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsUniversityAdmissionCampaignExists(int universityID, int specialityID, int year)
        {
            if (GetUniversitySpecialityAdmissionCampaigns(universityID)
                .Any(ac => ac.UniversitySpecialityID == GetUniversitySpecialityBySpeciality(universityID, specialityID).ID))
            {
                UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh = GetUniversitySpecialityAdmissionCampaigns(universityID)
                .Where(ac => ac.UniversitySpecialityID == GetUniversitySpecialityBySpeciality(universityID, specialityID).ID).Single();
                if (universitySpecialityAdmissionCampaigh.Year == year)
                {
                    return true;
                }
            }
            return false;
        }

        #region Registries
        /// <summary>
        /// Добавляет новый Enrolle (Register)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="patronymic"></param>
        /// <param name="passport"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Добавляет новый University (Admin)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Добавляет Speciality (Global, Admin)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Speciality RegisterSpeciality(string name, string code)
        {
            Speciality speciality = new(name, code);
            _ = Specialities.Add(speciality);
            _ = SaveChanges();

            return speciality;
        }

        /// <summary>
        /// Добавляет UniversitySpeciality (Local, University)
        /// </summary>
        /// <param name="universityID"></param>
        /// <param name="specialityID"></param>
        /// <returns></returns>
        public UniversitySpeciality RegisterUniversitySpeciality(int universityID, int specialityID)
        {
            UniversitySpeciality universitySpeciality = new(universityID, specialityID);
            _ = UniversitySpecialities.Add(universitySpeciality);
            _ = SaveChanges();

            return universitySpeciality;
        }

        /// <summary>
        /// Добавляет новый Exam по имени из параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Exam RegisterExam(string name)
        {
            Exam exam = new(name);
            _ = Exams.Add(exam);
            _ = SaveChanges();

            return exam;
        }

        /// <summary>
        /// Добавляет UniversitySpecialityAdmissionCampaigh (Local, University)
        /// </summary>
        /// <param name="universitySpecialityID"></param>
        /// <param name="placesCount"></param>
        /// <param name="year"></param>
        /// <param name="exam1ID"></param>
        /// <param name="exam2ID"></param>
        /// <param name="exam3ID"></param>
        /// <returns></returns>
        public UniversitySpecialityAdmissionCampaigh RegisterUniversitySpecialityAdmissionCampaigh(int universitySpecialityID, int placesCount, int year, int exam1ID, int exam2ID, int exam3ID)
        {
            UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh = new(universitySpecialityID, placesCount, year, exam1ID, exam2ID, exam3ID);
            _ = UniversitySpecialityAdmissionCampaigns.Add(universitySpecialityAdmissionCampaigh);
            _ = SaveChanges();

            return universitySpecialityAdmissionCampaigh;
        }

        /// <summary>
        /// Добавляет Petition (Enrolle)
        /// </summary>
        /// <param name="enrolleID"></param>
        /// <param name="universitySpecialityAdmissionCampaighID"></param>
        /// <param name="exam1ID"></param>
        /// <param name="exam2ID"></param>
        /// <param name="exam3ID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public Petition RegisterPetition(int enrolleID, int universitySpecialityAdmissionCampaighID, int exam1Value, int exam2Value, int exam3Value, DateTime date)
        {
            Petition petition = new(enrolleID, universitySpecialityAdmissionCampaighID, exam1Value, exam2Value, exam3Value, date);
            _ = Petitions.Add(petition);
            _ = SaveChanges();

            return petition;
        }
        #endregion

        #region Removes
        /// <summary>
        /// Удаляет University и соответствующего User по ID из параметра
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveUniversity(int ID)
        {
            University university = Universities.Where(u => u.ID == ID).Single();
            _ = Users.Where(u => u.ID == university.UserID).ExecuteDelete();
            _ = Universities.Where(u => u.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }

        /// <summary>
        /// Удаляет Speciality по ID из параметра
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveSpeciality(int ID)
        {
            _ = Specialities.Where(u => u.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }

        /// <summary>
        /// Удаляет Exam по ID из параметра
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveExam(int ID)
        {
            _ = Exams.Where(e => e.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }

        /// <summary>
        /// Удаляет приемную кампанию
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveUniversitySpecialityAdmissionCampaign(int ID)
        {
            _ = UniversitySpecialityAdmissionCampaigns.Where(ac => ac.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }

        /// <summary>
        /// Удаляет специальность ВУЗа
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveUniversitySpeciality(int ID)
        {
            _ = UniversitySpecialities.Where(us => us.ID == ID).ExecuteDelete();
            _ = SaveChanges();
        }
        #endregion

        #endregion

        #endregion
    }
}
