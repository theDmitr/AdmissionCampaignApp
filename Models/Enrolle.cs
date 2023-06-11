using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class Enrolle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Passport { get; set; }
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
