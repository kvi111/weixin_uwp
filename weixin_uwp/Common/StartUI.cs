using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using weixin_uwp.Common.model;
using weixin_uwp.Controls;

namespace weixin_uwp
{
    public enum EnumMsgType
    {
        MSGTYPE_TEXT = 1,
        MSGTYPE_IMAGE = 3,
        MSGTYPE_VOICE = 34,
        MSGTYPE_VERIFYMSG = 37,
        MSGTYPE_POSSIBLEFRIEND_MSG = 40,
        MSGTYPE_SHARECARD = 42,
        MSGTYPE_VIDEO = 43,
        MSGTYPE_EMOTICON = 47,
        MSGTYPE_LOCATION = 48,
        MSGTYPE_APP = 49,
        MSGTYPE_VOIPMSG = 50,
        MSGTYPE_STATUSNOTIFY = 51,
        MSGTYPE_VOIPNOTIFY = 52,
        MSGTYPE_VOIPINVITE = 53,
        MSGTYPE_MICROVIDEO = 62,
        MSGTYPE_SYSNOTICE = 9999,
        MSGTYPE_SYS = 10000,
        MSGTYPE_RECALLED = 10002
    }
    public class StartUI : WechatApi
    {
        //private static final ExecutorService executorService = Executors.newFixedThreadPool(3);
        //public List<GroupMessage> listGroupMsg = new List<GroupMessage>();
        //public List<UserMessage> listUserMsg = new List<UserMessage>();

        private MessageHandle messageHandle;

        public StartUI() : base(Environment.empty())
        {
            //setMsgHandle(new TulingRobot(environment));
            //start();
        }

        public void setMsgHandle(MessageHandle messageHandle)
        {
            this.messageHandle = messageHandle;
        }
        /// <summary>
        /// 等待扫码二维码和等待登录
        /// </summary>
        public async Task WaitForLogin()
        {
            await LoginPage.instance.SetTip("扫描二维码登录");
            while (true)
            {
                WriteLog(Const.LOG_MSG_SCAN_QRCODE);
                if (await waitforlogin(1) == false)
                {
                    continue;
                }
                break;
            }
            await LoginPage.instance.SetTip("请在手机上点击登录");
            while (true)
            {
                WriteLog(Const.LOG_MSG_CONFIRM_LOGIN);
                if (await waitforlogin(0) == false)
                {
                    continue;
                }
                break;
            }
        }

        /**
         * 启动机器人
         */
        public async Task start()
        {
            WriteLog(Const.LOG_MSG_START);

            if (await GetUUID() == false) //获取uuid
            {
                await LoginPage.instance.SetTip("获取uuid失败",true);
                return;
            }

            if (await GenQrCode() == false) //获取二维码
            {
                await LoginPage.instance.SetTip("获取二维码失败", true);
                return;
            }

            await WaitForLogin();//等待用户扫描、确定

            await LoginPage.instance.SetTip("正在登录......");

            if (await Login() == false) //登录
            {
                await LoginPage.instance.SetTip("登录失败", true);
                return;
            }

            if (await Webwxinit() == false) //初始化
            {
                await LoginPage.instance.SetTip("初始化失败", true);
                return;
            }

            if (await OpenStatusNotify() == false) //开启微信状态通知
            {
                await LoginPage.instance.SetTip("开启微信状态通知失败", true);
                return;
            }

            if (await GetContact() == false) //获取联系人
            {
                await LoginPage.instance.SetTip("获取联系人失败", true);
                return;
            }

            if (await GetAllGroupInfo() == false) //获取群信息
            {
                await LoginPage.instance.SetTip("获取群信息失败", true);
                return;
            }

            //跳转到MainPage，填充联系人列表
            await LoginPage.instance.GoMainPage(this);

            await this.Listen();
        }

