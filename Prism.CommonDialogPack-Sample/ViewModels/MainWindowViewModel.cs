using Prism.CommonDialogPack;
using Prism.CommonDialogPack.Extensions;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;

namespace Prism.CommonDialogPack_Sample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ReactiveProperty<string> ResultMessage { get; } = new ReactiveProperty<string>();

        public ReactiveCommand ShowNotificationDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowConfirmationDialog { get; } = new ReactiveCommand();

        private readonly IDialogService dialogService;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.ShowNotificationDialog.Subscribe(() =>
            {
                var param = new DialogParameters
                {
                    { DialogParameterName.Message, "Notification Called" },
                    { DialogParameterName.Title, "Hoge" },
                    { DialogParameterName.WindowStyle, (System.Windows.Style)App.Current.FindResource("dialogStyle") },
                };
                this.dialogService.ShowDialog(DialogName.Notification, param, res =>
                {
                    this.ResultMessage.Value = "Notification!";
                });
            });
            this.ShowConfirmationDialog.Subscribe(() =>
            {
                this.dialogService.ShowConfirmation("Confirmation Called", "Confirmation", res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        this.ResultMessage.Value = "Confirmed OK!";
                    }
                    else if (res.Result == ButtonResult.Cancel)
                    {
                        this.ResultMessage.Value = "Confirmed Cancel!";
                    }
                    else
                    {
                        this.ResultMessage.Value = $"Confirmed {res.Result.ToString()}";
                    }
                });
            });
        }
    }
}
