using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ConfirmationDialogViewModel : DialogViewModelBase
    {
        private DelegateCommand okCommand;
        /// <summary>
        /// OK command.
        /// </summary>
        public DelegateCommand OKCommand => this.okCommand ??= new DelegateCommand(this.OK);

        private DelegateCommand cancelCommand;
        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand => this.cancelCommand ??= new DelegateCommand(this.Cancel);

        private string message = string.Empty;
        /// <summary>
        /// Confirmation message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        public string okButtonText = "OK";
        /// <summary>
        /// OK button text.
        /// </summary>
        public string OKButtonText
        {
            get { return this.okButtonText; }
            set { SetProperty(ref this.okButtonText, value); }
        }

        public string cancelButtonText = "キャンセル";
        /// <summary>
        /// Cancel button text.
        /// </summary>
        public string CancelButtonText
        {
            get { return this.cancelButtonText; }
            set { SetProperty(ref this.cancelButtonText, value); }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfirmationDialogViewModel"/> class.
        /// </summary>
        public ConfirmationDialogViewModel()
        {
            this.Title = "Confirmation";
            this.Width = 300;
            this.Height = 150;
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.OK"/>.
        /// </summary>
        protected virtual void OK()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.Cancel"/>.
        /// </summary>
        protected virtual void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
        /// <summary>
        /// Called when the dialog is opened.
        /// </summary>
        /// <param name="parameters">The parameters passed to the dialog.</param>
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            // Configure parameters
            if (parameters.TryGetValue(DialogParameterNames.Message, out string message))
            {
                this.Message = message;
            }
            if (parameters.TryGetValue(DialogParameterNames.OKButtonText, out string okButtonText))
            {
                this.OKButtonText = okButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
            }
        }
    }
}
