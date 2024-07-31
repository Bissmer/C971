using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TermsCoursesTracker.Schemas
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }
        public int TermId { get; set; }
        [MaxLength(100)]
        public string CourseTitle { get; set; }
        public string CourseStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [MaxLength(100)]
        public string InstructorName { get; set; }
        [MaxLength(15)]
        public string InstructorPhone { get; set; }
        [MaxLength(50)]
        public string InstructorEmail { get; set; }
        public bool Notification { get; set; }
        [MaxLength(200)]
        public string AdditionalNotes { get; set; }
        public int NotificationIdStart { get; set; }
        public int NotificationIdEnd { get; set; }
    }
}
