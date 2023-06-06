using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public Exam(string name)
        {
            Name = name;
        }

        public Exam() { }
    }
}
