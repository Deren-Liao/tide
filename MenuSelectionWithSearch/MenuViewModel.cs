using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MenuSelectionWithSearch
{
    public class MenuItemViewModel
    {
        static MenuItemViewModel()
        {
            TopLevelInstances = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { Header = "Beta",
                    MenuItems = new ObservableCollection<MenuItemViewModel>
                        {
                            new MenuItemViewModel { Header = "Beta1" },
                            new MenuItemViewModel { Header = "Beta2",
                                MenuItems = new ObservableCollection<MenuItemViewModel>
                                {
                                    new MenuItemViewModel { Header = "Beta1a" },
                                    new MenuItemViewModel { Header = "Beta1b" },
                                    new MenuItemViewModel { Header = "Beta1c" }
                                }
                            },
                            new MenuItemViewModel { Header = "Beta3" }
                        }
                }
            };
        }

        public static ObservableCollection<MenuItemViewModel> TopLevelInstances { get; }

        private readonly ICommand _command;

        public MenuItemViewModel()
        {
            _command = new CommandViewModel(Execute);
        }

        public string Header { get; set; }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        public ICommand Command
        {
            get
            {
                return _command;
            }
        }

        private void Execute()
        {
            // (NOTE: In a view model, you normally should not use MessageBox.Show()).
            MessageBox.Show("Clicked at " + Header);
        }
    }

    public class CommandViewModel : ICommand
    {
        private readonly Action _action;

        public CommandViewModel(Action action)
        {
            _action = action;
        }

        public void Execute(object o)
        {
            _action();
        }

        public bool CanExecute(object o)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
