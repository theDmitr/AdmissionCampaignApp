using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class UniversitySpeciality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int UniversityID { get; set; }

        [Required]
        public int SpecialityID { get; set; }

        public UniversitySpeciality(int universityID, int specialityID)
        {
            UniversityID = universityID;
            SpecialityID = specialityID;
        }

        public UniversitySpeciality() { }
    }
}
