using CSGOUtility.Models;
using CSGOUtility.ViewModels;
using System;
using System.Windows.Controls;

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
                comboboxModes.SelectedIndex = (int)GameModes.Competitive;
            }
        }
    }
}
