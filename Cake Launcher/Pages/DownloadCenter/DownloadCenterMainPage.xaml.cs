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
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace Cake_Launcher.Pages.DownloadCenter
{
    /// <summary>
    /// DownloadCenterMainPage.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadCenterMainPage : Page
    {
        Pages.DownloadCenter.ResourcesView ResourcesView = new Pages.DownloadCenter.ResourcesView();
        public DownloadCenterMainPage()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);//设置的间隔为一分钟
            timer.IsEnabled = true;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {

        }
        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "下载中心暂未制作完成，敬请期待";
                    break;
                case 1:
                    flipview.BannerText = "CurseForge 上热门 —— Just Enough Items";
                    break;
                case 2:
                    flipview.BannerText = "隆重向您推荐：Sodium";
                    break;
            }
        }
        private void TileClick_ModDownload(object sender, RoutedEventArgs e)
        {
            DownloadContent.Content = new Frame
            {
                Content = ResourcesView
            };
        }
    }
}
