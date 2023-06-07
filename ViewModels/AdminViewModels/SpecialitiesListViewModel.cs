using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class SpecialitiesListViewModel : ViewModel
    {
        #region bindingFields
        private Speciality selectedItem;
        public ObservableCollection<Speciality> Specialities => new(dataContext.Specialities.ToArray());
        public Speciality SelectedItem { get => selectedItem; set => Set(ref selectedItem, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminMenu => new(PageUriProvider.AdminMenu);
        public PageCallbackCommand Edit => new(EditCallback);
        #endregion

        private void EditCallback(Page page)
        {
            if (SelectedItem == null)
            {
                return;
            }

            SpecialityEditViewModel.SpecialityID = SelectedItem.ID;
            NavigateToPage(page, PageUriProvider.AdminSpecialityEdit);
        }
    }
}
