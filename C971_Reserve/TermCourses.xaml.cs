using System.Collections.ObjectModel;
using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve;

public partial class TermCourses : ContentPage
{
	private  readonly ProjectDatabase _db;
	private Term _origTerm;
	private ObservableCollection<Course> _courses;

	public TermCourses(ProjectDatabase db, Term term)
	{
		InitializeComponent();
		_db = db;
		_origTerm = term;

		TermTitleLabel.Text = term.Title;
        TermDateRangeLabel.Text = term.StartDate.ToString("MM/dd/yyyy") + " - " + term.EndDate.ToString("MM/dd/yyyy");

		LoadCourses();
    }

	private async void LoadCourses()
    {
        _courses = new ObservableCollection<Course>(await _db.GetCoursesAsync(_origTerm.Id));
        CoursesCollectionView.ItemsSource = _courses;
    }

	private async void OnEditCourseSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem)sender;
		var course = (Course)swipeItem.BindingContext;
    }

    private async void OnDeleteCourseSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem)sender;
        var course = (Course)swipeItem.BindingContext;
        bool confirm = await DisplayAlert("Delete Confirmation", $"Do you want to delete the course {course.CourseTitle}?", "Yes", "No");
        if (confirm)
        {
            await _db.DeleteCourseAsync(course);
            _courses.Remove(course);
        }
    }

    public  async void OnAddCourseButtonClicked(object sender, EventArgs e)
    {
        var addCoursePage = new AddTermCourse(_db, _origTerm.Id);
        addCoursePage.Disappearing += (s, args) => LoadCourses();
        await Navigation.PushAsync(addCoursePage);
        
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}