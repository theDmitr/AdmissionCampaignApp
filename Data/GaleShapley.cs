﻿using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            while (petitions.Count > 0 && enrolles.Count > 0)
            {
                Petition petition = petitions.FirstOrDefault();
                Enrolle enrolle = enrolles.Where(e => e.ID == petition.EnrolleID).Single();
                UniversitySpecialityAdmissionCampaigh admissionCampaigh = admissionCampaigns.Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID).Single();

                if (enrollesPetitions.GetValueOrDefault(enrolle).IndexOf(petition) == 0)
                {
                    if (admissionCampaighPetitions.GetValueOrDefault(admissionCampaigh).IndexOf(petition) < admissionCampaigh.PlacesCount)
                    {
                        petition.EnrolleCurrentStatus = Petition.EnrolleStatus.Accepted;
                        petitions.Remove(petition);

                        foreach (Petition lpetition in new ObservableCollection<Petition>(petitions.Where(p => p.EnrolleID == enrolle.ID)))
                        {
                            lpetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                            petitions.Remove(lpetition);

                            foreach (ObservableCollection<Petition> pet in admissionCampaighPetitions.Values)
                            {
                                while (pet.Contains(petition) && petition.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                                {
                                    petition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                                    pet.Remove(petition);
                                }
                            }
                        }
                        enrolles.Remove(enrolle);
                    }
                }

                if (admissionCampaigh.PlacesCount <= dataContext.Petitions
                    .Where(p => dataContext.UniversitySpecialityAdmissionCampaighs
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year == DateTime.Now.Year).Where(p => p.UniversitySpecialityAdmissionCampaighID == admissionCampaigh.ID && p.EnrolleCurrentStatus == Petition.EnrolleStatus.Accepted).ToList().Count)
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
