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

namespace Cake_Launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();
        public static LauncherCore Core = LauncherCore.Create();

        public MainWindow()
        {
            InitializeComponent();
            var versions = Core.GetVersions().ToArray();
            versionCombo.ItemsSource = versions;
            versionCombo.DisplayMemberPath = "Id";
            List<String> javaList = new List<String>();
            foreach (string i in KMCCC.Tools.SystemTools.FindJava())
            {
                javaList.Add(i);
            }
            javaList.Add(tools.GetJavaPath());
            JavaPathCombo.ItemsSource = javaList;
            JavaPathCombo.SelectedItem = JavaPathCombo.Items[0];
            versionCombo.SelectedItem = versionCombo.Items[0];
        }
        
        public void OpenHelp()
        {

        }

        public void StartGame()
        {
            if (versionCombo.Text != string.Empty && JavaPathCombo.Text != string.Empty && Offline.OfflineID.Text != string.Empty && MemoryTextBox.Text != string.Empty)
            {
                try
                {
                    Core.JavaPath = (string)JavaPathCombo.SelectedItem;
                    var ver = (KMCCC.Launcher.Version)versionCombo.SelectedItem;
                    var result = Core.Launch(new LaunchOptions
                    {
                        Version = ver, 
                        MaxMemory = Convert.ToInt32(MemoryTextBox.Text), 
                        Authenticator = new OfflineAuthenticator(Offline.OfflineID.Text), //离线启动，ZhaiSoul那儿为你要设置的游戏名
                                                                                 //Authenticator = new YggdrasilLogin("邮箱", "密码", true), // 正版启动，最后一个为是否twitch登录
                        Mode = LaunchMode.MCLauncher, 
                                                      // Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                        Size = new WindowSize { Height = 768, Width = 1280 } 
                    });

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
    }
}

