using System.Windows.Controls;

namespace Prism.CommonDialogPack.Views
{
    /// <summary>
    /// Interaction logic for FileSaveDialog
    /// </summary>
    public partial class FileSaveDialog : UserControl
    {
        public FileSaveDialog()
        {
            InitializeComponent();
        }

        private void SelectedFileNameTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            textBox.SelectAll();
        }

        private void SelectedFileNameTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (!textBox.IsFocused)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }
    }
}
