using System.Windows;
using SqueakIDE.Settings;
using SqueakIDE.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace SqueakIDE.Dialogs
{
    public partial class SettingsDialog : ModernWindow
    {
        public IDESettings Settings { get; private set; }

        public SettingsDialog(IDESettings currentSettings)
        {
            InitializeComponent();
            Settings = currentSettings;
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            // Mouse Trail settings
            EnableTrailCheckbox.IsChecked = Settings.EnableMouseTrail;
            EnableSparklesCheckbox.IsChecked = Settings.EnableSparkles;
            TrailLengthSlider.Value = Settings.TrailLength;
            TrailOpacitySlider.Value = Settings.TrailOpacity;
            SparkleCountSlider.Value = Settings.SparkleCount;

            // Set the selected color in the combo box
            foreach (ComboBoxItem item in TrailColorComboBox.Items)
            {
                if ((string)item.Tag == Settings.TrailColor)
                {
                    TrailColorComboBox.SelectedItem = item;
                    break;
                }
            }

            // Mouse Mascot settings
            EnableMascotCheckbox.IsChecked = Settings.EnableMascot;
            EnableSoundsCheckbox.IsChecked = Settings.EnableMascotSounds;
            MascotScaleSlider.Value = Settings.MascotScale;
        }

        private void SaveSettings()
        {
            // Mouse Trail settings
            Settings.EnableMouseTrail = EnableTrailCheckbox.IsChecked ?? false;
            Settings.EnableSparkles = EnableSparklesCheckbox.IsChecked ?? false;
            Settings.TrailLength = (int)TrailLengthSlider.Value;
            Settings.TrailOpacity = TrailOpacitySlider.Value;
            Settings.SparkleCount = (int)SparkleCountSlider.Value;

            // Get selected color from combo box
            if (TrailColorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                Settings.TrailColor = (string)selectedItem.Tag;
            }

            // Mouse Mascot settings
            Settings.EnableMascot = EnableMascotCheckbox.IsChecked ?? false;
            Settings.EnableMascotSounds = EnableSoundsCheckbox.IsChecked ?? false;
            Settings.MascotScale = MascotScaleSlider.Value;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 