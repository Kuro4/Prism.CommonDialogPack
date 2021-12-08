using Prism.CommonDialogPack;
using Prism.CommonDialogPack.Extensions;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ReactiveCommand ShowNotificationDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowConfirmationDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowSingleFolderSelectDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowMultiFolderSelectDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowSingleFileSelectDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowMultiFileSelectDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowFileSaveDialog { get; } = new ReactiveCommand();
        public ReactiveCommand ShowCustomizedFileSaveDialog { get; } = new ReactiveCommand();

        private readonly IDialogService dialogService;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.ShowNotificationDialog.Subscribe(_ =>
            {
                // Standard
                //var param = new DialogParameters
                //{
                //    { DialogParameterNames.Message, "Notification" },
                //    { DialogParameterNames.Title, "Notification" },
                //    { DialogParameterNames.WindowStyle, (System.Windows.Style)App.Current.FindResource("dialogStyle") },
                //};
                //this.dialogService.ShowDialog(DialogNames.Notification, param, res => this.ResultMessage.Value = "Notification");

                // Extensions
                this.dialogService.ShowNotification("Notification", "Notification", res => this.ResultMessage.Value = "Notification");
            });
            this.ShowConfirmationDialog.Subscribe(_ =>
            {
                this.dialogService.ShowConfirmation("Confirmation?", "Confirmation", res =>
                {
                    if (res.Result == ButtonResult.OK)
                        this.ResultMessage.Value = "Confirmed OK";
                    else if (res.Result == ButtonResult.Cancel)
                        this.ResultMessage.Value = "Confirmed Cancel";
                    else
                        this.ResultMessage.Value = $"Confirmed {res.Result}";
                });
            });
            this.ShowSingleFolderSelectDialog.Subscribe(_ =>
            {
                this.dialogService.ShowFolderSelectDialog("SingleFolderSelect", false, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var selectedPath = res.Parameters.GetValue<IEnumerable<string>>(DialogResultParameterNames.SelectedPaths).First();
                        this.ResultMessage.Value = $"Selected Folder: {selectedPath}";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Cancel Single Folder Select";
                    }
                });
            });
            this.ShowMultiFolderSelectDialog.Subscribe(_ =>
            {
                this.dialogService.ShowFolderSelectDialog("MultiFolderSelect", true, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var selectedPaths = res.Parameters.GetValue<IEnumerable<string>>(DialogResultParameterNames.SelectedPaths);
                        this.ResultMessage.Value = $"Selected Folders:{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", selectedPaths)}";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Cancel Multi Folder Select";
                    }
                });
            });
            this.ShowSingleFileSelectDialog.Subscribe(_ =>
            {
                this.dialogService.ShowFileSelectDialog("SingleFileSelect", false, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var selectedPaths = res.Parameters.GetValue<IEnumerable<string>>(DialogResultParameterNames.SelectedPaths);
                        if (selectedPaths != null && selectedPaths.Any())
                            this.ResultMessage.Value = $"Selected File: {selectedPaths.First()}";
                        else
                            this.ResultMessage.Value = "File Not Selected";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Cancel Single File Select";
                    }
                });
            });
            this.ShowMultiFileSelectDialog.Subscribe(_ =>
            {
                var filters = new[]
                {
                    new FileFilter("All Files (*.*)"),
                    new FileFilter("Text File (*.txt; *.csv)", new[] { ".txt", ".csv" }),
                    new FileFilter("Excel File (*.xlsx; *.xlsm; *.xls)", ".xlsx", ".xlsm", ".xls"),
                };
                this.dialogService.ShowFileSelectDialog("MultiFileSelect", true, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var selectedPaths = res.Parameters.GetValue<IEnumerable<string>>(DialogResultParameterNames.SelectedPaths);
                        if (selectedPaths != null && selectedPaths.Any())
                            this.ResultMessage.Value = $"Selected Files:{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", selectedPaths)}";
                        else
                            this.ResultMessage.Value = "File Not Selected";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Cancel Multi File Select";
                    }
                },
                // Add File Filters
                filters: filters);
            });
            this.ShowFileSaveDialog.Subscribe(_ =>
            {
                this.dialogService.ShowFileSaveDialog("FileSave", res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var saveFilePath = res.Parameters.GetValue<string>(DialogResultParameterNames.SaveFilePath);
                        this.ResultMessage.Value = $"Save File Path: {saveFilePath}";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Cancel File Save";
                    }
                });
            });
            this.ShowCustomizedFileSaveDialog.Subscribe(_ =>
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
                this.dialogService.ShowDialog(DialogNames.FileSaveDialog, param, res =>
                {
                    if (res.Result == ButtonResult.OK)
                    {
                        var saveFilePath = res.Parameters.GetValue<string>(DialogResultParameterNames.SaveFilePath);
                        this.ResultMessage.Value = $"Save File Path: {saveFilePath}";
                    }
                    else
                    {
                        this.ResultMessage.Value = "Cancel File Save";
                    }
                });
            });
        }
    }
}
