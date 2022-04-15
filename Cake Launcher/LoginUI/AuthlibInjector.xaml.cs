using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cake_Launcher.LoginUI
{
    /// <summary>
    /// Authlib_Injector.xaml 的交互逻辑
    /// </summary>
    public partial class Authlib_Injector : Page
    {
        public AuthlibInjectorAvatarChoose authlibInjectorAvatarChoose = new AuthlibInjectorAvatarChoose();
        public SquareMinecraftLauncher.Minecraft.Skin skin = new SquareMinecraftLauncher.Minecraft.Skin();
        public Authlib_Injector()
        {
            InitializeComponent();
        }
        private async void ButtonClick_LoginToAPIRoot(object sender, RoutedEventArgs e)
        {
            LoginProgress.Visibility = Visibility.Visible;
            SquareMinecraftLauncher.Minecraft.Tools tools = new SquareMinecraftLauncher.Minecraft.Tools();
            ///var t = Task.Run(() =>
            ///{
            ///return tools.GetAuthlib_Injector(APIRoot.Text, Username.Text, Password.Password);
            ///});
            ///await t;
            skin = tools.GetAuthlib_Injector(APIRoot.Text, Username.Text, Password.Password);
            Content = new Frame() { Content = authlibInjectorAvatarChoose };
            authlibInjectorAvatarChoose.ChooseAvatar.ItemsSource = skin.NameItem;
        }
    }
}
