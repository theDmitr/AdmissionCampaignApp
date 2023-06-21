using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.EnrolleViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        public static int petitionsToUniversity = 5;
        public static int petitionsToUniversitySpeciality = 5;

        public ApplicationViewModel()
        {
            universities = new(dataContext.Universities);
        }

        #region BindingFields
        private string errorMessage;
        private ObservableCollection<University> universities;
        private ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> admissionCampaighs;
        private University selectedUniversity;
        private UniversitySpecialityAndAdmissionCampaigh selectedAdmissionCampaigh;

        private Exam exam1;
        private Exam exam2;
        private Exam exam3;

        private int exam1Value;
        private int exam2Value;
        private int exam3Value;

        public University SelectedUniversity
        {
            get => selectedUniversity;
            set
            {
                _ = Set(ref selectedUniversity, value);
                AdmissionCampaighs = new(GetUniversitySpecialityAndAdmissionCampaighs(selectedUniversity.ID).Where(ac => ac.Year >= DateTime.Now.Year));
                SelectedSpeciality = null;
            }
        }

        public UniversitySpecialityAndAdmissionCampaigh SelectedSpeciality
        {
            get => selectedAdmissionCampaigh;
            set
            {
                _ = Set(ref selectedAdmissionCampaigh, value);
                if (selectedAdmissionCampaigh == null)
                {
                    Exam1 = null;
                    Exam2 = null;
                    Exam3 = null;
                }
                else
                {
                    Exam1 = selectedAdmissionCampaigh.Exam1;
                    Exam2 = selectedAdmissionCampaigh.Exam2;
                    Exam3 = selectedAdmissionCampaigh.Exam3;
                }
            }
        }

        public Exam Exam1 { get => exam1; set => Set(ref exam1, value); }
        public Exam Exam2 { get => exam2; set => Set(ref exam2, value); }
        public Exam Exam3 { get => exam3; set => Set(ref exam3, value); }

        public int Exam1Value { get => exam1Value; set => Set(ref exam1Value, value); }
        public int Exam2Value { get => exam2Value; set => Set(ref exam2Value, value); }
        public int Exam3Value { get => exam3Value; set => Set(ref exam3Value, value); }

        public ObservableCollection<University> Universities { get => universities; set => Set(ref universities, value); }
        public ObservableCollection<UniversitySpecialityAndAdmissionCampaigh> AdmissionCampaighs { get => admissionCampaighs; set => Set(ref admissionCampaighs, value); }

        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToEnrollePersonal => new(PageUriProvider.EnrollePersonal);
        public PageCallbackCommand Send => new(SendCallback);
        #endregion

        private void SendCallback(Page page)
        {
            if (selectedUniversity == null)
            {
                ErrorMessage = "Выберите ВУЗ!";
                return;
            }

            if (selectedAdmissionCampaigh == null)
            {
                ErrorMessage = "Выберите специальность!";
                return;
            }

            if (dataContext.Petitions.Any(p => p.UniversitySpecialityAdmissionCampaighID == selectedAdmissionCampaigh.AdmissionCampaighID && p.EnrolleID == dataContext.GetEnrolleFromSession.ID))
            {
                ErrorMessage = "Вы уже подавали заявку на данную специальность!";
                return;
            }

            ObservableCollection<University> universities = new(
                dataContext.Universities.Where(u => dataContext.Petitions.Any(p => p.Date.AddDays(365) >= DateTime.Now
                && (p.EnrolleID == dataContext.GetEnrolleFromSession.ID)
                && dataContext.UniversitySpecialityAdmissionCampaigns.Any(ac => (ac.ID == p.UniversitySpecialityAdmissionCampaighID)
                && dataContext.UniversitySpecialities.Any(us => us.ID == ac.UniversitySpecialityID && us.UniversityID == u.ID)))));

            ObservableCollection<Petition> petitions = new(
                dataContext.Petitions.Where(p => p.EnrolleID == dataContext.GetEnrolleFromSession.ID
                && selectedUniversity.ID == dataContext.UniversitySpecialities
                .Where(us => us.ID == dataContext.UniversitySpecialityAdmissionCampaigns
                .Where(ac => ac.ID == selectedAdmissionCampaigh.AdmissionCampaighID).Single().UniversitySpecialityID).Single().UniversityID));


            if (universities.Count >= petitionsToUniversity && !universities.Contains(selectedUniversity))
            {
                ErrorMessage = $"Вы не можете подать в год заявок больше, чем в {petitionsToUniversity} ВУЗов!";
                return;
            }

            if (universities.Contains(selectedUniversity) && petitions.Count >= petitionsToUniversitySpeciality)
            {
                ErrorMessage = $"Вы не можете подать в год больше, чем {petitionsToUniversitySpeciality} заявок в один ВУЗ!";
                return;
            }

            _ = dataContext.RegisterPetition(dataContext.GetEnrolleFromSession.ID, selectedAdmissionCampaigh.AdmissionCampaighID,
                Exam1Value, Exam2Value, Exam3Value,
                DateTime.Now);

            NavigateToPage(page, PageUriProvider.EnrollePersonal);
        }
    }
}
