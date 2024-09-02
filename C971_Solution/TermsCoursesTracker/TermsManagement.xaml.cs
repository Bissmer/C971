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
        private const string PopulateDataKey = "IsDataPopulated";

        public TermsManagement()
        {
            InitializeComponent();
            string dbPath = Constants.DatabasePath;
            _database = new ProjectDatabase(dbPath);
            _viewModel = new TermsViewModel(_database);

            BindingContext = _viewModel;

        }

        /// <summary>
        /// Method to trigger the data population when the main page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await PopulateDataAsync();
            await _viewModel.LoadTerms();
        }

        /// <summary>
        /// Method to populate the database with test data. Check if there is are any terms in the database.
        /// </summary>
        /// <returns></returns>
        private async Task PopulateDataAsync()
        {
            bool isDataPopulated = Preferences.Get(PopulateDataKey, false);
            var terms = await _database.GetTermsAsync();
            if (!isDataPopulated || terms.Count == 0)
            {
                var testData = new TestData(_database);
                await testData.PopulateTestDataAsync();
                Preferences.Set("IsDataPopulated", true);
            }
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

            bool confirm = await DisplayAlert("Delete Term", $"Do you want to delete term {term.TermTitle} and all connected courses?", "Yes", "No");
            if (confirm)
            {
                await _database.DeleteTermAsync(term);
                await _viewModel.LoadTerms();
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
