using Antlr4.Runtime;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SqueakIDE.Completion;
using SqueakIDE.Controls;
using SqueakIDE.Dialogs;
using SqueakIDE.Editor;
using SqueakIDE.Git;
using SqueakIDE.Project;
using SqueakIDE.Services;
using SqueakIDE.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Themes;
using System.Windows.Interop;
using SqueakIDE.Models;
using SqueakIDE.Themes;
using SqueakIDE.Extensions;
using SqueakIDE.Settings;
using SqueakIDE.Debugging;

namespace SqueakIDE
{
    public partial class MainWindow : Window
    {
        private TextMarkerService _textMarkerService;
        public string CurrentFilePath;
        private SearchPanel _searchPanel;
        public ICommand SaveCommand { get; }
        private List<SearchResult> _currentSearchResults = new();
        private int _currentSearchIndex = -1;
        public SqueakProject CurrentProject;
        private bool _isInitialized = false;
        private SearchOptions _searchOptions = new SearchOptions();
        private Dictionary<string, List<SearchResult>> _searchResults = new();
        private HashSet<string> _pinnedTabs = new();
        private GitManager _gitManager;
        private readonly ValidationService _validationService = new();
        private LiveShareClient _liveShareClient;
        private bool _isHandlingRemoteChange;
        private string _currentSessionUrl;
        private ObservableCollection<ChatMessage> _chatMessages = new();
        private AIChatService _aiService;
        private ILoggerFactory _loggerFactory;
        private LogLevel _currentLogLevel = LogLevel.Error;
        private ConsoleWindow _consoleWindow;
        private IntPtr Handle => new WindowInteropHelper(this).Handle;
        private double _defaultWidth = 1200;
        private double _defaultHeight = 800;
        private double _defaultLeft;
        private double _defaultTop;
        private readonly ExtensionManager _extensionManager;
        private readonly ExtensionHost _extensionHost;
        private MouseTrail _mouseTrail;
        private IDESettings _settings;
        private SqueakDebugger _debugger;
        private DebugVisualizer _debugVisualizer;
        private readonly SqueakSpeakInterpreterVisitor _interpreter;

        private class SearchResult
        {
            public int StartOffset { get; set; }
            public int Length { get; set; }
            public ICSharpCode.AvalonEdit.TextEditor Editor { get; set; }
            public LayoutDocument Document { get; set; }

            public SearchResult(int startOffset, int length)
            {
                StartOffset = startOffset;
                Length = length;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SaveCommand = new RelayCommand(ExecuteSave);

            InitializeUI();
            InitializeLiveShare();

            DataContext = this;
            _isInitialized = true;

            _aiService = new AIChatService();
            ChatMessages.ItemsSource = _chatMessages;

            Loaded += MainWindow_Loaded;
            // Store initial position after window is loaded
            Loaded += (s, e) => 
            {
                _defaultLeft = Left;
                _defaultTop = Top;
            };
            InitializeThemeMenu();

            _extensionHost = new ExtensionHost(this);
            _extensionManager = new ExtensionManager(_extensionHost);

            // Subscribe to the Loaded event
            this.Loaded += MainWindow_Loaded;

            var debugService = new SqueakDebuggerService(_interpreter);
            _interpreter = new SqueakSpeakInterpreterVisitor(
                new Dictionary<string, object>(), 
                "", 
                _loggerFactory?.CreateLogger<SqueakSpeakInterpreterVisitor>(),
                (Squeak.IDebuggerService)debugService
            );
            _debugVisualizer = new DebugVisualizer(DebugOverlay, DebugHighlight, VariablesView, CallStackView);
            _debugger = new SqueakDebugger(debugService, _debugVisualizer);
            DebugToolbar.DataContext = new DebugCommands(_debugger, debugService);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeLoggerFactory();
            InitializeThemeMenu();
            InitializeMouseTrail();

            // Load extensions after window is fully loaded
            var extensionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extensions");
            if (Directory.Exists(extensionsPath))
            {
                _extensionManager.LoadExtensions(extensionsPath);
            }
        }

        private void InitializeMouseTrail()
        {
            _settings = IDESettings.Load();
            _mouseTrail = new MouseTrail();
            OverlayCanvas.Children.Add(_mouseTrail);
        }

