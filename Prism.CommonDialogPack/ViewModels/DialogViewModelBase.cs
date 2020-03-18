using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Prism.CommonDialogPack.ViewModels
{
    public class DialogViewModelBase : BindableBase, IStyleableDialogAware
    {
        private string title = string.Empty;
        public string Title
        {
            get { return this.title; }
            set { SetProperty(ref this.title, value); }
        }

        private Style windowStyle = DialogStyles.CommonDialogStyle;
        public Style WindowStyle
        {
            get { return this.windowStyle; }
            set { SetProperty(ref this.windowStyle, value); }
        }

        public double Width { get; set; } = 300;

        public double Height { get; set; } = 150;

        private double contentWidth = double.NaN;
        public double ContentWidth
        {
            get { return this.contentWidth; }
            set { SetProperty(ref this.contentWidth, value); }
        }

        private double contentHeight = double.NaN;
        public double ContentHeight
        {
            get { return this.contentHeight; }
            set { SetProperty(ref this.contentHeight, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            this.RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue(DialogParameterNames.Title, out string title))
            {
                this.Title = title;
            }
            if (parameters.TryGetValue(DialogParameterNames.Width, out double width))
            {
                this.Width = width;
            }
            if (parameters.TryGetValue(DialogParameterNames.Height, out double height))
            {
                this.Height = height;
            }
            if (parameters.TryGetValue(DialogParameterNames.WindowStyle, out Style windowStyle))
            {
                this.WindowStyle = windowStyle;
            }
        }
    }
}
