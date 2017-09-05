using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace weixin_uwp.Common.model
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            From = new Contact();
            To = new ObjectBase();
            dateTime = DateTime.Now;
        }

        /// <summary>
        /// 发送人
        /// </summary>
        public Contact From { get; set; }

        /// <summary>
        /// 接收人(如果是群消息的话，为Group；是两人对聊，为Contact)
        /// </summary>
        public ObjectBase To { get; set; }

        ///// <summary>
        ///// 群（如果是群消息的话，不会为空）
        ///// </summary>
        //public Group FromGroup { get; set; }

        /// <summary>
        /// 原始消息体
        /// </summary>
        public JObject RawMsg { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public string messageId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 发坐标时显示地点名称
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// 消息类型
        /// </summary>
        public EnumMsgType messageType { get; set; }

        /// <summary>
        /// 消息接收时间
        /// </summary>
        public DateTime dateTime { get; set; }

        /// <summary>
        /// 是否为我发送的消息
        /// </summary>
        public bool isMine { get; set; }

        /// <summary>
        /// 是否为群消息
        /// </summary>
        public bool isGroup
        {
            get
            {
                if (From != null && string.IsNullOrEmpty(From.UserName)==false && From.UserName.StartsWith("@@"))
                {
                    return true;
                }
                if (To != null && string.IsNullOrEmpty(To.UserName) == false && To.UserName.StartsWith("@@"))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
