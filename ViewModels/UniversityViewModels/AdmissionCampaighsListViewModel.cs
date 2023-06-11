using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class AdmissionCampaighsListViewModel : ViewModel
    {
        public AdmissionCampaighsListViewModel()
        {
            AdmissionCampaighs = GetUniversitySpecialityAndAdmissionCampaighs(dataContext.GetUniversityFromSession.ID);
        }

        #region BindingFields
        public ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> AdmissionCampaighs { get; set; }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        #endregion
    }
}
