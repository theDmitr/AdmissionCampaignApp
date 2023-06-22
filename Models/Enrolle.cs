using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class Enrolle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        public string Passport { get; set; }

        [Required]
        public int UserID { get; set; }

        public Enrolle(string name, string surname, string lastname, string passport, int userid)
        {
            Name = name;
            Surname = surname;
            Patronymic = lastname;
            Passport = passport;
            UserID = userid;
        }

        public Enrolle() { }
    }
}
