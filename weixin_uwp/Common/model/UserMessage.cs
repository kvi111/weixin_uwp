using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weixin_uwp
{
    public class UserMessage
    {
        public UserMessage(WechatApi wechatApi)
        {
            this.WechatApi = wechatApi;
        }
        public WechatApi WechatApi { get; set; }

        public EnumMsgType MsgType { get; set; }

        public JObject RawMsg { get; set; }

        public String Location { get; set; }

        public String Log { get; set; }

        public String Text { get; set; }

        public String MsgId { get; set; }

        public String FromUserName { get; set; }

        public String ToUserName { get; set; }

        public override String ToString()
        {
            return "UserMessage(" +
                    "location='" + Location + '\'' +
                    ", log='" + Log + '\'' +
                    ", text='" + Text + '\'' +
                    ')';
        }

        public Boolean isEmpty()
        {
            return null == Text || null == RawMsg;
        }

        public void sendText(String msg, String uid)
        {
            WechatApi.sendText(msg, uid);
        }
    }
}
