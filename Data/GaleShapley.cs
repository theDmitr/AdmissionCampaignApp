using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year
                    == dataContext.UniversitySpecialityAdmissionCampaighs.Where(ac => ac.ID == admissionCampaighID).Single().Year));

            foreach (Petition pet in petitions)
            {
                pet.EnrolleCurrentStatus = Petition.EnrolleStatus.Processing;
            }

            foreach (UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh in admissionCampaigns)
            {
                ObservableCollection<Petition> lpetitions = new(petitions.Where(p => p.UniversitySpecialityAdmissionCampaighID == universitySpecialityAdmissionCampaigh.ID));
                admissionCampaighPetitions.Add(universitySpecialityAdmissionCampaigh, new(lpetitions.OrderByDescending(p => p.Exam1Value + p.Exam2Value + p.Exam3Value)));
            }

            foreach (Enrolle enrolle in enrolles)
            {
                enrollesPetitions.Add(enrolle, new(petitions.Where(p => p.EnrolleID == enrolle.ID)));
            }

            int idx = 0;

            while (petitions.Count > 0 && enrolles.Count > 0)
            {
                Petition petition = petitions[idx];
                Enrolle enrolle = enrolles.Where(e => e.ID == petition.EnrolleID).Single();
                UniversitySpecialityAdmissionCampaigh admissionCampaigh = admissionCampaigns.Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID).Single();

                if (admissionCampaigh.PlacesCount <= admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh)
                    .Count(p => p.UniversitySpecialityAdmissionCampaighID == admissionCampaigh.ID && p.EnrolleCurrentStatus == Petition.EnrolleStatus.Accepted))
                {
                    foreach (Petition pet in admissionCampaighPetitions[admissionCampaigh].ToList())
                    {
                        if (pet.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                        {
                            pet.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                            _ = admissionCampaighPetitions[admissionCampaigns.Where(ac => ac.ID == pet.UniversitySpecialityAdmissionCampaighID).Single()].Remove(pet);
                            _ = enrollesPetitions[enrolles.Where(e => e.ID == pet.EnrolleID).Single()].Remove(pet);
                        }
                        _ = petitions.Remove(pet);
                    }

                    _ = admissionCampaigns.Remove(admissionCampaigh);
                }

                else if (enrollesPetitions.GetValueOrDefault(enrolle).IndexOf(petition) == 0
                    && admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh).IndexOf(petition) < admissionCampaigh.PlacesCount)
                {
                    petition.EnrolleCurrentStatus = Petition.EnrolleStatus.Accepted;

                    foreach (ObservableCollection<Petition> pet in admissionCampaighPetitions.Values.ToList())
                    {
                        foreach (Petition spet in pet.Where(p => p.EnrolleID == enrolle.ID).ToList())
                        {
                            if (spet.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                            {
                                spet.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                                _ = admissionCampaighPetitions[admissionCampaigns.Where(ac => ac.ID == spet.UniversitySpecialityAdmissionCampaighID).Single()].Remove(spet);
                                _ = enrollesPetitions[enrolles.Where(e => e.ID == spet.EnrolleID).Single()].Remove(spet);
                            }
                            _ = petitions.Remove(spet);
                        }
                    }
                    _ = enrolles.Remove(enrolle);
                }

                if (++idx >= petitions.Count)
                {
                    idx = 0;
                }
            }

            foreach (Petition doPetition in new ObservableCollection<Petition>(dataContext.Petitions
                    .Where(p => dataContext.UniversitySpecialityAdmissionCampaighs
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year
                    == dataContext.UniversitySpecialityAdmissionCampaighs.Where(ac => ac.ID == admissionCampaighID).Single().Year)))
            {
                if (doPetition.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                {
                    doPetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                }
            }
        }
    }
}
