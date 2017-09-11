using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace weixin_uwp.Controls
{
    [TemplatePart(Name = "textBlock11", Type = typeof(TextBlock))]
    //[TemplatePart(Name = "rectangle1", Type = typeof(Rectangle))]
    public sealed class RoundControl : Windows.UI.Xaml.Controls.Control
    {

        public RoundControl()
        {
            this.DefaultStyleKey = typeof(RoundControl);

            this.Loaded += RoundControl_Loaded;
        }

        private void RoundControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.FontSize = 15; //固定字号

            if (this.Background == null)
            {
                SolidColorBrush scb = new SolidColorBrush();
                scb.Color = Color.FromArgb(255, 244, 84, 84);
                this.Background = scb;
            }

            //SolidColorBrush soCoBr = (SolidColorBrush)GetValue(ForegroundProperty);
            //if (this.Foreground == null)
            //{
            //    SolidColorBrush scb = new SolidColorBrush();
            //    scb.Color = Color.FromArgb(255, 0, 0, 0);
            //    this.Foreground = scb;
            //}

            if (double.IsNaN(this.Width) || this.Width == 0)
            {
                this.Width = GetSize(Text).Width;
                this.Height = GetSize(Text).Height;
                SetValue(RadiusProperty, this.Height / 2);
            }
            if (Text == "0" || string.IsNullOrEmpty(Text))
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 根据文字得到控件的宽度和高度
        /// </summary>
        /// <param name="txtValue"></param>
        /// <returns></returns>
        private Size GetSize(string txtValue)
        {
            TextBlock tb = new TextBlock();
            if (string.IsNullOrEmpty(txtValue))
            {
                txtValue = "0";
            }
            tb.Text = txtValue;
            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));//可以得到tb.ActualWidth，tb.ActualHeight为文本的实际宽高

            double perLen = tb.ActualWidth / tb.Text.Length; //每个字符的宽度
            double actualWidth = tb.ActualHeight;
            if (tb.Text.Length > 1)
            {
                actualWidth = tb.ActualHeight + (tb.Text.Length - 1) * perLen;
            }
            if (actualWidth < tb.ActualHeight)
            {
                actualWidth = tb.ActualHeight;
            }
            return new Size(actualWidth, tb.ActualHeight);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RoundControl), new PropertyMetadata(default(string)));

        public string Text
        {
            set
            {
                SetValue(TextProperty, value);

                Size size = GetSize(value);

                this.Height = size.Height;
                SetValue(RadiusProperty, size.Height / 2);
                SetValue(WidthProperty, size.Width);
            }
            get
            {
                return (string)GetValue(TextProperty);
            }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(RoundControl), new PropertyMetadata(default(double)));

        public double Radius
        {
            set
            {
                SetValue(RadiusProperty, value);
            }
            get
            {
                return (double)GetValue(RadiusProperty);
            }
        }
    }
}
