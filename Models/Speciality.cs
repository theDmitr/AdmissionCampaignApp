using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class Speciality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int Exam1ID { get; set; }

        [Required]
        public int Exam2ID { get; set; }

        [Required]
        public int Exam3ID { get; set; }

        public Speciality(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public Speciality() { }
    }
}
