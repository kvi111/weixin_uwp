using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace weixin_uwp.Controls
{
    public sealed partial class bubbleRightControl : UserControl
    {
        public bubbleRightControl()
        {
            this.InitializeComponent();

            this.PointerEntered += BubbleRightControl_PointerEntered;
            this.PointerExited += BubbleRightControl_PointerExited;
        }

        private void BubbleRightControl_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 146, 218, 97));
            rect1.Fill = brush;
            poly1.Fill = brush;
        }

        private void BubbleRightControl_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 158, 235, 107));
            rect1.Fill = brush;
            poly1.Fill = brush;
        }

        /// <summary>
        /// 根据文字的实际高度和宽度来设置控件的大小
        /// </summary>
        private void SetSize()
        {
            int widthMargin = 36, heightMargin = 18, widthMax = 306, widthMin = 40, heightMin = 40;
            //计算文字的实际高度和宽度
            this.textBlock1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //Size sizeText = this.textBlock1.DesiredSize;

            this.Height = this.textBlock1.ActualHeight + heightMargin; //sizeText.Height + heightMargin;
            this.Width = this.textBlock1.ActualWidth + widthMargin; //sizeText.Width + widthMargin;

            if (this.Width > widthMax)  //最宽306，超过就换行
            {
                this.Width = widthMax;
                this.textBlock1.TextWrapping = TextWrapping.Wrap;
                this.textBlock1.Width = this.Width - widthMargin; //超长设定宽度是关键，不然高度计算出来的是单行的高度，不是实际多行的高度

                this.textBlock1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //Size sizeText1 = this.textBlock1.DesiredSize;
                this.Height = this.textBlock1.ActualHeight + heightMargin;//sizeText1.Height + heightMargin;
                //this.textBlock1.TextTrimming = TextTrimming.WordEllipsis;
            }
            else if (this.Width < widthMin)
            {
                this.Width = widthMin;
            }

            if (this.Height < heightMin)
            {
                this.Height = heightMin;
            }

            poly1.Points.Clear();
            poly1.Points.Add(new Point() { X = this.Width, Y = 21 });
            poly1.Points.Add(new Point() { X = this.Width - 6, Y = 15 });
            poly1.Points.Add(new Point() { X = this.Width - 6, Y = 27 });
        }

        /// <summary>
        /// 文字内容
        /// </summary>
        public string Text
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.textBlock1.Text = "";
                    return;
                }

                this.textBlock1.Text = Utils.SetEmoji(value);

                SetSize();
            }
            get
            {
                return this.textBlock1.Text;
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public new double FontSize
        {
            set
            {
                this.textBlock1.FontSize = value;

                SetSize();
            }
            get
            {
                return this.textBlock1.FontSize;
            }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public new Brush Background
        {
            set
            {
                this.rect1.Fill = value;
                this.poly1.Fill = value;
            }
            get
            {
                return this.rect1.Fill;
            }
        }

        public new Brush Foreground
        {
            set
            {
                this.textBlock1.Foreground = value;
            }
            get
            {
                return this.textBlock1.Foreground;
            }
        }
    }
}
