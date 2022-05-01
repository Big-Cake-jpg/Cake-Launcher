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
using MahApps.Metro.Controls;

namespace Cake_Launcher.Pages.DownloadCenter
{
    /// <summary>
    /// DownloadCenterMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadCenterMainPage : Page
    {
        public DownloadCenterMainPage()
        {
            InitializeComponent();
        }
        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "Cupcakes!";
                    break;
                case 1:
                    flipview.BannerText = "Xbox!";
                    break;
                case 2:
                    flipview.BannerText = "Chess!";
                    break;
            }
        }
    }
}
