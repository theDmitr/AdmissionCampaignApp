using AdmissionCampaign.Commands;
using AdmissionCampaign.Data;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class EnrollesListViewModel : ViewModel
    {
        public EnrollesListViewModel()
        {
            // Если петиции понадобятся этого года, то можно передать петиции, которые >= текущему году
            //Enrolles = GaleShapley.GetUniversityEnrollesList(dataContext.GetUniversityFromSession, new(dataContext.Universities), dataContext.GetEnrollesHavingPetitions, new(dataContext.Petitions));
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
                Set(ref selectedUniversitySpeciality, value);
                AdmissionCampaighs = new(GetUniversitySpecialityAndAdmissionCampaighs(dataContext.GetUniversityFromSession.ID).Where(o => o.Speciality.ID == selectedUniversitySpeciality.ID));
                Enrolles = null;
            }
        }

        public UniversitySpecialityAndAdmissionCampaigh SelectedAdmissionCampaigh
        {
            get => selectedAdmissionCampaigh;
            set
            {
                Set(ref selectedAdmissionCampaigh, value);

                if (selectedAdmissionCampaigh != null)
                {
                    GaleShapley.GaleShapleySort(new(dataContext.Enrolles), new(dataContext.Petitions
                    .Where(p => p.UniversitySpecialityAdmissionCampaighID == selectedAdmissionCampaigh.AdmissionCampaighID)), new(dataContext.UniversitySpecialityAdmissionCampaighs));
                    dataContext.SaveChanges();

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

    public class GaleShapley
    {
        public static void GaleShapleySort(ObservableCollection<Enrolle> enrolles, ObservableCollection<Petition> petitions, ObservableCollection<UniversitySpecialityAdmissionCampaigh> admissionCampaigns)
        {
            Dictionary<UniversitySpecialityAdmissionCampaigh, ObservableCollection<Petition>> admissionCampaighPetitions = new();
            Dictionary<Enrolle, ObservableCollection<Petition>> enrollesPetitions = new();

            ObservableCollection<Petition> doPetitions = new(petitions);

            foreach (UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh in admissionCampaigns)
            {
                ObservableCollection<Petition> lpetitions = new(petitions.Where(p => p.UniversitySpecialityAdmissionCampaighID == universitySpecialityAdmissionCampaigh.ID));
                admissionCampaighPetitions.Add(universitySpecialityAdmissionCampaigh, new(lpetitions.OrderByDescending(p => p.Exam1Value + p.Exam2Value + p.Exam3Value)));
            }

            foreach (Enrolle enrolle in enrolles)
                enrollesPetitions.Add(enrolle, new(petitions.Where(p => p.EnrolleID == enrolle.ID)));

            while (petitions.Count > 0 && enrolles.Count > 0)
            {
                Petition petition = petitions.FirstOrDefault();
                Enrolle enrolle = enrolles.Where(e => e.ID == petition.EnrolleID).Single();
                UniversitySpecialityAdmissionCampaigh admissionCampaigh = admissionCampaigns.Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID).Single();

                if (enrollesPetitions.GetValueOrDefault(enrolle).IndexOf(petition) == 0)
                {
                    if (admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh).IndexOf(petition) <= admissionCampaigh.PlacesCount)
                    {
                        petition.EnrolleCurrentStatus = Petition.EnrolleStatus.Accepted;
                        petitions.Remove(petition);
                        foreach (Petition lpetition in new ObservableCollection<Petition>(petitions.Where(p => p.EnrolleID == enrolle.ID)))
                        {
                            lpetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                            petitions.Remove(lpetition);
                        }
                        enrolles.Remove(enrolle);
                    }
                }

                if (admissionCampaigh.PlacesCount == petitions.Where(p => p.UniversitySpecialityAdmissionCampaighID == admissionCampaigh.ID).ToList().Count)
                {
                    admissionCampaigns.Remove(admissionCampaigh);
                    foreach (Petition cpetition in admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh))
                    {
                        if (cpetition.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                            cpetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                        petitions.Remove(cpetition);
                    }
                }
            }

            foreach (Petition doPetition in doPetitions)
            {
                if (doPetition.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                    doPetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
            }
        }
    }
}
