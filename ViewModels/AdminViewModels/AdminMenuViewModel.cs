using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class AdminMenuViewModel : ViewModel
    {
        public AdminMenuViewModel() { }

        #region Commands
        public static NavigationCommand MoveToAdminAddUniversity { get => new(PageUriProvider.AdminAddUniversity); }
        public static NavigationCommand MoveToAdminUniversitiesList { get => new(PageUriProvider.AdminUniversitiesList); }
        public PageCallbackCommand Quit { get => new(QuitCallback); }
        #endregion
    }
}
