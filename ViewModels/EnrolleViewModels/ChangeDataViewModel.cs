using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class ChangeDataViewModel : ViewModel
    {
        public ChangeDataViewModel()
        {
            enrolle = dataContext.Enrolles.Where(e => e.UserID == dataContext.SessionUserID).Single();
            Surname = enrolle.Surname;
            Name = enrolle.Name;
            Patronymic = enrolle.Lastname;
            Passport = enrolle.Passport;
        }

        #region BindingFields
        private readonly Enrolle enrolle;
        private string surname;
        private string name;
        private string patronymic;
        private string passport;
        private string errorMessage;

        public string Surname { get => surname; set => Set(ref surname, value); }
        public string Name { get => name; set => Set(ref name, value); }
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }
        public string Passport { get => passport; set => Set(ref passport, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToEnrollePersonal { get => new(PageUriProvider.EnrollePersonal); }
        public PageCallbackCommand Save { get => new(SaveCallback); }
        #endregion

        private void SaveCallback(Page page)
        {
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

            if (enrolle.Passport != Passport && dataContext.PassportExists(Passport))
            {
                ErrorMessage = "Аккаунт с таким номером паспорта уже зарегистрирован!";
                return;
            }

            enrolle.Surname = Surname;
            enrolle.Name = Name;
            enrolle.Lastname = Patronymic;
            enrolle.Passport = Passport;

            dataContext.SaveChanges();

            NavigateToPage(page, PageUriProvider.EnrollePersonal);
        }
    }
}
