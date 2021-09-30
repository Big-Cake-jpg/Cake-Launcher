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
using KMCCC.Launcher;
using KMCCC.Authentication;
using System.Windows.Forms;
using KMCCC.Tools;
using Cake_Launcher.LoginUI;
using SquareMinecraftLauncher;
using microsoft_launcher;

namespace Cake_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {

        LoginUI.Offline Offline = new LoginUI.Offline();
        LoginUI.Mojang Mojang = new LoginUI.Mojang();
        LoginUI.Microsoft Microsoft = new LoginUI.Microsoft();
        public int launchMode = 1;
        SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();
        public static LauncherCore Core = LauncherCore.Create();

        public MainWindow()
        {
            InitializeComponent();
            var versions = Core.GetVersions().ToArray();
            versionCombo.ItemsSource = versions;
            List<string> javaList = new List<string>();
            foreach(string i in KMCCC.Tools.SystemTools.FindJava())
            {
                javaList.Add(i);
            }
            javaList.Add(tools.GetJavaPath());
            JavaPathCombo.ItemsSource = javaList;
            JavaPathCombo.SelectedItem = JavaPathCombo.Items[0];
            versionCombo.SelectedItem = versionCombo.Items[0];
        }

        public async void StartGame()
        {
            LaunchOptions launchOptions = new LaunchOptions();
            switch (launchMode)
            {
                case 1:
                    launchOptions.Authenticator = new OfflineAuthenticator(Offline.UserID.Text);
                    break;
                case 2:
                    launchOptions.Authenticator = new YggdrasilLogin(Mojang.MojangEmail.Text, Mojang.MojangPassword.Password, false);
                    break;
            }

            launchOptions.MaxMemory = Convert.ToInt32(MemoryTextBox.Text);
            if (versionCombo.Text != string.Empty &&
                JavaPathCombo.Text != string.Empty &&
                (Offline.UserID.Text != string.Empty || (Mojang.MojangEmail.Text != string.Empty && Mojang.MojangPassword.Password != string.Empty) &&
                MemoryTextBox.Text != string.Empty))
            { 

                try
                {
                    if (launchMode != 3)
                    {
                    Core.JavaPath = (string)JavaPathCombo.SelectedItem;
                    var ver = (KMCCC.Launcher.Version)versionCombo.SelectedItem;
                    launchOptions.Version = ver;

                    var result = Core.Launch(launchOptions);
                    if (!result.Success)
                    {
                        //MessageBox.Show(result.ErrorMessage, result.ErrorType.ToString());
                        switch (result.ErrorType)
                        {
                            case ErrorType.NoJAVA:
                                System.Windows.MessageBox.Show("你的 Java 环境存在问题，可能你曾删除过系统内的 Java，请尝试重新安装 Java\n详细信息：" + result.ErrorMessage, "错误", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
                                break;
                            case ErrorType.AuthenticationFailed:
                                System.Windows.MessageBox.Show(this, "正版验证失败！请检查你的账号密码", "账号错误\n详细信息：" + result.ErrorMessage, (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
                                break;
                            case ErrorType.UncompressingFailed:
                                System.Windows.MessageBox.Show(this, "可能的多开或文件损坏，请确认文件完整且不要多开\n如果你不是多开游戏的话，请检查libraries文件夹是否完整\n详细信息：" + result.ErrorMessage, "可能的多开或文件损坏", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
                                break;
                            default:
                                System.Windows.MessageBox.Show(this,
                                    result.ErrorMessage + "\n" +
                                    (result.Exception == null ? string.Empty : result.Exception.StackTrace),
                                    "启动错误，请将此窗口截图向开发者寻求帮助");
                                break;
                        }
                    }
                    }
                    else
                    {
                        microsoft_launcher.MicrosoftAPIs microsoftAPIs = new microsoft_launcher.MicrosoftAPIs();
                        var v = Microsoft.browser.Source.ToString().Replace(microsoftAPIs.cutUri,string.Empty);
                        var t = Task.Run(() =>{
                            return microsoftAPIs.GetAccessTokenAsync(v,false).Result;
                        });
                        await t;
                        var v1 = microsoftAPIs.GetAllThings(t.Result.access_token, false);
                        SquareMinecraftLauncher.Minecraft.Game game = new SquareMinecraftLauncher.Minecraft.Game();
                        await game.StartGame(versionCombo.Text, JavaPathCombo.Text, Convert.ToInt32(MemoryTextBox.Text), v1.name, v1.uuid, v1.mcToken, string.Empty, string.Empty);
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

