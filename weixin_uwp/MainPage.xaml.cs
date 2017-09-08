using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using weixin_uwp.Common.model;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace weixin_uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region var
        public static MainPage instance;
        StartUI startUI;

        /// <summary>
        /// 当前对话对象的UserName
        /// </summary>
        ObjectBase selectObjectBase = new ObjectBase();

        Dictionary<string, List<ChatMessage>> dictChatList = new Dictionary<string, List<ChatMessage>>();//聊天信息

        ObservableCollection<Conversation> ocSessionInfo = new ObservableCollection<Conversation>();
        //SortedList<string, Conversation> listSession = new SortedList<string, Conversation>(); //会话列表

        ObservableCollection<Conversation> ocContactInfo = new ObservableCollection<Conversation>();
        //SortedList<string, Conversation> listContact = new SortedList<string, Conversation>(); //联系人列表
        #endregion

        #region Init
        public MainPage()
        {
            this.InitializeComponent();

            instance = this;

            //将应用界面扩展至 Titlebar 区域
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //修改标题栏颜色
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            Windows.UI.Color color = new Windows.UI.Color();
            color.R = 245;
            color.G = 245;
            color.B = 245;
            //titleBar.BackgroundColor = color;
            titleBar.ButtonBackgroundColor = color;

            //自定义标题栏控件：
            //可以将界面中任意控件指定为标题栏，指定后该控件将具有标题栏的行为特性，如拖动窗口、右键弹出窗口操作菜单等。
            //Window.Current.SetTitleBar(grid);

            //CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //CoreDispatcherPriority.High,
            //new DispatchedHandler(() =>
            //{
            //    btnUserImage.Source = LoginPage.instance.userImageSource;
            //    btnSessionList.Focus(FocusState.Pointer);
            //}));

            textBoxInput.SendClick += TextBoxInput_SendClick;
        }



        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null) return;

            startUI = (StartUI)e.Parameter;

            //foreach (JObject contact in startUI.memberList)
            //{
            //    Conversation userInfo = new Conversation();
            //    userInfo.FromObj.Name = ((JValue)contact["NickName"]).Value.ToString();
            //    userInfo.subHeading = "";
            //    userInfo.FromObj.UserName = ((JValue)contact["UserName"]).Value.ToString();

            //    if (listContact.Keys.Contains(userInfo.FromObj.Name) == false)
            //    {
            //        listContact.Add(userInfo.FromObj.Name, userInfo);
            //    }
            //    if (dictUserInfo.ContainsKey(userInfo.FromObj.UserName) == false)
            //    {
            //        //userInfo.headImgBytes = await LoginPage.instance.startUI.GetHeadImg(userInfo.userName);
            //        //userInfo.headImg = await Utils.ByteArrayToBitmapImage(userInfo.headImgBytes);

            //        dictUserInfo.Add(userInfo.FromObj.UserName, userInfo);
            //    }
            //}
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            btnUserImage.Source = LoginPage.instance.userImageSource;
            btnSessionList.Focus(FocusState.Pointer);
            this.contactList1.Visibility = Visibility.Collapsed; //隐藏

            //session列表数据绑定
            this.sessionList1.ItemsSource = null;
            this.sessionList1.ItemsSource = ocSessionInfo;
            foreach (ObjectBase uInfo in startUI.sessionList.Values)
            {
                if (ocSessionInfo.Count(a => a.FromObj.UserName == uInfo.UserName) <= 0)
                {
                    await uInfo.GetHeadImage();
                    ocSessionInfo.Add(new Conversation() { FromObj = uInfo, unReadMsgCount = 0 });
                }
            }

            //联系人列表数据绑定
            //this.contactList1.ItemsSource = null;
            //this.contactList1.ItemsSource = ocContactInfo;
            //foreach (UserInfo uInfo in listContact.Values)
            //{
            //    byte[] bytes = await LoginPage.instance.startUI.GetHeadImg(uInfo.userName);
            //    uInfo.headImg = await Utils.ByteArrayToBitmapImage(bytes);
            //    ocContactInfo.Add(uInfo);
            //}
        }
        #endregion

        #region Message
        /// <summary>
        /// 在会话列表设置聊天新消息和未读消息数量
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isSetUnReadNumber">是否设置未读消息数量（当前会话对象不需要设置）</param>
        public async Task SetNewMessageNumber(ChatMessage msg, bool isSetUnReadNumber)
        {
            try
            {
                string sessionUserName = "";
                if (msg.isGroup)
                {
                    sessionUserName = msg.To.UserName;
                }
                else
                {
                    sessionUserName = msg.From.UserName != startUI.loginUser.UserName ? msg.From.UserName : msg.To.UserName;
                }
                bool isFind = false;
                for (int i = 0; i < ocSessionInfo.Count; i++)
                {
                    if (ocSessionInfo[i].FromObj.UserName == sessionUserName)
                    {
                        ocSessionInfo[i].subHeading = msg.isGroup ? msg.From.Name + ":" + msg.message : msg.message;
                        msg.From.HeadImage = msg.isGroup ? msg.From.HeadImage : ocSessionInfo[i].FromObj.HeadImage; //await Utils.ByteArrayToBitmapImage(ocSessionInfo[i].headImgBytes);
                        if (isSetUnReadNumber)
                        {
                            ocSessionInfo[i].unReadMsgCount += 1;
                        }
                        ocSessionInfo.Move(i, 0);
                        //sessionList1.Items.VectorChanged += Items_VectorChanged;
                        //sessionList1.UpdateLayout();
                        //sessionList1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        isFind = true;
                        break;
                    }
                }

                if (isFind == false) //如果会话列表没有找到，就去联系人列表去找
                {
                    //if (msg.isGroup)
                    //{
                    //    startUI.getGroupById(fromUserName);
                    //}
                    Conversation conv = new Conversation();
                    conv.FromObj = sessionUserName != msg.From.UserName ? msg.To : msg.From;
                    await conv.FromObj.GetHeadImage();//确保头像出来
                    conv.subHeading = msg.isGroup ? msg.From.Name + ":" + msg.message : msg.message;
                    conv.unReadMsgCount = 1;
                    ocSessionInfo.Insert(0, conv);
                }
            }
            catch (Exception ex)
            {
                startUI.WriteLog("SetNewMessageNumber 异常：{0}", ex.Message);
            }
        }

        public async Task AddMessage(ChatMessage msg)
        {
            string fromUserName = msg.isGroup ? msg.To.UserName : msg.From.UserName;
            if (isCurr(fromUserName)) //发来消息是否来自于当前聊天窗口用户
            {
                await chatlist1.Append(msg);
                await SetNewMessageNumber(msg, false);
            }
            else
            {
                await SetNewMessageNumber(msg, true);
            }
            SaveChatMessage(msg);
        }

        /// <summary>
        /// 保存聊天记录
        /// </summary>
        /// <param name="msg"></param>
        public void SaveChatMessage(ChatMessage msg)
        {
            //string fromUserName = msg.isGroup ? msg.To.UserName : msg.From.UserName;
            string sessionUserName = "";
            if (msg.isGroup)
            {
                sessionUserName = msg.To.UserName;
            }
            else
            {
                sessionUserName = msg.From.UserName != startUI.loginUser.UserName ? msg.From.UserName : msg.To.UserName;
            }
            //string key = msg.From.UserName;// msg.isMine ? msg.To.UserName : fromUserName;
            if (dictChatList.ContainsKey(sessionUserName))
            {
                dictChatList[sessionUserName].Add(msg);
            }
            else
            {
                dictChatList.Add(sessionUserName, new List<ChatMessage>() { msg });
            }
        }

        /// <summary>
        /// 是否是当前聊天对象
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool isCurr(string userName)
        {
            if (selectObjectBase.UserName == userName && string.IsNullOrEmpty(userName) == false)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region btnSessionList
        private void btnSessionList_Click(object sender, RoutedEventArgs e)
        {
            contactList1.Visibility = Visibility.Collapsed;
            sessionList1.Visibility = Visibility.Visible;
        }

        private void btnSessionList_GotFocus(object sender, RoutedEventArgs e)
        {
            btnSessionListImage.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/img/TabBar_Chat_Btn_Click.scale-200.png"));
            btnContactListImage.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/img/TabBar_Contacts_Btn_Hover.scale-200.png"));
        }
        private void sessionList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spWelcome.Visibility = Visibility.Collapsed;

            Conversation userInfo = (Conversation)this.sessionList1.SelectedItem;
            if (userInfo == null) return;

            textBlockNickName.Text = userInfo.FromObj.Name;
            selectObjectBase = userInfo.FromObj;

            //把未读数置为0
            if (userInfo.unReadMsgCount > 0)
            {
                userInfo.unReadMsgCount = 0;
                int indexInt = ocSessionInfo.IndexOf(userInfo);
                ocSessionInfo.Move(indexInt, indexInt);
            }

            //读取聊天记录
            chatlist1.Clear();
            if (dictChatList.ContainsKey(userInfo.FromObj.UserName))
            {
                List<ChatMessage> chatlist = dictChatList[userInfo.FromObj.UserName];
                chatlist1.Append(chatlist);
            }
        }
        #endregion

        #region btnContactList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContactList_Click(object sender, RoutedEventArgs e)
        {
            //listSession.Values[0].unReadMsgCount = 100;
            contactList1.Visibility = Visibility.Visible;
            sessionList1.Visibility = Visibility.Collapsed;
        }

        private void btnContactList_GotFocus(object sender, RoutedEventArgs e)
        {
            //ImageSource iSo = new BitmapImage(new Uri(this.BaseUri, "Assets/img/TabBar_Contacts_Btn_Click.scale-200.png"));
            btnContactListImage.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/img/TabBar_Contacts_Btn_Click.scale-200.png"));
            btnSessionListImage.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/img/TabBar_Chat_Btn_Hover.scale-200.png"));
        }

        private void contactList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spWelcome.Visibility = Visibility.Collapsed;

            Conversation userInfo = (Conversation)this.contactList1.SelectedItem;
            textBlockNickName.Text = userInfo.FromObj.Name;
            selectObjectBase = userInfo.FromObj;
        }
        #endregion

        #region Other
        private void TextBoxInput_SendClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxInput.Text.Trim()))
            {
                //MessageBox.Show("不能发送空白信息！");
                return;
            }
            if (string.IsNullOrEmpty(selectObjectBase.UserName))
            {
                //MessageBox.Show("请先选定一个联系人！");
                return;
            }
            LoginPage.instance.startUI.sendText(textBoxInput.Text, selectObjectBase.UserName);

            ChatMessage chatMsg = new ChatMessage();
            chatMsg.dateTime = DateTime.Now;
            chatMsg.isMine = true;
            chatMsg.message = textBoxInput.Text;
            chatMsg.From = LoginPage.instance.startUI.loginUser;
            chatMsg.To = selectObjectBase;
            //chatMsg.To.HeadImage = dictUserInfo[chatMsg.To.UserName].FromObj.HeadImage;
            chatMsg.messageType = EnumMsgType.MSGTYPE_TEXT;
            chatlist1.Append(chatMsg);
            //chatListBox1.Items.Add(new CCWin.SkinControl.ChatListItem() {  Text = textBoxInput.Text });
            textBoxInput.Text = "";

            SaveChatMessage(chatMsg);
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            //p2.StartBringIntoView();


            //var dialog = new MessageDialog("当前设置尚未保存，你确认要退出该页面吗?", "消息提示");
            //dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
            //dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

            ////设置默认按钮，不设置的话默认的确认按钮是第一个按钮
            //dialog.DefaultCommandIndex = 0;
            //dialog.CancelCommandIndex = 1;

            ////获取返回值
            //var result = dialog.ShowAsync();
        }

        ///// <summary>
        ///// 用户在其他地方登录
        ///// </summary>
        ///// <returns></returns>
        //public async Task OtherLogin()
        //{
        //    ContentDialog errDialog = new ContentDialog()
        //    {
        //        Title = "登录异常",
        //        Content = "有用户在其他地方登录了该微信！",
        //        PrimaryButtonText = "Ok"
        //    };

        //    ContentDialogResult result = await errDialog.ShowAsync();
        //    if (result == ContentDialogResult.Primary)
        //    {
        //        this.Frame.Navigate(typeof(LoginPage));
        //    }
        //}
        #endregion
    }
}
