using AdmissionCampaign.Commands;
using AdmissionCampaign.Models;
using AdmissionCampaign.ViewModels.Base;
using System.Linq;
using System.Windows.Controls;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class ExamEditViewModel : ViewModel
    {
        public static int ExamID { get; set; }

        public ExamEditViewModel()
        {
            exam = dataContext.Exams.Where(s => s.ID == ExamID).Single();
            Name = exam.Name;
        }

        private readonly Exam exam;

        #region BindingFields
        private string name = "";
        private string errorMessage;

        public string Name { get => name; set => Set(ref name, value); }
        public string ErrorMessage { get => errorMessage; set => Set(ref errorMessage, value); }
        #endregion

        #region Commands
        public static NavigationCommand MoveToAdminExamsList => new(PageUriProvider.AdminExamsList);
        public PageCallbackCommand Save => new(SaveCallback);
        public PageCallbackCommand Remove => new(RemoveCallback);
        #endregion

        private void SaveCallback(Page page)
        {
            if (!IsValidSpecialityName(Name))
            {
                ErrorMessage = "Название предмета может содержать только кириллицу и символы пробела!";
                return;
            }

            if (exam.Name != Name && dataContext.IsExamNameExists(Name))
            {
                ErrorMessage = "Предмет с данным названием уже существует!";
                return;
            }

            exam.Name = Name;

            _ = dataContext.SaveChanges();

            NavigateToPage(page, PageUriProvider.AdminExamsList);
        }

        private void RemoveCallback(Page page)
        {
            if (dataContext.IsUniversitySpecialityAdmissionCampaighExamExists(exam.ID))
            {
                ErrorMessage = "Данный предмет используется в настоящий момент!";
                return;
            }

            dataContext.RemoveExam(exam.ID);

            NavigateToPage(page, PageUriProvider.AdminExamsList);
        }
    }
}
