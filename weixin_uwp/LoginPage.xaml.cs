using Windows.UI.Xaml.Controls;
using Windows.System.Threading;
using System.Threading.Tasks;
using System;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace weixin_uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public static LoginPage instance;
        public ImageSource userImageSource;//当前用户头像
        public StartUI startUI;
        public Task t;

        public LoginPage()
        {
            this.InitializeComponent();

            //修改标题栏颜色
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            Windows.UI.Color color = new Windows.UI.Color();
            color.R = 245;
            color.G = 245;
            color.B = 245;
            //titleBar.BackgroundColor = color;
            titleBar.ButtonBackgroundColor = color;

            instance = this;
            startUI = new StartUI();

            //IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
            //async (workItem) =>
            //{
            //    await startUI.start();
            //});

        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await startUI.start();
        }
        public async Task SetTip(string msg,bool isShowDialog = false)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            CoreDispatcherPriority.High,
            new DispatchedHandler(() =>
            {
                this.textTip.Text = msg;
            }));

            if(isShowDialog)
            {
                await LoginErr(msg);
            }
        }

        public async Task SetQRImage(Byte[] data)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            CoreDispatcherPriority.High,
            new DispatchedHandler(async () =>
            {
                image1.Source = await Utils.BytesToImage(data);
                userImageSource = image1.Source;
            }));
        }

        public async Task GoMainPage(StartUI startUI) //JArray sessionList
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            CoreDispatcherPriority.High,
            new DispatchedHandler(() =>
            {
                this.Frame.Navigate(typeof(MainPage), startUI);//sessionList
            }));
        }

        public async Task LoginErr(string msg)
        {
            ContentDialog errDialog = new ContentDialog()
            {
                Title = "登录异常",
                Content = msg,
                PrimaryButtonText = "Ok"
            };

            ContentDialogResult result = await errDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }
        }
    }
}
