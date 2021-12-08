using Prism.Services.Dialogs;
using System.Windows;

namespace Prism.CommonDialogPack
{
    public interface IStyleableDialogAware : IDialogAware
    {
        /// <summary>
        /// Dialog startup location.
        /// </summary>
        WindowStartupLocation StartupLocation { get; set; }
        /// <summary>
        /// Dialog resize mode.
        /// </summary>
        ResizeMode ResizeMode { get; set; }
        /// <summary>
        /// Dialog size to content.
        /// </summary>
        SizeToContent SizeToContent { get; set; }
        /// <summary>
        /// Dialog window style.
        /// </summary>
        Style WindowStyle { get; set; }
        /// <summary>
        /// Dialog window width.
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// Dialog window height.
        /// </summary>
        double Height { get; set; }
        /// <summary>
        /// Dialog content width.
        /// </summary>
        double ContentWidth { get; set; }
        /// <summary>
        /// Dialog content height.
        /// </summary>
        double ContentHeight { get; set; }
    }
}
