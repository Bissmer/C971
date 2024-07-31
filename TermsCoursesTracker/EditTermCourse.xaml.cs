using Plugin.LocalNotification;
using System.Text.RegularExpressions;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class EditTermCourse : ContentPage
{
	private ProjectDatabase _database;
	private Course _course;
	private bool _changesMade = false;

	
	public EditTermCourse(ProjectDatabase database, Course course)
	{
		InitializeComponent();
        _database = database;
        _course = course;

        CourseTitleEntry.Text = course.CourseTitle;
        CourseStatusPicker.SelectedItem = course.CourseStatus;
        StartDatePicker.Date = course.StartDate;
        EndDatePicker.Date = course.EndDate;
        InstructorNameEntry.Text = course.InstructorName;
        InstructorPhoneEntry.Text = course.InstructorPhone;
        InstructorEmailEntry.Text = course.InstructorEmail;
        NotificationSwitch.IsToggled = course.Notification;
        AdditionalNotesEntry.Text = course.AdditionalNotes;

        CourseTitleEntry.TextChanged += OnFieldChanged;
        CourseStatusPicker.SelectedIndexChanged += OnFieldChanged;
        StartDatePicker.DateSelected += OnFieldChanged;
        EndDatePicker.DateSelected += OnFieldChanged;
        InstructorNameEntry.TextChanged += OnFieldChanged;
        InstructorPhoneEntry.TextChanged += OnFieldChanged;
        InstructorEmailEntry.TextChanged += OnFieldChanged;
        NotificationSwitch.Toggled += OnNotificationToggled;
        AdditionalNotesEntry.TextChanged += OnFieldChanged;

        ValidateFields();
    }

    private void OnFieldChanged(object sender, EventArgs e)
    {
        _changesMade = true;
        ValidateFields();
    }


    private bool ValidateFields()
    {
        bool isValid = true;

        // Validation for Course Title
        bool isTitleValid = !string.IsNullOrWhiteSpace(CourseTitleEntry.Text);
        TitleValidationLabel.IsVisible = !isTitleValid;
        if (!isTitleValid) isValid = false;

        // Validation for Instructor Name
        bool isInstructorNameValid = !string.IsNullOrWhiteSpace(InstructorNameEntry.Text);
        InstructorNameValidationLabel.IsVisible = !isInstructorNameValid;
        if (!isInstructorNameValid) isValid = false;

        // Validation for Instructor Phone
        bool isInstructorPhoneValid = IsValidPhoneNumber(InstructorPhoneEntry.Text);
        InstructorPhoneValidationLabel.IsVisible = !isInstructorPhoneValid;
        if (!isInstructorPhoneValid) isValid = false;

        // Validation for Instructor Email
        bool isInstructorEmailValid = IsValidEmail(InstructorEmailEntry.Text);
        InstructorEmailValidationLabel.IsVisible = !isInstructorEmailValid;
        if (!isInstructorEmailValid) isValid = false;

        // Validation for Dates
        bool areDatesValid = StartDatePicker.Date <= EndDatePicker.Date;
        DateValidationLabel.IsVisible = !areDatesValid;
        if (!areDatesValid) isValid = false;

        
        UpdateButton.IsEnabled = isValid;

        return isValid;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;
        //regex supports +,digit between 1 and 9 followed by up to 14 digits,
        //zero or more groups of a dot followed by one to four digits
        var phoneRegex = new Regex(@"^\+?[1-9]\d{0,14}(\.\d{1,4})*$");
        return phoneRegex.IsMatch(phoneNumber);
    }



    private void OnNotificationToggled(object sender, ToggledEventArgs e)
    {
        NotificationLabel.IsVisible = e.Value;
        OnFieldChanged(sender, e);
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (_changesMade)
        {
            bool result = await DisplayAlert("Cancel Course Edit", "Are you sure you want to discard changes?", "Yes", "No");
            if (!result)
            {
                return;
            }
        }
        await Navigation.PopAsync();
    }

    private async void OnUpdateButtonClicked(object sender, EventArgs e)
    {
        if (!ValidateFields())
        {
            return;
        }

        _course.CourseTitle = CourseTitleEntry.Text;
        _course.CourseStatus = CourseStatusPicker.SelectedItem?.ToString();
        _course.StartDate = StartDatePicker.Date;
        _course.EndDate = EndDatePicker.Date;
        _course.InstructorName = InstructorNameEntry.Text;
        _course.InstructorPhone = InstructorPhoneEntry.Text;
        _course.InstructorEmail = InstructorEmailEntry.Text;
        _course.Notification = NotificationSwitch.IsToggled;
        _course.AdditionalNotes = AdditionalNotesEntry.Text;

        // Remove existing notifications if any
        LocalNotificationCenter.Current.Cancel(_course.NotificationIdStart);
        LocalNotificationCenter.Current.Cancel(_course.NotificationIdEnd);

        if (NotificationSwitch.IsToggled)
        {
            // Schedule new notifications
            _course.NotificationIdStart = ScheduleNotification(_course.CourseTitle, "Course Start", _course.StartDate);
            _course.NotificationIdEnd = ScheduleNotification(_course.CourseTitle, "Course End", _course.EndDate);
        }

        await _database.SaveCourseAsync(_course);
        await Navigation.PopAsync();
    }

    private int ScheduleNotification(string courseTitle, string eventTitle, DateTime eventDate)
    {
        int notificationId = new Random().Next(10000);
        var formattedDate = eventDate.ToString("MM/dd/yyyy");
        var notification = new NotificationRequest

        {
            BadgeNumber = 1,
            Description = $"{eventTitle} for {courseTitle} is scheduled for {formattedDate}",
            Title = $"{eventTitle} Notification",
            ReturningData = "Test",
            NotificationId = notificationId,
            Schedule =
            {
                NotifyTime = eventDate,
            }
        };
        LocalNotificationCenter.Current.Show(notification);
        return notificationId;


    }

}