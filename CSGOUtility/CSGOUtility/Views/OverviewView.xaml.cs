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
    /// Interaction logic for OverviewView.xaml
    /// </summary>
    public partial class OverviewView : UserControl
    {
        private OverviewViewModel viewModel;

        public OverviewView()
        {
            InitializeComponent();
        }

        private async void Grid_Initialized(object sender, EventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = new OverviewViewModel();
                await viewModel.Init();
                DataContext = viewModel;
                comboboxModes.SelectedIndex = 0;
            }
        }
    }
}
