using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class AddExamViewModel : ViewModel
    {
        #region BindingFields
        private string name = "";
        private string errorMessage;

        public string Name { get => name; set => Set(ref name, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminMenu => new(PageUriProvider.AdminMenu);
        public PageCallbackCommand Add => new(AddCallback);
        #endregion

        private void AddCallback(Page page)
        {
            if (!IsValidSpecialityName(Name))
            {
                ErrorMessage = "Название предмета может содержать только кириллицу и символы пробела!";
                return;
            }

            if (dataContext.ExamsNameExists(Name))
            {
                ErrorMessage = "Предмет с данным названием уже существует!";
                return;
            }

            _ = dataContext.RegisterExam(Name);

            NavigateToPage(page, PageUriProvider.AdminMenu);
        }
    }
}
