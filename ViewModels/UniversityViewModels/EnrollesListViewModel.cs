using AdmissionCampaign.Commands;
using AdmissionCampaign.Data;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class EnrollesListViewModel : ViewModel
    {
        public EnrollesListViewModel()
        {
            UniversitySpecialities = new(dataContext.GetUniversitySpecialitiesAsSpecialities(dataContext.GetUniversityFromSession.ID));
        }

        #region BindingFields
        private ObservableCollection<Speciality> universitySpecialities;
        private ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> admissionCampaighs;

        private Speciality selectedUniversitySpeciality;
        private UniversitySpecialityAndAdmissionCampaigh selectedAdmissionCampaigh;

        private ObservableCollection<EnrolleAndPetition> enrolles;

        public ObservableCollection<Speciality> UniversitySpecialities { get => universitySpecialities; set => Set(ref universitySpecialities, value); }
        public ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> AdmissionCampaighs { get => admissionCampaighs; set => Set(ref admissionCampaighs, value); }

        public Speciality SelectedUniversitySpeciality
        {
            get => selectedUniversitySpeciality;
            set
            {
                _ = Set(ref selectedUniversitySpeciality, value);
                AdmissionCampaighs = new(GetUniversitySpecialityAndAdmissionCampaighs(dataContext.GetUniversityFromSession.ID).Where(o => o.Speciality.ID == selectedUniversitySpeciality.ID));
                Enrolles = null;
            }
        }

        public UniversitySpecialityAndAdmissionCampaigh SelectedAdmissionCampaigh
        {
            get => selectedAdmissionCampaigh;
            set
            {
                _ = Set(ref selectedAdmissionCampaigh, value);

                if (selectedAdmissionCampaigh != null)
                {
                    new GaleShapley().GaleShapleySort(new(dataContext.Enrolles), selectedAdmissionCampaigh.AdmissionCampaighID, new(dataContext.UniversitySpecialityAdmissionCampaighs));
                    _ = dataContext.SaveChanges();

                    Enrolles = GetEnrollesAndPetitions(dataContext.GetUniversityFromSession.ID, selectedAdmissionCampaigh.AdmissionCampaighID);
                }
            }
        }

        public ObservableCollection<EnrolleAndPetition> Enrolles { get => enrolles; set => Set(ref enrolles, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        #endregion
    }
}
