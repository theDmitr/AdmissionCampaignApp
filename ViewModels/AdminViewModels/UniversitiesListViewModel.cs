using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class UniversitiesListViewModel : ViewModel
    {
        public UniversitiesListViewModel() { }

        #region Commands
        public static NavigationCommand MoveToAdminMenu { get => new(PageUriProvider.AdminMenu); }
        #endregion
    }
}