        private void InitializeLoggerFactory()
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole()
                       .SetMinimumLevel(_currentLogLevel);
            });
        }

        private void InitializeUI()
        {
            DockingManager.Theme = new VS2010Theme();
            DockingManager.Foreground = Brushes.White;
            
            _debugVisualizer = new DebugVisualizer(DebugOverlay, DebugHighlight, VariablesView, CallStackView);
            var debugService = new SqueakDebuggerService(_interpreter);
            _debugger = new SqueakDebugger(debugService, _debugVisualizer);
            
            // Set DataContext for the XAML-defined toolbar
            DebugToolbar.DataContext = new DebugCommands(_debugger, debugService);
        }

        #region File Management

        private void ExecuteSave()
        {
            if (TryGetCurrentEditor(out var editor, out var document))
            {
                SaveFile(editor, document);
            }
        }

        public bool TryGetCurrentEditor(out ICSharpCode.AvalonEdit.TextEditor editor, out LayoutDocument document)
        {
            editor = null;
            document = null;

            if (EditorTabs.SelectedContent is LayoutDocument selectedDocument &&
                selectedDocument.Content is ICSharpCode.AvalonEdit.TextEditor currentEditor)
            {
                editor = currentEditor;
                document = selectedDocument;
                return true;
            }

            return false;
        }

        private void SaveFile(ICSharpCode.AvalonEdit.TextEditor editor, LayoutDocument document)
        {
            var filePath = Path.Combine(CurrentFilePath ?? string.Empty, document.Title.TrimEnd('*'));

            try
            {
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, editor.Text);
                    editor.IsModified = false;
                    document.Title = document.Title.TrimEnd('*');
                }
                else if (SaveAs(editor, document))
                {
                    editor.IsModified = false;
                    document.Title = document.Title.TrimEnd('*');
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error saving file: {ex.Message}");
            }

            _extensionHost.OnFileSaved(filePath);
        }

        private bool SaveAs(ICSharpCode.AvalonEdit.TextEditor editor, LayoutDocument document)
        {
            var saveFileDialog = CreateSaveFileDialog(document.Title);

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, editor.Text);
                    document.Title = Path.GetFileName(saveFileDialog.FileName);
                    CurrentFilePath = Path.GetDirectoryName(saveFileDialog.FileName);
                    return true;
                }
                catch (Exception ex)
                {
                    ShowError($"Error saving file: {ex.Message}");
                }
            }

            return false;
        }

        private Microsoft.Win32.SaveFileDialog CreateSaveFileDialog(string fileName)
        {
            return new Microsoft.Win32.SaveFileDialog
            {
                Filter = "SqueakSpeak Files (*.ssp)|*.ssp|All Files (*.*)|*.*",
                FileName = fileName
            };
        }

        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PopulateFolderExplorer(dialog.SelectedPath);
                    CurrentFilePath = dialog.SelectedPath;
                }
            }
        }

        private void PopulateFolderExplorer(string folderPath)
        {
            FolderExplorer.Items.Clear();
            var rootDirectory = new DirectoryInfo(folderPath);

            try
            {
                var rootNode = CreateDirectoryNode(rootDirectory);
                FolderExplorer.Items.Add(rootNode);
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("Access to some files or directories was denied.");
            }
        }

        private TreeViewItem CreateDirectoryNode(DirectoryInfo directory)
        {
            var node = new TreeViewItem
            {
                Header = directory.Name,
                Tag = directory.FullName
            };

            foreach (var subDir in directory.GetDirectories())
            {
                node.Items.Add(CreateDirectoryNode(subDir));
            }

            foreach (var file in directory.GetFiles())
            {
                node.Items.Add(CreateFileNode(file));
            }

            return node;
        }

        private TreeViewItem CreateFileNode(FileInfo file)
        {
            return new TreeViewItem
            {
                Header = file.Name,
                Tag = file.FullName,
                Foreground = new SolidColorBrush(ThemeManager.Instance.CurrentTheme.ForegroundColor),
            };
        }

        #endregion

        #region Editor Management

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            var newDocument = CreateEditorDocument("Untitled", "");
            EditorTabs.Children.Add(newDocument);
            newDocument.IsSelected = true;
        }

        private LayoutDocument CreateEditorDocument(string title, string content = "")
        {
            var editor = CreateConfiguredEditor(content);
            var newDocument = new LayoutDocument
            {
                Title = title,
                Content = editor
            };

            // Handle text changes at document level
            editor.TextChanged += async (s, e) =>
            {
                if (!newDocument.Title.EndsWith("*"))
                {
                    newDocument.Title = newDocument.Title + "*";
                }
                ValidateCurrentFile();
                Editor_TextChanged(s, e);
            };

            newDocument.Closing += HandleTabClosing;
            return newDocument;
        }

        private void HandleTabClosing(object _document, System.ComponentModel.CancelEventArgs e)
        {
            if (_document is LayoutDocument document &&
                document.Content is ICSharpCode.AvalonEdit.TextEditor editor &&
                editor.IsModified)
            {
                var result = ShowUnsavedChangesDialog(document.Title);

                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        SaveAs(editor, document);
                        break;
                }
            }
        }

        private MessageBoxResult ShowUnsavedChangesDialog(string documentTitle)
        {
            return MessageBox.Show(
                $"Do you want to save changes to {documentTitle}?",
                "Unsaved Changes",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning
            );
        }

        private ICSharpCode.AvalonEdit.TextEditor CreateConfiguredEditor(string content)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            var editor = new ICSharpCode.AvalonEdit.TextEditor
            {
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                ShowLineNumbers = true,
                Background = new SolidColorBrush(theme.EditorBackground),
                Foreground = new SolidColorBrush(theme.EditorText),
                LineNumbersForeground = new SolidColorBrush(theme.EditorLineNumbers),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Text = content,
                IsModified = false
            };

            ConfigureEditor(editor);
            return editor;
        }

        private void ConfigureEditor(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            ConfigureSyntaxHighlighting(editor);
            ConfigureTextMarkerService(editor);
            ConfigureSearchPanel(editor);
            ConfigureCaretTracking(editor);
            ConfigureCompletion(editor);

            editor.TextArea.TextView.CurrentLineBackground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
            editor.TextArea.TextView.Options.HighlightCurrentLine = true;
            editor.TextArea.TextView.Options.EnableEmailHyperlinks = false;
            editor.TextArea.TextView.Options.EnableHyperlinks = false;
            editor.ShowLineNumbers = true;
            editor.Options.EnableRectangularSelection = true;
            editor.Options.EnableTextDragDrop = true;
            ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(editor.TextArea);
            editor.KeyDown += Editor_KeyDown;

            new AutoClosingBrackets(editor.TextArea);

            // Add breakpoint margin
            var breakpointMargin = new BreakpointMargin(editor);
            editor.TextArea.LeftMargins.Insert(0, breakpointMargin);

            breakpointMargin.MouseDown += async (s, e) =>
            {
                var line = editor.GetLineFromMousePosition(e.GetPosition(editor));
                if (line >= 0)
                {
                    var breakpoint = new Breakpoint { Line = line };
                    await _debugger.ToggleBreakpoint(breakpoint);
                    breakpointMargin.ToggleBreakpoint(line);
                }
            };
        }

        private void ConfigureSyntaxHighlighting(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("SqueakIDE.SqueakHighlighting.xshd"))
            using (var reader = XmlReader.Create(stream))
            {
                var highlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                
                // Update colors based on theme
                highlighting.GetNamedColor("Keyword").Foreground = new SimpleHighlightingBrush(theme.KeywordColor);
                highlighting.GetNamedColor("String").Foreground = new SimpleHighlightingBrush(theme.StringColor);
                highlighting.GetNamedColor("Number").Foreground = new SimpleHighlightingBrush(theme.NumberColor);
                highlighting.GetNamedColor("Comment").Foreground = new SimpleHighlightingBrush(theme.CommentColor);
                highlighting.GetNamedColor("Operator").Foreground = new SimpleHighlightingBrush(theme.OperatorColor);
                highlighting.GetNamedColor("FunctionCall").Foreground = new SimpleHighlightingBrush(theme.EditorText);
                editor.SyntaxHighlighting = highlighting;
            }
        }

        private void ConfigureTextMarkerService(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _textMarkerService = new TextMarkerService(editor);
            editor.TextArea.TextView.BackgroundRenderers.Add(_textMarkerService);
            editor.TextArea.TextView.LineTransformers.Add(_textMarkerService);
            editor.TextChanged += (sender, e) => HandleTextChanged(editor);
        }

        private void ConfigureSearchPanel(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            _searchPanel = SearchPanel.Install(editor.TextArea);
            _searchPanel.Background = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            _searchPanel.Foreground = Brushes.White;
        }

        private void ConfigureCaretTracking(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            editor.TextArea.Caret.PositionChanged += (s, e) =>
            {
                if (_liveShareClient != null)
                {
                    var currentDoc = DockingManager.ActiveContent as LayoutDocument;
                    _liveShareClient.SendCursorPosition(editor.CaretOffset, editor, currentDoc.Title);
                }

                UpdateStatusBar();
            };
        }

        private void ConfigureCompletion(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            var completionProvider = new CompletionProvider();

            editor.TextArea.TextEntering += (s, e) => HandleTextEntering(e, completionProvider, editor.TextArea);
            editor.TextArea.TextEntered += (s, e) => HandleTextEntered(e, completionProvider, editor.TextArea);
            editor.TextArea.PreviewKeyDown += (s, e) => HandleCompletionKeyDown(e, editor.TextArea);
        }

        private void HandleTextEntering(TextCompositionEventArgs e, CompletionProvider provider, ICSharpCode.AvalonEdit.Editing.TextArea textArea)
        {
            if (e.Text.Length > 0 && (char.IsLetterOrDigit(e.Text[0]) || e.Text == "-" || e.Text == ">"))
            {
                provider.ShowCompletion(textArea);
            }
        }

        private void HandleTextEntered(TextCompositionEventArgs e, CompletionProvider provider, ICSharpCode.AvalonEdit.Editing.TextArea textArea)
        {
            if (e.Text == "-" || e.Text == ">" || char.IsLetterOrDigit(e.Text[0]))
            {
                provider.ShowCompletion(textArea);
            }
        }

        private void HandleCompletionKeyDown(KeyEventArgs e, ICSharpCode.AvalonEdit.Editing.TextArea textArea)
        {
            if (e.Key == Key.Tab || e.Key == Key.Return)
            {
                var completion = textArea.GetService(typeof(CompletionWindow)) as CompletionWindow;
                if (completion?.CompletionList != null)
                {
                    completion.CompletionList.RequestInsertion(e);
                    e.Handled = true;
                }
            }
        }

        private void HandleTextChanged(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            try
            {
                var markerService = editor.TextArea.TextView.BackgroundRenderers
                    .OfType<TextMarkerService>()
                    .FirstOrDefault();

                if (markerService != null)
                {
                    editor.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            markerService.Clear();
                        }
                        catch (Exception)
                        {
                            // Ignore visual lines invalid exception
                        }
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                }
            }
            catch (Exception)
            {
                // Ignore any exceptions during text change handling
            }
        }

        private void HighlightError(SyntaxError error)
        {
            var marker = _textMarkerService.Create(error.StartIndex, error.Length);
            marker.MarkerType = TextMarkerType.SquigglyUnderline;
            marker.MarkerColor = Colors.Red;
        }

        #endregion

        #region Code Execution and Validation

        private async void RunCode(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TryGetCurrentEditor(out var editor, out _))
                {
                    var codeText = editor.Text;
                    _debugVisualizer.SetCurrentEditor(editor);
                    await _debugger.StartDebugging();

                    // Create console window on UI thread
                    if (_consoleWindow == null)
                    {
                        Dispatcher.Invoke(() => {
                            _consoleWindow = new ConsoleWindow();
                            _consoleWindow.Closed += ConsoleWindow_Closed; // Subscribe to the Closed event
                            _consoleWindow.Show();
                        });
                    }

                    await Task.Run(() =>
                    {
                        var inputStream = new AntlrInputStream(codeText);
                        var lexer = new SqueakSpeakLexer(inputStream);
                        var tokens = new CommonTokenStream(lexer);
                        var parser = new SqueakSpeakParser(tokens);
                        var tree = parser.program();

                        try 
                        {
                            _interpreter.Visit(tree);
                        }
                        catch (Exception ex)
                        {
                            _debugger.OnExceptionThrown(this, new Debugging.ExceptionEventArgs { Exception = ex });
                            throw;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConsoleWindow_Closed(object sender, EventArgs e)
        {
            _consoleWindow = null;
        }

        private void ValidateCode(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TryGetCurrentEditor(out var editor, out _))
                {
                    ValidateSyntax(editor.Text);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Validation Error: {ex.Message}");
            }
        }

        private void ValidateSyntax(string code)
        {
            var parser = CreateParser(code);
            var errorListener = new ErrorCollector();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            parser.program();
            DisplayValidationResults(parser, errorListener);
        }

        private SqueakSpeakParser CreateParser(string code)
        {
            var inputStream = new AntlrInputStream(code);
            var lexer = new SqueakSpeakLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            return new SqueakSpeakParser(tokens);
        }

        private void DisplayValidationResults(SqueakSpeakParser parser, ErrorCollector errorListener)
        {
            ErrorList.Items.Clear();

            if (parser.NumberOfSyntaxErrors == 0)
            {
                OutputBox.Text = "Code is valid!";
            }
            else
            {
                foreach (var error in errorListener.Errors)
                {
                    var errorDetails = $"Syntax Error at line {error.Line}, column {error.Column}: {error.Message}";
                    ErrorList.Items.Add(errorDetails);
                }
            }
        }

        #endregion

        #region Helpers

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region Events
        private void FolderExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as ProjectTreeItem;
            if (selectedItem?.Type == ProjectTreeItem.ItemType.SourceFile && File.Exists(selectedItem.FullPath))
            {
                OpenFileInEditor(selectedItem.FullPath);
            }
        }

        private void OpenFileInEditor(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                var existingDocument = FindExistingDocument(fileName);

                if (existingDocument != null)
                {
                    existingDocument.IsSelected = true;
                    return;
                }

                var content = File.ReadAllText(filePath);
                var newDocument = CreateEditorDocument(fileName, content);
                EditorTabs.Children.Add(newDocument);
                newDocument.IsSelected = true;
            }
            catch (IOException ex)
            {
                ShowError($"Could not open file: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError($"Access denied: {ex.Message}");
            }

            _extensionHost.OnFileOpened(filePath);
        }

        private LayoutDocument FindExistingDocument(string title)
        {
            // Normalize both titles by removing * and trimming
            var normalizedTitle = title.Replace("*", "").Trim();
            return EditorTabs.Children
                .OfType<LayoutDocument>()
                .FirstOrDefault(doc => doc.Title.Replace("*", "").Trim() == normalizedTitle);
        }

        public void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetCurrentEditor(out var editor, out var document))
            {
                SaveAs(editor, document);
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // _searchService.Clear();
        }
        #endregion

        public void LoadProject(SqueakProject project)
        {
            CurrentProject = project;
            if (project != null)
            {
                _gitManager = new GitManager(project.RootPath);
                InitializeGit();
                UpdateProjectExplorer();
            }
        }

        private void InitializeGit()
        {
            if (CurrentProject == null) return;

            var initDialog = new GitInitDialog();
            if (initDialog.ShowDialog() == true)
            {
                try
                {
                    if (initDialog.SkipGit)
                    {
                        _gitManager = null;
                        return;
                    }

                    if (initDialog.IsClone)
                    {
                        _gitManager = GitManager.CloneRepository(
                            initDialog.CloneUrl,
                            CurrentProject.RootPath,
                            initDialog.Username,
                            initDialog.Email,
                            initDialog.Password
                        );
                    }
                    else
                    {
                        _gitManager = GitManager.InitializeRepository(
                            CurrentProject.RootPath,
                            initDialog.Username,
                            initDialog.Email
                        );
                    }

                    UpdateBranchList();
                    UpdateGitStatus();
                    MessageBox.Show("Git repository initialized successfully.", "Git",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    ShowError($"Error initializing Git repository: {ex.Message}");
                }
            }
        }

        private void UpdateProjectExplorer()
        {
            FolderExplorer.Items.Clear();

            if (CurrentProject == null) return;

            var projectNode = new ProjectTreeItem(
                CurrentProject.Name,
                ProjectTreeItem.ItemType.Project,
                CurrentProject.RootPath);

            // Add source files
            var sourcesNode = new ProjectTreeItem(
                "Source Files",
                ProjectTreeItem.ItemType.Folder,
                CurrentProject.RootPath);

            foreach (var file in CurrentProject.SourceFiles)
            {
                var fullPath = Path.Combine(CurrentProject.RootPath, file);
                sourcesNode.Items.Add(new ProjectTreeItem(
                    Path.GetFileName(file),
                    ProjectTreeItem.ItemType.SourceFile,
                    fullPath));
            }
            projectNode.Items.Add(sourcesNode);

            // Add references
            var referencesNode = new ProjectTreeItem(
                "References",
                ProjectTreeItem.ItemType.Folder,
                CurrentProject.RootPath);

            foreach (var reference in CurrentProject.References)
            {
                referencesNode.Items.Add(new ProjectTreeItem(
                    Path.GetFileName(reference),
                    ProjectTreeItem.ItemType.Reference,
                    reference));
            }
            projectNode.Items.Add(referencesNode);

            FolderExplorer.Items.Add(projectNode);
            projectNode.IsExpanded = true;
        }

        private void UpdateGitStatus()
        {
            if (_gitManager?.IsGitRepository != true) return;

            var statuses = _gitManager.GetFileStatuses();
            foreach (var item in GetAllProjectItems())
            {
                if (item is ProjectTreeItem treeItem)
                {
                    var relativePath = Path.GetRelativePath(CurrentProject.RootPath, treeItem.FullPath);
                    if (statuses.TryGetValue(relativePath, out var status))
                    {
                        treeItem.UpdateStatus(status);
                    }
                }
            }
        }

        private IEnumerable<object> GetAllProjectItems()
        {
            var items = new List<object>();
            foreach (var item in FolderExplorer.Items)
            {
                items.Add(item);
                if (item is TreeViewItem tvi)
                {
                    items.AddRange(GetChildItems(tvi));
                }
            }
            return items;
        }

        private IEnumerable<object> GetChildItems(TreeViewItem item)
        {
            var items = new List<object>();
            foreach (var child in item.Items)
            {
                items.Add(child);
                if (child is TreeViewItem tvi)
                {
                    items.AddRange(GetChildItems(tvi));
                }
            }
            return items;
        }

        private void AddNewFileToProject()
        {
            if (CurrentProject == null) return;

            var dialog = new SaveFileDialog
            {
                Filter = "Squeak Files (*.ssp)|*.ssp|All Files (*.*)|*.*",
                InitialDirectory = CurrentProject.RootPath,
                Title = "Add New File"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Create the file if it doesn't exist
                    if (!File.Exists(dialog.FileName))
                    {
                        File.WriteAllText(dialog.FileName, string.Empty);
                    }

                    // Add to project and save
                    CurrentProject.AddSourceFile(dialog.FileName);
                    CurrentProject.Save();

                    // Update UI
                    UpdateProjectExplorer();

                    // Open the new file in editor
                    OpenFileInEditor(dialog.FileName);
                }
                catch (Exception ex)
                {
                    ShowError($"Error creating file: {ex.Message}");
                }
            }
        }

        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            AddNewFileToProject();
        }

        private void AddExistingFile_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProject == null) return;

            var dialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                InitialDirectory = CurrentProject.RootPath,
                Title = "Add Existing File"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentProject.AddSourceFile(dialog.FileName);
                    CurrentProject.Save();
                    UpdateProjectExplorer();
                }
                catch (Exception ex)
                {
                    ShowError($"Error adding file: {ex.Message}");
                }
            }
        }

        private void RefreshProjectExplorer_Click(object sender, RoutedEventArgs e)
        {
            UpdateProjectExplorer();
        }

        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProject == null) return;

            var dialog = new TextInputDialog("New Folder", "Enter folder name:");
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var path = Path.Combine(CurrentProject.RootPath, dialog.Result);
                    Directory.CreateDirectory(path);
                    UpdateProjectExplorer();
                }
                catch (Exception ex)
                {
                    ShowError($"Error creating folder: {ex.Message}");
                }
            }
        }

        private void RemoveFromProject_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProject == null) return;

            var item = FolderExplorer.SelectedItem as ProjectTreeItem;
            if (item?.FullPath != null)
            {
                try
                {
                    CurrentProject.RemoveSourceFile(item.FullPath);
                    CurrentProject.Save();
                    UpdateProjectExplorer();
                }
                catch (Exception ex)
                {
                    ShowError($"Error removing file: {ex.Message}");
                }
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProject == null) return;

            var item = FolderExplorer.SelectedItem as ProjectTreeItem;
            if (item?.FullPath == null) return;

            try
            {
                // Check if file is open in editor
                var openDoc = FindExistingDocument(Path.GetFileName(item.FullPath));
                if (openDoc != null)
                {
                    MessageBox.Show("Please close the file before deleting.", "File In Use",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{Path.GetFileName(item.FullPath)}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (File.Exists(item.FullPath))
                    {
                        File.Delete(item.FullPath);
                    }
                    else if (Directory.Exists(item.FullPath))
                    {
                        Directory.Delete(item.FullPath, true);
                    }
                    CurrentProject.RemoveSourceFile(item.FullPath);
                    CurrentProject.Save();
                    UpdateProjectExplorer();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error deleting: {ex.Message}");
            }
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            var item = FolderExplorer.SelectedItem as ProjectTreeItem;
            if (item?.FullPath != null)
            {
                Clipboard.SetText(item.FullPath);
            }
        }

        private void OpenInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var item = FolderExplorer.SelectedItem as ProjectTreeItem;
            if (item?.FullPath != null)
            {
                try
                {
                    if (File.Exists(item.FullPath))
                    {
                        Process.Start("explorer.exe", $"/select,\"{item.FullPath}\"");
                    }
                    else if (Directory.Exists(item.FullPath))
                    {
                        Process.Start("explorer.exe", item.FullPath);
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error opening explorer: {ex.Message}");
                }
            }
        }

        private void CloseAllButThis_Click(object sender, RoutedEventArgs e)
        {
            var currentDoc = DockingManager.ActiveContent as LayoutDocument;
            if (currentDoc == null) return;

            var docsToClose = DockingManager.Layout.Descendents()
                .OfType<LayoutDocument>()
                .Where(d => d != currentDoc)
                .ToList();

            foreach (var doc in docsToClose)
            {
                if (CanCloseDocument(doc))
                {
                    doc.Close();
                }
            }
        }

        private void CloseAllTabs_Click(object sender, RoutedEventArgs e)
        {
            var docs = DockingManager.Layout.Descendents()
                .OfType<LayoutDocument>()
                .ToList();

            foreach (var doc in docs)
            {
                if (CanCloseDocument(doc))
                {
                    doc.Close();
                }
            }
        }

        private void SaveAllTabs_Click(object sender, RoutedEventArgs e)
        {
            var docs = DockingManager.Layout.Descendents()
                .OfType<LayoutDocument>()
                .ToList();

            foreach (var doc in docs)
            {
                SaveDocument(doc);
            }
        }

        private bool CanCloseDocument(LayoutDocument doc)
        {
            if (doc?.Content == null) return true;  // Add null check

            if (_pinnedTabs.Contains(doc.Title.Replace("📌", "").Trim()))
            {
                MessageBox.Show("This tab is pinned. Unpin it first to close.", "Pinned Tab",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (doc.Content is ICSharpCode.AvalonEdit.TextEditor editor && editor.IsModified)
            {
                var result = MessageBox.Show(
                    $"Save changes to {doc.Title}?",
                    "Save Changes",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                    return false;

                if (result == MessageBoxResult.Yes)
                    return SaveDocument(doc);
            }
            return true;
        }

        private bool SaveDocument(LayoutDocument doc)
        {
            if (doc.Content is ICSharpCode.AvalonEdit.TextEditor editor)
            {
                try
                {
                    var filePath = Path.Combine(CurrentFilePath ?? string.Empty, doc.Title.TrimEnd('*'));
                    if (File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, editor.Text);
                        editor.IsModified = false;
                        doc.Title = doc.Title.TrimEnd('*');
                        return true;
                    }
                    else
                    {
                        return SaveAs(editor, doc);
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error saving file: {ex.Message}");
                }
            }
            return false;
        }

        private void DisplaySearchResults()
        {
            if (_currentSearchResults?.Any() != true) return;

            var editor = GetCurrentEditor();
            if (editor == null) return;

            _textMarkerService?.Clear();

            foreach (var result in _currentSearchResults)
            {
                var marker = _textMarkerService.Create(result.StartOffset, result.Length);
                marker.MarkerType = TextMarkerType.SquigglyUnderline;
                marker.MarkerColor = Colors.Yellow;
            }
        }

        public ICSharpCode.AvalonEdit.TextEditor GetCurrentEditor()
        {
            if (TryGetCurrentEditor(out var editor, out _))
            {
                return editor;
            }
            return null;
        }
        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            if (DockingManager.ActiveContent is LayoutDocument doc && CanCloseDocument(doc))
            {
                doc.Close();
            }
        }

        private void SaveTab_Click(object sender, RoutedEventArgs e)
        {
            if (DockingManager.ActiveContent is LayoutDocument doc)
            {
                SaveDocument(doc);
            }
        }

        private void PinTab_Click(object sender, RoutedEventArgs e)
        {
            if (DockingManager.ActiveContent is LayoutDocument doc)
            {
                if (_pinnedTabs.Contains(doc.Title.Replace("📌", "").Trim()))
                {
                    _pinnedTabs.Remove(doc.Title.Replace("📌", "").Trim());
                    doc.Title = doc.Title.Replace("📌", "").Trim();
                    ((MenuItem)sender).Header = "Pin Tab";
                }
                else
                {
                    _pinnedTabs.Add(doc.Title.Replace("📌", "").Trim());
                    doc.Title = $"{doc.Title} 📌";
                    ((MenuItem)sender).Header = "Unpin Tab";
                }
            }
        }

        private void GitCommit_Click(object sender, RoutedEventArgs e)
        {
            if (_gitManager?.IsGitRepository != true) return;

            var changedFiles = _gitManager.GetFileStatuses();
            if (!changedFiles.Any())
            {
                MessageBox.Show("No changes to commit.", "Git",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new GitCommitDialog(changedFiles);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _gitManager.Commit(dialog.CommitMessage, dialog.SelectedFiles);
                    UpdateGitStatus();
                    MessageBox.Show("Changes committed successfully.", "Git",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    ShowError($"Error committing changes: {ex.Message}");
                }
            }
        }

        private void GitPull_Click(object sender, RoutedEventArgs e)
        {
            if (_gitManager?.IsGitRepository != true) return;

            try
            {
                _gitManager.Pull();
                UpdateGitStatus();
                MessageBox.Show("Pull completed successfully.", "Git",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError($"Error pulling changes: {ex.Message}");
            }
        }

        private void GitPush_Click(object sender, RoutedEventArgs e)
        {
            if (_gitManager?.IsGitRepository != true) return;

            try
            {
                _gitManager.Push();
                MessageBox.Show("Push completed successfully.", "Git",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError($"Error pushing changes: {ex.Message}");
            }
        }

        private void BranchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_gitManager?.IsGitRepository != true) return;

            var selectedBranch = BranchComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedBranch))
            {
                try
                {
                    _gitManager.SwitchBranch(selectedBranch);
                    UpdateGitStatus();
                }
                catch (Exception ex)
                {
                    ShowError($"Error switching branch: {ex.Message}");
                }
            }
        }

        private void UpdateBranchList()
        {
            if (_gitManager?.IsGitRepository != true) return;

            BranchComboBox.Items.Clear();
            foreach (var branch in _gitManager.GetBranches())
            {
                BranchComboBox.Items.Add(branch);
            }
            BranchComboBox.SelectedItem = _gitManager.GetCurrentBranch();
        }

        private void GitLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_gitManager == null)
            {
                InitializeGit();
            }
            else
            {
                var loginDialog = new GitLoginDialog();
                if (loginDialog.ShowDialog() == true)
                {
                    _gitManager.SetCredentials(
                        loginDialog.Username,
                        loginDialog.Email,
                        loginDialog.Password
                    );
                    UpdateBranchList();
                    UpdateGitStatus();
                }
            }
        }

        private void ValidateCurrentFile()
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;
            LayoutDocument doc = DockingManager.ActiveContent as LayoutDocument;
            if (doc == null) return;

            _validationService.ValidateFile(doc.Title, editor.Text, this);

            // Update UI with validation results
            UpdateValidationUI(doc.Title);
        }

        private void UpdateValidationUI(string filePath)
        {
            try
            {
                if (_textMarkerService != null)
                {
                    _textMarkerService.Clear();
                }

                if (_validationService.HasErrors(filePath))
                {
                    foreach (var error in _validationService.GetErrors()[filePath])
                    {
                        var marker = _textMarkerService.Create(0, 1); // You'll need to calculate proper positions
                        marker.MarkerType = TextMarkerType.SquigglyUnderline;
                        marker.MarkerColor = Colors.Red;
                    }
                }

                if (_validationService.HasWarnings(filePath))
                {
                    foreach (var warning in _validationService.GetWarnings()[filePath])
                    {
                        var marker = _textMarkerService.Create(0, 1); // You'll need to calculate proper positions
                        marker.MarkerType = TextMarkerType.SquigglyUnderline;
                        marker.MarkerColor = Colors.Yellow;
                    }
                }

                // Update status bar
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                StatusText.Text = "Validation UI update failed";
            }
        }

        private void UpdateStatusBar()
        {
            var editor = GetCurrentEditor();
            if (editor == null) return;

            var doc = editor.Parent as LayoutDocument;
            if (doc == null) return;

            var errorCount = _validationService.HasErrors(doc.Title) ?
                _validationService.GetErrors()[doc.Title].Count : 0;
            var warningCount = _validationService.HasWarnings(doc.Title) ?
                _validationService.GetWarnings()[doc.Title].Count : 0;

            StatusText.Text = $"Errors: {errorCount} Warnings: {warningCount}";
        }

        private void InitializeLiveShare()
        {
            _liveShareClient = new LiveShareClient(this);
            _liveShareClient.MessageReceived += HandleLiveShareMessage;
        }

        private async void StartLiveShare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a new session
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://localhost:5000/create-session", null);
                    var result = await response.Content.ReadFromJsonAsync<LiveShareResponse>();

                    // Store the session URL
                    _currentSessionUrl = result.Link;

                    // Connect to the session
                    await _liveShareClient.ConnectAsync(_currentSessionUrl);

                    // Show the session URL in a custom dialog
                    var linkDialog = new LiveShareLinkDialog(_currentSessionUrl);
                    linkDialog.Owner = this;
                    linkDialog.ShowDialog();

                    // Update UI to show connected state
                    StatusText.Text = "LiveShare Connected (Host)";

                    // Share current file content if any
                    if (TryGetCurrentEditor(out var editor, out var document))
                    {
                        await _liveShareClient.SendFileChangeAsync(document.Title, editor.Text);
                    }

                    // After successful connection, send project structure
                    if (CurrentProject != null)
                    {
                        await _liveShareClient.SendProjectStructure(CurrentProject);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Failed to start LiveShare: {ex.Message}");
            }
        }

        private async void HandleLiveShareMessage(object sender, LiveShareMessage message)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    _isHandlingRemoteChange = true;

                    switch (message.Type)
                    {
                        case "fileChange":
                            HandleFileChange(message);
                            break;

                        case "projectStructure":
                            HandleProjectStructure(message);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling message: {ex}");
                    ShowError($"Error handling LiveShare message: {ex.Message}");
                }
                finally
                {
                    _isHandlingRemoteChange = false;
                }
            });
        }

        private void HandleFileChange(LiveShareMessage message)
        {
            if (string.IsNullOrEmpty(message.FilePath)) return;

            Console.WriteLine($"Received file change for: {message.FilePath}");

            var existingDoc = FindExistingDocument(message.FilePath);
            if (existingDoc == null)
            {
                Console.WriteLine($"Creating new document for {message.FilePath}");
                var newDoc = CreateEditorDocument(message.FilePath, message.Content);
                EditorTabs.Children.Add(newDoc);
            }
            else if (existingDoc.Content is ICSharpCode.AvalonEdit.TextEditor editor)
            {
                Console.WriteLine($"Updating existing document {message.FilePath}");
                var caretOffset = editor.CaretOffset;
                editor.Text = message.Content;
                try
                {
                    editor.CaretOffset = Math.Min(caretOffset, editor.Text.Length);
                }
                catch { /* Ignore caret positioning errors */ }
            }

            StatusText.Text = $"LiveShare: Received update for {message.FilePath}";
        }

        private void HandleProjectStructure(LiveShareMessage message)
        {
            if (message.Project == null) return;

            Console.WriteLine($"Received project structure: {message.Project.Name}");

            // Create a temporary project for participants
            CurrentProject = new SqueakProject
            {
                Name = message.Project.Name,
                RootPath = message.Project.RootPath,
                SourceFiles = message.Project.SourceFiles,
                References = message.Project.References
            };

            // Update UI
            UpdateProjectExplorer();
            StatusText.Text = $"LiveShare: Synchronized project structure";
        }

        private class LiveShareResponse
        {
            public string Link { get; set; }
        }

        private async void JoinLiveShare_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new LiveShareJoinDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    await _liveShareClient.ConnectAsync(dialog.SessionUrl);
                    StatusText.Text = "LiveShare Connected (Participant)";

                    Console.WriteLine("Requesting current files from host");
                    await _liveShareClient.SendMessageAsync(JsonSerializer.Serialize(new LiveShareMessage
                    {
                        Type = "requestFiles"
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error joining session: {ex}");
                    ShowError($"Failed to join LiveShare session: {ex.Message}");
                }
            }
        }

        private async void EndLiveShare_Click(object sender, RoutedEventArgs e)
        {
            if (_liveShareClient?.IsConnected == true)
            {
                await _liveShareClient.DisconnectAsync();
                StatusText.Text = "LiveShare Disconnected";
            }
        }

        public IEnumerable<(string Title, string Content)> GetOpenDocuments()
        {
            return EditorTabs.Children.OfType<LayoutDocument>()
                .Where(doc => doc.Content is ICSharpCode.AvalonEdit.TextEditor)
                .Select(doc => (
                    doc.Title,
                    (doc.Content as ICSharpCode.AvalonEdit.TextEditor).Text
                ));
        }

        // Add this helper method to find the document for an editor
        private LayoutDocument GetDocumentForEditor(ICSharpCode.AvalonEdit.TextEditor editor)
        {
            return EditorTabs.Children
                .OfType<LayoutDocument>()
                .FirstOrDefault(doc => doc.Content == editor);
        }

        // Update the Editor_TextChanged method
        private async void Editor_TextChanged(object sender, EventArgs e)
        {
            if (_isHandlingRemoteChange || _liveShareClient?.IsConnected != true)
                return;

            try
            {
                var editor = sender as ICSharpCode.AvalonEdit.TextEditor;
                if (editor == null) return;

                var doc = GetDocumentForEditor(editor);
                if (doc != null)
                {
                    Console.WriteLine($"Local change detected in {doc.Title}");
                    await _liveShareClient.SendFileChangeAsync(doc.Title, editor.Text);
                    StatusText.Text = $"LiveShare: Sent update for {doc.Title}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending change: {ex}");
                ShowError($"Error sending LiveShare update: {ex.Message}");
            }
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            await SendChatMessage();
        }

        private async void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                e.Handled = true;
                await SendChatMessage();
            }
        }

        private async Task SendChatMessage()
        {
            var message = MessageInput.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            // Add user message
            _chatMessages.Add(new ChatMessage
            {
                Sender = "You",
                Content = message,
                IsUser = true
            });

            MessageInput.Text = string.Empty;
            ScrollToBottom();

            try
            {
                // Get current editor context
                var context = GetEditorContext();

                // Get AI response
                var response = await _aiService.GetResponseAsync(message, context);

                // Add AI response
                _chatMessages.Add(new ChatMessage
                {
                    Sender = "AI Assistant",
                    Content = response,
                    IsUser = false
                });

                ScrollToBottom();
            }
            catch (Exception ex)
            {
                ShowError($"Error getting AI response: {ex.Message}");
            }
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.Dispatcher.InvokeAsync(() =>
            {
                ChatScrollViewer.ScrollToBottom();
            });
        }

        private EditorContext GetEditorContext()
        {
            var currentEditor = GetCurrentEditor();
            return new EditorContext
            {
                CurrentFile = currentEditor != null ? GetDocumentForEditor(currentEditor)?.Title : null,
                CurrentContent = currentEditor?.Text,
                OpenFiles = GetOpenDocuments().ToDictionary(
                    doc => doc.Title,
                    doc => doc.Content
                ),
                ProjectFiles = CurrentProject?.SourceFiles?.ToList() ?? new List<string>()
            };
        }

        private async void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.K && Keyboard.Modifiers == ModifierKeys.Control)
            {
                var editor = sender as ICSharpCode.AvalonEdit.TextEditor;
                if (editor == null) return;
                e.Handled = true;

                // Get selected text or current line
                var selectedText = editor.SelectedText;
                var start = editor.SelectionStart;
                var length = editor.SelectionLength;

                // If no selection, use current line
                if (string.IsNullOrEmpty(selectedText))
                {
                    var line = editor.Document.GetLineByOffset(editor.CaretOffset);
                    start = line.Offset;
                    length = line.Length;
                    selectedText = editor.Document.GetText(start, length);
                }

                var currentDoc = GetDocumentForEditor(editor);
                var fullContext = new EditorContext
                {
                    CurrentFile = currentDoc?.Title,
                    CurrentContent = editor.Document.Text,
                    SelectedText = selectedText,
                    SelectionStart = start,
                    SelectionLength = length
                };

                var dialog = new AIPromptDialog(selectedText);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        // Store original text for restoration
                        var originalText = editor.Document.GetText(start, length);
                        var previewText = new StringBuilder();

                        // Clear the existing text
                        editor.Document.Replace(start, length, "");

                        // Create marker for highlighting
                        var marker = new AIPreviewMarker(editor.Document, start);
                        editor.TextArea.TextView.BackgroundRenderers.Add(marker);

                        // Create floating toolbar
                        var toolbar = new AIPreviewToolbar();
                        var popup = new Popup
                        {
                            Child = toolbar,
                            StaysOpen = true,
                            Placement = PlacementMode.Relative,
                            PlacementTarget = editor
                        };

                        var pos = editor.TextArea.TextView.GetVisualPosition(
                            new TextViewPosition(editor.Document.GetLineByOffset(start).LineNumber, 1),
                            VisualYPosition.LineBottom);
                        popup.HorizontalOffset = pos.X;
                        popup.VerticalOffset = pos.Y;
                        popup.IsOpen = true;

                        // Handle accept/reject
                        toolbar.AcceptClicked += (s, args) =>
                        {
                            editor.TextArea.TextView.BackgroundRenderers.Remove(marker);
                            popup.IsOpen = false;
                        };

                        toolbar.RejectClicked += (s, args) =>
                        {
                            editor.Document.Replace(start, previewText.Length, originalText);
                            editor.TextArea.TextView.BackgroundRenderers.Remove(marker);
                            popup.IsOpen = false;
                        };

                        // Stream the AI response
                        var batchedText = new StringBuilder();
                        var updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
                        updateTimer.Tick += (s, args) =>
                        {
                            if (batchedText.Length > 0)
                            {
                                editor.Document.Insert(start + previewText.Length, batchedText.ToString());
                                previewText.Append(batchedText);
                                marker.Length = previewText.Length;
                                batchedText.Clear();
                            }
                        };
                        updateTimer.Start();

                        await _aiService.StreamCodeCompletionAsync(
                            dialog.UserPrompt,
                            fullContext,
                            token =>
                            {
                                batchedText.Append(token);
                            });

                        updateTimer.Stop();
                        if (batchedText.Length > 0)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                editor.Document.Insert(start + previewText.Length, batchedText.ToString());
                                previewText.Append(batchedText);
                                marker.Length = previewText.Length;
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError($"AI Error: {ex.Message}");
                    }
                }
            }
        }

        private void LogLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogLevelComboBox.SelectedItem == null) return;

            var selectedLevel = ((ComboBoxItem)LogLevelComboBox.SelectedItem).Content.ToString();
            _currentLogLevel = Enum.Parse<LogLevel>(selectedLevel);

            // Recreate logger factory with new minimum level
            InitializeLoggerFactory();

            _loggerFactory.CreateLogger<SqueakSpeakInterpreterVisitor>()
                .LogInformation("Log level changed to: {Level}", _currentLogLevel);
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized || Width == SystemParameters.WorkArea.Width)
            {
                // Restore to default size
                WindowState = WindowState.Normal;
                Width = _defaultWidth;
                Height = _defaultHeight;
                Left = _defaultLeft;
                Top = _defaultTop;
            }
            else
            {
                // Store current size if it's different from default
                if (WindowState == WindowState.Normal)
                {
                    _defaultWidth = Width;
                    _defaultHeight = Height;
                    _defaultLeft = Left;
                    _defaultTop = Top;
                }

                var screen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);
                var workingArea = screen.WorkingArea;
                
                WindowState = WindowState.Normal;
                Left = workingArea.Left;
                Top = workingArea.Top;
                Width = workingArea.Width;
                Height = workingArea.Height;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            }
            else
            {
                DragMove();
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.Source == this)
            {
                Point p = e.GetPosition(this);
                if (p.X <= 6 || p.X >= ActualWidth - 6 || p.Y <= 6 || p.Y >= ActualHeight - 6)
                {
                    ResizeWindow(p);
                }
            }
        }

        private void ResizeWindow(Point p)
        {
            if (p.X <= 6 && p.Y <= 6)
            {
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
                DragResize(HorizontalAlignment.Left, VerticalAlignment.Top);
            }
            else if (p.X >= ActualWidth - 6 && p.Y <= 6)
                DragResize(HorizontalAlignment.Right, VerticalAlignment.Top);
            else if (p.X <= 6 && p.Y >= ActualHeight - 6)
                DragResize(HorizontalAlignment.Left, VerticalAlignment.Bottom);
            else if (p.X >= ActualWidth - 6 && p.Y >= ActualHeight - 6)
                DragResize(HorizontalAlignment.Right, VerticalAlignment.Bottom);
            else if (p.X <= 6)
                DragResize(HorizontalAlignment.Left);
            else if (p.X >= ActualWidth - 6)
                DragResize(HorizontalAlignment.Right);
            else if (p.Y <= 6)
                DragResize(VerticalAlignment.Top);
            else if (p.Y >= ActualHeight - 6)
                DragResize(VerticalAlignment.Bottom);
        }

        private void DragResize(HorizontalAlignment horizontal)
        {
            WindowState = WindowState.Normal;
            DragResize(this, horizontal, VerticalAlignment.Center);
        }

        private void DragResize(VerticalAlignment vertical)
        {
            WindowState = WindowState.Normal;
            DragResize(this, HorizontalAlignment.Center, vertical);
        }

        private void DragResize(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            WindowState = WindowState.Normal;
            DragResize(this, horizontal, vertical);
        }

        private void DragResize(Window window, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            SendMessage(Handle, 0x112, (IntPtr)(61440 + GetResizeMode(horizontal, vertical)), IntPtr.Zero);
        }

        private static int GetResizeMode(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            return vertical switch
            {
                VerticalAlignment.Top when horizontal == HorizontalAlignment.Left => 4,
                VerticalAlignment.Top when horizontal == HorizontalAlignment.Right => 5,
                VerticalAlignment.Top => 3,
                VerticalAlignment.Bottom when horizontal == HorizontalAlignment.Left => 7,
                VerticalAlignment.Bottom when horizontal == HorizontalAlignment.Right => 8,
                VerticalAlignment.Bottom => 6,
                _ when horizontal == HorizontalAlignment.Left => 1,
                _ when horizontal == HorizontalAlignment.Right => 2,
                _ => 0
            };
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void InitializeThemeMenu()
        {
            UpdateCustomThemesMenu();
            ThemeManager.Instance.ApplyTheme("OceanDepths"); // Default theme
        }

        private void UpdateCustomThemesMenu()
        {
            CustomThemesMenu.Items.Clear();
            var themes = ThemeManager.Instance.GetCustomThemes();
            
            foreach (var theme in themes)
            {
                var menuItem = new MenuItem
                {
                    Header = theme.Name,
                    Tag = theme.Name
                };
                menuItem.Click += Theme_Click;
                CustomThemesMenu.Items.Add(menuItem);
            }
        }

        private void Theme_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string themeName = menuItem.Tag.ToString();
                ThemeManager.Instance.ApplyTheme(themeName);
                RefreshOpenEditors();
                RefreshTreeViewTheme();
            }
        }

        private void RefreshTreeViewTheme()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            
            // Set TreeView colors
            FolderExplorer.Background = new SolidColorBrush(theme.PrimaryBackground);
            FolderExplorer.Foreground = new SolidColorBrush(theme.ForegroundColor);
            
            // Clear and reapply ItemContainerStyle to force update
            var style = new Style(typeof(TreeViewItem));
            style.Setters.Add(new Setter(TreeViewItem.BackgroundProperty, new SolidColorBrush(theme.PrimaryBackground)));
            style.Setters.Add(new Setter(TreeViewItem.ForegroundProperty, new SolidColorBrush(theme.ForegroundColor)));
            FolderExplorer.ItemContainerStyle = style;
            
            // Force visual refresh
            FolderExplorer.Items.Refresh();
        }

        private void CreateTheme_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ThemeEditorDialog();
            if (dialog.ShowDialog() == true)
            {
                UpdateCustomThemesMenu();
            }
        }

        private void EditTheme_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ThemeEditorDialog(ThemeManager.Instance.CurrentTheme);
            if (dialog.ShowDialog() == true)
            {
                UpdateCustomThemesMenu();
                ThemeManager.Instance.ApplyTheme(ThemeManager.Instance.CurrentTheme.Name);
                RefreshOpenEditors();
            }
        }

        private void UpdateEditorTheme()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            foreach (var doc in EditorTabs.Children.OfType<LayoutDocument>())
            {
                if (doc.Content is ICSharpCode.AvalonEdit.TextEditor editor)
                {
                    editor.Background = new SolidColorBrush(theme.EditorBackground);
                    editor.Foreground = new SolidColorBrush(theme.EditorText);
                    editor.TextArea.TextView.CurrentLineBackground = 
                        new SolidColorBrush(theme.EditorCurrentLine);

                    // Update syntax highlighting
                    UpdateSyntaxHighlighting(editor.SyntaxHighlighting, theme);
                    editor.TextArea.TextView.Redraw();
                }
            }
        }

        private void UpdateSyntaxHighlighting(IHighlightingDefinition highlighting, Themes.Theme theme)
        {
            foreach (var color in highlighting.NamedHighlightingColors)
            {
                switch (color.Name)
                {
                    case "Comment":
                        color.Foreground = new SimpleHighlightingBrush(theme.CommentColor);
                        break;
                    case "String":
                        color.Foreground = new SimpleHighlightingBrush(theme.StringColor);
                        break;
                    case "Keyword":
                        color.Foreground = new SimpleHighlightingBrush(theme.KeywordColor);
                        break;
                    case "Number":
                        color.Foreground = new SimpleHighlightingBrush(theme.NumberColor);
                        break;
                    case "Operator":
                        color.Foreground = new SimpleHighlightingBrush(theme.OperatorColor);
                        break;
                }
            }
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                ThemesPopup.IsOpen = !ThemesPopup.IsOpen;
            }
        }

        private void DeleteTheme_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && 
                menuItem.Parent is ContextMenu contextMenu && 
                contextMenu.PlacementTarget is MenuItem themeMenuItem)
            {
                string themeName = themeMenuItem.Tag?.ToString() ?? themeMenuItem.Header.ToString();
                ThemeManager.Instance.DeleteTheme(themeName);
                UpdateCustomThemesMenu();
            }
        }

        private void RefreshOpenEditors()
        {
            UpdateEditorTheme();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SettingsDialog(_settings);
            if (dialog.ShowDialog() == true)
            {
                _settings = dialog.Settings;
                _settings.Save();
                _mouseTrail.Clear(); // Reset trail with new settings
            }
        }
    }
}
