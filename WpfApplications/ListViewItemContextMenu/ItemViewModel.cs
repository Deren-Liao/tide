using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using GoogleCloudExtension.Utils;

namespace ListViewItemContextMenu
{
    public class ItemViewModel : Model
    {
        public string Str1 => "str1";
        public string Str2 => "str2";

        public ProtectedCommand CopyCommand { get; }

        public ProtectedCommand DoubleClickCommand { get; }

        public FontWeight TextWeight => FontWeights.Bold;

        public ItemViewModel()
        {
            String.Compare(null, null, StringComparison.OrdinalIgnoreCase);
            CopyCommand = new ProtectedCommand(() => MessageBox.Show("Copy me"));

            DoubleClickCommand = new ProtectedCommand(() => MessageBox.Show("Double Click"));
        }
    }
}
