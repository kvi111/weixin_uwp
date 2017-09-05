using Windows.Foundation;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace weixin_uwp.Controls
{
    public sealed partial class TimerTextControl : UserControl
    {
        public TimerTextControl()
        {
            this.InitializeComponent();
        }

        public TimerTextControl(string text)
        {
            this.InitializeComponent();
            this.Text = text;
        }

        /// <summary>
        /// 文字内容
        /// </summary>
        public string Text
        {
            set
            {
                this.textBlock1.Text = value;
                this.textBlock1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //Size sizeText1 = this.textBlock1.DesiredSize;
                this.Width = this.textBlock1.ActualWidth + 20;
            }
            get
            {
                return this.textBlock1.Text;
            }
        }

        public new double FontSize
        {
            set
            {
                this.textBlock1.FontSize = value;
                this.textBlock1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //Size sizeText1 = this.textBlock1.DesiredSize;
                this.Width = this.textBlock1.ActualWidth + 20;
                this.rect1.Height = this.textBlock1.ActualHeight + 6;
            }
            get
            {
                return this.textBlock1.FontSize;
            }
        }
    }
}
