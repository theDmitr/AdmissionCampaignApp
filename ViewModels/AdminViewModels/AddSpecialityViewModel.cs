using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class AddSpecialityViewModel : ViewModel
    {
        #region BindingFields
        private string name = "";
        private string code = "";
        private string errorMessage;

        public string Name { get => name; set => Set(ref name, value); }
        public string Code { get => code; set => Set(ref code, value); }
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
                ErrorMessage = "Название специальности может содержать только кириллицу и символы пробела!";
                return;
            }

            if (dataContext.IsSpecialityNameExists(Name))
            {
                ErrorMessage = "Специальность с данным названием уже существует!";
                return;
            }

            if (!IsValidSpecialityCode(Code))
            {
                ErrorMessage = "Код специальности не соответствует формату XX.XX.XX!";
                return;
            }

            _ = dataContext.RegisterSpeciality(Name, Code);

            NavigateToPage(page, PageUriProvider.AdminMenu);
        }
    }
}
