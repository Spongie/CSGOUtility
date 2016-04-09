using CSGOUtility.ViewModels;
using System.Windows.Controls;

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
