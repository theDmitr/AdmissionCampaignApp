using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class AddSpecialityViewModel : ViewModel
    {
        private string name;
        private string code;

        public string Name { get => name; set => Set(ref name, value); }
        public string Code { get => code; set => Set(ref code, value); }

        public NavigationCommand MoveToAdminMenu { get => new(PageUriProvider.AdminMenu); }
    }
}
