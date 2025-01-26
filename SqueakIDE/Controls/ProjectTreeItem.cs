using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SqueakIDE.Controls
{
    public class ProjectTreeItem : TreeViewItem
    {
        public enum ItemType
        {
            Project,
            Folder,
            SourceFile,
            Reference
        }

        public enum FileStatus
        {
            Normal,
            Modified,
            New,
            Deleted,
            Error
        }

        public ItemType Type { get; set; }
        public string FullPath { get; set; }
        public FileStatus Status { get; set; }

        public ProjectTreeItem(string header, ItemType type, string fullPath, FileStatus status = FileStatus.Normal)
        {
            Header = header;
            Type = type;
            FullPath = fullPath;
            Status = status;
            UpdateVisual();
        }

        public void UpdateStatus(FileStatus newStatus)
        {
            Status = newStatus;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            
            var icon = new TextBlock
            {
                Text = Type switch
                {
                    ItemType.Project => "📁",
                    ItemType.Folder => "📂",
                    ItemType.SourceFile => "📄",
                    ItemType.Reference => "🔗",
                    _ => "📄"
                },
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.White
            };

            var text = new TextBlock
            {
                Text = Status != FileStatus.Normal ? $"{Header.ToString()} {GetStatusIndicator()}" : Header.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Status switch
                {
                    FileStatus.Modified => Brushes.Yellow,
                    FileStatus.New => Brushes.LightGreen,
                    FileStatus.Deleted => Brushes.Red,
                    FileStatus.Error => Brushes.OrangeRed,
                    _ => Brushes.White
                }
            };

            stackPanel.Children.Add(icon);
            stackPanel.Children.Add(text);
            Header = stackPanel;
        }

        private string GetStatusIndicator() => Status switch
        {
            FileStatus.Modified => "●",
            FileStatus.New => "+",
            FileStatus.Deleted => "-",
            FileStatus.Error => "⚠",
            _ => string.Empty
        };
    }
} 