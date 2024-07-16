using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve;

public partial class AddTermCourse : ContentPage
{
    private ProjectDatabase _db;
    private readonly int _temId;
    public AddTermCourse(ProjectDatabase db, int temId)
	{
		InitializeComponent();
        _db = db;
        _temId = temId;
	}

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var newCourse = new Course
        {
            TermId = _temId,
            CourseTitle = CourseTitleEntry.Text,
            StartDate = StartDatePicker.Date,
            EndDate = EndDatePicker.Date,
            InstructorName = InstructorNameEntry.Text,
            InstructorPhone = InstructorPhoneEntry.Text,
            InstructorEmail = InstructorEmailEntry.Text,
            Notification = NotificationSwitch.IsToggled ? "On" : "Off",
            AdditionalNotes = NotesEditor.Text
            
        };

        await _db.SaveCourseAsync(newCourse);
        await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


}