using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class Petition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int EnrolleID { get; set; }

        [Required]
        public int UniversitySpecialityAdmissionCampaighID { get; set; }

        [Required]
        public int Exam1Value { get; set; }

        [Required]
        public int Exam2Value { get; set; }

        [Required]
        public int Exam3Value { get; set; }

        [Required]
        public EnrolleStatus EnrolleCurrentStatus { get; set; } = EnrolleStatus.Processing;

        [Required]
        public DateTime Date { get; set; }

        public Petition(int enrolleID, int universitySpecialityAdmissionCampaighID, int exam1Value, int exam2value, int exam3Value, DateTime date)
        {
            EnrolleID = enrolleID;
            UniversitySpecialityAdmissionCampaighID = universitySpecialityAdmissionCampaighID;
            Exam1Value = exam1Value;
            Exam2Value = exam2value;
            Exam3Value = exam3Value;
            Date = date;
        }

        public Petition() { }

        public enum EnrolleStatus
        {
            Processing, Accepted, Refusal
        }
    }
}
