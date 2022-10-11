using Prism.Services.Dialogs;
using System.Collections.Generic;

namespace Prism.CommonDialogPack.Extensions
{
    public class PathSelectDialogResult : IDialogResult
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
        /// Selected paths.
        /// </summary>
        public IEnumerable<string> SelectedPaths { get; }
        /// <summary>
        /// Initialize a new instance of the <see cref="PathSelectDialogResult"/> class.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="result"></param>
        /// <param name="selectedPaths"></param>
        public PathSelectDialogResult(IDialogParameters parameters, ButtonResult result, IEnumerable<string> selectedPaths)
        {
            Parameters = parameters;
            Result = result;
            SelectedPaths = selectedPaths;
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="PathSelectDialogResult"/> class.
        /// <br/>Get SelectedPaths from IDialogParameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="result"></param>
        public PathSelectDialogResult(IDialogParameters parameters, ButtonResult result)
        {
            Parameters = parameters;
            Result = result;
            if (parameters.TryGetValue(DialogResultParameterNames.SelectedPaths, out IEnumerable<string> selectedPaths))
            {
                this.SelectedPaths = selectedPaths;
            }
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="PathSelectDialogResult"/> class.
        /// <br/>Get SelectedPaths from IDialogParameters.
        /// </summary>
        /// <param name="dialogResult"></param>
        public PathSelectDialogResult(IDialogResult dialogResult) : this(dialogResult.Parameters, dialogResult.Result)
        {
        }
    }
}
