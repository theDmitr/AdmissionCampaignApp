using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        public ApplicationViewModel() { }

        #region Commands
        public static NavigationCommand MoveToEnrollePersonal { get => new(PageUriProvider.EnrollePersonal); }
        #endregion
    }
}
