using Plugin.LocalNotification;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class EditCourseAssessment : ContentPage
{
	private ProjectDatabase _database;
	private Assessment _assessment;
	private bool _changesMade = false;

	public EditCourseAssessment(ProjectDatabase database, Assessment assessment)
	{
		InitializeComponent();
		_database = database;
		_assessment = assessment;

		AssessmentTitleEntry.Text = assessment.AssessmentTitle;
		StartDatePicker.Date = assessment.StartDate;
		EndDatePicker.Date = assessment.EndDate;
        AssessmentTypePicker.SelectedItem = assessment.AssessmentType;
		NotificationSwitch.IsToggled = assessment.Notification;

		AssessmentTitleEntry.TextChanged += OnFieldChanged;
		StartDatePicker.DateSelected += OnFieldChanged;
		EndDatePicker.DateSelected += OnFieldChanged;
		AssessmentTypePicker.SelectedIndexChanged += OnFieldChanged;
		NotificationSwitch.Toggled += OnFieldChanged;

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

        // Enable Save Button
        UpdateButton.IsEnabled = isValid;

        return isValid;
    }


    private int ScheduleNotification(string assessmentType, string assesmentTitle, string eventTitle, DateTime eventDate)
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

        LocalNotificationCenter.Current.Show(notification);
        return notificationId;
    }
    private void OnNotificationToggled(object sender, ToggledEventArgs e)
    {
        PositiveNotificationLabel.IsVisible = e.Value;
        NegativeNotificationLabel.IsVisible = !e.Value;
        OnFieldChanged(sender, e);
    }

    private async void OnUpdateButtonClicked(object sender, EventArgs e)
    {
        if (!ValidateFields())
        {
            return;
        }
        
        _assessment.AssessmentTitle = AssessmentTitleEntry.Text;
       _assessment.AssessmentType = AssessmentTypePicker.SelectedItem?.ToString();
       _assessment.StartDate = StartDatePicker.Date;
       _assessment.EndDate = EndDatePicker.Date;
       _assessment.Notification = NotificationSwitch.IsToggled;

       LocalNotificationCenter.Current.Cancel(_assessment.NotificationIdStart);
       LocalNotificationCenter.Current.Cancel(_assessment.NotificationIdEnd);

        if (NotificationSwitch.IsToggled)
        {
           _assessment.NotificationIdStart = ScheduleNotification(_assessment.AssessmentType, _assessment.AssessmentTitle, "Assessment Start", _assessment.StartDate);
           _assessment.NotificationIdEnd = ScheduleNotification(_assessment.AssessmentType, _assessment.AssessmentTitle, "Assessment End", _assessment.StartDate);
        }

        await _database.SaveAssessmentAsync(_assessment);
        await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (_changesMade)
        {
            bool result = await DisplayAlert("Cancel Assessment Edit", "Are you sure you want to discard assessment edit?", "Yes", "No");
            if (!result)
            {
                return;
            }
        }
        await Navigation.PopAsync();
    }

}