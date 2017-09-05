using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weixin_uwp.Common.model;

namespace weixin_uwp
{
    public interface MessageHandle
    {

        /**
         * 保存微信消息
         *
         * @param msg
         */
        void WxSync(JObject msg);

        void userMessage(ChatMessage userMessage);

        void groupMessage(GroupMessage groupMessage);

        void groupMemberChange(String groupId, JArray memberList);

        void groupListChange(String groupId, JArray memberList);

    }
}
