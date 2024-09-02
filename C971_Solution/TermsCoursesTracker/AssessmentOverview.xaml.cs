using TermsCoursesTracker.Schemas;

namespace TermsCoursesTracker;

public partial class AssessmentOverview : ContentPage
{
	private readonly Assessment _assessment;
	public AssessmentOverview(Assessment assessment)
	{
		InitializeComponent();
		_assessment = assessment;
		BindingContext = _assessment;
	}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}