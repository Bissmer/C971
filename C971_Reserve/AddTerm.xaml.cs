using Android.App.Usage;
using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve;

public partial class AddTerm : ContentPage
{
    private readonly ProjectDatabase _db;
	public AddTerm(ProjectDatabase db)
	{
		InitializeComponent();
		_db = db;
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
        await Navigation.PopAsync();
    }
}