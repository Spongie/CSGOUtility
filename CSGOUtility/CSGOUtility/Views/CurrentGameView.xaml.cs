using CSGOUtility.ViewModels;
using System;
using System.Collections.Generic;
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
    }
}
