using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FireBlade.SystemManager
{
    /// <summary>
    /// Interaction logic for SearchListView.xaml
    /// </summary>
    public partial class SearchListView : UserControl
    {
        public ObservableCollection<SearchEntryInfo> Results { get; set; } = new ObservableCollection<SearchEntryInfo>();

        public SearchListView()
        {
            InitializeComponent();
        }

        public event EventHandler<SearchEntryInfo> OnItemClick;

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb.SelectedItem is SearchEntryInfo info)
            {
                OnItemClick?.Invoke(this, info);
            }
        }
    }

    public class SearchEntryInfo : DependencyObject
    {
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public System.Windows.Forms.TreeNode Node { get; set; }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(ImageSource),
            typeof(SearchEntryInfo));
    }
}
