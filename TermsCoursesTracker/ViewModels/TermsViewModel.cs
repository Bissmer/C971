using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermsCoursesTracker.Services;
using TermsCoursesTracker.Schemas;

namespace TermsCoursesTracker.ViewModels
{
    /// <summary>
    /// View model for the terms data binding.
    /// </summary>
    public class TermsViewModel : INotifyPropertyChanged
    {
        private readonly ProjectDatabase _database;
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

        public TermsViewModel(ProjectDatabase database)
        {
            _database = database;
            LoadTerms();
        }

        public async void LoadTerms()
        {
            var terms = await _database.GetTermsAsync();
            Terms = new ObservableCollection<Term>(terms);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
