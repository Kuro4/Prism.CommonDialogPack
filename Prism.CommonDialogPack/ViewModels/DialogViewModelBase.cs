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

        private double width = 300;
        public double Width
        {
            get { return this.width; }
            set { SetProperty(ref this.width, value); }
        }

        private double height = 150;
        public double Height
        {
            get { return this.height; }
            set { SetProperty(ref this.height, value); }
        }

        private Style windowStyle = DialogStyles.CommonDialogStyle;
        public Style WindowStyle
        {
            get { return this.windowStyle; }
            set { SetProperty(ref this.windowStyle, value); }
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
            if (parameters.TryGetValue(DialogParameterName.Title, out string title))
            {
                this.Title = title;
            }
            if (parameters.TryGetValue(DialogParameterName.Width, out double width))
            {
                this.Width = width;
            }
            if (parameters.TryGetValue(DialogParameterName.Height, out double height))
            {
                this.Height = height;
            }
            if (parameters.TryGetValue(DialogParameterName.WindowStyle, out Style windowStyle))
            {
                this.WindowStyle = windowStyle;
            }
        }
    }
}
