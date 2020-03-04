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
    }
}
