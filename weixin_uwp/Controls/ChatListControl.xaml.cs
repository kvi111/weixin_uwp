using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using weixin_uwp.Common.model;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace weixin_uwp.Controls
{
    public sealed partial class ChatListControl : UserControl
    {
        private double WIDTH = 40;
        public ChatListControl()
        {
            this.InitializeComponent();
        }

        protected override void OnApplyTemplate()
        {

        }

        //public ChatListControl(ChatMessage chatList)
        //{
        //    this.InitializeComponent();
        //    this.Append(chatList);
        //}
        //public ChatListControl(List<ChatMessage> chatLists)
        //{
        //    this.InitializeComponent();
        //    this.Append(chatLists);
        //}

        public void Clear()
        {
            Items = new List<ChatMessage>();
            sp1.Children.Clear();
        }

        public async Task Append(List<ChatMessage> chatMsgList)
        {
            if (chatMsgList == null) return;

            foreach (ChatMessage chatMsg in chatMsgList)
            {
                await Append(chatMsg);
            }
        }
        public async Task Append(ChatMessage chatMsg)
        {
            if (chatMsg == null) return;
            if (Items == null)
            {
                Items = new List<ChatMessage>();
            }
            Items.Add(chatMsg);

            if (Items.Count == 1 || (Items.Count >= 2 && (chatMsg.dateTime - Items[Items.Count - 2].dateTime) > new TimeSpan(0, 5, 0)))
            {
                //两次会话小于5分钟才显示时间
                TimerTextControl timerText = new TimerTextControl(chatMsg.dateTime.ToString("HH:mm"));
                timerText.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                timerText.FontSize = 12;
                sp1.Children.Add(timerText);
            }

            //添加聊天内容
            StackPanel stackPanelHorizontal = new StackPanel();
            stackPanelHorizontal.Orientation = Orientation.Horizontal;
            //sp.BorderBrush = new SolidColorBrush(Colors.Red);
            //sp.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            Image image = new Image();
            image.Width = WIDTH;
            BitmapImage bimage = await chatMsg.From.GetHeadImage();
            if (bimage != null)
            {
                image.Source = bimage;//new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-100.png"));
                image.Height = GetHeight(bimage.PixelHeight, bimage.PixelWidth);
            }
            else
            {
                image.Height = 40;
            }
            double topMargin = (WIDTH - image.Height) / 2;
            if (chatMsg.isMine)
            {
                stackPanelHorizontal.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;

                //右气泡文字控件
                bubbleRightControl bubbleRight = new bubbleRightControl();
                bubbleRight.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                bubbleRight.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                bubbleRight.Margin = new Windows.UI.Xaml.Thickness(0, 16, 0, 16);
                bubbleRight.Text = chatMsg.message;
                stackPanelHorizontal.Children.Add(bubbleRight);

                //头像图片
                image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                image.Margin = new Windows.UI.Xaml.Thickness(10, 16 + topMargin, 30, 16);
                //image.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                stackPanelHorizontal.Children.Add(image);

                sp1.Children.Add(stackPanelHorizontal);
            }
            else
            {
                stackPanelHorizontal.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;

                //头像图片
                image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                image.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                image.Margin = new Windows.UI.Xaml.Thickness(30, 16 + topMargin, 10, 16);
                //image.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                stackPanelHorizontal.Children.Add(image);


                //左气泡文字控件
                bubbleLeftControl bubbleLeft = new bubbleLeftControl();
                bubbleLeft.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                bubbleLeft.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                bubbleLeft.Text = chatMsg.message;

                double bubbleLeftTopMargin = 0;
                if (chatMsg.isGroup) //如果是群消息且不是本人发的，就需要显示他的名称，气泡控件下移，留出地方显示名称
                {
                    StackPanel stackPanelVertical = new StackPanel();
                    stackPanelVertical.Orientation = Orientation.Vertical;

                    bubbleLeftTopMargin = WIDTH / 2;
                    //显示名称
                    EmojiTextControl emojiTextControl = new EmojiTextControl();
                    emojiTextControl.FontSize = 12;
                    emojiTextControl.Foreground = new SolidColorBrush() { Color = new Windows.UI.Color() { A = 255, R = 152, G = 152, B = 152 } };
                    emojiTextControl.Text = chatMsg.From.Name;
                    emojiTextControl.Margin = new Windows.UI.Xaml.Thickness(8, 16, 0, 0);
                    stackPanelVertical.Children.Add(emojiTextControl);

                    bubbleLeft.Margin = new Windows.UI.Xaml.Thickness(0, 3, 0, 16);
                    stackPanelVertical.Children.Add(bubbleLeft);

                    stackPanelHorizontal.Children.Add(stackPanelVertical);
                }
                else
                {
                    bubbleLeft.Margin = new Windows.UI.Xaml.Thickness(0, 16, 0, 16);
                    stackPanelHorizontal.Children.Add(bubbleLeft);
                }

                sp1.Children.Add(stackPanelHorizontal);
            }
            stackPanelHorizontal.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            scrolls.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            scrolls.ChangeView(null, sp1.ActualHeight + stackPanelHorizontal.DesiredSize.Height, null);
            //scrolls.ChangeView(null, sp1.ActualHeight + stackPanelHorizontal.ActualHeight, null);
        }
        private double GetHeight(double height, double width)
        {
            return WIDTH * height / width;
        }

        private List<ChatMessage> Items
        {
            set; get;
        }

        private ChatMessage GetLastOne()
        {
            if (Items != null && Items.Count > 0)
            {
                return Items[Items.Count - 1];
            }
            return null;
        }
    }
}
