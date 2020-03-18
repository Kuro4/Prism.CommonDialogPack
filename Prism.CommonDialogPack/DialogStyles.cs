using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Prism.CommonDialogPack
{
    public static class DialogStyles
    {
        public static readonly Style CommonDialogStyle = new Style(typeof(Window));
        public static readonly Style ExplorerDialogStyle = new Style(typeof(Window));

        static DialogStyles()
        {
            CommonDialogStyle.Setters.Add(new Setter(Window.ResizeModeProperty, ResizeMode.NoResize));

            ExplorerDialogStyle.Setters.Add(new Setter(Window.ResizeModeProperty, ResizeMode.CanResizeWithGrip));
            ExplorerDialogStyle.Setters.Add(new Setter(Window.MinWidthProperty, 525D));
            ExplorerDialogStyle.Setters.Add(new Setter(Window.MinHeightProperty, 350D));
        }
    }
}
