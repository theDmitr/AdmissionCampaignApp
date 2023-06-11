using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionCampaign.Models
{
    public class EnrolleAndPetition
    {
        public string Initials { get; set; }

        public Speciality Speciality { get; set; }

        public string Exam1Value { get; set; }
        public string Exam2Value { get; set; }
        public string Exam3Value { get; set; }
        
        public EnrolleAndPetition(string initials, Speciality speciality, string exam1Value, string exam2Value, string exam3Value)
        {
            Initials = initials;
            Speciality = speciality;
            Exam1Value = exam1Value;
            Exam2Value = exam2Value;
            Exam3Value = exam3Value;
        }
    }
}
