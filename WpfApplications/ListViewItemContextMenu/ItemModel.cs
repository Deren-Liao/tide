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
    public class ItemModel : Model
    {
        public string Str1 { get; }
        public string Str2 => "str2";

        public FontWeight TextWeight => FontWeights.Bold;

        public ItemModel(string name)
        {
            Str1 = name;
            String.Compare(null, null, StringComparison.OrdinalIgnoreCase);
        }
    }
}
