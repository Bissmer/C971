using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve;

public partial class EditTerm : ContentPage
{
    private readonly ProjectDatabase _db;
	private Term _origTerm;
	private bool _changesMade = false;
    private bool _isInitialized = true;

    public EditTerm(ProjectDatabase db, Term term)
	{
		InitializeComponent();
		_db = db;
		_origTerm = term;

        BindingContext = new Term
        {
            Id = term.Id,
            Title = term.Title,
            StartDate = term.StartDate,
            EndDate = term.EndDate
        };

        _isInitialized = false;
        NavigationPage.SetHasBackButton(this, false);
    }

    private void OnFieldChanged(object sender, EventArgs e)
    {
        if(_isInitialized) return;

        if (!_changesMade)
        {
            _changesMade = true;
            SaveButton.IsEnabled = true;
        }
        
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var term = (Term)BindingContext;
        bool confirm = await DisplayAlert("Edit Confirmation", $"Do you want to save changes?",
            "Yes", "No");

            if (_changesMade) 
                if (confirm)
                {
                    {
                        await _db.SaveTermAsync(term);
                    }
                    await Navigation.PopAsync();
            }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Unsaved Changes", $"Your changes won't be saved. Do you wish to proceed?",
            "Yes", "No");
        if (confirm)
        {
            await Navigation.PopAsync();
        }
        
    }
    //
    protected override bool OnBackButtonPressed()
    {
        if(_changesMade)
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
        return  base.OnBackButtonPressed();
    }
}