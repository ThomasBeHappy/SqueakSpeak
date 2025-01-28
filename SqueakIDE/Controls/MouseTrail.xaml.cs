using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Reflection;
using System;
using System.Windows.Input;
using SqueakIDE.Settings;
using System.Linq;
using System.IO;

namespace SqueakIDE.Controls
{
    public class MouseTrail : Canvas
    {
        private readonly List<UIElement> _trailElements = new();
        private readonly Queue<Point> _trailPoints = new();
        private Point _lastPoint;
        private Point _lastValidPoint;
        private readonly IDESettings _settings;
        private readonly BitmapImage _pawPrint;
        private double _lastAngle;
        private int _pawCount = 0;
        private const double MOVEMENT_THRESHOLD = 20.0; // Minimum movement to create new paw
        private const double SMOOTHING_FACTOR = 0.4; // Lower = smoother movement (0-1)
        private DateTime _lastMovementTime;
        private const double FADE_OUT_DELAY_MS = 1000; // Start fading after 1 second of no movement
        private const double FADE_OUT_DURATION_MS = 1000; // Take 1 second to fade out
        private bool _isFading;
        
        public MouseTrail()
        {
            _settings = IDESettings.Load();
            IsHitTestVisible = false;
            
            // Load paw print image
            _pawPrint = new BitmapImage();
            _pawPrint.BeginInit();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SqueakIDE.Resources.paw.png"))
            {
                if (stream != null)
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    
                    _pawPrint.StreamSource = memoryStream;
                    _pawPrint.CacheOption = BitmapCacheOption.OnLoad;
                }
            }
            _pawPrint.EndInit();
            
            // Don't try to freeze the BitmapImage directly
            
            CompositionTarget.Rendering += OnRendering;
            _lastMovementTime = DateTime.Now;
            _isFading = false;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (!_settings.EnableMouseTrail) return;

            var currentPos = Mouse.GetPosition(this);
            
            // Smooth the movement
            var smoothedX = _lastValidPoint.X + (currentPos.X - _lastValidPoint.X) * SMOOTHING_FACTOR;
            var smoothedY = _lastValidPoint.Y + (currentPos.Y - _lastValidPoint.Y) * SMOOTHING_FACTOR;
            var smoothedPos = new Point(smoothedX, smoothedY);
            
            // Calculate movement distance
            var distance = Point.Subtract(smoothedPos, _lastPoint).Length;
            
            if (distance > MOVEMENT_THRESHOLD)
            {
                // Calculate movement angle and add 180 degrees here
                var deltaX = smoothedPos.X - _lastPoint.X;
                var deltaY = smoothedPos.Y - _lastPoint.Y;
                var angle = (Math.Atan2(deltaY, deltaX) * (180 / Math.PI)) + 180;
                
                // Smooth the rotation
                angle = _lastAngle + (angle - _lastAngle) * SMOOTHING_FACTOR;
                
                AddTrailElement(smoothedPos, angle);
                _lastPoint = smoothedPos;
                _lastAngle = angle;
                _lastMovementTime = DateTime.Now;
                _isFading = false;
            }
            
            // Check if we should start fading
            var timeSinceLastMovement = (DateTime.Now - _lastMovementTime).TotalMilliseconds;
            if (!_isFading && timeSinceLastMovement > FADE_OUT_DELAY_MS)
            {
                StartFadeOut();
            }
            
            _lastValidPoint = smoothedPos;
            UpdateTrail();
        }

        private void StartFadeOut()
        {
            _isFading = true;
            
            foreach (var element in _trailElements)
            {
                var fadeOut = new DoubleAnimation(
                    element.Opacity,
                    0,
                    TimeSpan.FromMilliseconds(FADE_OUT_DURATION_MS)
                )
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                
                fadeOut.Completed += (s, e) =>
                {
                    if (_isFading) // Only remove if we're still in fade-out mode
                    {
                        Children.Remove(element);
                    }
                };
                
                element.BeginAnimation(OpacityProperty, fadeOut);
            }

            // Also fade out any existing sparkles
            foreach (var child in Children.OfType<Ellipse>().ToList())
            {
                var fadeOut = new DoubleAnimation(
                    child.Opacity,
                    0,
                    TimeSpan.FromMilliseconds(FADE_OUT_DURATION_MS / 2) // Sparkles fade faster
                );
                
                fadeOut.Completed += (s, e) => Children.Remove(child);
                child.BeginAnimation(OpacityProperty, fadeOut);
            }
        }

