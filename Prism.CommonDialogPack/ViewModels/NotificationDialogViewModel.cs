using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Prism.CommonDialogPack.ViewModels
{
    public class NotificationDialogViewModel : DialogViewModelBase
    {
        private DelegateCommand closeDialogCommand;
        public DelegateCommand CloseDialogCommand => closeDialogCommand ?? (closeDialogCommand = new DelegateCommand(CloseDialog));

        private string message;
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        private string buttonText = "OK";
        public string ButtonText
        {
            get { return this.buttonText; }
            set { SetProperty(ref this.buttonText, value); }
        }

        public NotificationDialogViewModel()
        {
            this.Title = "Notification";
        }

        protected virtual void CloseDialog()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            this.Message = parameters.GetValue<string>(DialogParameterName.Message);
            if (parameters.TryGetValue(DialogParameterName.ButtonText, out string buttonText))
            {
                this.ButtonText = buttonText;
            }
        }
    }
}
