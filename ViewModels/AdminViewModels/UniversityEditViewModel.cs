using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class UniversityEditViewModel : ViewModel
    {
        public static int UniversityID { get; set; }

        public UniversityEditViewModel()
        {
            university = dataContext.Universities.Where(u => u.ID == UniversityID).Single();
            Name = university.Name;
        }

        private readonly University university;

        #region BindingFields
        private string name = "";
        private string errorMessage;

        public string Name { get => name; set => Set(ref name, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminUniversitiesList => new(PageUriProvider.AdminUniversitiesList);
        public PageCallbackCommand Save => new(SaveCallback);
        public PageCallbackCommand Remove => new(RemoveCallback);
        #endregion

        private void SaveCallback(Page page)
        {
            if (!IsValidName(Name))
            {
                ErrorMessage = "Некорректно введено название учебного заведения!";
                return;
            }

            if (university.Name != Name && dataContext.IsUniversityNameExists(Name))
            {
                ErrorMessage = "Аккаунт с таким номером паспорта уже зарегистрирован!";
                return;
            }

            university.Name = Name;

            _ = dataContext.SaveChanges();

            NavigateToPage(page, PageUriProvider.AdminUniversitiesList);
        }

        private void RemoveCallback(Page page)
        {
            dataContext.RemoveUniversity(university.ID);

            NavigateToPage(page, PageUriProvider.AdminUniversitiesList);
        }
    }
}
