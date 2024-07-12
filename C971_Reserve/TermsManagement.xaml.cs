using C971_Reserve.Schemas;
using C971_Reserve.Services;
using C971_Reserve.Views;

namespace C971_Reserve
{
    public partial class TermsManagement : ContentPage
    {

        private TermsViewModel _viewModel;
        private ProjectDatabase _db;

        public TermsManagement()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "C971_Reserve.db3");
            _db = new ProjectDatabase(dbPath);
            _viewModel = new TermsViewModel(_db);

            BindingContext = _viewModel;
        }


        private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var term = (Term)swipeItem.BindingContext;

            var editTermPage = new EditTerm(_db, term);
            await Navigation.PushAsync(editTermPage);
            //refresh the terms list when the edit term page is closed
            editTermPage.Disappearing += (s, args) => _viewModel.LoadTerms();

        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var term = (Term)swipeItem.BindingContext;

            bool confirm = await DisplayAlert("Delete Confirmation", $"Do you want to delete the term {term.Title}?",
                "Yes", "No");
            if (confirm)
            {
                await _db.DeleteTermAsync(term);
                _viewModel.Terms.Remove(term);
            }
        }

        private async void OnAddTermButtonClicked(object sender, EventArgs e)
        {
            var addTermPage = new AddTerm(_db);
            await Navigation.PushAsync(addTermPage);
            //refresh the terms list when the add term page is closed
            addTermPage.Disappearing += (s, args) => _viewModel.LoadTerms();
        }

    }

}
