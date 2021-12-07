using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows;

namespace Prism.CommonDialogPack.ViewModels
{
    public class NotificationDialogViewModel : DialogViewModelBase
    {
        private DelegateCommand closeDialogCommand;
        /// <summary>
        /// Close command.
        /// </summary>
        public DelegateCommand CloseDialogCommand => this.closeDialogCommand ??= new DelegateCommand(this.CloseDialog);

        private string message = string.Empty;
        /// <summary>
        /// Notification message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        private string buttonText = "OK";
        /// <summary>
        /// Button text.
        /// </summary>
        public string ButtonText
        {
            get { return this.buttonText; }
            set { SetProperty(ref this.buttonText, value); }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="NotificationDialogViewModel"/> class.
        /// </summary>
        public NotificationDialogViewModel()
        {
            this.Title = "Notification";
            this.Width = 300;
            this.Height = 150;
        }
        /// <summary>
        /// Close the dialog.
        /// </summary>
        protected virtual void CloseDialog()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK));
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
            if (parameters.TryGetValue(DialogParameterNames.ButtonText, out string buttonText))
            {
                this.ButtonText = buttonText;
            }            
        }
    }
}
