using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;

namespace TermsCoursesTracker;

public partial class CourseOverview : ContentPage
{

	private readonly ProjectDatabase _database;
	private readonly Course _course;

	public CourseOverview(ProjectDatabase database, Course course)
	{
		InitializeComponent();
        _database = database;
		_course = course;

        CourseTitleLabel.Text = _course.CourseTitle;
        CourseTitleValueLabel.Text = _course.CourseTitle;
        CourseStatusValueLabel.Text = _course.CourseStatus;
        StartDateValueLabel.Text = _course.StartDate.ToString("MM/dd/yyyy");
        EndDateValueLabel.Text = _course.EndDate.ToString("MM/dd/yyyy");
        InstructorNameValueLabel.Text = _course.InstructorName;
        InstructorPhoneValueLabel.Text = _course.InstructorPhone;
        InstructorEmailValueLabel.Text = _course.InstructorEmail;
        NotificationsValueLabel.Text = _course.Notification ? "Yes" : "No";
        AdditionalNotesValueLabel.Text = _course.AdditionalNotes;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnCourseAssessmentsButtonClicked(object sender, EventArgs e)
    {
        var courseAssessmentsPage = new CourseAssessments(_database, _course);
        await Navigation.PushAsync(courseAssessmentsPage);
    }

    private async void OnShareButtonClicked(object sender, EventArgs e)
    {
        var shareMessage = new ShareTextRequest
        {
            Title = "Share Notes",
            Text = GenerateShareMessage()
        };

        await Share.RequestAsync(shareMessage);
    }

    private string GenerateShareMessage()
    {
        return $"Course Notes: {_course.AdditionalNotes}";
    }
}

