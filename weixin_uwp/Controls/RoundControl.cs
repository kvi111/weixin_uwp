using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace weixin_uwp.Controls
{
    public sealed class RoundControl : Windows.UI.Xaml.Controls.Control
    {
        public RoundControl()
        {
            this.DefaultStyleKey = typeof(RoundControl);

            //this.Loaded += RoundControl_Loaded;
            this.Loading += RoundControl_Loading;
            //this.DataContextChanged += RoundControl_DataContextChanged;
        }

        //private void RoundControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        //{
        //    //throw new NotImplementedException();
        //}

        private void RoundControl_Loading(FrameworkElement sender, object args)
        {
            this.Height = 18; //固定高度
            this.FontSize = 12; //固定字号

            //this.Radius = this.Height / 2;
            SetValue(RadiusProperty, this.Height / 2);

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
                this.Width = GetWidth(Text);
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
        /// 根据文字得到控件的宽度
        /// </summary>
        /// <param name="txtValue"></param>
        /// <returns></returns>
        private double GetWidth(string txtValue)
        {
            if (string.IsNullOrEmpty(txtValue))
            {
                txtValue = "0";
            }
            double dbValueLen = (txtValue == "0" ? 0 : txtValue.ToString().Length);
            double dbWidth = this.Height + (dbValueLen <= 1 ? 0 : (dbValueLen - 1) * 6);
            return dbWidth;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RoundControl), new PropertyMetadata(default(string)));

        public string Text
        {
            set
            {
                SetValue(TextProperty, value);

                double intWidth = GetWidth(value);
                SetValue(WidthProperty, intWidth);
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
