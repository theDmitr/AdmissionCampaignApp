namespace AdmissionCampaign.Models
{
    public class UniversitySpecialityAndAdmissionCampaigh
    {
        public University University { get; set; }
        public Speciality Speciality { get; set; }
        public int AdmissionCampaighID { get; set; }
        public int PlacesCount { get; set; }
        public int Year { get; set; }
        public Exam Exam1 { get; set; }
        public Exam Exam2 { get; set; }
        public Exam Exam3 { get; set; }

        public string YearSpeciality => $"{Year} - {Speciality}";

        public UniversitySpecialityAndAdmissionCampaigh(University university, Speciality speciality, int admissionCampaighID, int placesCount, int year, Exam exam1, Exam exam2, Exam exam3)
        {
            University = university;
            Speciality = speciality;
            AdmissionCampaighID = admissionCampaighID;
            PlacesCount = placesCount;
            Year = year;
            Exam1 = exam1;
            Exam2 = exam2;
            Exam3 = exam3;
        }

        public override string ToString()
        {
            return YearSpeciality;
        }
    }
}
