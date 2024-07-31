using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class AddTerm : ContentPage
{
	private readonly ProjectDatabase _database;
	private bool _changesMade = false;
    public AddTerm(ProjectDatabase database)
	{
		InitializeComponent();
		_database = database;
        TermTitleEntry.TextChanged += OnFieldChanged;
        StartDatePicker.DateSelected += OnFieldChanged;
        EndDatePicker.DateSelected += OnFieldChanged;
    }

    /// <summary>
    /// Method to handle the event when a field is changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	private void OnFieldChanged(object sender, EventArgs e)
    {
        
        _changesMade = true;
        ValidateFields();
    }


    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {

        if (!ValidateFields()) 
        {
            return;
        }

        var newTerm = new Term
            {
                TermTitle = TermTitleEntry.Text,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date
            };
            await _database.SaveTermAsync(newTerm);
            await Navigation.PopAsync();
    }

    /// <summary>
    /// Method to validate AddTerm form fields
    /// </summary>
    /// <returns></returns>
    private bool ValidateFields()
    {
        bool isValid = true;

        // Title validation
        bool isTitleValid = !string.IsNullOrWhiteSpace(TermTitleEntry.Text);
        TermValidationLabel.IsVisible = !isTitleValid;
        if (!isTitleValid) isValid = false;

        // Date validation
        bool areDatesValid = StartDatePicker.Date <= EndDatePicker.Date;
        DateValidationLabel.IsVisible = !areDatesValid;
        if (!areDatesValid) isValid = false;

        // Enable Save button only if both title and dates are valid
        SaveButton.IsEnabled = isValid;

        return isValid;
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (_changesMade)
        {
            bool confirm = await DisplayAlert("Cancel Confirmation", "Do you want to discard term add?", "Yes", "No");
            if (confirm)
            {
                await Navigation.PopAsync();
            }
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    /// <summary>
    /// Method to handle the swipe back event
    /// </summary>
    /// <returns></returns>
    protected override bool OnBackButtonPressed()
    {
        if (_changesMade)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool confirm = await DisplayAlert("Cancel Confirmation", "Do you want to discard term add?", "Yes",
                    "No");
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
