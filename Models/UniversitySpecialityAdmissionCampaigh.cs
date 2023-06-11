using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class UniversitySpecialityAdmissionCampaigh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int UniversitySpecialityID { get; set; }

        [Required]
        public int PlacesCount { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Exam1ID { get; set; }

        [Required]
        public int Exam2ID { get; set; }

        [Required]
        public int Exam3ID { get; set; }

        public UniversitySpecialityAdmissionCampaigh(int universitySpecialityID, int placesCount, int year, int exam1ID, int exam2ID, int exam3ID)
        {
            UniversitySpecialityID = universitySpecialityID;
            PlacesCount = placesCount;
            Year = year;
            Exam1ID = exam1ID;
            Exam2ID = exam2ID;
            Exam3ID = exam3ID;
        }

        public UniversitySpecialityAdmissionCampaigh() { }
    }
}
