namespace AdmissionCampaign.Models
{
    public class PetitionAndAdmissionCampaigh
    {
        public string UniversityName { get; set; }
        public UniversitySpecialityAndAdmissionCampaigh SpecialityAndAdmissionCampaigh { get; set; }
        public Petition.EnrolleStatus EnrolleStatus { get; set; }

        public PetitionAndAdmissionCampaigh(string universityName, UniversitySpecialityAndAdmissionCampaigh specialityAndAdmissionCampaigh, Petition.EnrolleStatus enrolleStatus)
        {
            UniversityName = universityName;
            SpecialityAndAdmissionCampaigh = specialityAndAdmissionCampaigh;
            EnrolleStatus = enrolleStatus;
        }
    }
}
