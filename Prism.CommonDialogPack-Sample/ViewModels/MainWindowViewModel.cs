using Prism.CommonDialogPack;
using Prism.CommonDialogPack.Extensions;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public ReactiveCommand ShowFolderSelectDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowFileSelectDialog { get; } = new ReactiveCommand();

        private readonly IDialogService dialogService;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.ShowNotificationDialog.Subscribe(() =>
            {
                var param = new DialogParameters
                {
                    { DialogParameterNames.Message, "Notification Called" },
                    { DialogParameterNames.Title, "Hoge" },
                    //{ DialogParameterName.WindowStyle, (System.Windows.Style)App.Current.FindResource("dialogStyle") },
                };
                this.dialogService.ShowDialog(DialogNames.Notification, param, res =>
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
            this.ShowFolderSelectDialog.Subscribe(() =>
            {
                var param = new DialogParameters
                {
                    { DialogParameterNames.Title, "Hoge" },
                    //{ DialogParameterNames.CanMultiSelect, true },
                };
                this.dialogService.ShowDialog("FolderSelectDialog", param, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var selectedPaths = res.Parameters.GetValue<IEnumerable<string>>(DialogParameterNames.SelectedPaths);
                        this.ResultMessage.Value = $"Selected Folders:{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", selectedPaths)}";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Folder Select Cancel!";
                    }
                });
            });
            this.ShowFileSelectDialog.Subscribe(() =>
            {
                var filters = new[]
                {
                    new FileFilter("すべてのファイル (*.*)"),
                    new FileFilter("テキストファイル (*.txt; *.csv)", new[] { ".txt", ".csv" }),
                    new FileFilter("エクセルファイル (*.xlsx; *.xlsm; *.xls)", ".xlsx", ".xlsm", ".xls"),
                };
                var param = new DialogParameters
                {
                    { DialogParameterNames.Title, "Hoge" },
                    { DialogParameterNames.CanMultiSelect, true },
                    { DialogParameterNames.Filters, filters },
                };
                this.dialogService.ShowDialog("FileSelectDialog", param, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var selectedPaths = res.Parameters.GetValue<IEnumerable<string>>(DialogParameterNames.SelectedPaths);
                        this.ResultMessage.Value = $"Selected Files:{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", selectedPaths)}";
                    }
                    else
                    {
                        this.ResultMessage.Value = "File Select Cancel!";
                    }
                });
            });
        }
    }
}
