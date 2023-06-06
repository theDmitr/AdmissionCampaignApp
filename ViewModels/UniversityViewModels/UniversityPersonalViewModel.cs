using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class UniversityPersonalViewModel : ViewModel
    {
        public UniversityPersonalViewModel() { }

        #region Commands
        public NavigationCommand AddSpeciality { get => new(PageUriProvider.AdminAddSpeciality); }
        /*AnnounceAdmissionCampaighCommand
        MoveToSpecialitiesPageCommand
        MoveToAdmissionCampaighsPageCommand
        MoveToEnrollesPageCommand
        QuitCommand*/
        #endregion
    }
}
