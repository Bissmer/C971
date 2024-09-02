using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermsCoursesTracker.Schemas;

namespace TermsCoursesTracker.Services
{
    public class TestData
    
    {
        private readonly ProjectDatabase _database;

        public TestData(ProjectDatabase database)
        {
            _database = database;
        }

        public async Task PopulateTestDataAsync()
        {
            try
            {
                //Add Terms
                var term = new Term
                {
                    TermTitle = "Winter 2025",
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 6, 30)
                };

                await _database.SaveTermAsync(term);

                //Add Course
                var course = new Course
                {
                    TermId = term.Id,
                    CourseTitle = "Mobile App Development",
                    CourseStatus = "Plan to take",
                    StartDate = new DateTime(2025, 1, 2),
                    EndDate = new DateTime(2025, 2, 2),
                    InstructorName = "Anika Patel",
                    InstructorPhone = "555-123-4567",
                    InstructorEmail = "anika.patel@strimeuniversity.edu",
                    Notification = true,
                    AdditionalNotes = "This course will cover the mobile application development using .Net Maui",
                };

                await _database.SaveCourseAsync(course);
                await NotificationService.ScheduleCourseNotification(course.CourseTitle, "Course Start", course.StartDate);
                await NotificationService.ScheduleCourseNotification(course.CourseTitle, "Course End", course.EndDate);


                //Add Assessments

                var assessment1 = new Assessment
                {
                    CourseId = course.CourseId,
                    AssessmentTitle = "Mobile Application Development",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 1, 15),
                    AssessmentType = "Performance",
                    Notification = true
                };


                assessment1.NotificationIdStart = await NotificationService.ScheduleAssessmentNotification(assessment1.AssessmentType, assessment1.AssessmentTitle, "Assessment Start", assessment1.StartDate);
                assessment1.NotificationIdEnd = await NotificationService.ScheduleAssessmentNotification(assessment1.AssessmentType, assessment1.AssessmentTitle, "Assessment End", assessment1.EndDate);
                await _database.SaveAssessmentAsync(assessment1);

                var assessment2 = new Assessment
                {
                    CourseId = course.CourseId,
                    AssessmentTitle = "Mobile Application Development Paper",
                    StartDate = new DateTime(2025, 1, 25),
                    EndDate = new DateTime(2025, 1, 27),
                    AssessmentType = "Objective",
                    Notification = true
                };
                assessment2.NotificationIdStart = await NotificationService.ScheduleAssessmentNotification(assessment2.AssessmentType, assessment2.AssessmentTitle, "Assessment Start", assessment2.StartDate);
                assessment2.NotificationIdEnd = await NotificationService.ScheduleAssessmentNotification(assessment2.AssessmentType, assessment2.AssessmentTitle, "Assessment End", assessment2.EndDate);
                await _database.SaveAssessmentAsync(assessment2);

            } catch (Exception ex)
            {
                Console.WriteLine($"Error populating data: {ex.Message}");
            }
            

        }
        }
}