        private void AddTrailElement(Point position, double angle)
        {
            // If we were fading, clear any remaining faded elements
            if (_isFading)
            {
                _trailElements.Clear();
                _trailPoints.Clear();
                Children.Clear();
                _isFading = false;
                _pawCount = 0;
            }

            // Create alternating left/right paw prints using the counter
            var isLeft = _pawCount % 2 == 0;
            _pawCount++;
            
            // Calculate perpendicular offset for left/right placement
            var perpendicularAngle = angle - 90;
            var radians = perpendicularAngle * Math.PI / 180;
            var offsetDistance = 8;
            
            // Calculate offset position - simplified approach
            var offsetX = -Math.Sin(angle * Math.PI / 180) * offsetDistance * (isLeft ? 1 : -1);
            var offsetY = Math.Cos(angle * Math.PI / 180) * offsetDistance * (isLeft ? 1 : -1);
            
            // Apply offset to position
            var adjustedPosition = new Point(
                position.X + offsetX,
                position.Y + offsetY
            );

            var image = new Image
            {
                Source = _pawPrint,
                Width = 20,
                Height = 20,
                Opacity = 1, // Start invisible for fade-in
                RenderTransformOrigin = new Point(0.5, 0.5),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Gray,
                    Direction = 315,
                    ShadowDepth = 2,
                    Opacity = 0.3,
                    BlurRadius = 4
                }
            };

            var transformGroup = new TransformGroup();
            
            // Base rotation for movement direction - both paws should face the same way
            var baseRotation = angle - 90;
            transformGroup.Children.Add(new RotateTransform(baseRotation));
            
            // Add slight tilt for more natural look
            var tilt = isLeft ? -15 : 15;
            transformGroup.Children.Add(new RotateTransform(tilt));
            
            image.RenderTransform = transformGroup;

            SetLeft(image, adjustedPosition.X - 10);
            SetTop(image, adjustedPosition.Y - 10);

            _trailElements.Add(image);
            _trailPoints.Enqueue(position);
            Children.Add(image);

            // Animate fade in
            var fadeIn = new DoubleAnimation(0, _settings.TrailOpacity, TimeSpan.FromMilliseconds(150));
            image.BeginAnimation(OpacityProperty, fadeIn);

            // Add sparkles if enabled
            if (_settings.EnableSparkles)
            {
                AddSparkles(position);
            }

            // Remove old elements if we exceed the trail length
            while (_trailElements.Count > _settings.TrailLength)
            {
                var oldElement = _trailElements[0];
                _trailElements.RemoveAt(0);
                _trailPoints.Dequeue();
                
                // Animate fade out
                var fadeOut = new DoubleAnimation(oldElement.Opacity, 0, TimeSpan.FromMilliseconds(200));
                fadeOut.Completed += (s, e) => Children.Remove(oldElement);
                oldElement.BeginAnimation(OpacityProperty, fadeOut);
            }
        }

        private void AddSparkles(Point position)
        {
            var random = new Random();
            for (int i = 0; i < _settings.SparkleCount; i++)
            {
                var sparkle = new Ellipse
                {
                    Width = random.Next(2, 5),
                    Height = random.Next(2, 5),
                    Fill = new RadialGradientBrush(
                        Colors.Gold,
                        Color.FromArgb(0, 255, 215, 0)
                    ),
                    Effect = new BlurEffect { Radius = 2 },
                    Opacity = _settings.TrailOpacity
                };

                // Random offset for sparkle position
                var angle = random.NextDouble() * Math.PI * 2;
                var distance = random.Next(5, 15);
                var offsetX = Math.Cos(angle) * distance;
                var offsetY = Math.Sin(angle) * distance;

                SetLeft(sparkle, position.X + offsetX);
                SetTop(sparkle, position.Y + offsetY);

                Children.Add(sparkle);

                // Animate sparkle
                var fadeOut = new DoubleAnimation(_settings.TrailOpacity, 0, TimeSpan.FromMilliseconds(400));
                var scaleTransform = new ScaleTransform(1, 1);
                sparkle.RenderTransform = scaleTransform;
                
                var scaleAnimation = new DoubleAnimation(1, 0.1, TimeSpan.FromMilliseconds(400));
                
                fadeOut.Completed += (s, e) => Children.Remove(sparkle);
                sparkle.BeginAnimation(OpacityProperty, fadeOut);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            }
        }

        private void UpdateTrail()
        {
            // Update opacity of existing elements based on their position in the trail
            for (int i = 0; i < _trailElements.Count; i++)
            {
                var element = _trailElements[i];
                var targetOpacity = _settings.TrailOpacity * (1 - (double)i / _settings.TrailLength);
                
                // Smoothly animate to target opacity
                var currentOpacity = element.Opacity;
                if (Math.Abs(currentOpacity - targetOpacity) > 0.01)
                {
                    var opacityAnimation = new DoubleAnimation(
                        targetOpacity,
                        TimeSpan.FromMilliseconds(100)
                    );
                    element.BeginAnimation(OpacityProperty, opacityAnimation);
                }
            }
        }

        public void Clear()
        {
            _isFading = true;
            foreach (var element in _trailElements)
            {
                var fadeOut = new DoubleAnimation(
                    element.Opacity,
                    0,
                    TimeSpan.FromMilliseconds(FADE_OUT_DURATION_MS)
                )
                {
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                fadeOut.Completed += (s, e) => Children.Remove(element);
                element.BeginAnimation(OpacityProperty, fadeOut);
            }
            _trailElements.Clear();
            _trailPoints.Clear();
        }
    }
} 