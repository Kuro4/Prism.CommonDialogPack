using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Prism.CommonDialogPack.Behaviors
{
    public static class BringableTreeViewItemBehavior
    {
        public static bool GetIsBringTreeViewItem(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(IsBringTreeViewItemProperty);
        }
        public static void SetIsBringTreeViewItem(TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(IsBringTreeViewItemProperty, value);
        }
        public static readonly DependencyProperty IsBringTreeViewItemProperty = 
            DependencyProperty.RegisterAttached("IsBringTreeViewItem", typeof(bool), typeof(BringableTreeViewItemBehavior), new UIPropertyMetadata(false, OnIsBringTreeViewItemChanged));
        static void OnIsBringTreeViewItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TreeViewItem item) || !(e.NewValue is bool newValue)) return;
            if (newValue)
            {
                item.Selected += OnTreeViewItemSelected;
                if (item.IsSelected)
                {
                    item.RaiseEvent(new RoutedEventArgs(TreeViewItem.SelectedEvent));
                }
            }
            else
            {
                item.Selected -= OnTreeViewItemSelected;
            }
        }
        static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(sender, e.OriginalSource)) return;
            if (e.OriginalSource is TreeViewItem item)
            {
                item.BringIntoView();
            }
        }
    }
}
