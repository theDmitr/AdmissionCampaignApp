using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class UniversitiesListViewModel : ViewModel
    {
        #region BindingFields
        private University selectedItem;
        public ObservableCollection<University> Universities => new(dataContext.Universities.ToArray());
        public University SelectedItem { get => selectedItem; set => Set(ref selectedItem, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminMenu => new(PageUriProvider.AdminMenu);
        public PageCallbackCommand Edit => new(EditCallback);
        public PageCallbackCommand ChangePassword => new(ChangePasswordCallback);
        #endregion

        private void EditCallback(Page page)
        {
            if (SelectedItem == null)
            {
                return;
            }

            UniversityEditViewModel.UniversityID = SelectedItem.ID;
            NavigateToPage(page, PageUriProvider.AdminUniversityEdit);
        }

        private void ChangePasswordCallback(Page page)
        {
            if (SelectedItem == null)
            {
                return;
            }

            ChangeUniversityPasswordViewModel.UserID = SelectedItem.UserID;
            NavigateToPage(page, PageUriProvider.AdminChangeUniversityPassword);
        }
    }
}
