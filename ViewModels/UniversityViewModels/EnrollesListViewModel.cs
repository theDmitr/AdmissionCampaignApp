using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class EnrollesListViewModel : ViewModel
    {
        #region BindingFields
        public ObservableCollection<Enrolle> Enrolles => new(dataContext.Enrolles.ToArray());
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        #endregion
    }
}
