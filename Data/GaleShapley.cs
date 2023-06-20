using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AdmissionCampaign.Data
{
    public class GaleShapley : ViewModel
    {
        public void GaleShapleySort(ObservableCollection<Enrolle> enrolles, int admissionCampaighID, ObservableCollection<UniversitySpecialityAdmissionCampaigh> admissionCampaigns)
        {
            Dictionary<UniversitySpecialityAdmissionCampaigh, ObservableCollection<Petition>> admissionCampaighPetitions = new();
            Dictionary<Enrolle, ObservableCollection<Petition>> enrollesPetitions = new();

            ObservableCollection<Petition> petitions = new(dataContext.Petitions
                    .Where(p => dataContext.UniversitySpecialityAdmissionCampaighs
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year == DateTime.Now.Year));

            foreach (Petition pet in petitions)
                pet.EnrolleCurrentStatus = Petition.EnrolleStatus.Processing;

            foreach (UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh in admissionCampaigns)
            {
                ObservableCollection<Petition> lpetitions = new(petitions.Where(p => p.UniversitySpecialityAdmissionCampaighID == universitySpecialityAdmissionCampaigh.ID));
                admissionCampaighPetitions.Add(universitySpecialityAdmissionCampaigh, new(lpetitions.OrderByDescending(p => p.Exam1Value + p.Exam2Value + p.Exam3Value)));
            }

            foreach (Enrolle enrolle in enrolles)
                enrollesPetitions.Add(enrolle, new(petitions.Where(p => p.EnrolleID == enrolle.ID)));

            int idx = 0;

            while (petitions.Count > 0 && enrolles.Count > 0)
            {
                Petition petition = petitions[idx];
                Enrolle enrolle = enrolles.Where(e => e.ID == petition.EnrolleID).Single();
                UniversitySpecialityAdmissionCampaigh admissionCampaigh = admissionCampaigns.Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID).Single();

                if (admissionCampaigh.PlacesCount <= admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh)
                    .Count(p => p.UniversitySpecialityAdmissionCampaighID == admissionCampaigh.ID && p.EnrolleCurrentStatus == Petition.EnrolleStatus.Accepted))
                {
                    ObservableCollection<Petition> pets = new(admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh));
                    foreach (Petition pet in pets)
                    {
                        if (pet.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                        {
                            pet.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                            admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh).Remove(pet);
                            enrollesPetitions[enrolles.Where(e => e.ID == pet.EnrolleID).Single()].Remove(pet);
                        }
                        petitions.Remove(pet);
                    }
                    
                    admissionCampaigns.Remove(admissionCampaigh);
                }

                else if (enrollesPetitions.GetValueOrDefault(enrolle).IndexOf(petition) == 0
                    && admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh).IndexOf(petition) < admissionCampaigh.PlacesCount)
                {
                    petition.EnrolleCurrentStatus = Petition.EnrolleStatus.Accepted;

                    ObservableCollection<ObservableCollection<Petition>> pets = new(admissionCampaighPetitions.Values);

                    foreach (ObservableCollection<Petition> pet in pets)
                    {
                        foreach (Petition spet in pet.Where(p => p.EnrolleID == enrolle.ID))
                        {
                            if (spet.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                            {
                                spet.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                                admissionCampaighPetitions[admissionCampaigh].Remove(spet);
                            }
                            petitions.Remove(spet);
                        }
                    }
                    enrolles.Remove(enrolle);
                }

                if (++idx >= petitions.Count)
                    idx = 0;
            }

            foreach (Petition doPetition in new ObservableCollection<Petition>(dataContext.Petitions
                    .Where(p => dataContext.UniversitySpecialityAdmissionCampaighs
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year == DateTime.Now.Year)))
            {
                if (doPetition.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                    doPetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
            }
        }
    }
}
