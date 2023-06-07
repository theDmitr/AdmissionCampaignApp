using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class UniversityPersonalViewModel : ViewModel
    {
        #region Commands
        public static NavigationCommand AnnounceAdmissionCampaigh => new(PageUriProvider.UniversityAnnounceAdmissionCampaigh);
        public static NavigationCommand MoveToAdmissionCampaighs => new(PageUriProvider.UniversityAdmissionCampaighsList);
        public static NavigationCommand MoveToEnrollesList => new(PageUriProvider.UniversityEnrollesList);
        public PageCallbackCommand Quit => new(QuitCallback);
        #endregion
    }
}
