using Cake_Launcher.LoginUI;
using Cake_Launcher.Pages;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        LoginUI.Authlib_Injector AuthlibInjector = new LoginUI.Authlib_Injector();
        Pages.GameSettings gamesettings = new Pages.GameSettings();
        Pages.Settings.SettingsMainPage settingsmain = new Pages.Settings.SettingsMainPage();
        public int launchMode = 1;
        microsoft_launcher.MicrosoftAPIs microsoftAPIs = new microsoft_launcher.MicrosoftAPIs();
        SquareMinecraftLauncher.Minecraft.Game game = new SquareMinecraftLauncher.Minecraft.Game();
        SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();
        SquareMinecraftLauncher.MinecraftDownload minecraftDownload = new SquareMinecraftLauncher.MinecraftDownload();
        Setting setting = new Setting();
        string SettingPath = @"cakelauncher.json";
        RegisterSetting registerSetting = new RegisterSetting();
        public class Setting
        {
            public string RAM = "1024";
        }
        public class RegisterSetting
        {
            public string name = "Big_Cake";
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
            bool isFirst = true;
            using (RegistryKey Key1 = Registry.CurrentUser.OpenSubKey("SOFTWARE"))
            {
                foreach (var i in Key1.GetSubKeyNames())
                {
                    if (i == "Cake Launcher")
                    {
                        isFirst = false;
                    }
                }
            }
            if (isFirst)
            {
                using (RegistryKey key = Registry.CurrentUser)
                {
                    using (RegistryKey software = key.CreateSubKey("Software\\Cake Launcher"))
                    {
                        software.SetValue("name", registerSetting.name);
                    }
                }
            }
            else
            {
                using (RegistryKey key = Registry.CurrentUser)
                {
                    using (RegistryKey software = key.CreateSubKey("Software\\Cake Launcher"))
                    {
                        registerSetting.name = software.GetValue("name").ToString();
                    }
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            LauncherInitialization();
            ServicePointManager.DefaultConnectionLimit = 512;
            var versions = tools.GetAllTheExistingVersion();
            gamesettings.versionCombo.ItemsSource = versions;
            gamesettings.JavaPathCombo.ItemsSource = tools.GetJavaPath();
            if (gamesettings.versionCombo.Items.Count != 0)
                gamesettings.versionCombo.SelectedItem = gamesettings.versionCombo.Items[0];
            if (gamesettings.JavaPathCombo.Items.Count != 0)
                gamesettings.JavaPathCombo.SelectedItem = gamesettings.JavaPathCombo.Items[0];
            gamesettings.MemoryTextBox.Text = setting.RAM;
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
                    if (gamesettings.versionCombo.Text != string.Empty &&
                        gamesettings.JavaPathCombo.Text != string.Empty &&
                        (Offline.UserID.Text != string.Empty || Mojang.MojangEmail.Text != string.Empty && Mojang.MojangPassword.Password != string.Empty &&
                        gamesettings.MemoryTextBox.Text != string.Empty))
                    {
                        switch (launchMode)
                        {
                            case 1:
                                startbutton.Content = "Stage 2 - 启动游戏";
                                await game.StartGame(gamesettings.versionCombo.Text, gamesettings.JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(gamesettings.MemoryTextBox.Text), Offline.UserID.Text);
                                break;
                            case 2:
                                startbutton.Content = "Stage 2 - Mojang 正版登录";
                                await game.StartGame(gamesettings.versionCombo.Text, gamesettings.JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(gamesettings.MemoryTextBox.Text), Mojang.MojangEmail.Text, Mojang.MojangPassword.Password);
                                startbutton.Content = "Stage 3 - 启动游戏";
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
                                await game.StartGame(gamesettings.versionCombo.Text, gamesettings.JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(gamesettings.MemoryTextBox.Text), v1.name, v1.uuid, v1.mcToken, string.Empty, string.Empty);
                                break;
                            case 4:
                                startbutton.Content = "Stage 2 - 登录";
                                await game.StartGame(gamesettings.versionCombo.Text, gamesettings.JavaPathCombo.SelectedValue.ToString(), Convert.ToInt32(gamesettings.MemoryTextBox.Text), AuthlibInjector.authlibInjectorAvatarChoose.ChooseAvatar.Text, AuthlibInjector.authlibInjectorAvatarChoose.ChooseAvatar.SelectedValue.ToString(), AuthlibInjector.skin.accessToken, string.Empty, string.Empty);
                                startbutton.Content = "Stage 3 - 启动游戏";
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
            LoginContent.Content = new Frame
            {
                Content = Microsoft
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
        /// <summary>Authlib-Injector 外置登录
        private void TileClick_AuthlibInjector(object sender, RoutedEventArgs e)
        {
            LoginContent.Content = new Frame
            {
                Content = AuthlibInjector
            };
            launchMode = 4;
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
        private void MemoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.RAM = gamesettings.MemoryTextBox.Text;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            File.WriteAllText(SettingPath, JsonConvert.SerializeObject(setting));
        }
        private void TileClick_NeverGonnaGiveYouUp(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.bilibili.com/video/BV1va411w7aM/");
        }
        private void TileClick_GameSettings(object sender, RoutedEventArgs e)
        {
            PageContent.Content = new Frame
            {
                Content = gamesettings
            };
        }
        private void ClosePages(object sender, RoutedEventArgs e)
        {
            PageContent.Content = new Frame
            {
                Content = new Frame()
            };
        }
        private void FeedBack(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Big-Cake-jpg/Cake-Launcher/issues/new");
        }
        private void TileClick_OpenSettings(object sender, RoutedEventArgs e)
        {
            PageContent.Content = new Frame
            {
                Content = settingsmain
            };
        }
        private void TileClick_CloseLoginUI(object sender, RoutedEventArgs e)
        {
            LoginContent.Content = new Frame
            {
                Content = new Frame()
            };
        }
    }
}

