using System.Windows.Controls;

namespace Prism.CommonDialogPack.Views
{
    /// <summary>
    /// Interaction logic for FolderSelectDialog
    /// </summary>
    public partial class FolderSelectDialog : UserControl
    {
        public FolderSelectDialog()
        {
            InitializeComponent();
        }

        private void SelectedFolderPathTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            textBox.SelectAll();
        }

        private void SelectedFolderPathTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
