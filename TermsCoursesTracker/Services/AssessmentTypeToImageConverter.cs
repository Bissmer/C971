using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermsCoursesTracker.Services
{
    /// <summary>
    /// Class to handle the conversion of the assessment type to the corresponding image
    /// </summary>
    class AssessmentTypeToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.ToString() == "Performance" ? "performance.png" : "objective.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
