using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task Download()
        {
            var client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            byte[] file = await client.DownloadDataTaskAsync(@"https://dl.dropboxusercontent.com/u/86487959/MagicDrafter/setup.exe");

        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            

        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await Download();
        }
    }
}
