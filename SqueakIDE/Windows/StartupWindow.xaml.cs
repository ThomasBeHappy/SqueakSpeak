using System.Collections.ObjectModel;
using System.Windows;
using SqueakIDE.Project;
using SqueakIDE.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SqueakIDE.Windows
{
    public partial class StartupWindow : Window
    {
        public class RecentProject
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        private ObservableCollection<RecentProject> _recentProjects;

        public SqueakProject SelectedProject { get; private set; }
        public bool ContinueWithoutProject { get; private set; }

        public StartupWindow()
        {
            InitializeComponent();
            LoadRecentProjects();
        }

        private void LoadRecentProjects()
        {
            _recentProjects = new ObservableCollection<RecentProject>();
            // Load from settings/file
            var recentProjectsFile = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SqueakIDE",
                "recentprojects.json"
            );

            if (File.Exists(recentProjectsFile))
            {
                var json = File.ReadAllText(recentProjectsFile);
                var projects = System.Text.Json.JsonSerializer.Deserialize<List<RecentProject>>(json);
                foreach (var project in projects)
                {
                    _recentProjects.Add(project);
                }
            }

            RecentProjectsList.ItemsSource = _recentProjects;
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewProjectDialog();
            if (dialog.ShowDialog() == true)
            {
                var project = SqueakProject.Create(dialog.ProjectName, dialog.ProjectPath);
                project.Save();
                SelectedProject = project;
                AddToRecentProjects(project);
                DialogResult = true;
                Hide();
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Squeak Project (*.squeakproj)|*.squeakproj",
                Title = "Open Project"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedProject = SqueakProject.Load(dialog.FileName);
                AddToRecentProjects(SelectedProject);
                DialogResult = true;
                Hide();
            }
        }

        private void ContinueWithoutProject_Click(object sender, RoutedEventArgs e)
        {
            SelectedProject = null;
            DialogResult = true;
            Hide();
        }

        private void RecentProjectsList_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = RecentProjectsList.SelectedItem as RecentProject;
            if (selectedItem != null && File.Exists(selectedItem.Path))
            {
                SelectedProject = SqueakProject.Load(selectedItem.Path);
                DialogResult = true;
                Hide();
            }
        }

        private void AddToRecentProjects(SqueakProject project)
        {
            var recentProject = new RecentProject
            {
                Name = project.Name,
                Path = Path.Combine(project.RootPath, $"{project.Name}.squeakproj")
            };

            // Add to start of list and remove duplicates
            _recentProjects.Remove(_recentProjects.FirstOrDefault(p => p.Path == recentProject.Path));
            _recentProjects.Insert(0, recentProject);

            // Keep only last 10 projects
            while (_recentProjects.Count > 10)
                _recentProjects.RemoveAt(_recentProjects.Count - 1);

            // Save to file
            var recentProjectsFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SqueakIDE",
                "recentprojects.json"
            );

            Directory.CreateDirectory(Path.GetDirectoryName(recentProjectsFile));
            var json = System.Text.Json.JsonSerializer.Serialize(_recentProjects);
            File.WriteAllText(recentProjectsFile, json);
        }
    }
} 