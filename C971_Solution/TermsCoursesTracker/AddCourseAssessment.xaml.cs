using Plugin.LocalNotification;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class AddCourseAssessment : ContentPage
{
	private readonly ProjectDatabase _database;
	private readonly int _courseId;
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

    /// <summary>
    /// Method to validate AddCourseAssessment form fields
    /// </summary>
    /// <returns></returns>
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
        AddButton.BackgroundColor = isValid ? Color.FromArgb("#1c9630") : Color.FromArgb("#9E9E9E");

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

        await _database.SaveAssessmentAsync(newAssessment);
        if (NotificationSwitch.IsToggled)
        {
            newAssessment.NotificationIdStart = await NotificationService.ScheduleAssessmentNotification(newAssessment.AssessmentType, newAssessment.AssessmentTitle, "Assessment Start", newAssessment.StartDate);
            newAssessment.NotificationIdEnd = await NotificationService.ScheduleAssessmentNotification(newAssessment.AssessmentType, newAssessment.AssessmentTitle, "Assessment End", newAssessment.EndDate);
        }

        
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

    /// <summary>
    /// Method to show or hide labels based on the switch value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnNotificationToggled(object sender, ToggledEventArgs e)
    {
        PositiveNotificationLabel.IsVisible = e.Value;
        NegativeNotificationLabel.IsVisible = !e.Value;
        OnFieldChanged(sender, e);
    }

    /// <summary>
    /// Method to handle the swipe back event
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        if (_changesMade)
        {
            Dispatcher.Dispatch(async () =>
            {
                bool confirm = await DisplayAlert("Cancel Confirmation", "Do you want to discard assessment add?", "Yes", "No");
                if (confirm)
                {
                    await Navigation.PopAsync();
                }
            });
            return true;
        }
        return base.OnBackButtonPressed();
    }



}