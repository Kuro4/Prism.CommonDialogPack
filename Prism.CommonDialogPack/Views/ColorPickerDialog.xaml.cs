using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prism.CommonDialogPack.Views
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog
    /// </summary>
    public partial class ColorPickerDialog : UserControl
    {
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        private void DisplayColorCode_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayColorCode.Visibility = Visibility.Hidden;
            this.InputColorCode.Visibility = Visibility.Visible;
            this.InputColorCode.Text = this.DisplayColorCode.Text;
            this.InputColorCode.Focus();
            this.InputColorCode.SelectAll();
        }

        private void InputColorCode_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayColorCode.Visibility = Visibility.Visible;
            this.InputColorCode.Visibility = Visibility.Hidden;
        }

        private void DisplayHue_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayHue.Visibility = Visibility.Hidden;
            this.InputHue.Visibility = Visibility.Visible;
            this.InputHue.Text = this.DisplayHue.Text;
            this.InputHue.Focus();
            this.InputHue.SelectAll();
        }

        private void InputHue_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayHue.Visibility = Visibility.Visible;
            this.InputHue.Visibility = Visibility.Hidden;
        }

        private void DisplaySaturation_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplaySaturation.Visibility = Visibility.Hidden;
            this.InputSaturation.Visibility = Visibility.Visible;
            this.InputSaturation.Text = this.DisplaySaturation.Text;
            this.InputSaturation.Focus();
            this.InputSaturation.SelectAll();
        }

        private void InputSaturation_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplaySaturation.Visibility = Visibility.Visible;
            this.InputSaturation.Visibility = Visibility.Hidden;
        }

        private void DisplayBrightness_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayBrightness.Visibility = Visibility.Hidden;
            this.InputBrightness.Visibility = Visibility.Visible;
            this.InputBrightness.Text = this.DisplayBrightness.Text;
            this.InputBrightness.Focus();
            this.InputBrightness.SelectAll();
        }

        private void InputBrightness_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayBrightness.Visibility = Visibility.Visible;
            this.InputBrightness.Visibility = Visibility.Hidden;
        }

        private void DisplayRed_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayRed.Visibility = Visibility.Hidden;
            this.InputRed.Visibility = Visibility.Visible;
            this.InputRed.Text = this.DisplayRed.Text;
            this.InputRed.Focus();
            this.InputRed.SelectAll();
        }

        private void InputRed_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayRed.Visibility = Visibility.Visible;
            this.InputRed.Visibility = Visibility.Hidden;
        }

        private void DisplayGreen_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayGreen.Visibility = Visibility.Hidden;
            this.InputGreen.Visibility = Visibility.Visible;
            this.InputGreen.Text = this.DisplayGreen.Text;
            this.InputGreen.Focus();
            this.InputGreen.SelectAll();
        }

        private void InputGreen_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayGreen.Visibility = Visibility.Visible;
            this.InputGreen.Visibility = Visibility.Hidden;
        }

        private void DisplayBlue_GotFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayBlue.Visibility = Visibility.Hidden;
            this.InputBlue.Visibility = Visibility.Visible;
            this.InputBlue.Text = this.DisplayBlue.Text;
            this.InputBlue.Focus();
            this.InputBlue.SelectAll();
        }

        private void InputBlue_LostFocus(object sender, RoutedEventArgs e)
        {
            this.DisplayBlue.Visibility = Visibility.Visible;
            this.InputBlue.Visibility = Visibility.Hidden;
        }

        private void Input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                FrameworkElement element = sender as FrameworkElement;
                element?.MoveFocus(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next));
            }
        }

        private void BasicRectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.BasicColors.SelectedItem is null)
            {
                return;
            }
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed || e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                BindingOperations.GetBindingExpression(this.BasicColors, System.Windows.Controls.Primitives.Selector.SelectedItemProperty).UpdateSource();
            }
        }

        private void CustomRectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.CustomColors.SelectedItem is null)
            {
                return;
            }
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed || e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                BindingOperations.GetBindingExpression(this.CustomColors, System.Windows.Controls.Primitives.Selector.SelectedItemProperty).UpdateSource();
            }
        }
    }
}
