using AdmissionCampaign.Commands;
using AdmissionCampaign.ViewModels.Base;

namespace AdmissionCampaign.ViewModels.AdminViewModels
{
    public class AdminMenuViewModel : ViewModel
    {
        #region Commands
        public static NavigationCommand AddUniversity => new(PageUriProvider.AdminAddUniversity);
        public static NavigationCommand AddSpeciality => new(PageUriProvider.AdminAddSpeciality);
        public static NavigationCommand AddExam => new(PageUriProvider.AdminAddExam);
        public static NavigationCommand MoveToUniversitiesList => new(PageUriProvider.AdminUniversitiesList);
        public static NavigationCommand MoveToSpecialitiesList => new(PageUriProvider.AdminSpecialitiesList);
        public static NavigationCommand MoveToExamsList => new(PageUriProvider.AdminExamsList);
        public static NavigationCommand MoveToEnrollesList => new(PageUriProvider.AdminEnrollesList);
        public PageCallbackCommand Quit => new(QuitCallback);
        #endregion
    }
}
