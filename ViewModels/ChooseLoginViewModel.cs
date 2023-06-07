using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels
{
    public class ChooseLoginViewModel : ViewModel
    {
        public ChooseLoginViewModel() { }

        #region Commands
        public static NavigationCommand MoveToEnrolleRegister => new(PageUriProvider.EnrolleRegister);
        public static NavigationCommand MoveToAuth => new(PageUriProvider.AuthPage);
        #endregion
    }
}
