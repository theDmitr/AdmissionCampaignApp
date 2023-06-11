using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.UniversityViewModels
{
    public class AnnounceAdmissionCampaignViewModel : ViewModel
    {
        public AnnounceAdmissionCampaignViewModel()
        {
            Specialities = dataContext.GetUniversitySpecialitiesAsSpecialities(dataContext.GetUniversityFromSession.ID);
            Exams = new(dataContext.Exams);
        }

        #region BindingFields
        private ObservableCollection<Speciality> specialities;
        private ObservableCollection<Exam> exams;

        private Speciality selectedSpeciality;
        private Exam selectedExam1;
        private Exam selectedExam2;
        private Exam selectedExam3;

        private string year = "";
        private string placesCount = "";
        private string errorMessage;

        public ObservableCollection<Speciality> Specialities { get => specialities; set => Set(ref specialities, value); }
        public ObservableCollection<Exam> Exams { get => exams; set => Set(ref exams, value); }

        public Speciality SelectedSpeciality { get => selectedSpeciality; set => Set(ref selectedSpeciality, value); }
        public Exam SelectedExam1 { get => selectedExam1; set => Set(ref selectedExam1, value); }
        public Exam SelectedExam2 { get => selectedExam2; set => Set(ref selectedExam2, value); }
        public Exam SelectedExam3 { get => selectedExam3; set => Set(ref selectedExam3, value); }

        public string Year { get => year; set => Set(ref year, value); }
        public string PlacesCount { get => placesCount; set => Set(ref placesCount, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToUniversityPersonal => new(PageUriProvider.UniversityPersonal);
        public PageCallbackCommand Add => new(AddCallback);
        #endregion

        private void AddCallback(Page page)
        {
            if (!int.TryParse(year, out int intYear) || intYear < DateTime.Now.Year)
            {
                ErrorMessage = "Некорректно указан год!";
                return;
            }

            if (SelectedSpeciality == null)
            {
                ErrorMessage = "Укажите специальность!";
                return;
            }

            if (dataContext.IsUniversityAdmissionCampaignExists(dataContext.GetUniversityFromSession.ID, SelectedSpeciality.ID, intYear))
            {
                ErrorMessage = "Приемная кампания на эту специальность в этот год уже существует!";
                return;
            }

            if (!int.TryParse(placesCount, out int intPlacesCount) || intPlacesCount <= 0)
            {
                ErrorMessage = "Некорректно указано количество бюджетных мест!";
                return;
            }

            if (SelectedExam1 == null)
            {
                ErrorMessage = "Укажите ЕГЭ - 1!";
                return;
            }

            if (SelectedExam2 == null)
            {
                ErrorMessage = "Укажите ЕГЭ - 2!";
                return;
            }

            if (SelectedExam3 == null)
            {
                ErrorMessage = "Укажите ЕГЭ - 3!";
                return;
            }

            if (SelectedExam1 == SelectedExam2 || SelectedExam2 == SelectedExam3 || SelectedExam1 == SelectedExam3)
            {
                ErrorMessage = "Не может быть дубликатов ЕГЭ!";
                return;
            }

            _ = dataContext.RegisterUniversitySpecialityAdmissionCampaigh(
                dataContext.GetUniversitySpecialityBySpeciality(dataContext.GetUniversityFromSession.ID, SelectedSpeciality.ID).ID,
                intPlacesCount,
                intYear,
                SelectedExam1.ID,
                SelectedExam2.ID,
                SelectedExam3.ID);

            NavigateToPage(page, PageUriProvider.UniversityPersonal);
        }
    }
}
