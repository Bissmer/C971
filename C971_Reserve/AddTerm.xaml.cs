using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve;

public partial class AddTerm : ContentPage
{
    private readonly ProjectDatabase _db;
    private bool _changesMade = false;
    private bool _isInitialized = true;
    public AddTerm(ProjectDatabase db)
	{
		InitializeComponent();
		_db = db;
        _isInitialized = false;
        NavigationPage.SetHasBackButton(this, false);
    }


    private void OnFieldChanged(object sender, EventArgs e)
    {
        if (_isInitialized) return;

        if (!_changesMade)
        {
            _changesMade = true;
            SaveButton.IsEnabled = true;
        }

    }
    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var newTerm = new Term
        {
            Title = TermTitleEntry.Text,
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date
        };

            await _db.SaveTermAsync(newTerm);
            await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if(_changesMade)
        {
            bool confirm = await DisplayAlert("Cancel Confirmation", $"Do you want to discard changes?",
                "Yes", "No");
            if (confirm)
            {
                await Navigation.PopAsync();
            }
        }
    }
    protected override bool OnBackButtonPressed()
    {
        if (_changesMade)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool confirm = await DisplayAlert("Unsaved Changes", $"Your changes won't be saved. Do you wish to proceed?",
                    "Yes", "No");
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