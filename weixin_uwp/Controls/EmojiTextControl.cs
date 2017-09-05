using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace weixin_uwp.Controls
{
    //[TemplatePart(Name = "stackPanel1", Type = typeof(StackPanel))]
    [TemplatePart(Name = "textBlock1", Type = typeof(TextBlock))]
    public sealed class EmojiTextControl : Control
    {
        public EmojiTextControl()
        {
            this.DefaultStyleKey = typeof(EmojiTextControl);

            //this.Loading += EmojiTextControl_Loading;


        }

        protected override void OnApplyTemplate()
        {
            if (Foreground == null)
            {
                Foreground = new SolidColorBrush(Colors.Black);// { Color = new Windows.UI.Color() { A = 255, R = 0, G = 0, B = 0 } };
            }
            SetEmoji((string)GetValue(TextProperty));
        }

        //private void EmojiTextControl_Loading(FrameworkElement sender, object args)
        //{

        //}

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EmojiTextControl), new PropertyMetadata(default(string)));

        public string Text
        {
            set
            {
                SetValue(TextProperty, value);
                SetEmoji(value);
            }
            get
            {
                return (string)GetValue(TextProperty);
            }
        }

        //public new static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(EmojiTextControl), new PropertyMetadata(default(Brush)));

        //public new Brush Foreground
        //{
        //    set
        //    {
        //        SetValue(ForegroundProperty, value);
        //    }
        //    get
        //    {
        //        return (Brush)GetValue(ForegroundProperty);
        //    }
        //}

        public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(EmojiTextControl), new PropertyMetadata(default(double)));

        public new double Width
        {
            set
            {
                SetValue(WidthProperty, value);
            }
            get
            {
                return (double)GetValue(WidthProperty);
            }
        }
        /// <summary>
        /// 设置文本(注意：虽然此方法内部没有await, 但却使用了async Task，这个不能去掉，否则会在ChatListControl里面给Text赋值的时候报错！！！)
        /// </summary>
        /// <param name="strEmoji"></param>
        /// <returns></returns>
        public async Task SetEmoji(string strEmoji)
        {
            TextBlock tb = this.GetTemplateChild("textBlock1") as Windows.UI.Xaml.Controls.TextBlock;
            tb.Text = "";
            if (string.IsNullOrEmpty(strEmoji)) return;

            //string emojiStr = "<span class=\"emoji emoji1f49d\"></span>图强一年级4班交流群<span class=\"emoji emoji1f33b\"></span>";
            tb.Text = Utils.SetEmoji(strEmoji);
        }
    }
}
