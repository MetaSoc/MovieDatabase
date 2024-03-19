using Dapper;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace MovieDatabase.Model
{
    public class SqliteDataAccess
    {
        public static ObservableCollection<string> ReadGenres()
        {
            string text = File.ReadAllText("../../../Genres/genres.txt");
            string[] arr = text.Split(' ');
            ObservableCollection<string> list = new ObservableCollection<string>(arr);
            return list;
        }
        public static ObservableCollection<Item> LoadDb()
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
            var output = cnn.Query<Item>($"SELECT Title, ReleaseDate, Rating, Genres, Overview, Duration, CAST(IsWatched AS INTEGER) AS IsWatched, UserRating FROM Movies").ToList();
            ObservableCollection<Item> list = new ObservableCollection<Item>(output);
            return list;
        }

        public static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
