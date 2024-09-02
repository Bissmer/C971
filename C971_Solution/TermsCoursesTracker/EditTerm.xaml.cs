using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class EditTerm : ContentPage
{
    private readonly ProjectDatabase _database;
    private readonly Term _term;
    private bool _changesMade = false;
    public EditTerm(ProjectDatabase database, Term term)
	{
        InitializeComponent();
        _database = database;
        _term = term;


        TermTitleEntry.Text = _term.TermTitle;
        StartDatePicker.Date = _term.StartDate;
        EndDatePicker.Date = _term.EndDate;

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


    /// <summary>
    /// Method to validate EditTerm form fields
    /// </summary>
    /// <returns></returns>
    private bool ValidateFields()
    {
        bool isValid = true;

        bool isTitleValid = !string.IsNullOrWhiteSpace(TermTitleEntry.Text);
        TermValidationLabel.IsVisible = !isTitleValid;
        if (!isTitleValid) isValid = false;

        bool areDatesValid = StartDatePicker.Date <= EndDatePicker.Date;
        DateValidationLabel.IsVisible = !areDatesValid;
        if (!areDatesValid) isValid = false;

        SaveButton.IsEnabled = isValid;
        SaveButton.BackgroundColor = isValid ? Color.FromArgb("#1c9630") : Color.FromArgb("#9E9E9E");

        return isValid;
    }


    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (!ValidateFields())
        {
            return;
        }

        // Update the term with new values
        _term.TermTitle = TermTitleEntry.Text;
        _term.StartDate = StartDatePicker.Date;
        _term.EndDate = EndDatePicker.Date;

        await _database.SaveTermAsync(_term);
        await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (_changesMade)
        {
            bool confirm = await DisplayAlert("Cancel Confirmation", "Do you want to discard term edit?", "Yes", "No");
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

    protected override bool OnBackButtonPressed()
    {
        if (_changesMade)
        {
            Dispatcher.Dispatch(async () =>
            {
                bool confirm = await DisplayAlert("Cancel Confirmation", "Do you want to discard term edit?", "Yes",
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

