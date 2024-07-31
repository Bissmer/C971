using TermsCoursesTracker.Schemas;
using TermsCoursesTracker.Services;
using TermsCoursesTracker.ViewModels;
using Environment = System.Environment;

namespace TermsCoursesTracker
{
    public partial class TermsManagement : ContentPage
    {
        private readonly TermsViewModel _viewModel;
        private readonly ProjectDatabase _database;

        public TermsManagement()
        {
            InitializeComponent();
            string dbPath = Constants.DatabasePath;
            _database = new ProjectDatabase(dbPath);
            _viewModel = new TermsViewModel(_database);

            BindingContext = _viewModel;

        }

        private async void OnEditTermSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var term = (Term)swipeItem.BindingContext;

            var editTermPage = new EditTerm(_database, term);
            await Navigation.PushAsync(editTermPage);
            // Refresh the list of terms when coming back from EditTerm
            editTermPage.Disappearing += (s, args) => _viewModel.LoadTerms();
        }

        private async void OnDeleteTermSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var term = (Term)swipeItem.BindingContext;

            bool confirm = await DisplayAlert("Delete Term", $"Do you want to delete {term.TermTitle}?", "Yes", "No");
            if (confirm)
            {
                await _database.DeleteTermAsync(term);
                _viewModel.LoadTerms();
            }
        }

        private async void OnAddTermButtonClicked(object sender, EventArgs e)
        {
            var addTermPage = new AddTerm(_database);
            await Navigation.PushAsync(addTermPage);
            //refresh the terms list when the AddTerm page is closed
            addTermPage.Disappearing += (s, args) => _viewModel.LoadTerms();
        }

        /// <summary>
        /// Opens a view to display the courses for the selected term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnTermTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var term = (Term)frame.BindingContext;

            var termCoursesPage = new TermCourses(_database, term);
            await Navigation.PushAsync(termCoursesPage);
            
        }

    }

}
