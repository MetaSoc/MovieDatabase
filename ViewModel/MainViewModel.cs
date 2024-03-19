using MovieDatabase.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Xceed.Wpf.Toolkit;

namespace MovieDatabase.ViewModel
{
    class MainViewModel : BaseViewModel
    {

        public MainViewModel()
        {
            Entries = SqliteDataAccess.LoadDb();
            Genres = SqliteDataAccess.ReadGenres();
            NumberOfRecords = Entries.Count;
        }
        //Relay Methods
        private void Filter(string title, string selectedGenres, double rating, double release, bool? isWatched, double userRating, int duration)
        {
            Entries = SqliteDataAccess.LoadDb();
            for (int i = 0; i < Entries.Count; i++)
            {
                if (isWatched is not null && Entries[i].IsWatched != isWatched)
                {
                    Entries.RemoveAt(i);
                    i--;
                    continue;
                }

                if (title != null)
                {
                    if (title.Length > 2 || title.Length == 0)
                    {
                        if (!Entries[i].Title.ToUpper(CultureInfo.InvariantCulture).Contains(title.ToUpper(CultureInfo.InvariantCulture)))
                        {
                            Entries.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                    else if (title.Length < 3)
                    {
                        MessageBox.Show("Enter minimum 3 characters.", "Not enough characters");
                        break;
                    }
                }

                if (selectedGenres != null && Entries[i].Genres != null)
                {
                    List<string> genresList = selectedGenres.Split('\n').ToList();
                    if (!genresList.All(genre => Entries[i].Genres.Contains(genre)))
                    {
                        Entries.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if (Entries[i].Rating < rating)
                {
                    Entries.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Entries[i].UserRating < userRating)
                {
                    Entries.RemoveAt(i);
                    i--;
                    continue;
                }

                if (Entries[i].ReleaseDate < release)
                {
                    Entries.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Entries[i].Duration < duration)
                {
                    Entries.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            NumberOfRecords = Entries.Count;
        }

        private void GetOverview(int index)
        {
            if (index >= 0)
                MessageBox.Show(Entries[index].Overview,
                $"Movie description: {Entries[index].Title}");
        }
        //RelayCommands
        public RelayCommand FilterCommand => new(execute => Filter(Title, SelectedGenres, Rating, ReleaseDate, IsWatched, UserRating, Duration));
        public RelayCommand SortTitleCommand => new(execute => SortTitle());
        public RelayCommand SortDurationCommand => new(execute => SortDuration());
        public RelayCommand SortRatingCommand => new(execute => SortRating());
        public RelayCommand SortReleaseCommand => new(execute => SortRelease());


        private ObservableCollection<Item> _entries;
        public ObservableCollection<Item> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }
        public void SortTitle()
        {
            _wasSorted = true;
            _wasDurationSorted = _wasRatingSorted = _wasReleaseSorted = false;
            if (_wasTitleSorted)
            {
                Entries = new ObservableCollection<Item>(Entries.OrderBy(i => i.Title));
                _wasTitleSorted = false;
            }
            else
            {
                Entries = new ObservableCollection<Item>(Entries.OrderByDescending(i => i.Title));
                _wasTitleSorted = true;
            }
        }
        public void SortDuration()
        {
            _wasSorted = true;
            _wasTitleSorted = true;
            _wasRatingSorted = _wasReleaseSorted = false;
            if (_wasDurationSorted)
            {
                Entries = new ObservableCollection<Item>(Entries.OrderBy(i => i.Duration));
                _wasDurationSorted = false;
            }
            else
            {
                Entries = new ObservableCollection<Item>(Entries.OrderByDescending(i => i.Duration));
                _wasDurationSorted = true;
            }
        }
        public void SortRating()
        {
            _wasSorted = true;
            _wasTitleSorted = true;
            _wasDurationSorted = _wasReleaseSorted = false;
            if (_wasRatingSorted)
            {
                Entries = new ObservableCollection<Item>(Entries.OrderBy(i => i.Rating));
                _wasRatingSorted = false;
            }
            else
            {
                Entries = new ObservableCollection<Item>(Entries.OrderByDescending(i => i.Rating));
                _wasRatingSorted = true;
            }
        }
        public void SortRelease()
        {
            _wasSorted = true;
            _wasTitleSorted = true;
            _wasRatingSorted = _wasDurationSorted = false;
            if (_wasReleaseSorted)
            {
                Entries = new ObservableCollection<Item>(Entries.OrderBy(i => i.ReleaseDate));
                _wasReleaseSorted = false;
            }
            else
            {
                Entries = new ObservableCollection<Item>(Entries.OrderByDescending(i => i.ReleaseDate));
                _wasReleaseSorted = true;
            }
        }

        public ObservableCollection<string> Genres { get; set; }

        private bool _wasSorted = false;
        private bool _wasTitleSorted = true;
        private bool _wasDurationSorted = false;
        private bool _wasRatingSorted = false;
        private bool _wasReleaseSorted = false;
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _selectedGenres;
        public string SelectedGenres
        {
            get => _selectedGenres;
            set
            {
                _selectedGenres = value;
                OnPropertyChanged();
            }
        }


        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        private double _rating;
        public double Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged();
            }
        }
        private double _releaseDate;
        public double ReleaseDate
        {
            get => _releaseDate;
            set
            {
                _releaseDate = value;
                OnPropertyChanged();
            }
        }
        private bool? _isWatched = null;
        public bool? IsWatched
        {
            get => _isWatched;
            set
            {
                _isWatched = value;
                OnPropertyChanged();
            }
        }

        private bool? _isWatchedResult;
        public bool? IsWatchedResult
        {
            get => _isWatchedResult;
            set
            {
                _isWatchedResult = value;
                OnPropertyChanged();
            }
        }

        private double _userRating;
        public double UserRating
        {
            get => _userRating;
            set
            {
                _userRating = value;
                OnPropertyChanged();
            }
        }

        private double _userRatingResult;
        public double UserRatingResult
        {
            get => _userRatingResult;
            set
            {
                _userRatingResult = value;
                OnPropertyChanged();
            }
        }
        private int _numberOfRecords;
        public int NumberOfRecords
        {
            get => _numberOfRecords;
            set
            {
                _numberOfRecords = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndex = -2;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                OnPropertyChanged();
                if (_wasSorted)
                {
                    _wasSorted = false;
                    return;
                }
                GetOverview(value);
                _selectedIndex = -2;
            }
        }
    }
}
