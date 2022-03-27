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
using Cake_Launcher.LoginUI;
using SquareMinecraftLauncher;
using microsoft_launcher;
using SquareMinecraftLauncher.Minecraft;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Cake_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {

        LoginUI.Offline Offline = new LoginUI.Offline();
        LoginUI.Microsoft Microsoft = new LoginUI.Microsoft();
        LoginUI.Mojang Mojang = new LoginUI.Mojang();
        public int launchMode = 1;
        microsoft_launcher.MicrosoftAPIs microsoftAPIs = new microsoft_launcher.MicrosoftAPIs();
        SquareMinecraftLauncher.Minecraft.Game game = new SquareMinecraftLauncher.Minecraft.Game();
        SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();
        SquareMinecraftLauncher.MinecraftDownload minecraftDownload = new SquareMinecraftLauncher.MinecraftDownload();

        Setting setting = new Setting();

        string SettingPath = @"cakelauncher.json";

        public class Setting
        {
            public string RAM = "1024";
/// public string OfflineUser = Offline.UserID.Text;
        }

        public void LauncherInitialization()
        {
            if (!File.Exists(SettingPath))
            {
                File.WriteAllText(SettingPath, JsonConvert.SerializeObject(setting));
            }
            else
            {
                setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(SettingPath));        
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            LauncherInitialization();
            ServicePointManager.DefaultConnectionLimit = 512;
            var versions = tools.GetAllTheExistingVersion();
            versionCombo.ItemsSource = versions;
            JavaPathCombo.ItemsSource = tools.GetJavaPath();
            if (versionCombo.SelectedItem != null)
            versionCombo.SelectedItem = versionCombo.Items[0];
            if (JavaPathCombo.SelectedItem != null)
            JavaPathCombo.SelectedItem = JavaPathCombo.Items[0];
            MemoryTextBox.Text = setting.RAM;
        }
        #region
        //public void CompleteFile()
        //{
        //    try
        //    {
        //        tools.DownloadSourceInitialization(DownloadSource.bmclapiSource);
        //        var v = tools.GetMissingFile(versionCombo.SelectedValue.ToString());
        //        Gac.DownLoadFile downLoadFile = new Gac.DownLoadFile();
        //        foreach (var i in v)
        //        {
        //            downLoadFile.AddDown(i.Url, i.path);
        //        }
        //        downLoadFile.StartDown(0);
        //    }
        //    catch (Exception e)
        //    {
        //        System.Windows.MessageBox.Show(e.ToString(), "补全文件失败");
        //    }
        //}
        #endregion

        public async void GameStart()
        {
            try
            {
                if (startbutton.Content.ToString() == "确认启动吗？")
                {
                    startbutton.Content = "Stage 1 - 补全文件";
                    //CompleteFile();
                    if (versionCombo.Text != string.Empty &&
                        JavaPathCombo.Text != string.Empty &&
                        (Offline.UserID.Text != string.Empty || Mojang.MojangEmail.Text != string.Empty && Mojang.MojangPassword.Password != string.Empty &&
                        MemoryTextBox.Text != string.Empty))
                    {
                        switch (launchMode)
                        {
                            case 1:
                                startbutton.Content = "Stage 2 - 启动游戏";
                                await game.StartGame(versionCombo.Text, JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(MemoryTextBox.Text), Offline.UserID.Text);
                                break;
                            case 2:
                                startbutton.Content = "Stage 2 - Mojang 正版登录";
                                await game.StartGame(versionCombo.Text, JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(MemoryTextBox.Text), Mojang.MojangEmail.Text, Mojang.MojangPassword.Password);
                                break;
                            case 3:
                                startbutton.Content = "Stage 2 - 微软正版登录";
                                microsoft_launcher.MicrosoftAPIs microsoftAPIs = new microsoft_launcher.MicrosoftAPIs();
                                var v = Microsoft.browser.Source.ToString().Replace(microsoftAPIs.cutUri, string.Empty);
                                var t = Task.Run(() =>
                                {
                                    return microsoftAPIs.GetAccessTokenAsync(v, false).Result;
                                });
                                await t;
                                var v1 = microsoftAPIs.GetAllThings(t.Result.access_token, false);
                                startbutton.Content = "Stage 3 - 启动游戏";
                                await game.StartGame(versionCombo.Text, JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(MemoryTextBox.Text), v1.name, v1.uuid, v1.mcToken, string.Empty, string.Empty);
                                break;
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("信息未填完整", "错误");
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("启动失败" + e.Message, "错误");
            }
            finally
            {
                startbutton.Content = "确认启动吗？";
            }
        }


        private void ButtonClick_StartGame(object sender, RoutedEventArgs e)
        {
            GameStart();
        }

        private void TileClick_OpenHelp(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>微软正版
        private void TileClick_Microsoft(object sender, RoutedEventArgs e)
        {
            MetroNavigationWindow Microsoft = new MetroNavigationWindow();
            {
                Microsoft.Source = new Uri("/LoginUI/Microsoft.xaml", UriKind.Relative);
                Microsoft.Show();
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
        private void TileClick_OpenSettings(object sender, RoutedEventArgs e)
        {
            Page Settings = new Page();
            Settings.Content = "/Settings.xaml";
        }
    }
}

