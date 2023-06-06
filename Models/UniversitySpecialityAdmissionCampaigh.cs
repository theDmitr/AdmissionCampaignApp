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
        public int UniversityID { get; set; }

        [Required]
        public int SpecialityID { get; set; }

        [Required]
        public int PlacesCount { get; set; }

        [Required]
        public int Year { get; set; }

        public UniversitySpecialityAdmissionCampaigh(int universityID, int specialityID, int placesCount, int year)
        {
            UniversityID = universityID;
            SpecialityID = specialityID;
            PlacesCount = placesCount;
            Year = year;
        }

        public UniversitySpecialityAdmissionCampaigh() { }
    }
}
