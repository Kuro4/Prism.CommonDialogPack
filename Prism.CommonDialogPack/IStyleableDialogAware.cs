using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Prism.CommonDialogPack
{
    public interface IStyleableDialogAware : IDialogAware
    {
        Style WindowStyle { get; }
        double Width { get; }
        double Height { get; }
        double ContentWidth { get; }
        double ContentHeight { get; }
    }
}
