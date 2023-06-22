using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public AccountType AcountType { get; set; }

        public User(string login, string password, AccountType acountType)
        {
            Login = login;
            Password = password;
            AcountType = acountType;
        }

        public User() { }

        public enum AccountType
        {
            Admin, University, Enrolle
        }
    }
}
