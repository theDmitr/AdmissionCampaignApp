using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class EnrollesListViewModel : ViewModel
    {
        #region BindingFields
        private Enrolle selectedItem;

        public ObservableCollection<Enrolle> Enrolles => new(dataContext.Enrolles);
        public Enrolle SelectedItem { get => selectedItem; set => Set(ref selectedItem, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminMenu => new(PageUriProvider.AdminMenu);
        public PageCallbackCommand ChangePassword => new(ChangePasswordCallback);
        #endregion

        private void ChangePasswordCallback(Page page)
        {
            if (SelectedItem == null)
            {
                return;
            }

            ChangeEnrollePasswordViewModel.UserID = SelectedItem.UserID;
            NavigateToPage(page, PageUriProvider.AdminChangeEnrollePassword);
        }
    }
}