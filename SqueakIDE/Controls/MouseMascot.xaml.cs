using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;

namespace SqueakIDE.Controls;

public partial class MouseMascot : UserControl
{
    private readonly int SPRITE_WIDTH = 16;
    private readonly int SPRITE_HEIGHT = 16;
    private WriteableBitmap _spriteSheet;
    private int _currentFrame = 0;
    private Dictionary<string, (int StartFrame, int EndFrame, double FrameDelay)> _animations = new Dictionary<string, (int StartFrame, int EndFrame, double FrameDelay)>
    {
        { "idle", (144, 144, 0.2) },
        { "walk_up", (147, 150, 0.15) },
        { "walk_down", (151, 154, 0.15) },
        { "walk_left", (155, 158, 0.15) },
        { "walk_right", (159, 162, 0.15) }, 
    };

    private readonly Dictionary<string, string> _reactions = new Dictionary<string, string>
    {
        { "success", "Squeak! Your code runs perfectly! 🧀" },
        { "error", "Oh no! We found a bug! Let's fix it! 🐛" },
        { "save", "Your cheese is safely stored! 📝" },
        { "compile", "Let me check this code... *sniff* *sniff*" }
    };

    private CancellationTokenSource _animationCts = new CancellationTokenSource();

    public MouseMascot()
    {
        InitializeComponent();
        LoadSpriteSheet();
        _ = StartAnimation("idle"); // Fire and forget the initial idle animation
    }

    private void LoadSpriteSheet()
    {
        // Load the spritesheet
        var uri = new Uri("pack://application:,,,/SqueakIDE;component/Resources/mouse_spritesheet.png");
        var bitmap = new BitmapImage(uri);
        _spriteSheet = new WriteableBitmap(bitmap);
    }

    private async Task PlayAnimation(string animationName)
    {
        if (_animations.TryGetValue(animationName, out var animation))
        {
            for (int frame = animation.StartFrame; frame <= animation.EndFrame; frame++)
            {
                UpdateSprite(frame);
                await Task.Delay(TimeSpan.FromSeconds(animation.FrameDelay));
            }
        }
    }

    private void UpdateSprite(int frameNumber)
    {
        // Calculate source rectangle from spritesheet
        int row = frameNumber / (_spriteSheet.PixelWidth / SPRITE_WIDTH);
        int col = frameNumber % (_spriteSheet.PixelWidth / SPRITE_WIDTH);
        
        Int32Rect sourceRect = new Int32Rect(
            col * SPRITE_WIDTH,
            row * SPRITE_HEIGHT,
            SPRITE_WIDTH,
            SPRITE_HEIGHT
        );

        // Update the image source
        MouseImage.Source = new CroppedBitmap(_spriteSheet, sourceRect);
    }

    private async Task StartAnimation(string animationName)
    {
        // Cancel any existing animation
        _animationCts.Cancel();
        _animationCts = new CancellationTokenSource();

        try
        {
            while (!_animationCts.Token.IsCancellationRequested)
            {
                await PlayAnimation(animationName);
                
                // If it's not an idle animation, return to idle after one play
                if (animationName != "idle")
                {
                    animationName = "idle";
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Animation was cancelled, this is expected
        }
    }
} 