using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using GoogleCloudExtension.Utils;

namespace ListViewItemContextMenu
{
    public class MainViewModel : ViewModelBase
    {
        public List<ItemModel> Items => _items;

        ItemModel _selected;
        public ItemModel Selected
        {
            get { return _selected; }
            set { SetValueAndRaise(out _selected, value); }
        }

        public ProtectedCommand DoubleClickCommand { get; }

        private List<ItemModel> _items = new List<ItemModel>
        {
            new ItemModel("item 1"),
            new ItemModel("item 2")
        };

        //public ICommand CopyCommand = new ProtectedCommand(Copy);


        public MainViewModel()
        {
            DoubleClickCommand = new ProtectedCommand(() => Selected?.OnDoubleClick.Execute(null));
        }
    }
}
