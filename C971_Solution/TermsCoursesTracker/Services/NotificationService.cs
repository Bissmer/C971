using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermsCoursesTracker.Services
{
    public class NotificationService
    {
        public static async Task<int> ScheduleAssessmentNotification(string assessmentType, string assessmentTitle, string eventTitle, DateTime eventDate)
        {
            var formattedDate = eventDate.ToString("MM/dd/yyyy");
            int notificationId = new Random().Next(10000);
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = $"[{assessmentType} assessment] - {eventTitle} for {assessmentTitle} is scheduled for {formattedDate}",
                Title = $"{eventTitle} Notification",
                ReturningData = "Test data",
                NotificationId = notificationId,
                Schedule =
                {
                    NotifyTime = eventDate,
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            return notificationId;
        }

        public static async Task<int> ScheduleCourseNotification(string courseTitle, string eventTitle, DateTime eventDate)
        {
            var formattedDate = eventDate.ToString("MM/dd/yyyy");
            int notificationId = new Random().Next(10000);
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = $"{eventTitle} for {courseTitle} is scheduled for {formattedDate}",
                Title = $"{eventTitle} Notification",
                ReturningData = "Test data",
                NotificationId = notificationId,
                Schedule =
                {
                    NotifyTime = eventDate,
                }
            };
            
            await LocalNotificationCenter.Current.Show(notification);
            return notificationId;
        }
    }


}
