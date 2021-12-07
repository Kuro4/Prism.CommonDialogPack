using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace Prism.CommonDialogPack.ViewModels
{
    public abstract class DialogViewModelBase : BindableBase, IStyleableDialogAware
    {
        private string title = string.Empty;
        /// <summary>
        /// Dialog window title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { SetProperty(ref this.title, value); }
        }

        private double contentWidth = double.NaN;
        /// <summary>
        /// Dialog content width.
        /// </summary>
        public double ContentWidth
        {
            get { return this.contentWidth; }
            set { SetProperty(ref this.contentWidth, value); }
        }

        private double contentHeight = double.NaN;
        /// <summary>
        /// Dialog content height.
        /// </summary>
        public double ContentHeight
        {
            get { return this.contentHeight; }
            set { SetProperty(ref this.contentHeight, value); }
        }

        public WindowStartupLocation StartupLocation { get; set; } = WindowStartupLocation.CenterOwner;
        public ResizeMode ResizeMode { get; set; } = ResizeMode.NoResize;
        public SizeToContent SizeToContent { get; set; } = SizeToContent.Manual;
        public Style WindowStyle { get; set; }
        public double Width { get; set; } = double.NaN;
        public double Height { get; set; } = double.NaN;
        /// <summary>
        /// Instructs the <see cref="IDialogWindow"/> to close the dialog.
        /// </summary>
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// Raise request close using <paramref name="dialogResult"/>.
        /// </summary>
        /// <param name="dialogResult"></param>
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            this.RequestClose?.Invoke(dialogResult);
        }
        /// <summary>
        /// Determines if the dialog can be closed.
        /// </summary>
        /// <returns>If true the dialog can be closed. If false the dialog will not close.</returns>
        public virtual bool CanCloseDialog()
        {
            return true;
        }
        /// <summary>
        /// Called when the dialog is closed.
        /// </summary>
        public virtual void OnDialogClosed()
        {
        }
        /// <summary>
        /// Called when the dialog is opened.
        /// </summary>
        /// <param name="parameters">The parameters passed to the dialog.</param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            // Configure parameters
            if (parameters.TryGetValue(DialogParameterNames.Title, out string title))
            {
                this.Title = title;
            }
            if (parameters.TryGetValue(DialogParameterNames.StartupLoaction, out WindowStartupLocation startupLoaction))
            {
                this.StartupLocation = startupLoaction;
            }
            if (parameters.TryGetValue(DialogParameterNames.ResizeMode, out ResizeMode resizeMode))
            {
                this.ResizeMode = resizeMode;
            }
            if (parameters.TryGetValue(DialogParameterNames.SizeToContent, out SizeToContent sizeToContent))
            {
                this.SizeToContent = sizeToContent;
            }
            if (parameters.TryGetValue(DialogParameterNames.WindowStyle, out Style windwoStyle))
            {
                this.WindowStyle = windwoStyle;
            }
            if (parameters.TryGetValue(DialogParameterNames.Width, out double width))
            {
                this.Width = width;
            }
            if (parameters.TryGetValue(DialogParameterNames.Height, out double height))
            {
                this.Height = height;
            }
            if (parameters.TryGetValue(DialogParameterNames.ContentWidth, out double contentWidth))
            {
                this.ContentWidth = contentWidth;
            }
            if (parameters.TryGetValue(DialogParameterNames.ContentHeight, out double contentHeight))
            {
                this.ContentHeight = contentHeight;
            }
        }
    }
}
