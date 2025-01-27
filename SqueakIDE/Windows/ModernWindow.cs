using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;

namespace SqueakIDE.Windows
{
    public class ModernWindow : Window
    {
        private Button minimizeButton;
        private Button maximizeButton;
        private Button closeButton;
        private Grid titleBar;
        private bool _isResizing;
        private Point _startPoint;
        private ResizeMode _previousResizeMode;
        private Rect _previousPosition;
        private const int RESIZE_BORDER = 6;
        private double _defaultWidth;
        private double _defaultHeight;
        private double _defaultLeft;
        private double _defaultTop;
        private IntPtr Handle { get; set; }

        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernWindow), 
                new FrameworkPropertyMetadata(typeof(ModernWindow)));
        }

        public ModernWindow()
        {
            this.ResizeMode = ResizeMode.CanResize;
            this.SourceInitialized += (s, e) =>
            {
                Handle = new WindowInteropHelper(this).Handle;
                StoreCurrentPosition();
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_TitleBar") is Grid titleBar)
            {
                titleBar.MouseLeftButtonDown += TitleBar_MouseLeftButtonDown;
            }

            if (GetTemplateChild("MinimizeButton") is Button minimizeButton)
            {
                minimizeButton.Click += MinimizeButton_Click;
            }

            if (GetTemplateChild("MaximizeButton") is Button maximizeButton)
            {
                maximizeButton.Click += MaximizeButton_Click;
            }

            if (GetTemplateChild("CloseButton") is Button closeButton)
            {
                closeButton.Click += CloseButton_Click;
            }
        }

        private void StoreCurrentPosition()
        {
            _defaultWidth = Width;
            _defaultHeight = Height;
            _defaultLeft = Left;
            _defaultTop = Top;
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

                var screen = System.Windows.Forms.Screen.FromHandle(Handle);
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
            e.Handled = true; // Prevent the event from bubbling up
            
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            }
            else if (e.ButtonState == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch (InvalidOperationException)
                {
                    // Button was released before we could start the drag
                }
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
    }
}