using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TermsCoursesTracker.Schemas
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }
        public int CourseId { get; set; }

        [MaxLength(100)] 
        public string AssessmentTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AssessmentType { get; set; }
        public bool Notification { get; set; }

        public int NotificationIdStart { get; set; }
        public int NotificationIdEnd { get; set; }
    }
}
