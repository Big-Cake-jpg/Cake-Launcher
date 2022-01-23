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
using System.Windows.Forms;
using KMCCC.Tools;
using Cake_Launcher.LoginUI;
using SquareMinecraftLauncher;

namespace Cake_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {

        Offline Offline = new Offline();
        Mojang Mojang = new Mojang();
        LoginUI.Microsoft Microsoft = new LoginUI.Microsoft();
        SquareMinecraftLauncher.Minecraft.Game game = new SquareMinecraftLauncher.Minecraft.Game();
        public int launchMode = 1;
        SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();

        public MainWindow()
        {
            InitializeComponent();
            var versions = tools.GetAllTheExistingVersion();
            versionCombo.ItemsSource = versions;
            List<string> javaList = new List<string>();
            javaList.Add(tools.GetJavaPath());
            JavaPathCombo.ItemsSource = javaList;
            JavaPathCombo.SelectedItem = JavaPathCombo.Items[0];
            versionCombo.SelectedItem = versionCombo.Items[0];
        }

        public async void StartGame()
            switch (launchMode)
            {
                case 1:
                    await game.StartGame();
                    break
            }

            
            if (versionCombo.Text != string.Empty &&
                JavaPathCombo.Text != string.Empty &&
                (Offline.UserID.Text != string.Empty || (Mojang.MojangEmail.Text != string.Empty && Mojang.MojangPassword.Password != string.Empty) &&
                MemoryTextBox.Text != string.Empty))
            { 

                try
                {
                    
                    
                    
                    {
                        microsoft_launcher.MicrosoftAPIs microsoftAPIs = new microsoft_launcher.MicrosoftAPIs();
                        var v = Microsoft.browser.Source.ToString().Replace(microsoftAPIs.cutUri, string.Empty);
                        var t = Task.Run(() =>
                        {
                            return microsoftAPIs.GetAccessTokenAsync(v, false).Result;
                        });
                        await t;
                        var v1 = microsoftAPIs.GetAllThings(t.Result.access_token, false);
                        
                        
                    }

                }
                catch
                {
                    System.Windows.MessageBox.Show("启动失败", "错误");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("信息未填完整", "错误");
            }
        }
        private void ButtonClick_StartGame(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void TileClick_OpenHelp(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>微软正版
        private void TileClick_Microsoft(object sender, RoutedEventArgs e)
        {
            MetroNavigationWindow window = new MetroNavigationWindow();
            {
                window.Source = new Uri("/LoginUI/Microsoft.xaml", UriKind.Relative);
                window.Show();
            };
            launchMode = 3;
        }

        /// <summary>离线登录
        private void TileClick_Offline(object sender, RoutedEventArgs e)
        {
            LoginContent.Content = new Frame
            {
                Content = Offline
            };
            launchMode = 1;
        }

        /// <summary>Mojang 正版
        private void TileClick_Mojang(object sender, RoutedEventArgs e)
        {
            LoginContent.Content = new Frame
            {
                Content = Mojang
            };
            launchMode = 2;
        }
    }
}

