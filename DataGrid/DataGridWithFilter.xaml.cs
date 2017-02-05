using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel;

namespace LogView
{

    [System.Windows.Markup.MarkupExtensionReturnType(typeof(IValueConverter))]
    public class VisbilityToBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
    }


    public partial class DataGridWithFilter : Window
    {
        public DataGridWithFilter()
        {
            InitializeComponent();
        }

        private ListCollectionView collectionView;

        Persons ps = new Persons();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ps.AddPage();

            collectionView = new ListCollectionView(ps);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Birthday"));
            dg.ItemsSource = collectionView;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string filter = t.Text;

            if (t.Name == "txtName")
            {
                // your Filter
                var yourCostumFilter = new Predicate<object>(item => ((Person)item).Name.Contains(filter));

                //now we add our Filter
                collectionView.Filter = yourCostumFilter;
                return;
            }

            ICollectionView cv = CollectionViewSource.GetDefaultView(dg.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    Person p = o as Person;
                    if (t.Name == "txtId")
                    {
                        int id;
                        if (!Int32.TryParse(filter, out id))
                        {
                            return true;
                        }
                        return (p.Id == id);
                    }
                    return (p.Name.ToUpper().StartsWith(filter.ToUpper()));
                };
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            if (row != null)
            {
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
            dg.UnselectAll();
        }


        private DataGridRow FindRowByPoint(Point p)
        {
            var ele = dg.InputHitTest(p);
            DependencyObject dep = (DependencyObject)ele;
            // navigate further up the tree
            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
            {
                return null;
            }

            DataGridRow row = dep as DataGridRow;
            return row;
        }

        private void dg_Mouse(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;


            DataGridCell cell = dep as DataGridCell;
            // do something

            // navigate further up the tree
            while ((dep != null) && !(dep is DataGridRow)) {
                dep = VisualTreeHelper.GetParent(dep);
            }

            DataGridRow row = dep as DataGridRow;
            if (row != null)
            {
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        private void dg_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dg_Mouse(sender, e);
        }
        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dg_Mouse(sender, e);
        }

        private void DataGridCell_KeyDown(object sender, MouseButtonEventArgs e )
        {

        }

        private void GridRowClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)sender;
            if (row != null)
            {
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

            void ShowHideDetails(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility =
                      row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private Person GetFirstRow()
        {
            var row = FindRowByPoint(new Point(5, 50));
            Debug.WriteLine($"FindRowByPoint(5, 50) returns {row}");
            if (null == row)
            {
                return null;
            }

            int rowIndex = dg.ItemContainerGenerator.IndexFromContainer(row);
            if (rowIndex > 0)
            {
                var person = dg.Items[rowIndex] as Person;
                Debug.WriteLine($"Find person returns person id {person.Id}");
                // firstRowTextBlock.Text = $"{person.Id} {person.Name}";
                return person;
            }
            else
            {
                return null;
            }
        }

        private void dtGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            GetFirstRow();

            var grid = sender as DataGrid;
            Debug.WriteLine($"dtGrid_ScrollChanged, sender is {grid.Name}");
            if (e.VerticalChange != 0)
            {
                Debug.WriteLine($"ScrollChangedEventArgs, VerticalChange {e.VerticalChange}, ViewportHeight {e.ViewportHeight}, VerticalOffset {e.VerticalOffset}");
            }

            ScrollViewer sv = e.OriginalSource as ScrollViewer;
            Debug.WriteLine($"{e.VerticalOffset}, {sv.ScrollableHeight}");
            if (e.VerticalOffset == sv.ScrollableHeight)
            {
                Debug.WriteLine("Now it is at bottom");

                ps.AddPage();
                //collectionView = new ListCollectionView(ps);
                //collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Birthday"));
                //dg.ItemsSource = collectionView;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ps.AddPage();
        }
    }
}
