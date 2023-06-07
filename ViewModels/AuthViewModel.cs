using AdmissionCampaign.Commands;
using AdmissionCampaign.Converters;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Security;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels
{
    public class AuthViewModel : ViewModel
    {
        public AuthViewModel() { }

        #region BindingFields
        private string login = "";
        private SecureString password;
        private string errorMessage;

        public string Login { get => login; set => Set(ref login, value); }
        public SecureString Password { get => password; set => Set(ref password, value); }
        public string ErrorMessage { get => errorMessage; private set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToChooseLogin => new(PageUriProvider.ChooseLogin);
        public PageCallbackCommand Auth => new(AuthCallback);
        #endregion

        private void AuthCallback(Page page)
        {
            if (Login.Length < 1)
            {
                ErrorMessage = "Введите логин!";
                return;
            }

            if (Password == null || Password.Length < 1)
            {
                ErrorMessage = "Введите пароль!";
                return;
            }

            User user = dataContext.GetUserByLogin(Login);

            if (user == null)
            {
                ErrorMessage = "Пользователь с данным логином не найден!";
                return;
            }

            if (SecureStringToHashStringConverter.ConvertSecureStringToString(Password) != user.Password)
            {
                ErrorMessage = "Неверный пароль!";
                return;
            }

            dataContext.SessionUserID = user.ID;

            if (user.AcountType == User.AccountType.Admin)
            {
                NavigateToPage(page, PageUriProvider.AdminMenu);
            }
            else if (user.AcountType == User.AccountType.University)
            {
                NavigateToPage(page, PageUriProvider.UniversityPersonal);
            }
            else
            {
                NavigateToPage(page, PageUriProvider.EnrollePersonal);
            }
        }
    }
}
