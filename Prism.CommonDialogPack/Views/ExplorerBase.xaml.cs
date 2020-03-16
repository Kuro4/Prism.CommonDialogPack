using Prism.Common;
using Prism.CommonDialogPack.Models;
using Prism.CommonDialogPack.ViewModels;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Prism.CommonDialogPack.Views
{
    /// <summary>
    /// Interaction logic for ExplorerBase
    /// </summary>
    public partial class ExplorerBase : UserControl
    {
        public ExplorerBase()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += ExplorerBase_PropertyChanged;
            this.FilesDataGrid.LoadingRow += (s, e) =>
            {
                e.Row.MouseDoubleClick += this.Row_MouseDoubleClick;
                e.Row.KeyDown += this.Row_KeyDown;
            };
            this.FilesDataGrid.UnloadingRow += (s, e) =>
            {
                e.Row.MouseDoubleClick -= this.Row_MouseDoubleClick;
                e.Row.KeyDown -= this.Row_KeyDown;
            };
        }

        private void ExplorerBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var vm = (ExplorerBaseViewModel)this.DataContext;
            var regionContext = (ExplorerBaseRegionContext)context.Value;
            vm.NameColumnText = regionContext.TextResource.FileName;
            vm.DateModifiedColumnText = regionContext.TextResource.FileDateModified;
            vm.TypeColumnText = regionContext.TextResource.FileType;
            vm.SizeColumnText = regionContext.TextResource.FileSize;
            vm.DisplayTarget = regionContext.DisplayTarget;
            vm.SelectionTarget = regionContext.SelectionTarget;
            vm.CanMultiSelect = regionContext.CanMultiSelect;
            if ((vm.FileExtensions == null && regionContext.FileExtensions != null) || 
                (vm.FileExtensions != null && regionContext.FileExtensions == null) ||
                (vm.FileExtensions != null && regionContext.FileExtensions != null && !vm.FileExtensions.SequenceEqual(regionContext.FileExtensions)))
            {
                vm.FileExtensions = regionContext.FileExtensions;
            }
            if (regionContext.RootFolders is null || !regionContext.RootFolders.Any() || vm.RootFolders.Any()) return;
            vm.History.Clear();
            vm.RootFolders.Clear();
            vm.RootFolders.AddRange(regionContext.RootFolders.Select(x => new Folder(x)));
            vm.RootFolders.First().IsSelected = true;
        }

        private void Row_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is DataGridRow row) || !row.IsSelected) return;
            var file = (ICommonFileSystemInfo)row.Item;
            if (file.FileType == FileType.Unknown) return;
            this.DisplayFolderPathTextBox.Text = file.Path;
            var binding = this.DisplayFolderPathTextBox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void Row_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!(sender is DataGridRow row) || !row.IsSelected || e.Key != System.Windows.Input.Key.Enter) return;
            var file = (ICommonFileSystemInfo)row.Item;
            e.Handled = true;
            if (file.FileType == FileType.Unknown) return;
            this.DisplayFolderPathTextBox.Text = file.Path;
            var binding = this.DisplayFolderPathTextBox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void DisplayFolderPathTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!(sender is TextBox) || e.Key != System.Windows.Input.Key.Enter) return;
            this.FilesDataGrid.Focus();
        }

        private void DisplayFolderPathTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            textBox.SelectAll();
        }

        private void DisplayFolderPathTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (!textBox.IsFocused)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }

        private void FilesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (ICommonFileSystemInfo file in e.AddedItems)
            {
                file.IsSelected = true;
            }
            foreach (ICommonFileSystemInfo file in e.RemovedItems)
            {
                file.IsSelected = false;
            }
        }
    }
}
