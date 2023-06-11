using AdmissionCampaign.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AdmissionCampaign.Converters
{
    public class SpecialityToCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not Speciality ? null : (object)(value as Speciality).Code;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
