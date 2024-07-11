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

        private void OnCounterClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var term = (Terms)swipeItem.CommandParameter;
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var term = (Terms)swipeItem.BindingContext;
            await _db.DeleteTermAsync(term);
            _viewModel.Terms.Remove(term);
        }

    }

}
