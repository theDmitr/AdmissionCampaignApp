using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class AddUniversitySpecialityViewModel : ViewModel
    {
        public AddUniversitySpecialityViewModel()
        {
            specialities = new(dataContext.Specialities);
        }

        #region BindingFields
        private ObservableCollection<Speciality> specialities;

        private Speciality selected;
        private string errorMessage;

        public ObservableCollection<Speciality> Specialities { get => specialities; set => Set(ref specialities, value); }

        public Speciality Selected { get => selected; set => Set(ref selected, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        public PageCallbackCommand Add => new(AddCallback);
        #endregion

        private void AddCallback(Page page)
        {
            if (Selected == null)
            {
                ErrorMessage = "Выберите специальность из списка!";
                return;
            }

            if (dataContext.IsUniversitySpecialityExists(dataContext.GetUniversityFromSession.ID, Selected.ID))
            {
                ErrorMessage = "Данная специальность уже существует в ВУЗе!";
                return;
            }

            _ = dataContext.RegisterUniversitySpeciality(dataContext.GetUniversityFromSession.ID, Selected.ID);

            NavigateToPage(page, PageUriProvider.UniversityPersonal);
        }
    }
}
