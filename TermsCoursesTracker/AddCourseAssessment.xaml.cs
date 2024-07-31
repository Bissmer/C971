using Plugin.LocalNotification;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class AddCourseAssessment : ContentPage
{
	private ProjectDatabase _database;
	private int _courseId;
	private bool _changesMade;

	public AddCourseAssessment(ProjectDatabase database, int courseId)
	{
        InitializeComponent();
		_database = database;
		_courseId = courseId;


        AssessmentTitleEntry.TextChanged += OnFieldChanged;
        StartDatePicker.DateSelected += OnFieldChanged;
        EndDatePicker.DateSelected += OnFieldChanged;
        AssessmentTypePicker.SelectedIndexChanged += OnFieldChanged;
        NotificationSwitch.Toggled += OnFieldChanged;
    }

    private void OnFieldChanged(object sender, EventArgs e)
    {
        _changesMade = true;
        ValidateFields();
    }

    private bool ValidateFields()
    {
        bool isValid = true;

        // Validation for Assessment Title
        bool isTitleValid = !string.IsNullOrWhiteSpace(AssessmentTitleEntry.Text);
        TitleValidationLabel.IsVisible = !isTitleValid;
        if (!isTitleValid) isValid = false;

        // Validation for Dates
        bool areDatesValid = StartDatePicker.Date <= EndDatePicker.Date;
        DateValidationLabel.IsVisible = !areDatesValid;
        if (!areDatesValid) isValid = false;

        // Validation for Assessment Type
        bool isTypeValid = AssessmentTypePicker.SelectedIndex != -1;
        TypeValidationLabel.IsVisible = !isTypeValid;
        if (!isTypeValid) isValid = false;

        // Enable Add Button
        AddButton.IsEnabled = isValid;

        return isValid;
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (!ValidateFields())
        {
            return;
        }

        var newAssessment = new Assessment
        {
            CourseId = _courseId,
            AssessmentTitle = AssessmentTitleEntry.Text,
            AssessmentType = AssessmentTypePicker.SelectedItem?.ToString(),
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date,
            Notification = NotificationSwitch.IsToggled
        };

        if (NotificationSwitch.IsToggled)
        {
            ScheduleNotification(newAssessment.AssessmentType, newAssessment.AssessmentTitle, "Assessment Start", newAssessment.StartDate);
            ScheduleNotification(newAssessment.AssessmentType, newAssessment.AssessmentTitle, "Assessment End", newAssessment.StartDate);
        }

        await _database.SaveAssessmentAsync(newAssessment);
        await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (_changesMade)
        {
            bool result = await DisplayAlert("Cancel Assessment Add", "Are you sure you want to discard assessment add?", "Yes", "No");
            if (!result)
            {
                return;
            }
        }
        await Navigation.PopAsync();
    }

    private void OnNotificationToggled(object sender, ToggledEventArgs e)
    {
        PositiveNotificationLabel.IsVisible = e.Value;
        NegativeNotificationLabel.IsVisible = !e.Value;
        OnFieldChanged(sender, e);
    }

    private async void ScheduleNotification(string assessmentType, string assesmentTitle, string eventTitle, DateTime eventDate)
    {
        var formattedDate = eventDate.ToString("MM/dd/yyyy");
        int notificationId = new Random().Next(10000);
        var notification = new NotificationRequest
        {
            BadgeNumber = 1,
            Description = $"[{assessmentType} assessment] - {eventTitle} for {assesmentTitle} is scheduled for {formattedDate}",
            Title = $"{eventTitle} Notification",
            ReturningData = "Test data",
            NotificationId = notificationId,
            Schedule =
            {
                NotifyTime = eventDate,
            }
        };

        await LocalNotificationCenter.Current.Show(notification);
    }

}