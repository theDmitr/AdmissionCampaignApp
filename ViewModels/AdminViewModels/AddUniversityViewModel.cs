using AdmissionCampaign.Commands;
using AdmissionCampaign.Converters;
using AdmissionCampaign.ViewModels.Base;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class AddUniversityViewModel : ViewModel
    {
        public AddUniversityViewModel() { }

        #region BindingFields
        private string name = "";
        private string login = "";
        private string password;
        private string errorMessage;

        public string Name { get => name; set => Set(ref name, value); }
        public string Login { get => login; set => Set(ref login, value); }
        public string Password { get => password; set => Set(ref password, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminPanel { get => new(PageUriProvider.AdminMenu); }

        public PageCallbackCommand Add { get => new(SaveCallback); }
        #endregion

        private void SaveCallback(Page page)
        {
            if (!IsValidName(Name))
            {
                ErrorMessage = "Некорректно введено название!";
                return;
            }

            if (dataContext.UniversityNameExists(Name))
            {
                ErrorMessage = "ВУЗ с данным названием уже зарегистрирован!";
                return;
            }

            if (!IsValidLogin(Login))
            {
                ErrorMessage = "Некорректно введён логин!";
                return;
            }

            if (Login.Length < 4 || Login.Length > 16)
            {
                ErrorMessage = "Длина логина может быть от 4 до 16 символов латинского алфавита!";
                return;
            }

            if (dataContext.LoginExists(Login))
            {
                ErrorMessage = "Введённый логин занят!";
                return;
            }

            if (Password == null)
            {
                ErrorMessage = "Придумайте пароль!";
                return;
            }

            _ = dataContext.RegisterUniversity(Login, SecureStringToHashStringConverter.ConvertStringToSecureString(Password), Name);

            NavigateToPage(page, PageUriProvider.AdminMenu);
        }
    }
}
