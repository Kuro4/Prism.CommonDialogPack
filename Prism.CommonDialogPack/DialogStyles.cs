using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Prism.CommonDialogPack
{
    public static class DialogStyles
    {
        public static readonly Style CommonDialogStyle = new Style(typeof(Window));

        static DialogStyles()
        {
            CommonDialogStyle.Setters.Add(new Setter(Window.SizeToContentProperty, SizeToContent.WidthAndHeight));
            CommonDialogStyle.Setters.Add(new Setter(Window.ResizeModeProperty, ResizeMode.NoResize));
        }
    }
}
