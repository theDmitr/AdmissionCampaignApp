using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class EnrollePetitionsViewModel : ViewModel
    {
        #region BindingFields
        public ObservableCollection<PetitionAndAdmissionCampaigh> Petitions
        {
            get
            {
                ObservableCollection<PetitionAndAdmissionCampaigh> result = new();
                foreach (Petition petition in dataContext.Petitions.Where(p => p.EnrolleID == dataContext.GetEnrolleFromSession.ID))
                {
                    University u = dataContext.Universities.Where(u => u.ID == dataContext.UniversitySpecialities.Where(us => us.ID == dataContext.UniversitySpecialityAdmissionCampaigns
                            .Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID).Single().UniversitySpecialityID).Single().UniversityID).Single();

                    result.Add(
                        new(
                            u.Name,
                            GetUniversitySpecialityAndAdmissionCampaighs(u.ID).Where(o => o.AdmissionCampaighID == petition.UniversitySpecialityAdmissionCampaighID).Single(),
                            petition.EnrolleCurrentStatus));
                }

                return result;
            }
        }
        #endregion

        #region Commands
        public static NavigationCommand MoveToEnrollePersonal => new(PageUriProvider.EnrollePersonal);
        #endregion
    }
}
