using AdmissionCampaign.Commands;
using AdmissionCampaign.Converters;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Security;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class RegisterViewModel : ViewModel
    {
        public RegisterViewModel() { }

        #region BindingFields
        private string login = "";
        private SecureString password;
        private SecureString confirmPassword;
        private string surname = "";
        private string name = "";
        private string patronymic = "";
        private string passport = "";
        private string errorMessage;

        public string Login { get => login; set => Set(ref login, value); }
        public SecureString Password { get => password; set => Set(ref password, value); }
        public SecureString ConfirmPassword { get => confirmPassword; set => Set(ref confirmPassword, value); }
        public string Surname { get => surname; set => Set(ref surname, value); }
        public string Name { get => name; set => Set(ref name, value); }
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }
        public string Passport { get => passport; set => Set(ref passport, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToChooseLogin { get => new(PageUriProvider.ChooseLogin); }
        public PageCallbackCommand Register { get => new(RegisterCallback); }
        #endregion

        private void RegisterCallback(Page page)
        {
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

            if (!IsValidName(Surname))
            {
                ErrorMessage = "Некорректно введена фамилия!";
                return;
            }

            if (!IsValidName(Name))
            {
                ErrorMessage = "Некорректно введено имя!";
                return;
            }

            if (!IsValidName(Patronymic))
            {
                ErrorMessage = "Некорректно введено отчество!";
                return;
            }

            if (!IsValidPassport(Passport))
            {
                ErrorMessage = "Некорректно указан номер пасспорта!";
                return;
            }

            if (dataContext.PassportExists(Passport))
            {
                ErrorMessage = "Аккаунт с таким номером паспорта уже зарегистрирован!";
                return;
            }

            if (Password == null)
            {
                ErrorMessage = "Придумайте пароль!";
                return;
            }

            if (ConfirmPassword == null)
            {
                ErrorMessage = "Подтвердите пароль!";
                return;
            }

            string password = SecureStringToHashStringConverter.ConvertSecureStringToString(Password);
            string confirmPassword = SecureStringToHashStringConverter.ConvertSecureStringToString(ConfirmPassword);

            if (password != confirmPassword)
            {
                ErrorMessage = "Введённые пароли не совпадают!";
                return;
            }

            Enrolle enrolle = dataContext.RegisterEnrolle(Login, Password, Name, Surname, Patronymic, Passport);
            dataContext.SessionUserID = enrolle.UserID;

            NavigateToPage(page, PageUriProvider.EnrollePersonal);
        }
    }
}
