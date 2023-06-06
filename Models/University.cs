using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmissionCampaign.Models
{
    public class University
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int UserID { get; set; }

        public University(string name, int userID)
        {
            Name = name;
            UserID = userID;
        }

        public University() { }
    }
}
