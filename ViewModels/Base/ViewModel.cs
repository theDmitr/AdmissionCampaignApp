using AdmissionCampaign.Data;
using AdmissionCampaign.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AdmissionCampaign.ViewModels.Base
{
    /// <summary>
    /// Базовый класс ViewModel
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        protected readonly DataContext dataContext = DataContext.Instance;

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region NavigationAndQuit
        public static void NavigateToPage(Page page, Uri uri)
        {
            if (page != null && uri != null)
            {
                _ = NavigationService.GetNavigationService(page).Navigate(uri);
            }
        }

        protected void QuitCallback(Page page)
        {
            dataContext.SessionUserID = -1;
            NavigateToPage(page, new Uri("../Views/Pages/ChooseLoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
        #endregion

        #region Validates
        protected static bool IsValidLogin(string login)
        {
            return new Regex("[A-Za-z0-9]").IsMatch(login);
        }

        protected static bool IsValidName(string name)
        {
            return new Regex("[А-Яа-яЁё]").IsMatch(name) && name.Length > 1;
        }

        protected static bool IsValidPassport(string passport)
        {
            return new Regex("^[0-9]{6}$").IsMatch(passport);
        }

        protected static bool IsValidSpecialityName(string name)
        {
            return new Regex("[А-Яа-яЁё\\s]+").IsMatch(name);
        }

        protected static bool IsValidSpecialityCode(string code)
        {
            return new Regex("\\d{2}\\.\\d{2}\\.\\d{2}").IsMatch(code);
        }
        #endregion

        #region GetUniversitySpecialityAndAdmissionCampaighs
        /// <summary>
        /// Метод для получения всех приемных кампаний ВУЗа в специальном объекте-контейнере
        /// </summary>
        /// <param name="universityID"></param>
        /// <returns></returns>
        protected ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> GetUniversitySpecialityAndAdmissionCampaighs(int universityID)
        {
            ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> result = new();
            ObservableCollection<UniversitySpecialityAdmissionCampaigh> universitySpecialityAdmissionCampaighs = dataContext.GetUniversitySpecialityAdmissionCampaigns(universityID);

            for (int i = 0; i < universitySpecialityAdmissionCampaighs.Count; i++)
            {
                UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh = universitySpecialityAdmissionCampaighs[i];

                result.Add(new(
                    dataContext.GetUniversityFromSession,
                    dataContext.GetSpeciality(dataContext.GetUniversitySpeciality(universitySpecialityAdmissionCampaigh.UniversitySpecialityID).SpecialityID),
                    universitySpecialityAdmissionCampaigh.ID,
                    universitySpecialityAdmissionCampaigh.PlacesCount,
                    universitySpecialityAdmissionCampaigh.Year,
                    dataContext.GetExam(universitySpecialityAdmissionCampaigh.Exam1ID),
                    dataContext.GetExam(universitySpecialityAdmissionCampaigh.Exam2ID),
                    dataContext.GetExam(universitySpecialityAdmissionCampaigh.Exam3ID)));
            }
            return result;
        }

        /// <summary>
        /// Получение всех принятых заявок на конкретную приемную кампанию
        /// </summary>
        /// <param name="universityID"></param>
        /// <param name="admissionCampaighID"></param>
        /// <returns></returns>
        protected ObservableCollection<EnrolleAndPetition> GetEnrollesAndPetitions(int universityID, int admissionCampaighID)
        {
            ObservableCollection<EnrolleAndPetition> result = new();

            ObservableCollection<Petition> petitions = new(dataContext.GetUniversityPetitions(universityID)
                .Where(p => p.UniversitySpecialityAdmissionCampaighID == admissionCampaighID && p.EnrolleCurrentStatus == Petition.EnrolleStatus.Accepted)
                .OrderByDescending(p => p.Exam1Value + p.Exam2Value + p.Exam3Value));

            foreach (Petition petition in petitions)
            {
                Enrolle enrolle = dataContext.Enrolles.Where(e => e.ID == petition.EnrolleID).Single();
                UniversitySpeciality universitySpeciality = dataContext.UniversitySpecialities
                    .Where(us => us.ID == dataContext.UniversitySpecialityAdmissionCampaigns
                    .Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID)
                    .Single().UniversitySpecialityID)
                    .Single();
                Speciality speciality = dataContext.GetSpeciality(universitySpeciality.SpecialityID);
                UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh = dataContext.UniversitySpecialityAdmissionCampaigns
                    .Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID)
                    .Single();

                result.Add(new(
                    string.Join(' ', new string[] { enrolle.Name, enrolle.Surname, enrolle.Patronymic }),
                    speciality,
                    dataContext.GetExam(universitySpecialityAdmissionCampaigh.Exam1ID),
                    dataContext.GetExam(universitySpecialityAdmissionCampaigh.Exam2ID),
                    dataContext.GetExam(universitySpecialityAdmissionCampaigh.Exam3ID),
                    petition.Exam1Value,
                    petition.Exam2Value,
                    petition.Exam3Value,
                    petitions.IndexOf(petition) + 1));
            }
            return result;
        }
        #endregion
    }

    /// <summary>
    /// Класс-контейнер для хранения путей к Представлениям (Views)
    /// </summary>
    public class PageUriProvider
    {
        #region Admin
        public static Uri AdminAddUniversity { get; } = GetUri("Admin/AddUniversityPage");
        public static Uri AdminMenu { get; } = GetUri("Admin/AdminMenuPage");
        public static Uri AdminAddSpeciality { get; } = GetUri("Admin/AddSpecialityPage");
        public static Uri AdminAddExam { get; } = GetUri("Admin/AddExamPage");
        public static Uri AdminUniversitiesList { get; } = GetUri("Admin/UniversitiesListPage");
        public static Uri AdminSpecialitiesList { get; } = GetUri("Admin/SpecialitiesListPage");
        public static Uri AdminExamsList { get; } = GetUri("Admin/ExamsListPage");
        public static Uri AdminEnrollesList { get; } = GetUri("Admin/EnrollesListPage");
        public static Uri AdminChangeEnrollePassword { get; } = GetUri("Admin/ChangeEnrollePasswordPage");
        public static Uri AdminChangeUniversityPassword { get; } = GetUri("Admin/ChangeUniversityPasswordPage");
        public static Uri AdminUniversityEdit { get; } = GetUri("Admin/UniversityEditPage");
        public static Uri AdminSpecialityEdit { get; } = GetUri("Admin/SpecialityEditPage");
        public static Uri AdminExamEdit { get; } = GetUri("Admin/ExamEditPage");
        #endregion

        #region Enrolle
        public static Uri EnrolleRegister { get; } = GetUri("Enrolle/RegisterPage");
        public static Uri EnrollePersonal { get; } = GetUri("Enrolle/PersonalPage");
        public static Uri EnrolleApplication { get; } = GetUri("Enrolle/ApplicationPage");
        public static Uri EnrollePetitions { get; } = GetUri("Enrolle/PetitionsPage");
        public static Uri EnrolleChangeData { get; } = GetUri("Enrolle/ChangeDataPage");
        #endregion

        #region University
        public static Uri UniversityPersonal { get; } = GetUri("University/UniversityPersonalPage");
        public static Uri UniversityAnnounceAdmissionCampaigh { get; } = GetUri("University/AnnounceAdmissionCampaighPage");
        public static Uri UniversityAdmissionCampaighsList { get; } = GetUri("University/AdmissionCampaighsListPage");
        public static Uri UniversityEnrollesList { get; } = GetUri("University/EnrollesListPage");
        public static Uri UniversityAddUniversitySpeciality { get; } = GetUri("University/AddUniversitySpecialityPage");
        public static Uri UniversitySpecialitiesList { get; } = GetUri("University/UniversitySpecialitiesListPage");
        #endregion

        #region Main
        public static Uri AuthPage { get; } = GetUri("AuthPage");
        public static Uri ChooseLogin { get; } = GetUri("ChooseLoginPage");
        #endregion

        /// <summary>
        /// Метод для получения пути к View (для избежания дублирующего кода)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Uri GetUri(string path)
        {
            return new($"../../Views/Pages/{path}.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
