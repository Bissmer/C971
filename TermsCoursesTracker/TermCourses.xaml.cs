using System.Collections.ObjectModel;
using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;
using TermsCoursesTracker.ViewModels;

namespace TermsCoursesTracker;

public partial class TermCourses : ContentPage
{
    private readonly ProjectDatabase _database;
    private readonly int _termId;
    private ObservableCollection<Course> _courses;

    public TermCourses(ProjectDatabase database, Term term)
	{
		InitializeComponent();
		_database = database;
		_termId = term.Id;

        TermTitleLabel.Text = term.TermTitle;
        TermDateRangeLabel.Text = term.StartDate.ToString("MM/dd/yyyy") + " - " + term.EndDate.ToString("MM/dd/yyyy");

        LoadCourses();

    }

    private async Task LoadCourses()
    {
        _courses = new ObservableCollection<Course>(await _database.GetCoursesAsync(_termId));
        CoursesCollectionView.ItemsSource = _courses;
    }



    private async void OnEditCourseSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem)sender;
        var course = (Course)swipeItem.BindingContext;

        var editCoursePage = new EditTermCourse(_database, course);
        await Navigation.PushAsync(editCoursePage);

        
        editCoursePage.Disappearing += (s, args) => LoadCourses();
    }

    private async void OnDeleteCoursesSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = (SwipeItem) sender;
        var course = (Course) swipeItem.BindingContext;

        bool confirm = await DisplayAlert("Delete Confirmation", "Do you want to delete this course?", "Yes", "No");

        if (confirm)
        {
            await _database.DeleteCourseAsync(course);
            _courses.Remove(course);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAddCourseButtonClicked(object sender, EventArgs e)
    {
        var addTermCoursePage = new AddTermCourse(_database,_termId);
        addTermCoursePage.Disappearing += (s, args) => LoadCourses();
        await Navigation.PushAsync(addTermCoursePage);
    }

    private async void OnCourseTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var course = (Course)frame.BindingContext;

        var courseOverviewPage = new CourseOverview(_database, course);
        await Navigation.PushAsync(courseOverviewPage);
    }
}