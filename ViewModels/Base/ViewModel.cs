using AdmissionCampaign.Data;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AdmissionCampaign.ViewModels.Base
{
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
                NavigationService.GetNavigationService(page).Navigate(uri);
        }

        protected void QuitCallback(Page page)
        {
            dataContext.SessionUserID = -1;
            NavigateToPage(page, new Uri("../Views/Pages/ChooseLoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
        #endregion

        #region Validates
        protected static bool IsValidLogin(string login) => new Regex("[A-Za-z0-9]").IsMatch(login);

        protected static bool IsValidName(string name) => new Regex("[А-Яа-яЁё]").IsMatch(name) && name.Length > 1;

        protected static bool IsValidPassport(string passport) => new Regex("^[0-9]{6}$").IsMatch(passport);
        #endregion
    }

    public class PageUriProvider
    {
        #region Admin
        public static Uri AdminAddUniversity { get; } = GetUri("Admin/AddUniversityPage");
        public static Uri AdminMenu { get; } = GetUri("Admin/AdminMenuPage");
        public static Uri AdminUniversitiesList { get; } = GetUri("Admin/UniversitiesListPage");
        public static Uri AdminAddSpeciality { get; } = GetUri("Admin/AddSpecialitylPage");
        #endregion

        #region Enrolle
        public static Uri EnrolleRegister { get; } = GetUri("Enrolle/RegisterPage");
        public static Uri EnrollePersonal { get; } = GetUri("Enrolle/PersonalPage");
        public static Uri EnrolleApplication { get; } = GetUri("Enrolle/ApplicationPage");
        public static Uri EnrolleChangeData { get; } = GetUri("Enrolle/ChangeDataPage");
        #endregion

        #region University
        public static Uri UniversityPersonal { get; } = GetUri("University/UniversityPersonalPage");
        #endregion

        #region Main
        public static Uri AuthPage { get; } = GetUri("AuthPage");
        public static Uri ChooseLogin { get; } = GetUri("ChooseLoginPage");
        #endregion

        private static Uri GetUri(string path) => new($"../../Views/Pages/{path}.xaml", UriKind.RelativeOrAbsolute);
    }
}
