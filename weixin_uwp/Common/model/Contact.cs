using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weixin_uwp.Common.model
{
    /// <summary>
    /// 微信联系人
    /// </summary>
    public class Contact : ObjectBase
    {
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        public string AttrStatus { get; set; }

    }
}
