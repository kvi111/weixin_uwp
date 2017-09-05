using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace weixin_uwp.Common.model
{
    public class Conversation : IComparable<Conversation> //: INotifyPropertyChanged,INotifyCompletion
    {
        public Conversation() {
            FromObj = new ObjectBase();
        }


        /// <summary>
        /// 可能是Contact也可能是Group
        /// </summary>
        public ObjectBase FromObj{ get; set; }
        //private string _heading;  //IObservable
        //public string heading { get; set; }
        ////{
        ////    get
        ////    {
        ////        return _heading;
        ////    }
        ////    set
        ////    {
        ////        _heading = value;
        ////        MyPropertyChanged();
        ////    }
        ////}

        //public BitmapImage headImg { get; set; }
        ////public byte[] headImgBytes { get; set; }
        ////{
        ////    get
        ////    {
        ////        byte[] bytes = LoginPage.instance.startUI.GetContactHeadImg(userName).Result;
        ////        return Utils.ByteArrayToBitmapImage(bytes);
        ////    }
        ////}
        //public string userName { get; set; }

        public string subHeading { get; set; }
        

        private int _unReadMsgCount;

        public int unReadMsgCount { get; set; }
        //{
        //    get
        //    {
        //        return _unReadMsgCount;
        //    }
        //    set
        //    {
        //        _unReadMsgCount = value;
        //        MyPropertyChanged();
        //    }
        //}

        

        public Color color { get; set; }

        //public void OnCompleted(Action continuation)
        //{
        //    //throw new NotImplementedException();
        //    //MyPropertyChanged();
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void MyPropertyChanged([CallerMemberName]string propertyName = "")
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public int CompareTo(Conversation other)
        {
            if (other == null)
                return 1;
            return this.FromObj.Name.CompareTo(other.FromObj.Name);
        }
    }
}
