using Prism.Common;
using Prism.CommonDialogPack.Models;
using Prism.CommonDialogPack.ViewModels;
using Prism.Regions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        }

        private void ExplorerBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Configure ViewModel
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
            vm.FileExtensions = regionContext.FileExtensions;
            vm.History.Clear();
            if (regionContext.RootFolders != null)
            {
                vm.InitializeRootFolders(regionContext.RootFolders.Select(x => new Folder(x)));
            }
        }

        private void CurrentFolderPathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox) || e.Key != Key.Enter)
            {
                return;
            }
            this.FilesDataGrid.Focus();
        }

        private void CurrentFolderPathTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox))
            {
                return;
            }
            textBox.SelectAll();
        }

        private void CurrentFolderPathTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is TextBox textBox))
            {
                return;
            }
            if (!textBox.IsFocused)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }

        private void FilesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Synchronize with IsSelected
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
