using MovieDatabase.ViewModel;
using System.Windows;

namespace MovieDatabase.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var vm = new MainViewModel();
            InitializeComponent();
        }
    }
}
