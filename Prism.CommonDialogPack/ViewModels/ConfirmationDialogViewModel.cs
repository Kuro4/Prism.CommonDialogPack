using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ConfirmationDialogViewModel : DialogViewModelBase
    {
        private DelegateCommand okCommand;
        public DelegateCommand OKCommand => okCommand ?? (okCommand = new DelegateCommand(OK));

        private DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand => cancelCommand ?? (cancelCommand = new DelegateCommand(Cancel));

        private string message;
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        public string okButtonText = "OK";
        public string OKButtonText
        {
            get { return this.okButtonText; }
            set { SetProperty(ref this.okButtonText, value); }
        }

        public string cancelButtonText = "Cancel";
        public string CancelButtonText
        {
            get { return this.cancelButtonText; }
            set { SetProperty(ref this.cancelButtonText, value); }
        }

        public ConfirmationDialogViewModel()
        {
            this.Title = "Confirmation";
        }

        protected virtual void OK()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        protected virtual void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            this.Message = parameters.GetValue<string>(DialogParameterName.Message);
            if (parameters.TryGetValue(DialogParameterName.OKButtonText, out string okButtonText))
            {
                this.OKButtonText = okButtonText;
            }
            if (parameters.TryGetValue(DialogParameterName.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
            }
        }
    }
}
