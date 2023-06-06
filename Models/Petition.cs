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
        public int UniversitySpecialityID { get; set; }

        [Required]
        public EnrolleStatus EnrolleCurrentStatus { get; set; } = EnrolleStatus.Processing;

        [Required]
        public DateTime Date { get; set; }

        public Petition(int enrolleID, int universitySpecialityID, DateTime date)
        {
            EnrolleID = enrolleID;
            UniversitySpecialityID = universitySpecialityID;
            Date = date;
        }

        public Petition() { }

        public enum EnrolleStatus
        {
            Processing, Accepted, Refusal
        }
    }
}
