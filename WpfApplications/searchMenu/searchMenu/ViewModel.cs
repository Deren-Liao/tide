
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;
namespace searchMenu
{
    public class ViewModel : Model
    {

        private List<string> _projects;
        private string _selectedProject;
        private bool _isReady = true;
        private string _searchText;
        private string _selectedRepo;
        private List<string> _repos;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetValueAndRaise(out _searchText, value);
                //if (!String.IsNullOrWhiteSpace(value))
                //{
                //    List<Project> newList = new List<Project>();
                //    _originalProjectsList.ForEach(x =>
                //    {
                //        if (x.Name.ToLowerInvariant().Contains(_searchText.ToLowerInvariant()))
                //        {
                //            newList.Add(x);
                //        }
                //    });
                //    Projects = newList;
                //    SelectedProject = Projects?.FirstOrDefault();
                //}
            }
        }

        public List<string> Projects
        {
            get { return _projects; }
            private set { SetValueAndRaise(out _projects, value); }
        }

        public string SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                var oldValue = _selectedProject;
                SetValueAndRaise(out _selectedProject, value);
                if (_selectedProject != null && _isReady && oldValue != _selectedProject)
                {

                    ListRepoAsync();
                }
            }
        }

        public List<string> Repositories
        {
            get { return _repos; }
            private set { SetValueAndRaise(out _repos, value); }
        }

        public string SelectedRepository
        {
            get { return _selectedRepo; }
            set { SetValueAndRaise(out _selectedRepo, value); }
        }

        private async void ListRepoAsync()
        {
            WriteLine($"ListRepoAsync current selected project is {SelectedProject}");
            await Task.Delay(500);
        }
    }
}
