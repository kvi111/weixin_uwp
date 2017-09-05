using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace weixin_uwp.Controls
{
    public sealed partial class EmojiControl : UserControl//, INotifyPropertyChanged
    {
        public EmojiControl()
        {
            this.InitializeComponent();

            //this.Loading += EmojiControl_Loading;
            //sp1.Loading += Sp1_Loading;
            //PropertyChanged += EmojiControl_PropertyChanged;
        }

        //private void EmojiControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        //private void Sp1_Loading(FrameworkElement sender, object args)
        //{
        //    //throw new NotImplementedException();
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected override void OnApplyTemplate()
        {
            //StackPanel sp1 = this.GetTemplateChild("stackPanel1") as Windows.UI.Xaml.Controls.StackPanel;
            SetEmoji(this.Text);
        }

        public void EmojiControl_Loading(FrameworkElement sender, object args)
        {
            //sp1.Height = this.Height;

            //SetEmoji();
        }

        public async Task SetEmoji(string strEmoji)
        {
            //return;
            if (string.IsNullOrEmpty(strEmoji)) return;

            //string emojiStr = "<span class=\"emoji emoji1f49d\"></span>图强一年级4班交流群<span class=\"emoji emoji1f33b\"></span>";
            //strEmoji = strEmoji.Replace("<span", "[span").Replace("></span>", "][/span]");
            string[] arrStr = strEmoji.Split(new string[] { "\"></span>", "<span class=\"" }, StringSplitOptions.RemoveEmptyEntries);
            //string[] arrStr = strEmoji.Split(new string[] { "\"][/span]", "[span class=\"" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in arrStr)
            {
                string str1 = str.Trim();
                if (string.IsNullOrEmpty(str1)) continue;
                if (str1.StartsWith("emoji emoji")) //表情
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-100.png"));
                    image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                    sp1.Children.Add(image);
                }
                else  //文字
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = str1;
                    tb.FontSize = this.FontSize;
                    tb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                    sp1.Children.Add(tb);
                }
            }
        }

        public string Text { set; get; }
    }
}
