using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace weixin_uwp.Controls
{
    public sealed partial class ContactList : UserControl
    {
        public event SelectionChangedEventHandler SelectionChanged;
        public ContactList()
        {
            this.InitializeComponent();

            this.listbox1.SelectionChanged += (sender, e) =>
            {
                if(SelectionChanged!=null)
                { 
                    SelectionChanged.Invoke(sender, e);
                }
            }; //ContactList_SelectionChanged;

            //this.listbox1.DataContextChanged += Listbox1_DataContextChanged;
        }

        //private void Listbox1_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        //{
        //    UpdateLayout();
        //    UpdateLayout1();
        //}

        //private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SelectionChanged.Invoke(sender, e);
        //}

        public ItemCollection Items
        {
            get
            {
                return this.listbox1.Items;
            }
        }

        public object SelectedItem
        {
            get
            {
                return this.listbox1.SelectedItem;
            }
        }

        public object ItemsSource
        {
            set
            {
                this.listbox1.ItemsSource = value;
            }
            get
            {
                return this.listbox1.ItemsSource;
            }
        }

        public new Brush Background
        {
            set
            {
                this.listbox1.Background = value;
            }
            get
            {
                return this.listbox1.Background;
            }
        }

        public void UpdateLayout1()
        {
            this.listbox1.UpdateLayout();
        }
    }
}
