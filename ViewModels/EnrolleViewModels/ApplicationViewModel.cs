using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        #region Commands
        public static NavigationCommand MoveToEnrollePersonal => new(PageUriProvider.EnrollePersonal);
        #endregion
    }
}
