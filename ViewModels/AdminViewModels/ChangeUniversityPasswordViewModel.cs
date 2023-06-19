using AdmissionCampaign.Commands;
using AdmissionCampaign.Converters;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class ChangeUniversityPasswordViewModel : ViewModel
    {
        public static int UserID = -1;

        #region BindingFields
        private string password;
        private string errorMessage;

        public string Password { get => password; set => Set(ref password, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversitiesList { get => new(PageUriProvider.AdminUniversitiesList); }
        public PageCallbackCommand Save { get => new(SaveCallback); }
        #endregion

        private void SaveCallback(Page page)
        {
            if (Password == null)
            {
                ErrorMessage = "Введите пароль!";
                return;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Длина пароля минимум 6 символов!";
                return;
            }

            dataContext.Users.Where(u => u.ID == UserID).Single().Password = SecureStringToHashStringConverter.ConvertSecureStringToString(SecureStringToHashStringConverter.ConvertStringToSecureString(Password));
            _ = dataContext.SaveChanges();

            NavigateToPage(page, PageUriProvider.AdminUniversitiesList);
        }
    }
}
