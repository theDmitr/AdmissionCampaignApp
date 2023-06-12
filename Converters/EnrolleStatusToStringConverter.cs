using AdmissionCampaign.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdmissionCampaign.Converters
{
    public class EnrolleStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Petition.EnrolleStatus)
                return null;
            return ((Petition.EnrolleStatus) value) switch
            {
                Petition.EnrolleStatus.Processing => "Отказ",
                Petition.EnrolleStatus.Accepted => "Одобрено",
                _ => "В рассмотрении"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
