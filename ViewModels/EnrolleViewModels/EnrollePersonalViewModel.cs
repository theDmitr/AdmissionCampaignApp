using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class EnrollePersonalViewModel : ViewModel
    {
        public EnrollePersonalViewModel() { }

        #region Commands
        public static NavigationCommand MoveToApplication { get => new(PageUriProvider.EnrolleApplication); }
        public static NavigationCommand MoveToChangeData { get => new(PageUriProvider.EnrolleChangeData); }
        public PageCallbackCommand Quit { get => new(QuitCallback); }
        #endregion
    }
}
