using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class ExamsListViewModel : ViewModel
    {
        #region BindingFields
        private Exam selectedItem;
        public ObservableCollection<Exam> Exams => new(dataContext.Exams.ToArray());
        public Exam SelectedItem { get => selectedItem; set => Set(ref selectedItem, value); }
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

            ExamEditViewModel.ExamID = SelectedItem.ID;
            NavigateToPage(page, PageUriProvider.AdminExamEdit);
        }
    }
}
