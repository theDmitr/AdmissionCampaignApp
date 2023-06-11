using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class SpecialityEditViewModel : ViewModel
    {
        public static int SpecialityID { get; set; }

        public SpecialityEditViewModel()
        {
            speciality = dataContext.Specialities.Where(s => s.ID == SpecialityID).Single();
            Name = speciality.Name;
            Code = speciality.Code;
        }

        private readonly Speciality speciality;

        #region BindingFields
        private string name = "";
        private string code = "";
        private string errorMessage;

        public string Name { get => name; set => Set(ref name, value); }
        public string Code { get => code; set => Set(ref code, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminSpecialitiesList => new(PageUriProvider.AdminSpecialitiesList);
        public PageCallbackCommand Save => new(SaveCallback);
        public PageCallbackCommand Remove => new(RemoveCallback);
        #endregion

        private void SaveCallback(Page page)
        {
            if (!IsValidSpecialityName(Name))
            {
                ErrorMessage = "Название специальности может содержать только кириллицу и символы пробела!";
                return;
            }

            if (speciality.Name != Name && dataContext.IsSpecialityNameExists(Name))
            {
                ErrorMessage = "Специальность с данным названием уже существует!";
                return;
            }

            if (!IsValidSpecialityCode(Code))
            {
                ErrorMessage = "Код специальности не соответствует формату XX.XX.XX!";
                return;
            }

            if (speciality.Code != Code && dataContext.IsSpecialityCodeExists(Code))
            {
                ErrorMessage = "Указанный код специальности уже существует!";
                return;
            }

            speciality.Name = Name;
            speciality.Code = Code;

            _ = dataContext.SaveChanges();

            NavigateToPage(page, PageUriProvider.AdminSpecialitiesList);
        }

        private void RemoveCallback(Page page)
        {
            dataContext.RemoveSpeciality(speciality.ID);

            NavigateToPage(page, PageUriProvider.AdminSpecialitiesList);
        }
    }
}
