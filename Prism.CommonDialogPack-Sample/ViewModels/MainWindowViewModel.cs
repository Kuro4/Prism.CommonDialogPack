using Prism.CommonDialogPack;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Extensions;
using Prism.CommonDialogPack.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Prism.CommonDialogPack_Sample.ViewModels
{
    // TODO: Enrichment of sample app.
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism.CommonDialogPack-Sample";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ReactiveProperty<string> ResultMessage { get; } = new ReactiveProperty<string>();

        public ReactiveCommand ShowNotificationDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowConfirmationDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowSingleFolderSelectDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowMultiFolderSelectDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowSingleFileSelectDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowMultiFileSelectDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowFileSaveDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowCustomizedFileSaveDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowProgressDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowIndeterminateProgreesDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowColorPickerDialogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ShowColorPickerDialogWithDisableCustomColorsStorageCommand { get; } = new ReactiveCommand();

        private readonly IDialogService dialogService;
        private readonly IEventAggregator eventAggregator;

        public MainWindowViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            this.dialogService = dialogService;
            this.eventAggregator = eventAggregator;
            this.ShowNotificationDialogCommand.Subscribe(this.ShowNotificationDialog);
            this.ShowConfirmationDialogCommand.Subscribe(this.ShowConfirmationDialog);
            this.ShowSingleFolderSelectDialogCommand.Subscribe(this.ShowSingleFolderSelectDialog);
            this.ShowMultiFolderSelectDialogCommand.Subscribe(this.ShowMultiFolderSelectDialog);
            this.ShowSingleFileSelectDialogCommand.Subscribe(this.ShowSingleFileSelectDialog);
            this.ShowMultiFileSelectDialogCommand.Subscribe(this.ShowMultiFileSelectDialog);
            this.ShowFileSaveDialogCommand.Subscribe(this.ShowFileSaveDialog);
            this.ShowCustomizedFileSaveDialogCommand.Subscribe(this.ShowCustomizedFileSaveDialog);
            this.ShowProgressDialogCommand.Subscribe(this.ShowProgressDialog);
            this.ShowIndeterminateProgreesDialogCommand.Subscribe(this.ShowIndeterminateProgreesDialog);
            this.ShowColorPickerDialogCommand.Subscribe(this.ShowColorPickerDialog);
            this.ShowColorPickerDialogWithDisableCustomColorsStorageCommand.Subscribe(this.ShowColorPickerDialogWithDisableCustomColorsStorage);
        }
        /// <summary>
        /// Show NotificationDialog.
        /// </summary>
        private void ShowNotificationDialog()
        {
            // Standard
            var param = new DialogParameters
            {
                { DialogParameterNames.Message, "Notification" },
                { DialogParameterNames.Title, "Notification" },
                // When specifying a WindowStyle StyleKey as a string.
                { DialogParameterNames.WindowStyle, "dialogStyle" },
                // When specifying WindowStyle directly.
                //{ DialogParameterNames.WindowStyle, (System.Windows.Style)App.Current.FindResource("dialogStyle") },
            };
            this.dialogService.ShowDialog(DialogNames.Notification, param, res => this.ResultMessage.Value = "Notification");

            // Extensions
            //this.dialogService.ShowNotificationDialog("Notification", "Notification", res => this.ResultMessage.Value = "Notification");
        }
        /// <summary>
        /// Show ConfirmationDialog.
        /// </summary>
        private void ShowConfirmationDialog()
        {
            this.dialogService.ShowConfirmationDialog("Confirmation", "Confirmation?", res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = "Confirmed OK";
                }
                else if (res.Result == ButtonResult.Cancel)
                {
                    this.ResultMessage.Value = "Confirmed Cancel";
                }
                else
                {
                    this.ResultMessage.Value = $"Confirmed {res.Result}";
                }
            });
        }
        /// <summary>
        /// Show SingleFolderSelectDialog.
        /// </summary>
        private void ShowSingleFolderSelectDialog()
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Title, "SingleFolderSelect" },
            };
            this.dialogService.ShowFolderSelectDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = res.SelectedPaths != null && res.SelectedPaths.Any()
                                             ? $"Selected Folder: {res.SelectedPaths.First()}"
                                             : "Folder Not Selected";
                }
                else
                {
                    this.ResultMessage.Value = "Cancel Single Folder Select";
                }
            });
        }
        /// <summary>
        /// Show MultiFolderSelectDialog.
        /// </summary>
        private void ShowMultiFolderSelectDialog()
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Title, "MultiFolderSelect" },
                { DialogParameterNames.CanMultiSelect, true },
            };
            this.dialogService.ShowFolderSelectDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = res.SelectedPaths != null && res.SelectedPaths.Any()
                                             ? $"Selected Folders:{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", res.SelectedPaths)}"
                                             : "Folder Not Selected";
                }
                else
                {
                    this.ResultMessage.Value = "Cancel Multi Folder Select";
                }
            });
        }
        /// <summary>
        /// Show SingleFileSelectDialog.
        /// </summary>
        private void ShowSingleFileSelectDialog()
        {
            var param = new DialogParameters()
            {
                {DialogParameterNames.Title, "SingleFileSelect" },
            };
            this.dialogService.ShowFileSelectDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = res.SelectedPaths != null && res.SelectedPaths.Any()
                                             ? $"Selected File: {res.SelectedPaths.First()}"
                                             : "File Not Selected";
                }
                else
                {
                    this.ResultMessage.Value = "Cancel Single File Select";
                }
            });
        }
        /// <summary>
        /// Show MultiFileSelectDialog.
        /// </summary>
        private void ShowMultiFileSelectDialog()
        {
            // Add File Filters
            var filters = new[]
            {
                new FileFilter("Text File (*.txt; *.csv)", new[] { ".txt", ".csv" }),
                new FileFilter("All Files (*.*)"),
                new FileFilter("Excel File (*.xlsx; *.xlsm; *.xls)", ".xlsx", ".xlsm", ".xls"),
            };
            var param = new DialogParameters()
            {
                { DialogParameterNames.Title, "MultiFileSelect" },
                { DialogParameterNames.Filters, filters },
                { DialogParameterNames.CanMultiSelect, true },
            };
            this.dialogService.ShowFileSelectDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = res.SelectedPaths != null && res.SelectedPaths.Any()
                                             ? $"Selected Files:{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", res.SelectedPaths)}"
                                             : "File Not Selected";
                }
                else
                {
                    this.ResultMessage.Value = "Cancel Multi File Select";
                }
            });
        }
        /// <summary>
        /// Show FileSaveDialog.
        /// </summary>
        private void ShowFileSaveDialog()
        {
            this.dialogService.ShowFileSaveDialog(res =>
            {
                if (res.Result == ButtonResult.OK)
                {

                    this.ResultMessage.Value = $"Save File Path: {res.SaveFilePath}";
                }
                else
                {
                    this.ResultMessage.Value = "Cancel File Save";
                }
            });
        }
        /// <summary>
        /// Show CustomizedFileSaveDialog.
        /// </summary>
        private void ShowCustomizedFileSaveDialog()
        {
            var textResource = new ExplorerBaseTextResource()
            {
                FileName = "Name",
                FileDateModified = "Date modified",
                FileType = "Type",
                FileSize = "Size",
            };
            var filters = new[]
            {
                new FileFilter("All Files (*.*)"),
                new FileFilter("Text File (*.txt)", ".txt"),
                new FileFilter("CSV File (*.csv)",".csv"),
            };
            var icons = new ExplorerIcons()
            {
                BackWardIcon = new BitmapImage(new Uri("/Resources/Backwards_16x.png", UriKind.Relative)),
                ForwardIcon = new BitmapImage(new Uri("/Resources/Forwards_16x.png", UriKind.Relative)),
            };
            Func<string, string> overwriteConfirmationMessageFunc = (x => $"{x} already exists.{Environment.NewLine}Do you want to replace it?");
            var param = new DialogParameters
                {
                    { DialogParameterNames.Title, "FileSave" },
                    { DialogParameterNames.DefaultSaveFileName, "Sample.txt" },
                    { DialogParameterNames.FileNamePrefixText, "File name:" },
                    { DialogParameterNames.FileTypePrefixText, "Save as type:" },
                    { DialogParameterNames.SaveButtonText, "Save" },
                    { DialogParameterNames.CancelButtonText, "Cancel" },
                    { DialogParameterNames.TextResource, textResource },
                    { DialogParameterNames.Filters, filters },
                    { DialogParameterNames.ExplorerIcons, icons },
                    { DialogParameterNames.OverwriteConfirmationTitle, "OverWrite Confirmation" },
                    { DialogParameterNames.OverwriteConfirmationMessageFunc, overwriteConfirmationMessageFunc },
                    { DialogParameterNames.OverwriteConfirmationOKButtonText, "Yes" },
                    { DialogParameterNames.OverwriteConfirmationCancelButtonText, "No" },
                    //{ DialogParameterNames.RootFolders, new [] { @"C:\" } },
                };
            this.dialogService.ShowFileSaveDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = $"Save File Path: {res.SaveFilePath}";
                }
                else
                {
                    this.ResultMessage.Value = "Cancel File Save";
                }
            });
        }
        /// <summary>
        /// Task to publish <see cref="ProgressEvent"/>.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ProgressAsync(int count, CancellationToken cancellationToken)
        {
            int step = count / 20;
            for (int i = 0; i < count; i += step)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
                var eventValue = new ProgressEventValue()
                {
                    Text = $"{i} / {count}",
                    Value = i,
                };
                this.eventAggregator.GetEvent<ProgressEvent>().Publish(eventValue);
                await Task.Delay(100);
            }
        }
        /// <summary>
        /// Show ProgressDialog.
        /// </summary>
        private void ShowProgressDialog()
        {
            Random random = new Random();
            int count = random.Next(0, 5000);
            var tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            var task = this.ProgressAsync(count, cancellationToken);
            var param = new DialogParameters()
            {
                { DialogParameterNames.Message, "Progress..." },
                { DialogParameterNames.Maximum, count },
                { DialogParameterNames.ProgressTask, task},
                { DialogParameterNames.IsNotifyProgressComplete, true },
                { DialogParameterNames.ProgressCompleteNotificationMessage, "Task completed." },
            };
            this.dialogService.ShowProgressDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = "Progress completed.";
                }
                else if (res.Result == ButtonResult.Cancel)
                {
                    tokenSource.Cancel();
                    this.ResultMessage.Value = "Progress canceled.";
                }
                else
                {
                    tokenSource.Cancel();
                    this.ResultMessage.Value = "Progress quit.";
                }
                tokenSource.Dispose();
            });

            // ProgressCompleteEvent.Publish() can also be used to notify the completion of a task.
            //bool isCancellationRequested = false;
            //Action action = async () =>
            //{
            //    for (int i = 0; i < count; i += step)
            //    {
            //        if (isCancellationRequested)
            //        {
            //            break;
            //        }
            //        var eventValue = new ProgressEventValue()
            //        {
            //            Text = $"{i} / {count}",
            //            Value = i,
            //        };
            //        this.eventAggregator.GetEvent<ProgressEvent>().Publish(eventValue);
            //        await Task.Delay(100);
            //    }
            //    if (!isCancellationRequested)
            //    {
            //        this.eventAggregator.GetEvent<ProgressCompleteEvent>().Publish();
            //    }
            //};
            //var param = new DialogParameters()
            //{
            //    { DialogParameterNames.Message, "Progress..." },
            //    { DialogParameterNames.Maximum, count },
            //};
            //action.Invoke();
            //this.dialogService.ShowDialog(DialogNames.ProgressDialog, param, res =>
            //{
            //    if (res.Result == ButtonResult.OK)
            //    {
            //        this.ResultMessage.Value = "Progress completed.";
            //    }
            //    else if (res.Result == ButtonResult.Cancel)
            //    {
            //        isCancellationRequested = true;
            //        this.ResultMessage.Value = "Progress canceled.";
            //    }
            //    else
            //    {
            //        isCancellationRequested = true;
            //        this.ResultMessage.Value = "Progress quit.";
            //    }
            //});
        }
        /// <summary>
        /// Task to publish <see cref="ProgressEvent"/> with text only.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task IndeterminateProgressAsync(int count, CancellationToken cancellationToken)
        {
            int step = count / 20;
            for (int i = 0; i < count; i += step)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
                var eventValue = new ProgressEventValue()
                {
                    Text = $"{i}",
                };
                this.eventAggregator.GetEvent<ProgressEvent>().Publish(eventValue);
                await Task.Delay(100);
            }
        }
        /// <summary>
        /// Show indeterminate ProgressDialog.
        /// </summary>
        private void ShowIndeterminateProgreesDialog()
        {
            Random random = new Random();
            int count = random.Next(0, 5000);
            var tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            var task = this.IndeterminateProgressAsync(count, cancellationToken);
            var param = new DialogParameters()
            {
                { DialogParameterNames.Message, "Progress..." },
                { DialogParameterNames.IsIndeterminate, true },
                { DialogParameterNames.ProgressTask, task},
                { DialogParameterNames.IsNotifyProgressComplete, true },
                { DialogParameterNames.ProgressCompleteNotificationMessage, "Task completed." },
            };
            this.dialogService.ShowProgressDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    this.ResultMessage.Value = "Progress (Indeterminate) completed.";
                }
                else if (res.Result == ButtonResult.Cancel)
                {
                    tokenSource.Cancel();
                    this.ResultMessage.Value = "Progress (Indeterminate) canceled.";
                }
                else
                {
                    tokenSource.Cancel();
                    this.ResultMessage.Value = "Progress (Indeterminate) quit.";
                }
                tokenSource.Dispose();
            });
        }
        /// <summary>
        /// Show ColorPickerDialog.
        /// </summary>
        private void ShowColorPickerDialog()
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Title, "ColorPicker" },
            };
            this.dialogService.ShowColorPickerDialog(param, res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("ColorPicker OK");
                    stringBuilder.AppendLine($"RGB: {res.RGB}");
                    stringBuilder.AppendLine($"HSV: {res.HSV}");
                    stringBuilder.AppendLine($"ColorCode: {res.ColorCode}");
                    this.ResultMessage.Value = stringBuilder.ToString();
                }
                else if (res.Result == ButtonResult.Cancel)
                {
                    this.ResultMessage.Value = $"ColorPicker Cancel";
                }
                else
                {
                    this.ResultMessage.Value = $"ColorPicker {res.Result}";
                }
            });
        }
        /// <summary>
        /// Show ColorPickerDialog with DisableCustomColorsStorage.
        /// </summary>
        private void ShowColorPickerDialogWithDisableCustomColorsStorage()
        {
            DialogSettings.DisableCustomColorsStorage();
            this.dialogService.ShowColorPickerDialog(res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("ColorPicker OK");
                    stringBuilder.AppendLine($"RGB: {res.RGB}");
                    stringBuilder.AppendLine($"HSV: {res.HSV}");
                    stringBuilder.AppendLine($"ColorCode: {res.ColorCode}");
                    this.ResultMessage.Value = stringBuilder.ToString();
                }
                else if (res.Result == ButtonResult.Cancel)
                {
                    this.ResultMessage.Value = $"ColorPicker Cancel";
                }
                else
                {
                    this.ResultMessage.Value = $"ColorPicker {res.Result}";
                }
            });
            DialogSettings.EnableCustomColorsStorage();
        }
    }
}
