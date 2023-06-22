using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdmissionCampaign.Data
{
    public class GaleShapley : ViewModel
    {
        /// <summary>
        /// Проводит сортировку заявок на конкретную специальность (т.е. на конкретный год) по модифицированному алгоритму Гейла-Шепли
        /// </summary>
        /// <param name="enrolles"></param>
        /// <param name="admissionCampaignID"></param>
        /// <param name="admissionCampaigns"></param>
        public void GaleShapleySort(ObservableCollection<Enrolle> enrolles, int admissionCampaignID, ObservableCollection<UniversitySpecialityAdmissionCampaigh> admissionCampaigns)
        {
            Dictionary<UniversitySpecialityAdmissionCampaigh, ObservableCollection<Petition>> admissionCampaignPetitions = new();
            Dictionary<Enrolle, ObservableCollection<Petition>> enrollesPetitions = new();

            ObservableCollection<Petition> petitions = new(dataContext.Petitions
                    .Where(p => dataContext.UniversitySpecialityAdmissionCampaigns
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year
                    == dataContext.UniversitySpecialityAdmissionCampaigns.Where(ac => ac.ID == admissionCampaignID).Single().Year)); // Все заявки на год

            foreach (Petition pet in petitions) 
            {
                pet.EnrolleCurrentStatus = Petition.EnrolleStatus.Processing; // Сброс статуса всех заявок (Если алгоритм уже работал с этими данными)
            }

            foreach (UniversitySpecialityAdmissionCampaigh universitySpecialityAdmissionCampaigh in admissionCampaigns)
            {
                ObservableCollection<Petition> lpetitions = new(petitions.Where(p => p.UniversitySpecialityAdmissionCampaighID == universitySpecialityAdmissionCampaigh.ID));
                admissionCampaignPetitions.Add(universitySpecialityAdmissionCampaigh, new(lpetitions.OrderByDescending(p => p.Exam1Value + p.Exam2Value + p.Exam3Value))); // Заполнение словаря, с парами: Приемная кампания : Заявка_1, Заявка_2, ... (По убыванию по сумме баллов за жкзамены)
            }

            foreach (Enrolle enrolle in enrolles)
            {
                enrollesPetitions.Add(enrolle, new(petitions.Where(p => p.EnrolleID == enrolle.ID))); // Заполнение словаря, с парами: Абитурент: Заявка_1, Заявка_2, ... (Чем ранбше подана заявка, тем выше её приоритет)
            }

            int idx = 0; // Индекс для прохода по всем заявкам

            while (petitions.Count > 0 && enrolles.Count > 0)
            {
                Petition petition = petitions[idx]; // Получение заявки
                Enrolle enrolle = enrolles.Where(e => e.ID == petition.EnrolleID).Single(); // Получение абитурента (автора заявки)
                UniversitySpecialityAdmissionCampaigh admissionCampaign = admissionCampaigns.Where(ac => ac.ID == petition.UniversitySpecialityAdmissionCampaighID).Single(); // Получение приемной капании (на которую подана заявка)

                if (admissionCampaign.PlacesCount <= admissionCampaignPetitions.GetValueOrDefault(admissionCampaign)
                    .Count(p => p.UniversitySpecialityAdmissionCampaighID == admissionCampaign.ID && p.EnrolleCurrentStatus == Petition.EnrolleStatus.Accepted)) // Если количество принятых заявок уже достигло количества мест в приемной кампании, то происходит удаление приемной кампании из рассмотрения алгоритмом
                {
                    foreach (Petition pet in admissionCampaignPetitions[admissionCampaign].ToList()) // Удаление остальных заявок абитурентов из всех соварей и коллекции заявок и установка их статуса на "Отказ"
                    {
                        if (pet.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                        {
                            pet.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                            _ = admissionCampaignPetitions[admissionCampaigns.Where(ac => ac.ID == pet.UniversitySpecialityAdmissionCampaighID).Single()].Remove(pet);
                            _ = enrollesPetitions[enrolles.Where(e => e.ID == pet.EnrolleID).Single()].Remove(pet);
                        }
                        _ = petitions.Remove(pet);
                    }

                    _ = admissionCampaigns.Remove(admissionCampaign);
                }

                else if (enrollesPetitions.GetValueOrDefault(enrolle).IndexOf(petition) == 0
                    && admissionCampaignPetitions.GetValueOrDefault(admissionCampaign).IndexOf(petition) < admissionCampaign.PlacesCount) // Если у абитурента заявка в самом высоком приоритете и достаточно мест в приемной кампании, то заявка одобряется
                {
                    petition.EnrolleCurrentStatus = Petition.EnrolleStatus.Accepted;

                    foreach (ObservableCollection<Petition> pet in admissionCampaignPetitions.Values.ToList()) // Удаление остальных заявок абитурента из всех соварей и установка их статуса на "Отказ"
                    {
                        foreach (Petition spet in pet.Where(p => p.EnrolleID == enrolle.ID).ToList())
                        {
                            if (spet.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                            {
                                spet.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                                _ = admissionCampaignPetitions[admissionCampaigns.Where(ac => ac.ID == spet.UniversitySpecialityAdmissionCampaighID).Single()].Remove(spet);
                                _ = enrollesPetitions[enrolles.Where(e => e.ID == spet.EnrolleID).Single()].Remove(spet);
                            }
                            _ = petitions.Remove(spet);
                        }
                    }
                    _ = enrolles.Remove(enrolle);
                }

                if (++idx >= petitions.Count) // Обнуление индекса
                {
                    idx = 0;
                }
            }

            foreach (Petition doPetition in new ObservableCollection<Petition>(dataContext.Petitions
                    .Where(p => dataContext.UniversitySpecialityAdmissionCampaigns
                    .Where(ac => ac.ID == p.UniversitySpecialityAdmissionCampaighID).Single().Year
                    == dataContext.UniversitySpecialityAdmissionCampaigns.Where(ac => ac.ID == admissionCampaignID).Single().Year))) // Для всех заявок, которые не приняты, установить "Отказ"
            {
                if (doPetition.EnrolleCurrentStatus != Petition.EnrolleStatus.Accepted)
                {
                    doPetition.EnrolleCurrentStatus = Petition.EnrolleStatus.Refusal;
                }
            }
        }
    }
}
