using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Extensions;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ProgressDialogViewModel : DialogViewModelBase
    {
        private DelegateCommand cancelCommand;
        /// <summary>
        /// Close command.
        /// </summary>
        public DelegateCommand CancelCommand => this.cancelCommand ??= new DelegateCommand(this.Cancel);

        private string message = string.Empty;
        /// <summary>
        /// Message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        private string progressText;
        /// <summary>
        /// Progress message.
        /// </summary>
        public string ProgressText
        {
            get { return progressText; }
            set { SetProperty(ref this.progressText, value); }
        }

        private string cancelButtonText = "キャンセル";
        /// <summary>
        /// Button text.
        /// </summary>
        public string CancelButtonText
        {
            get { return this.cancelButtonText; }
            set { SetProperty(ref this.cancelButtonText, value); }
        }

        private bool isIndeterminate = false;
        /// <summary>
        /// IsIndeterminate.
        /// </summary>
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            set { SetProperty(ref this.isIndeterminate, value); }
        }

        private double maximum = 100;
        /// <summary>
        /// Maximum.
        /// </summary>
        public double Maximum
        {
            get { return maximum; }
            set { SetProperty(ref this.maximum, value); }
        }

        private double minimum = 0;
        /// <summary>
        /// Minimum.
        /// </summary>
        public double Minimum
        {
            get { return minimum; }
            set { SetProperty(ref this.minimum, value); }
        }

        private double progressValue = 0;
        /// <summary>
        /// Progress value.
        /// </summary>
        public double ProgressValue
        {
            get { return progressValue; }
            set { SetProperty(ref this.progressValue, value); }
        }
        /// <summary>
        /// Whether to show notification dialog when a task is completed.
        /// </summary>
        private bool IsNotifyProgressComplete { get; set; } = false;
        /// <summary>
        /// Notification dialog Title.
        /// </summary>
        private string ProgressCompleteNotificationTitle { get; set; } = string.Empty;
        /// <summary>
        /// Notification dialog Message.
        /// </summary>
        private string ProgressCompleteNotificationMessage { get; set; } = string.Empty;
        /// <summary>
        /// Notification dialog ButtonText.
        /// </summary>
        private string ProgressCompleteNotificationButtonText { get; set; }
        /// <summary>
        /// Event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;
        /// <summary>
        /// Dialog service.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        /// Initialize a new instance of the <see cref="ProgressDialogViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public ProgressDialogViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            this.eventAggregator = eventAggregator;
            this.dialogService = dialogService;
            this.Title = "Progress";
            this.Width = 300;
            this.Height = 150;
        }

        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.Cancel"/>.
        /// </summary>
        protected virtual void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="parameters"><inheritdoc/></param>
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            // Subscribe event
            this.eventAggregator.GetEvent<ProgressEvent>().Subscribe(this.OnProgress);
            this.eventAggregator.GetEvent<ProgressCompleteEvent>().Subscribe(this.OnProgressComplete);
            // Configure parameters
            if (parameters.TryGetValue(DialogParameterNames.Message, out string message))
            {
                this.Message = message;
            }
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.IsIndeterminate, out bool isIndeterminate))
            {
                this.IsIndeterminate = isIndeterminate;
            }
            if (parameters.TryGetValue(DialogParameterNames.Maximum, out double maximum))
            {
                this.Maximum = maximum;
            }
            if (parameters.TryGetValue(DialogParameterNames.Minimum, out double minimum))
            {
                this.Minimum = minimum;
            }
            if (parameters.TryGetValue(DialogParameterNames.IsNotifyProgressComplete, out bool isNotifyProgressComplete))
            {
                this.IsNotifyProgressComplete = isNotifyProgressComplete;
            }
            if (parameters.TryGetValue(DialogParameterNames.ProgressCompleteNotificationTitle, out string progreesCompleteNotificationTitle))
            {
                this.ProgressCompleteNotificationTitle = progreesCompleteNotificationTitle;
            }
            if (parameters.TryGetValue(DialogParameterNames.ProgressCompleteNotificationMessage, out string progressCompleteNotificationMessage))
            {
                this.ProgressCompleteNotificationMessage = progressCompleteNotificationMessage;
            }
            if (parameters.TryGetValue(DialogParameterNames.ProgressCompleteNotificationButtonText, out string progressCompleteNotificationButtonText))
            {
                this.ProgressCompleteNotificationButtonText = progressCompleteNotificationButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.ProgressTask, out Task task))
            {
                Dispatcher.CurrentDispatcher.Invoke(async () =>
                {
                    try
                    {
                        await task;
                        this.OnProgressComplete();
                    }
                    catch (OperationCanceledException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                });        
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnDialogClosed()
        {
            base.OnDialogClosed();
            // Unsubscribe event
            this.eventAggregator.GetEvent<ProgressEvent>().Unsubscribe(this.OnProgress);
            this.eventAggregator.GetEvent<ProgressCompleteEvent>().Unsubscribe(this.OnProgressComplete);
        }
        /// <summary>
        /// Called when progress.
        /// </summary>
        /// <param name="eventValue"></param>
        public void OnProgress(ProgressEventValue eventValue)
        {
            this.ProgressText = eventValue.Text;
            this.ProgressValue = eventValue.Value;
        }
        /// <summary>
        /// Called when progress completed.
        /// </summary>
        public void OnProgressComplete()
        {
            if (this.IsNotifyProgressComplete)
            {
                var param = new DialogParameters()
                {
                    { DialogParameterNames.Title, this.ProgressCompleteNotificationTitle },
                    { DialogParameterNames.Message, this.ProgressCompleteNotificationMessage },
                    { DialogParameterNames.OKButtonText, this.ProgressCompleteNotificationButtonText },  
                };
                this.dialogService.ShowNotificationDialog(param, null);
            }
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }
    }
}
