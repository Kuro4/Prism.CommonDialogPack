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
            this.Loaded += this.FileSaveDialog_Loaded;
        }

        private void SaveFileNameTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            textBox.SelectAll();
        }

        private void SaveFileNameTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (!textBox.IsFocused)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }

        private void FileSaveDialog_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.SaveFileNameTextBox.Focus();
            this.Loaded -= this.FileSaveDialog_Loaded;
        }
    }
}
