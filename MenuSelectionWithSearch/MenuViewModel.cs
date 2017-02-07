using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GoogleCloudExtension.Utils;

namespace GoogleCloudExtension.StackdriverLogsViewer
{
    public class MenuItemViewModel : Model
    {
        static MenuItemViewModel()
        {
            TopLevelInstances = 
                new ObservableCollection<MenuItemViewModel>
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
                };
        }

        public static ObservableCollection<MenuItemViewModel> TopLevelInstances { get; }

        private readonly ICommand _command;
        private readonly ICommand _initCommand;

        public string ChooseAllHeader => "All from view model";

        public MenuItemViewModel()
        {
            _command = new CommandViewModel(Execute);
            _initCommand = new CommandViewModel(() => AddItems());
        }

        public string Header { get; set; }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        public ICommand MenuCommand
        {
            get
            {
                return _command;
            }
        }

        public ICommand OnSubmenuOpenCommand
        {
            get
            {
                return _initCommand;
            }
        }
        private bool loading = false;
        public bool IsSubmenuOpen { get; set; }

        private bool _isSubmenuPopulated = false;
        public bool IsSubmenuPopulated
        {
            get { return _isSubmenuPopulated; }
            set { SetValueAndRaise(ref _isSubmenuPopulated, value); }
        }


        private async Task AddItems()
        {
            if (IsSubmenuPopulated || loading)
            {
                return;
            }

            loading = true;
            Debug.WriteLine($"{Header} AddItems from viewModel");
            await System.Threading.Tasks.Task.Delay(5000);
            MenuItems.Add(new MenuItemViewModel() { Header = "Add a new 1" });
            MenuItems.Add(new MenuItemViewModel() { Header = "Add a new 2" });
            Debug.WriteLine($"{Header} Set OnInit false from  AddItems from viewModel");
            IsSubmenuPopulated = true;
            loading = false;
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
