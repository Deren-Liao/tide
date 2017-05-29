using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoogleCloudExtension.Utils;

namespace ListViewItemContextMenu
{
    public class MainViewModel : ViewModelBase
    {
        public List<ItemViewModel> Items => _items;

        ItemViewModel _selected;
        public ItemViewModel Selected
        {
            get { return _selected; }
            set { SetValueAndRaise(out _selected, value); }
        }

        public ProtectedCommand DoubleClickCommand { get; }

        private List<ItemViewModel> _items = new List<ItemViewModel>
        {
            new ItemViewModel(),
            new ItemViewModel()
        };

        public MainViewModel()
        {
            DoubleClickCommand = new ProtectedCommand(() => Selected?.DoubleClickCommand.Execute(null));
        }
    }
}
