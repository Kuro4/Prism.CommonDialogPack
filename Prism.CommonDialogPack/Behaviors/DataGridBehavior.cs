using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prism.CommonDialogPack.Behaviors
{
    public class DataGridBehavior : Behavior<DataGrid>
    {
        public ICommand EnterKeyCommand
        {
            get { return (ICommand)GetValue(EnterKeyCommandProperty); }
            set { SetValue(EnterKeyCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ForwardCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterKeyCommandProperty =
            DependencyProperty.Register("EnterKeyCommand", typeof(ICommand), typeof(DataGridBehavior), new PropertyMetadata(null));

        public ICommand BackKeyCommand
        {
            get { return (ICommand)GetValue(BackKeyCommandProperty); }
            set { SetValue(BackKeyCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BackwardCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackKeyCommandProperty =
            DependencyProperty.Register("BackKeyCommand", typeof(ICommand), typeof(DataGridBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Go Backward Command
        /// </summary>
        public ICommand XButton1Command
        {
            get { return (ICommand)GetValue(XButton1CommandProperty); }
            set { SetValue(XButton1CommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for XButton1Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XButton1CommandProperty =
            DependencyProperty.Register("XButton1Command", typeof(ICommand), typeof(DataGridBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Go Forward Commnad
        /// </summary>
        public ICommand XButton2Command
        {
            get { return (ICommand)GetValue(XButton2CommandProperty); }
            set { SetValue(XButton2CommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for XButton2Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XButton2CommandProperty =
            DependencyProperty.Register("XButton2Command", typeof(ICommand), typeof(DataGridBehavior), new PropertyMetadata(null));

        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DoubleClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register("DoubleClickCommand", typeof(ICommand), typeof(DataGridBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.DataGrid_Loaded;
            this.AssociatedObject.PreviewKeyDown += this.DataGrid_PreviewKeyDown;
            this.AssociatedObject.PreviewMouseDown += this.DataGrid_PreviewMouseDown;
            this.AssociatedObject.LoadingRow += this.DataGrid_LoadingRow;
            this.AssociatedObject.UnloadingRow += this.DataGrid_UnloadingRow;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= this.DataGrid_Loaded;
            this.AssociatedObject.PreviewKeyDown -= this.DataGrid_PreviewKeyDown;
            this.AssociatedObject.PreviewMouseDown -= this.DataGrid_PreviewMouseDown;
            this.AssociatedObject.LoadingRow -= this.DataGrid_LoadingRow;
            this.AssociatedObject.UnloadingRow -= this.DataGrid_UnloadingRow;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.FirstRowOrDataGridFocus();
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is DataGrid))
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Back:
                    BackKeyCommand?.Execute(null);
                    this.FirstRowOrDataGridFocus();
                    break;
                default:
                    break;
            }
        }

        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.XButton1:
                    XButton1Command?.Execute(null);
                    break;
                case MouseButton.XButton2:
                    XButton2Command?.Execute(null);
                    break;
                default:
                    return;
            }
            this.FirstRowOrDataGridFocus();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.PreviewKeyDown += this.Row_PreviewKeyDown;
            e.Row.MouseDoubleClick += this.OnPreviewMouseDoubleClick;
        }

        private void DataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.PreviewKeyDown -= this.Row_PreviewKeyDown;
            e.Row.MouseDoubleClick -= this.OnPreviewMouseDoubleClick;
        }

        private void Row_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is DataGridRow row))
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Enter:
                    EnterKeyCommand?.Execute(row.Item);
                    break;
                case Key.Back:
                    BackKeyCommand?.Execute(null);
                    break;
                default:
                    return;
            }
            this.FirstRowOrDataGridFocus();
            e.Handled = true;
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DataGridRow row) || e.ChangedButton != MouseButton.Left)
            {
                return;
            }
            DoubleClickCommand?.Execute(row.Item);
            this.FirstRowOrDataGridFocus();
        }

        private void FirstRowOrDataGridFocus()
        {
            this.AssociatedObject.UpdateLayout();
            if (this.AssociatedObject.HasItems)
            {
                // Focus to first row
                this.AssociatedObject.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                this.AssociatedObject.SelectedIndex = 0;
            }
            else
            {
                // Focus to DataGrid
                this.AssociatedObject.Focus();
            }
        }
    }
}
