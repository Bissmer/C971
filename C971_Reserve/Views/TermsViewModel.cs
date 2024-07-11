using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971_Reserve.Schemas;
using C971_Reserve.Services;

namespace C971_Reserve.Views
{
    public class TermsViewModel : INotifyPropertyChanged
    {
        private readonly ProjectDatabase _db;
        private ObservableCollection<Term> _terms;

        public ObservableCollection<Term> Terms
        {
            get => _terms;
            set
            {
                _terms = value;
                OnPropertyChanged(nameof(Terms));
            }
        }
        
        public TermsViewModel(ProjectDatabase db)
        {
            _db = db;
            LoadTerms();
        }

        /// <summary>
        /// Load terms from the database
        /// </summary>
        public async void LoadTerms()
        {
            var terms = await _db.GetTermsAsync();
            Terms = new ObservableCollection<Term>(terms);
        }



        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the PropertyChanged event when a property is changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
