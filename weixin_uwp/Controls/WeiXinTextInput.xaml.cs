using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace weixin_uwp.Controls
{
    public sealed partial class WeiXinTextInput : UserControl
    {
        public event RoutedEventHandler SendClick;
        public WeiXinTextInput()
        {
            this.InitializeComponent();

            button1.Click += (sender, e) =>
            {
                if (SendClick != null)
                {
                    SendClick.Invoke(sender, e);
                }
            };

            button1.PointerEntered += Button1_PointerEntered;
            button1.PointerExited += Button1_PointerExited; ;

            emojiImage1.PointerEntered += EmojiImage1_PointerEntered;
            emojiImage1.PointerExited += EmojiImage1_PointerExited;
            emojiImage1.PointerPressed += EmojiImage1_PointerPressed;
        }

        private void Button1_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            button1.Background = new SolidColorBrush() { Color = new Windows.UI.Color() { A = 255, R = 236, G = 236, B = 236 } };
        }

        private void Button1_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            button1.Background = new SolidColorBrush(Colors.White);
        }

        private void EmojiImage1_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            emojiImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/pc/Chat_Expression_Icon_Click.scale-200.png"));
        }
        private void EmojiImage1_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            emojiImage1.Source = new BitmapImage(new Uri("ms-appx:///Assets/pc/Chat_Expression_Icon.scale-200.png"));
        }

        private void EmojiImage1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        public string Text
        {
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                this.textBox1.Text = Utils.SetEmoji(value);
            }
            get
            {
                return this.textBox1.Text;
            }
        }
    }
}
