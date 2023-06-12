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

        public Exam Exam1 { get; set; }
        public Exam Exam2 { get; set; }
        public Exam Exam3 { get; set; }

        public int Exam1Value { get; set; }
        public int Exam2Value { get; set; }
        public int Exam3Value { get; set; }

        public int Place { get; set; }

        public int AllPoints => Exam1Value + Exam2Value + Exam3Value;

        public string Exam1String => $"{Exam1} : {Exam1Value}";
        public string Exam2String => $"{Exam2} : {Exam2Value}";
        public string Exam3String => $"{Exam3} : {Exam3Value}";

        public EnrolleAndPetition(string initials, Speciality speciality, Exam exam1, Exam exam2, Exam exam3, int exam1Value, int exam2Value, int exam3Value, int place)
        {
            Initials = initials;
            Speciality = speciality;
            Exam1 = exam1;
            Exam2 = exam2;
            Exam3 = exam3;
            Exam1Value = exam1Value;
            Exam2Value = exam2Value;
            Exam3Value = exam3Value;
            Place = place;
        }
    }
}
