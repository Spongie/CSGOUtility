using CSGOUtility.Data;
using CSGOUtility.Models;
using CSGOUtility.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSGOUtility.Views
{
    /// <summary>
    /// Interaction logic for CurrentGameView.xaml
    /// </summary>
    public partial class CurrentGameView : UserControl
    {
        private CurrentGameViewModel viewModel;

        public CurrentGameView()
        {
            InitializeComponent();
            viewModel = new CurrentGameViewModel();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Kills.Add(new Models.Kill("asd", true, DateTime.Now, 1));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await new Database().WriteDataAsync(viewModel.Kills);
            var x = await new Database().ReadDataAsync<Kill>();
            var s = new ObservableCollection<Kill>(x);
        }
    }
}
