using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Speciality> Specialities { get => specialities; set => Set(ref specialities, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        #endregion
    }
}
