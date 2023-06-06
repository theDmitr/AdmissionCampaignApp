using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class ExamUniversitySpeciality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int ExamID { get; set; }

        [Required]
        public int UniversitySpecialityAdmissionCampaighID { get; set; }

        public ExamUniversitySpeciality(int examID, int universitySpecialityAdmissionCampaighID)
        {
            ExamID = examID;
            UniversitySpecialityAdmissionCampaighID = universitySpecialityAdmissionCampaighID;
        }

        public ExamUniversitySpeciality() { }
    }
}
