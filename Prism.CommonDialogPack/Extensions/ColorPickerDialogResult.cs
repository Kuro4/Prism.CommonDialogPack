using Prism.CommonDialogPack.Models;
using Prism.Services.Dialogs;
using System;

namespace Prism.CommonDialogPack.Extensions
{
    public class ColorPickerDialogResult : IDialogResult
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
        /// RGB.
        /// </summary>
        public RGB RGB { get; }
        /// <summary>
        /// HSV.
        /// </summary>
        public HSV HSV { get; }
        /// <summary>
        /// Color Code.
        /// </summary>
        public string ColorCode { get; }
        /// <summary>
        /// Initialize a new instance of the <see cref="ColorPickerDialogResult"/> class.
        /// </summary>
        /// <param name="dialogParameters"></param>
        /// <param name="buttonResult"></param>
        /// <param name="rgb"></param>
        /// <param name="hsv"></param>
        /// <param name="colorCode"></param>
        public ColorPickerDialogResult(IDialogParameters dialogParameters, ButtonResult buttonResult, RGB rgb, HSV hsv, string colorCode)
        {
            this.Parameters = dialogParameters;
            this.Result = buttonResult;
            this.RGB = rgb;
            this.HSV = hsv;
            this.ColorCode = colorCode;
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="ColorPickerDialogResult"/> class.
        /// <br/>Get RGB, HSV, and ColorCode from IDialogParameters.
        /// </summary>
        /// <param name="dialogParameters"></param>
        /// <param name="buttonResult"></param>
        /// <exception cref="ArgumentException"></exception>
        public ColorPickerDialogResult(IDialogParameters dialogParameters, ButtonResult buttonResult)
        {
            this.Parameters = dialogParameters;
            this.Result = buttonResult;
            if (dialogParameters.TryGetValue(DialogResultParameterNames.RGB, out RGB rgb))
            {
                this.RGB = rgb;
            }
            if (dialogParameters.TryGetValue(DialogResultParameterNames.HSV, out HSV hsv))
            {
                this.HSV = hsv;
            }
            if (dialogParameters.TryGetValue(DialogResultParameterNames.ColorCode, out string colorCode))
            {
                this.ColorCode = colorCode;
            }
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="ColorPickerDialogResult"/> class.
        /// <br/>Get RGB, HSV, and ColorCode from IDialogParameters.
        /// </summary>
        /// <param name="dialogResult"></param>
        public ColorPickerDialogResult(IDialogResult dialogResult) : this(dialogResult.Parameters, dialogResult.Result)
        {
        }
    }
}
