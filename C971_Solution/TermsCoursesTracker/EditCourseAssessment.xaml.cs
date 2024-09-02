using Plugin.LocalNotification;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class EditCourseAssessment : ContentPage
{
	private readonly ProjectDatabase _database;
	private readonly Assessment _assessment;
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
        UpdateButton.BackgroundColor = isValid ? Color.FromArgb("#1c9630") : Color.FromArgb("#9E9E9E");

        return isValid;
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

       await _database.SaveAssessmentAsync(_assessment);

        if (NotificationSwitch.IsToggled)
        {
           _assessment.NotificationIdStart = await NotificationService.ScheduleAssessmentNotification(_assessment.AssessmentType, _assessment.AssessmentTitle, "Assessment Start", _assessment.StartDate);
           _assessment.NotificationIdEnd = await NotificationService.ScheduleAssessmentNotification(_assessment.AssessmentType, _assessment.AssessmentTitle, "Assessment End", _assessment.EndDate);
        }


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