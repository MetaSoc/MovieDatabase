using MovieDatabase.ViewModel;
using System.Data;
using System.Data.SQLite;

namespace MovieDatabase.Model
{
    public class Item : BaseViewModel
    {
        public string Title { get; set; }
        public double ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string Genres { get; set; }
        public string Overview { get; set; }
        public double Duration { get; set; }
        private double _userRating;
        public double UserRating
        {
            get => _userRating;
            set
            {
                OnPropertyChanged();
                _userRating = value;
            }
        }
        public double UserRatingResult
        {
            get => _userRating;
            set
            {
                _userRating = value;
                OnPropertyChanged();
                UpdateRating(this);
            }
        }
        private bool? _isWatched;
        public bool? IsWatched
        {
            get => _isWatched;
            set
            {
                OnPropertyChanged();
                _isWatched = value;
            }
        }
        public bool? IsWatchedResult
        {
            get => _isWatched;
            set
            {
                _isWatched = value;
                OnPropertyChanged();
                UpdateIsWatched(this);
            }
        }
        public void UpdateIsWatched(Item item)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            {
                cnn.Open();
                var command = cnn.CreateCommand();

                command.CommandText = "UPDATE Movies SET IsWatched = @IsWatched WHERE Title = @Title";

                var isWatchedParam = command.CreateParameter();
                isWatchedParam.ParameterName = "@IsWatched";
                isWatchedParam.Value = item.IsWatched;
                command.Parameters.Add(isWatchedParam);

                var titleParam = command.CreateParameter();
                titleParam.ParameterName = "@Title";
                titleParam.Value = item.Title;
                command.Parameters.Add(titleParam);

                command.ExecuteNonQuery();
            }
        }
        public void UpdateRating(Item item)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString());
            {
                cnn.Open();
                var command = cnn.CreateCommand();

                command.CommandText = "UPDATE Movies SET UserRating = @UserRating WHERE Title = @Title";

                var userRatingParam = command.CreateParameter();
                userRatingParam.ParameterName = "@UserRating";
                userRatingParam.Value = item.UserRating;
                command.Parameters.Add(userRatingParam);

                var titleParam = command.CreateParameter();
                titleParam.ParameterName = "@Title";
                titleParam.Value = item.Title;
                command.Parameters.Add(titleParam);

                command.ExecuteNonQuery();
            }
        }
    }

}
