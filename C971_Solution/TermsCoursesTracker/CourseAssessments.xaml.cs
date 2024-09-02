using System.Collections.ObjectModel;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class CourseAssessments : ContentPage
{
    private readonly ProjectDatabase _database;
    private readonly int _courseId;
    private ObservableCollection<Assessment> _assessments;
  
    public CourseAssessments( ProjectDatabase database, Course course)
    {
		InitializeComponent();
        _database = database;
        _courseId = course.CourseId;

        CourseTitleLabel.Text = $"{course.CourseTitle} Assessments";
        BindingContext = this;
        LoadAssessments();
	}

    public ObservableCollection<Assessment> Assessments
    {
        get => _assessments;
        set
        {
            _assessments = value;
            OnPropertyChanged();
        }
    }

    private async Task LoadAssessments()
    {
        var assessments = await _database.GetAssessmentsByTermAsync(_courseId);
        Assessments = new ObservableCollection<Assessment>(assessments);
    }

    private async void OnDeleteAssessmentSwipeItemInvoked(object sender, EventArgs e)
    {
        var assessment = (sender as SwipeItem).BindingContext as Assessment;
        bool confirm = await DisplayAlert("Delete Assessment", "Are you sure you want to delete this assessment?", "Yes", "No");
        if (confirm)
        {
            await _database.DeleteAssessmentAsync(assessment);
            Assessments.Remove(assessment);
        }
    }

    private async void OnEditAssessmentSwipeItemInvoked(object sender, EventArgs e)
    {
        var assessment = (sender as SwipeItem).BindingContext as Assessment;
        var editAssessmentPage = new EditCourseAssessment(_database, assessment);
        await Navigation.PushAsync(editAssessmentPage);
        editAssessmentPage.Disappearing += (s, args) => LoadAssessments();
    }

    private async void OnAddAssessmentButtonClicked(object sender, EventArgs e)
    {
        var addAssessmentPage = new AddCourseAssessment(_database, _courseId);
        await Navigation.PushAsync(addAssessmentPage);
        addAssessmentPage.Disappearing += (s, args) => LoadAssessments();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAssessmentTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var assessment = (Assessment)frame.BindingContext;
        var assessmentOverviewPage = new AssessmentOverview(assessment);
        await Navigation.PushAsync(assessmentOverviewPage);

    }
}