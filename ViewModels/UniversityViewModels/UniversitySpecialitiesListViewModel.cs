using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class UniversitySpecialitiesListViewModel : ViewModel
    {
        public UniversitySpecialitiesListViewModel()
        {
            Specialities = dataContext.GetUniversitySpecialitiesAsSpecialities(dataContext.GetUniversityFromSession.ID);
        }

        #region BindingFields
        private ObservableCollection<Speciality> specialities;
        private Speciality selected;
        private string errorMessage;

        public Speciality Selected { get => selected; set => Set(ref selected, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }

        public ObservableCollection<Speciality> Specialities { get => specialities; set => Set(ref specialities, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        public PageCallbackCommand Remove { get => new(RemoveCallback); }
        #endregion

        private void RemoveCallback(Page page)
        {
            if (Selected == null)
            {
                ErrorMessage = "Выберите специальность для удаления!";
                return;
            }

            if (dataContext.UniversitySpecialityAdmissionCampaigns.Any(ac => ac.UniversitySpecialityID == dataContext.GetUniversitySpecialityBySpeciality(dataContext.GetUniversityFromSession.ID, Selected.ID).ID))
            {
                ErrorMessage = "Данная специальность уже используется!";
                return;
            }

            dataContext.RemoveUniversitySpeciality(dataContext.GetUniversitySpecialityBySpeciality(dataContext.GetUniversityFromSession.ID, Selected.ID).ID);
            Specialities = dataContext.GetUniversitySpecialitiesAsSpecialities(dataContext.GetUniversityFromSession.ID);
            ErrorMessage = "";
        }
    }
}
