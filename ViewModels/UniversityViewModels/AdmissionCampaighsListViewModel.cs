using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class AdmissionCampaighsListViewModel : ViewModel
    {
        public AdmissionCampaighsListViewModel()
        {
            AdmissionCampaighs = GetUniversitySpecialityAndAdmissionCampaighs(dataContext.GetUniversityFromSession.ID);
        }

        #region BindingFields
        ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> admissionCampaighs;
        private UniversitySpecialityAndAdmissionCampaigh selected;
        private string errorMessage;
        
        public UniversitySpecialityAndAdmissionCampaigh Selected { get => selected; set => Set(ref selected, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        public ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> AdmissionCampaighs { get => admissionCampaighs; set => Set(ref admissionCampaighs, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        public PageCallbackCommand Remove { get => new(RemoveCallback); }
        #endregion

        private void RemoveCallback(Page page)
        {
            if (Selected == null)
            {
                ErrorMessage = "Выберите приемную кампанию для удаления!";
                return;
            }

            if (dataContext.Petitions.Any(p => p.UniversitySpecialityAdmissionCampaighID == Selected.AdmissionCampaighID))
            {
                ErrorMessage = "Заявки на данную приемную кампанию уже существуют!";
                return;
            }

            dataContext.RemoveUniversitySpecialityAdmissionCampaign(Selected.AdmissionCampaighID);
            AdmissionCampaighs = GetUniversitySpecialityAndAdmissionCampaighs(dataContext.GetUniversityFromSession.ID);
            ErrorMessage = "";
        }
    }
}
