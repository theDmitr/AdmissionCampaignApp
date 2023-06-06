using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
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
