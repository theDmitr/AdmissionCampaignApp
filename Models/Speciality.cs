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

        [NotMapped]
        public string[] Subjects { get; set; } = new string[3];

        public Speciality(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public Speciality() { }
    }
}
