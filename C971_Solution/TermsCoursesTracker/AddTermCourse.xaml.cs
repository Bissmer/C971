
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Plugin.LocalNotification;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;
using TermsCoursesTracker.ViewModels;

namespace TermsCoursesTracker;

public partial class AddTermCourse : ContentPage
{
	private readonly ProjectDatabase _database;
    private readonly int _termId;
    private bool _changesMade = false;
    public AddTermCourse(ProjectDatabase database, int termId)
	{
		InitializeComponent();
        _database = database;
        _termId = termId;

        CourseTitleEntry.TextChanged += OnFieldChanged;
        CourseStatusPicker.SelectedIndexChanged += OnFieldChanged;
        StartDatePicker.DateSelected += OnFieldChanged;
        EndDatePicker.DateSelected += OnFieldChanged;
        InstructorNameEntry.TextChanged += OnFieldChanged;
        InstructorPhoneEntry.TextChanged += OnFieldChanged;
        InstructorEmailEntry.TextChanged += OnFieldChanged;
        NotificationSwitch.Toggled += OnFieldChanged;
        AdditionalNotesEntry.TextChanged += OnFieldChanged;

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

        // Validation for Course Status
        bool isStatusValid = CourseStatusPicker.SelectedIndex != -1;
        StatusValidationLabel.IsVisible = !isStatusValid;
        if (!isStatusValid) isValid = false;

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

        // Enable Save Button 
        SaveButton.IsEnabled = isValid;
        SaveButton.BackgroundColor = isValid ? Color.FromArgb("#1c9630") : Color.FromArgb("#9E9E9E");

        return isValid;

    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if(!ValidateFields())
        {
            return;
        }

        var newCourse = new Course
        {
            TermId = _termId,
            CourseTitle = CourseTitleEntry.Text,
            CourseStatus = CourseStatusPicker.SelectedItem?.ToString(),
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date,
            InstructorName = InstructorNameEntry.Text,
            InstructorPhone = InstructorPhoneEntry.Text,
            InstructorEmail = InstructorEmailEntry.Text,
            Notification = NotificationSwitch.IsToggled,
            AdditionalNotes = AdditionalNotesEntry.Text
        };

        await _database.SaveCourseAsync(newCourse);

        if (NotificationSwitch.IsToggled)
        {
            await  NotificationService.ScheduleCourseNotification(newCourse.CourseTitle, "Course Start", newCourse.StartDate);
            await NotificationService.ScheduleCourseNotification(newCourse.CourseTitle, "Course End", newCourse.EndDate);
        }

        
        await Navigation.PopAsync();
    }


    /// <summary>
    /// Method to show or hide the positive or negative notification label based on the Set Notifications value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnNotificationToggled (object sender, ToggledEventArgs e)
    {
        PositiveNotificationLabel.IsVisible = e.Value;
        NegativeNotificationLabel.IsVisible = !e.Value;
        OnFieldChanged(sender, e);
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (_changesMade)
        {
            bool result = await DisplayAlert("Cancel Course Add", "Are you sure you want to discard course add?", "Yes", "No");
            if (!result)
            {
                return;
            }
        }
        await Navigation.PopAsync();
    }

    /// <summary>
    /// Override method to handle back button press (swipe) and remind of unsaved changes
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        if (_changesMade)
        {
            Dispatcher.Dispatch(async () =>
            {
                bool result = await DisplayAlert("Cancel Course Add", "Are you sure you want to discard course add?", "Yes", "No");
                if (result)
                {
                    await Navigation.PopAsync();
                }
            });
            return true;
        }
        return base.OnBackButtonPressed();
    }

    /// <summary>
    /// Method to validate email for the Instructor Email field
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private static bool IsValidEmail(string email)
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

    /// <summary>
    /// Method to validate phone number for the Instructor Phone field
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Basic phone number validation using a regular expression
        //regex supports +,digit between 1 and 9 followed by up to 14 digits,
        //zero or more groups of a dot or dash followed by one to four digits
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;
        var phoneRegex = new Regex(@"^\+?[1-9]\d{0,14}([.-]?\d{1,4})*$");
        return phoneRegex.IsMatch(phoneNumber);
    }

}
