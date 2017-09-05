using System;
using System.Collections.Generic;

namespace weixin_uwp.Common.model
{
    /// <summary>
    /// 微信群
    /// </summary>
    public class Group : ObjectBase
    {
        public Group()
        {
            Members = new Dictionary<String, Contact>();
        }

        /// <summary>
        /// 群成员
        /// </summary>
        public Dictionary<String, Contact> Members { get; set; }

        private string _headImageUrl = "";
        /// <summary>
        /// 群头像url
        /// </summary>
        public string HeadImageUrl
        {
            get
            {
                return "https://wx.qq.com" + _headImageUrl;
            }
            set
            {
                _headImageUrl = value;
            }
        }
    }
}
