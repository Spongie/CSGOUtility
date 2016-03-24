using CSGOUtility.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace CSGOUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
            CSGOEventListener.Instance.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CSGOEventListener.Instance.Stop();
        }
    }
}
