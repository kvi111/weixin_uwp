using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using weixin_uwp.Common.model;
using weixin_uwp.Controls;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace weixin_uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string str = "尔塞飞[呲牙]而啊手动阀<span class=\"emoji emoji1f639\"></span>啊手动阀";
            textInput1.Text = Utils.SetEmoji(str);
        }
    }
}