        /// <summary>
        /// retcode: 0 正常，1100 失败/退出微信
        /// selector:0 正常，2 新的消息，7 进入/离开聊天界面
        /// </summary>
        private async Task Listen()
        {
            bool isContinue = true;
            while (isContinue)
            {
                try
                {
                    await Task.Delay(2000);//暂停2秒
                    WriteLog("listen synccheck ...");
                    //retcode, selector
                    int[] checkResponse = await synccheck();
                    int retcode = checkResponse[0];
                    int selector = checkResponse[1];
                    WriteLog("retcode: {0}, selector: {1}", retcode, selector);
                    switch (retcode)
                    {
                        case 1100:
                            WriteLog(Const.LOG_MSG_LOGOUT);
                            break;
                        case 1101: //在其他地方登录了微信
                            WriteLog(Const.LOG_MSG_LOGIN_OTHERWHERE);
                            MainPage.instance.Frame.Navigate(typeof(LoginPage));
                            isContinue = false;
                            break;
                        case 1102:
                            WriteLog(Const.LOG_MSG_QUIT_ON_PHONE);
                            break;
                        case 0:
                            await this.handle(selector);
                            break;
                        default:
                            WriteLog("wxSync: {0}\n", wxSync().ToString());
                            break;
                    }
                    WriteLog("listen synccheck ...end.");
                }
                catch (Exception ex)
                {
                    WriteError("listen异常：{0} : {1}", ex.Message, ex.StackTrace);
                }
                //Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 和微信保持同步，获取最新消息
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> wxSync()
        {
            String url = conf["API_webwxsync"] + "?sid={0}&skey={1}&pass_ticket={2}";
            url = String.Format(url, session.getSid(), session.getSkey(), session.getPassTicket());

            Dictionary<String, Object> params1 = new Dictionary<String, Object>();
            params1.Add("BaseRequest", this.baseRequest);
            params1.Add("SyncKey", this.synckeyDic);
            params1.Add("rr", Utils.currentTimeMillis());

            JObject response = await doPost(url, params1);
            if (null == response)
            {
                return null;
            }

            //JsonObject dic = response.getAsJsonObject();
            if (null != response)
            {
                JToken baseResponse = response["BaseResponse"];
                if (null != baseResponse && ((JValue)baseResponse["Ret"]).Value.ToString() == "0")
                {
                    this.makeSynckey(response);
                }
            }
            return response;
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="selector"></param>
        private async Task handle(int selector)
        {
            switch (selector)
            {
                case 2:
                    JObject dic = await wxSync();
                    if (null != dic)
                    {
                        await handle_msg(dic);
                    }
                    break;
                case 7:
                    await wxSync();
                    break;
                case 0:
                    //Thread.Sleep(1000);
                    break;
                case 4:
                    // 保存群聊到通讯录
                    // 修改群名称
                    // 新增或删除联系人
                    // 群聊成员数目变化
                    dic = await wxSync();
                    if (null != dic)
                    {
                        await handle_mod(dic);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 处理发来的消息
        /// </summary>
        /// <param name="dic"></param>
        public async Task handle_msg(JObject dic)
        {
            WriteLog("handle message");
            if (null != messageHandle)
            {
                messageHandle.WxSync(dic);
            }

            int intMegCount = dic["AddMsgList"].Count();
            if (intMegCount == 0)
            {
                return;
            }

            WriteLog(Const.LOG_MSG_NEW_MSG, intMegCount);

            JArray msgs = (JArray)dic["AddMsgList"];
            foreach (JObject element in msgs)
            {
                //JsonObject msg = element.getAsJsonObject();
                String msgType = ((JValue)element["MsgType"]).Value.ToString();
                String content = ((JValue)element["Content"]).Value.ToString().Replace("&lt;", "<").Replace("&gt;", ">");
                ChatMessage userMessage = new ChatMessage();
                userMessage.messageId = ((JValue)element["MsgId"]).Value.ToString();
                userMessage.RawMsg = element;
                userMessage.messageType = (EnumMsgType)(int.Parse(msgType));
                userMessage.From.UserName = ((JValue)element["FromUserName"]).Value.ToString();
                userMessage.To.UserName = ((JValue)element["ToUserName"]).Value.ToString();

                switch (userMessage.messageType)
                {
                    case EnumMsgType.MSGTYPE_TEXT:
                        // 地理位置消息
                        if (content.Contains("pictype=location"))
                        {
                            String location = content.Split(new string[] { "<br/>" }, StringSplitOptions.None)[1];
                            userMessage.Location = location;
                            userMessage.message = String.Format(Const.LOG_MSG_LOCATION, location);
                        }
                        else
                        {
                            // 普通文本
                            String text = null;
                            if (content.Contains(":<br/>"))
                            {
                                text = content.Split(new string[] { ":<br/>" }, StringSplitOptions.None)[1];
                            }
                            else
                            {
                                text = content;
                            }
                            userMessage.message = text;
                            //userMessage.Log = text.Replace("<br/>", "\n");
                        }
                        break;
                    case EnumMsgType.MSGTYPE_IMAGE:
                        //byte[] bts = await GetMessageImg(msgId);
                        WriteLog(Const.LOG_MSG_PICTURE, "");
                        break;
                    case EnumMsgType.MSGTYPE_STATUSNOTIFY:
                        WriteLog(Const.LOG_MSG_NOTIFY_PHONE);
                        break;

                }
                //// 文本groupMessage
                //if (conf["MSGTYPE_TEXT"] == msgType)
                //{

                //}
                //else if (conf["MSGTYPE_STATUSNOTIFY"] == msgType)
                //{
                //    WriteLog(Const.LOG_MSG_NOTIFY_PHONE);
                //    return;
                //}

                await this.show_msg(userMessage);

                //Boolean isGroupMsg = (((JValue)element["FromUserName"]).Value.ToString() + ((JValue)element["ToUserName"]).Value.ToString()).Contains("@@");
                if (userMessage.isGroup)
                {
                    GroupMessage groupMessage = await make_group_msg(userMessage);
                    if (null != messageHandle)
                    {
                        messageHandle.groupMessage(groupMessage);
                    }
                }
                else
                {
                    if (null != messageHandle)
                    {
                        messageHandle.userMessage(userMessage);
                    }
                }
            }
        }
        private async Task show_msg(ChatMessage userMessage)
        {
            ObjectBase dst = new ObjectBase(); //可能是Contact，可能是Group
            Group group = null;
            JObject msg = userMessage.RawMsg;

            String content = ((JValue)msg["Content"]).Value.ToString();
            content = content.Replace("&lt;", "<").Replace("&gt;", ">");

            //String msg_id = ((JValue)msg["MsgId"]).Value.ToString();
            //String FromUserName = ((JValue)msg["FromUserName"]).Value.ToString();
            //String ToUserName = ((JValue)msg["ToUserName"]).Value.ToString();

            // 接收到来自群的消息
            if (userMessage.isGroup)
            {
                //如果群里面，自己发的消息，则ToUserName是群的UserName；别人发的FromUserName是群的UserName;
                String groupId = userMessage.From.UserName.StartsWith("@@") ? userMessage.From.UserName : userMessage.To.UserName;
                group = await this.getGroupById(groupId);
                if (content.Contains(":<br/>"))//群里面其他人发的消息
                {
                    String u_id = content.Split(new string[] { ":<br/>" }, StringSplitOptions.None)[0];
                    userMessage.From = await this.getGroupUserById(u_id, groupId);
                    //dst = new Dictionary<string, string>() { { "ShowName", "GROUP" } };
                }
                else//群里面自己发的消息
                {
                    userMessage.From = await this.getUserById(userMessage.From.UserName); //new Dictionary<string, string>() { { "ShowName", "SYSTEM" } };
                    dst = await getGroupUserById(userMessage.To.UserName, groupId);
                }
            }
            else
            {
                // 非群聊消息
                userMessage.From = await this.getUserById(userMessage.From.UserName);
                dst = await this.getUserById(userMessage.To.UserName);
            }

            if (null != group)
            {
                WriteLog("{0} {1} -> {2}: {3}\n", userMessage.messageId, userMessage.From.Name, group.Name, userMessage.message);
            }
            else
            {
                WriteLog("{0} {1} -> {2}: {3}\n", userMessage.messageId, userMessage.From.Name, dst.Name, userMessage.message);
            }

            if (string.IsNullOrEmpty(userMessage.message) == false)
            {
                //显示到界面上
                userMessage.To = group != null ? group : dst;
                userMessage.isMine = this.loginUser.UserName == userMessage.From.UserName ? true : false;

                await MainPage.instance.AddMessage(userMessage);//界面上处理新来的消息
            }
        }

        /// <summary>
        /// 群组有变动的时候触发此方法
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private async Task handle_mod(JObject dic)
        {
            WriteLog("handle modify");
            await handle_msg(dic);

            JArray modContactList = (JArray)dic["ModContactList"];
            foreach (JObject contact in modContactList)
            {
                //JsonObject m = element.getAsJsonObject();
                string userName = ((JValue)contact["UserName"]).Value.ToString();
                if (userName.StartsWith("@@"))
                {
                    Boolean in_list = false;
                    String g_id = userName;
                    foreach (Group group in groupList.Values.ToArray())
                    {
                        //JsonObject group = ge.getAsJsonObject();
                        if (g_id == group.UserName)
                        {
                            in_list = true;
                            //todo:未改完
                            //group.Add("MemberCount", contact["MemberCount"]);
                            //group.Add("NickName", contact["NickName"]);
                            //this.groupMemeberList.Add(g_id, (JArray)contact["MemberList"]);
                            if (null != messageHandle)
                            {
                                messageHandle.groupMemberChange(g_id, (JArray)contact["MemberList"]);
                            }
                            break;
                        }
                    }
                    if (!in_list)
                    {
                        //todo:未改完
                        //this.groupList.Add(contact);
                        //this.groupMemeberList.Add(g_id, (JArray)contact["MemberList"]);
                        if (null != messageHandle)
                        {
                            messageHandle.groupListChange(g_id, (JArray)contact["MemberList"]);
                            messageHandle.groupMemberChange(g_id, (JArray)contact["MemberList"]);
                        }
                    }
                }
                else if (userName == "@")
                {
                    //todo:未改完
                    //Boolean in_list = false;
                    //foreach (ObjectBase ue in memberList)
                    //{
                    //    String u_id = userName;
                    //    if (u_id == ue.UserName)
                    //    {
                    //        u = contact;
                    //        in_list = true;
                    //        break;
                    //    }
                    //}
                    //if (!in_list)
                    //{
                    //    this.memberList.Add(contact);
                    //}
                }
            }
        }

        private async Task<GroupMessage> make_group_msg(ChatMessage userMessage)
        {
            WriteLog("make group message");
            GroupMessage groupMessage = new GroupMessage(this);
            groupMessage.RawMsg = userMessage.RawMsg;
            groupMessage.MsgId = userMessage.messageId;
            groupMessage.FromUserName = userMessage.From.UserName;
            groupMessage.ToUserName = userMessage.To.UserName;
            groupMessage.MsgType = userMessage.messageType;
            groupMessage.Text = userMessage.message;

            String content = ((JValue)userMessage.RawMsg["Content"]).Value.ToString().Replace("&lt;", "<").Replace("&gt;", ">");

            Group group = null;
            Contact src = null;

            if (groupMessage.FromUserName.StartsWith("@@"))
            {
                //接收到来自群的消息
                String g_id = groupMessage.FromUserName;
                groupMessage.setGroupId(g_id);
                group = await this.getGroupById(g_id);
                if (content.Contains(":<br/>"))
                {
                    String u_id = content.Split(new string[] { ":<br/>" }, StringSplitOptions.None)[0];
                    src = await getGroupUserById(u_id, g_id);
                }
            }
            else if (groupMessage.ToUserName.StartsWith("@@"))
            {
                // 自己发给群的消息
                String g_id = groupMessage.ToUserName;
                groupMessage.setGroupId(g_id);
                String u_id = groupMessage.FromUserName;
                src = await this.getGroupUserById(u_id, g_id);
                group = await this.getGroupById(g_id);
            }

            if (null != src)
            {
                groupMessage.setUser_attrstatus(src.AttrStatus);
                groupMessage.setUser_display_name(src.DisplayName);
                groupMessage.setUser_nickname(src.Name);
            }
            if (null != group)
            {
                //groupMessage.setGroup_count(group.Members.Count);
                //groupMessage.setGroup_owner_uin(group.OwnerUin);
                groupMessage.setGroup_name(group.Name);
            }
            groupMessage.setTimestamp(((JValue)userMessage.RawMsg["CreateTime"]).Value.ToString());

            return groupMessage;
        }
    }
}
