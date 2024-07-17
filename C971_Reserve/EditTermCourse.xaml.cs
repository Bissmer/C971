using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve;

public partial class EditTermCourse : ContentPage
{
	private readonly ProjectDatabase _db;
	private Course _origCourse;
	public EditTermCourse(ProjectDatabase db, Course course)
	{
        InitializeComponent();
		_db = db;
		_origCourse = course;

		CourseTitleEntry.Text = _origCourse.CourseTitle;
        StartDatePicker.Date = _origCourse.StartDate;
		EndDatePicker.Date = _origCourse.EndDate;
        InstructorNameEntry.Text = _origCourse.InstructorName;
        InstructorPhoneEntry.Text = _origCourse.InstructorPhone;
		InstructorEmailEntry.Text = _origCourse.InstructorEmail;
        NotificationSwitch.IsToggled = _origCourse.Notification;
		AdditionalNotes.Text = _origCourse.AdditionalNotes;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
		_origCourse.CourseTitle = CourseTitleEntry.Text;
		_origCourse.StartDate = StartDatePicker.Date;
		_origCourse.EndDate = EndDatePicker.Date;
		_origCourse.InstructorName = InstructorNameEntry.Text;
		_origCourse.InstructorPhone = InstructorPhoneEntry.Text;
		_origCourse.InstructorEmail = InstructorEmailEntry.Text;
		_origCourse.Notification = NotificationSwitch.IsToggled;
		_origCourse.AdditionalNotes = AdditionalNotes.Text;

		await  _db.SaveCourseAsync(_origCourse);
		await Navigation.PopAsync();
    }

	private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

	private async void OnFieldChanged(object sender, EventArgs e)
    {
        SaveButton.IsEnabled = true;
    }

	private async void OnNotificationSwitchToggled(object sender, ToggledEventArgs e)
    {
        throw new NotImplementedException();
    }

}