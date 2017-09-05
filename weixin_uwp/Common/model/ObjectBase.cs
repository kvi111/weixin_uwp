using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace weixin_uwp.Common.model
{
    public class ObjectBase
    {
        //private BitmapImage _HeadImage;
        /// <summary>
        /// 头像
        /// </summary>
        public BitmapImage HeadImage { get; set; }
        //{
        //    get
        //    {
        //        //if (_HeadImage == null)
        //        //{
        //        //    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
        //        //    CoreDispatcherPriority.High,
        //        //    new DispatchedHandler(async () =>
        //        //    {
        //        //        _HeadImage = await GetHeadImage();
        //        //    }));
        //        //}

        //        Task t = new Task(async ()=> {
        //            await GetHeadImage();
        //        });
        //        t.RunSynchronously();
        //        //GetHeadImage();
        //        return _HeadImage;
        //    }
        //    set { _HeadImage = value; }
        //}

        public string _name = "";
        /// <summary>
        /// 展示名称
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(RemarkName) == false)
                {
                    return RemarkName;
                }
                return _name;
            }
            set { _name = value; }
        }

        /// <summary>
        /// 微信UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 微信UserName
        /// </summary>
        public string RemarkName { get; set; }

        /// <summary>
        /// 微信UserName
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VerifyFlag { get; set; }

        public Contact ToContact()
        {
            Contact contact = new Contact();
            contact.Name = Name;
            contact.UserName = UserName;
            contact.HeadImage = HeadImage;
            contact.HeadImgUrl = HeadImgUrl;
            contact.RemarkName = RemarkName;
            contact.VerifyFlag = VerifyFlag;

            return contact;
        }

        public Group ToGroup()
        {
            Group group = new Group();
            group.Name = Name;
            group.UserName = UserName;
            group.HeadImage = HeadImage;
            group.HeadImgUrl = HeadImgUrl;
            group.RemarkName = RemarkName;
            group.VerifyFlag = VerifyFlag;
            
            return group;
        }

        public async Task<BitmapImage> GetHeadImage()
        {
            if (HeadImage != null) return HeadImage;

            byte[] headImgBytes = await LoginPage.instance.startUI.GetHeadImg(UserName);
            HeadImage = await Utils.ByteArrayToBitmapImage(headImgBytes);
            return HeadImage;
        }
    }
}
