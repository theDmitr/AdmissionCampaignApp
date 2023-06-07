using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class EnrollePersonalViewModel : ViewModel
    {
        #region Commands
        public static NavigationCommand MoveToApplication => new(PageUriProvider.EnrolleApplication);
        public static NavigationCommand MoveToChangeData => new(PageUriProvider.EnrolleChangeData);
        public PageCallbackCommand Quit => new(QuitCallback);
        #endregion
    }
}
