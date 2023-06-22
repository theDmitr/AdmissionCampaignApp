using AdmissionCampaign.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AdmissionCampaign.Converters
{
    /// <summary>
    /// Конвертер статуса поступления в строку
    /// </summary>
    public class EnrolleStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not Petition.EnrolleStatus
                ? null
                : (object)((Petition.EnrolleStatus)value switch
                {
                    Petition.EnrolleStatus.Refusal => "Отказ",
                    Petition.EnrolleStatus.Accepted => "Одобрено",
                    _ => "В рассмотрении"
                });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
