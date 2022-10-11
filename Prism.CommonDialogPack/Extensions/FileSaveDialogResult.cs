using Prism.Services.Dialogs;

namespace Prism.CommonDialogPack.Extensions
{
    public class FileSaveDialogResult : IDialogResult
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IDialogParameters Parameters { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ButtonResult Result { get; }
        /// <summary>
        /// Save file path.
        /// </summary>
        public string SaveFilePath { get; }
        /// <summary>
        /// Initialize a new instance of the <see cref="FileSaveDialogResult"/> class.
        /// </summary>
        /// <param name="dialogParameters"></param>
        /// <param name="buttonResult"></param>
        /// <param name="saveFilePath"></param>
        public FileSaveDialogResult(IDialogParameters dialogParameters, ButtonResult buttonResult, string saveFilePath)
        {
            this.Parameters = dialogParameters;
            this.Result = buttonResult;
            this.SaveFilePath = saveFilePath;
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="FileSaveDialogResult"/> class.
        /// <br/>Get SaveFilePath from IDialogParameters.
        /// </summary>
        /// <param name="dialogParameters"></param>
        /// <param name="buttonResult"></param>
        public FileSaveDialogResult(IDialogParameters dialogParameters, ButtonResult buttonResult)
        {
            this.Parameters = dialogParameters;
            this.Result = buttonResult;
            if (dialogParameters.TryGetValue(DialogResultParameterNames.SaveFilePath, out string saveFilePath))
            {
                this.SaveFilePath = saveFilePath;
            }
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="FileSaveDialogResult"/> class.
        /// <br/>Get SaveFilePath from IDialogParameters.
        /// </summary>
        /// <param name="dialogResult"></param>
        public FileSaveDialogResult(IDialogResult dialogResult) : this(dialogResult.Parameters, dialogResult.Result)
        {
        }
    }
}
