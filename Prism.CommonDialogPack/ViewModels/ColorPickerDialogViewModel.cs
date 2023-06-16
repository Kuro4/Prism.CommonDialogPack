using Prism.Commands;
using Prism.CommonDialogPack.Models;
using Prism.CommonDialogPack.Resources;
using Prism.Services.Dialogs;
using System;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ColorPickerDialogViewModel : DialogViewModelBase
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

        private DelegateCommand addCustomColorCommand;
        /// <summary>
        /// AddCustomColor command.
        /// </summary>
        public DelegateCommand AddCustomColorCommand => this.addCustomColorCommand ??= new DelegateCommand(this.AddCustomColor, this.CanAddCustomColor);

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

        public string addCustomColorButtonText = "カスタムカラーへ追加";
        /// <summary>
        /// AddCustomColor button text.
        /// </summary>
        public string AddCustomColorButtonText
        {
            get { return this.addCustomColorButtonText; }
            set { SetProperty(ref this.addCustomColorButtonText, value); }
        }

        private RGBWithIndexViewModel[] customColors;
        /// <summary>
        /// CustomColors.
        /// </summary>
        public RGBWithIndexViewModel[] CustomColors
        {
            get { return customColors; }
            private set { SetProperty(ref customColors, value); }
        }

        private HSV defaultHSV;
        public HSV DefaultHSV
        {
            get { return defaultHSV; }
            set { SetProperty(ref defaultHSV, value); }
        }

        private RGB selectedBasicColor;
        /// <summary>
        /// Selected RGB in BasicColors.
        /// </summary>
        public RGB SelectedBasicColor
        {
            get { return this.selectedBasicColor; }
            set 
            {
                SetProperty(ref this.selectedBasicColor, value);
                this.DisplayRed = value.R.ToString();
                this.DisplayGreen = value.G.ToString();
                this.DisplayBlue = value.B.ToString();
                this.DisplayColorCode = value.ToColorCode();
                HSV hsv = value.ToHSV();
                this.DisplayHue = Math.Round(hsv.H).ToString();
                this.DisplaySaturation = Math.Round(hsv.S * 100).ToString();
                this.DisplayBrightness = Math.Round(hsv.V * 100).ToString();
                this.CurrentRGB = value;
                this.InputHSV = hsv;
                this.currentHSV = hsv;
            }
        }

        private RGBWithIndexViewModel selectedCustomColor;
        /// <summary>
        /// Selected RGB in CustomColors.
        /// </summary>
        public RGBWithIndexViewModel SelectedCutomColor
        {
            get { return selectedCustomColor; }
            set
            {
                SetProperty(ref selectedCustomColor, value);
                this.AddCustomColorCommand.RaiseCanExecuteChanged();
                this.DisplayRed = value.RGB.R.ToString();
                this.DisplayGreen = value.RGB.G.ToString();
                this.DisplayBlue = value.RGB.B.ToString();
                this.DisplayColorCode = value.RGB.ToColorCode();
                HSV hsv = value.RGB.ToHSV();
                this.DisplayHue = Math.Round(hsv.H).ToString();
                this.DisplaySaturation = Math.Round(hsv.S * 100).ToString();
                this.DisplayBrightness = Math.Round(hsv.V * 100).ToString();
                this.CurrentRGB = value.RGB;
                this.InputHSV = hsv;
                this.currentHSV = hsv;
            }
        }

        private HSV selectedHSV;
        /// <summary>
        /// Selected HSV in HSVPicker.
        /// </summary>
        public HSV SelectedHSV
        {
            get { return this.selectedHSV; }
            set 
            { 
                SetProperty(ref this.selectedHSV, value);
                this.DisplayHue = Math.Round(value.H).ToString();
                this.DisplaySaturation = Math.Round(value.S * 100).ToString();
                this.DisplayBrightness = Math.Round(value.V * 100).ToString();
                var rgb = value.ToRGB();
                this.DisplayRed = rgb.R.ToString();
                this.DisplayGreen = rgb.G.ToString();
                this.DisplayBlue = rgb.B.ToString();
                this.DisplayColorCode = rgb.ToColorCode();
                this.CurrentRGB = rgb;
                this.InputHSV = value;
                this.currentHSV = value;
            }
        }

        /// <summary>
        /// Current HSV.
        /// </summary>
        private HSV currentHSV;

        private RGB currentRGB;
        /// <summary>
        /// Current RGB.
        /// </summary>
        public RGB CurrentRGB
        {
            get { return this.currentRGB; }
            set { SetProperty(ref this.currentRGB, value); }
        }

        #region For Input
        #region HSV
        private HSV inputHSV;
        /// <summary>
        /// HSV for input.
        /// </summary>
        public HSV InputHSV
        {
            get { return this.inputHSV; }
            set { SetProperty(ref this.inputHSV, value); }
        }

        private string inputHue;
        /// <summary>
        /// Hue for input.
        /// </summary>
        public string InputHue
        {
            get { return this.inputHue; }
            set 
            {
                if (!float.TryParse(value, out float hue))
                {
                    return;
                }
                if (hue < 0)
                {
                    hue = 0;
                }
                else if(360 <= hue)
                {
                    hue = 359;
                }
                SetProperty(ref this.inputHue, hue.ToString());
                this.DisplayHue = hue.ToString();
                this.SetInputHSV(new HSV(hue, this.currentHSV.S, this.currentHSV.V));
            }
        }

        private string inputSaturation;
        /// <summary>
        /// Saturation for input.
        /// </summary>
        public string InputSaturation
        {
            get { return this.inputSaturation; }
            set
            { 
                if (!float.TryParse(value, out float saturation))
                {
                    return;
                }
                if (saturation < 0)
                {
                    saturation = 0;
                }
                else if(100 < saturation)
                {
                    saturation = 100;
                }
                SetProperty(ref this.inputSaturation, saturation.ToString());
                this.DisplaySaturation = saturation.ToString();
                this.SetInputHSV(new HSV(this.currentHSV.H, saturation / 100, this.currentHSV.V));
            }
        }

        private string inputBrightness;
        /// <summary>
        /// Brightness for input.
        /// </summary>
        public string InputBrightness
        {
            get { return this.inputBrightness; }
            set 
            {
                if (!float.TryParse(value, out float brightness))
                {
                    return;
                }
                if (brightness < 0)
                {
                    brightness = 0;
                }
                else if (100 < brightness)
                {
                    brightness = 100;
                }
                SetProperty(ref this.inputBrightness, brightness.ToString());
                this.DisplayBrightness = brightness.ToString();
                this.SetInputHSV(new HSV(this.currentHSV.H, this.currentHSV.S, brightness / 100));
            }
        }
        #endregion
        #region RGB
        private string inputColorCode;
        /// <summary>
        /// ColorCode for input.
        /// </summary>
        public string InputColorCode
        {
            get { return this.inputColorCode; }
            set
            {
                if (!Converters.ColorConverter.TryStringToRGB(value, out RGB rgb))
                {
                    return;
                }
                SetProperty(ref this.inputColorCode, value);
                this.DisplayColorCode = value;
                this.DisplayRed = rgb.R.ToString();
                this.DisplayGreen = rgb.G.ToString();
                this.DisplayBlue = rgb.B.ToString();
                HSV hsv = rgb.ToHSV();
                this.DisplayHue = Math.Round(hsv.H).ToString();
                this.DisplaySaturation = Math.Round(hsv.S * 100).ToString();
                this.DisplayBrightness = Math.Round(hsv.V * 100).ToString();
                this.CurrentRGB = rgb;
                this.InputHSV = hsv;
                this.currentHSV = hsv;
            }
        }

        private string inputRed;
        /// <summary>
        /// Red for input.
        /// </summary>
        public string InputRed
        {
            get { return this.inputRed; }
            set
            {
                if (!byte.TryParse(value, out byte red))
                {
                    return;
                }
                SetProperty(ref this.inputRed, value);
                this.DisplayRed = red.ToString();
                this.SetInputRGB(new RGB(red, this.CurrentRGB.G, this.CurrentRGB.B));
            }
        }

        private string inputGreen;
        /// <summary>
        /// Green for input.
        /// </summary>
        public string InputGreen
        {
            get { return this.inputGreen; }
            set
            {
                if (!byte.TryParse(value, out byte green))
                {
                    return;
                }
                SetProperty(ref this.inputGreen, value);
                this.DisplayGreen = green.ToString();
                this.SetInputRGB(new RGB(this.CurrentRGB.R, green, this.CurrentRGB.B));
            }
        }

        private string inputBlue;
        /// <summary>
        /// Blue for input.
        /// </summary>
        public string InputBlue
        {
            get { return this.inputBlue; }
            set 
            {
                
                if (!byte.TryParse(value, out byte blue))
                {
                    return;
                }
                SetProperty(ref this.inputBlue, value);
                this.DisplayBlue = blue.ToString();
                this.SetInputRGB(new RGB(this.CurrentRGB.R, this.CurrentRGB.G, blue));
            }
        }
        #endregion
        #endregion
        #region For Display
        private string displayHue;
        /// <summary>
        /// Hue for display.
        /// </summary>
        public string DisplayHue
        {
            get { return this.displayHue; }
            set { SetProperty(ref this.displayHue, value); }
        }

        private string displaySaturation;
        /// <summary>
        /// Saturation for display.
        /// </summary>
        public string DisplaySaturation
        {
            get { return this.displaySaturation; }
            set { SetProperty(ref this.displaySaturation, value); }
        }

        private string displayBrightness;
        /// <summary>
        /// Brightness for display.
        /// </summary>
        public string DisplayBrightness
        {
            get { return this.displayBrightness; }
            set { SetProperty(ref this.displayBrightness, value); }
        }

        private string displayColorCode;
        /// <summary>
        /// ColorCode for display.
        /// </summary>
        public string DisplayColorCode
        {
            get { return this.displayColorCode; }
            set { SetProperty(ref this.displayColorCode, value); }
        }

        private string displayRed;
        /// <summary>
        /// Red for display.
        /// </summary>
        public string DisplayRed
        {
            get { return this.displayRed; }
            set { SetProperty(ref this.displayRed, value); }
        }

        private string displayGreen;
        /// <summary>
        /// Green for display.
        /// </summary>
        public string DisplayGreen
        {
            get { return this.displayGreen; }
            set { SetProperty(ref this.displayGreen, value); }
        }

        private string displayBlue;
        /// <summary>
        /// Blue for display.
        /// </summary>
        public string DisplayBlue
        {
            get { return this.displayBlue; }
            set { SetProperty(ref this.displayBlue, value); }
        }
        #endregion
        /// <summary>
        /// Initialize a new instance of the <see cref="ColorPickerDialogViewModel"/> class.
        /// </summary>
        public ColorPickerDialogViewModel()
        {
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.Title = "ColorPicker";
            this.Width = 600;
            this.Height = 450;
            DialogSettings.GetCustomColors();
        }
        /// <summary>
        /// Called when the dialog is opened.
        /// </summary>
        /// <param name="parameters">The parameters passed to the dialog.</param>
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (Settings.Default.EnableCustomColorsStorage)
            {
                this.CustomColors = DialogSettings.GetCustomColors();
            }
            else
            {
                this.CustomColors = Enumerable.Range(0, 16).Select(i => new RGBWithIndexViewModel(new RGB(255, 255, 255), i)).ToArray();
            }
            if (parameters.TryGetValue(DialogParameterNames.OKButtonText, out string okButtonText))
            {
                this.OKButtonText = okButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.AddCustomColorButtonText, out string addCustomColorButtonText))
            {
                this.AddCustomColorButtonText = addCustomColorButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.DefaultColor, out RGB defaultRGB))
            {
                HSV hsv = defaultRGB.ToHSV();
                this.DefaultHSV = hsv;
                if (BasicColors.RGB.Any(x => x == defaultRGB))
                {
                    RGB target = BasicColors.RGB.First(x => x == defaultRGB);
                    this.SelectedBasicColor = target;
                }
                if (this.CustomColors.Any(x => x.RGB == defaultRGB))
                {
                    RGBWithIndexViewModel target = this.CustomColors.First(x => x.RGB == defaultRGB);
                    this.SelectedCutomColor = target;
                }
                this.DisplayRed = defaultRGB.R.ToString();
                this.DisplayGreen = defaultRGB.G.ToString();
                this.DisplayBlue = defaultRGB.B.ToString();
                this.DisplayHue = Math.Round(hsv.H).ToString();
                this.DisplaySaturation = Math.Round(hsv.S * 100).ToString();
                this.DisplayBrightness = Math.Round(hsv.V * 100).ToString();
                this.DisplayColorCode = defaultRGB.ToColorCode();
                this.CurrentRGB = defaultRGB;
                this.currentHSV = hsv;
            }
            else if (parameters.TryGetValue(DialogParameterNames.DefaultColor, out HSV defaultHSV))
            {
                RGB rgb = defaultHSV.ToRGB();
                this.DefaultHSV = defaultHSV;
                if (BasicColors.RGB.Any(x => x == rgb))
                {
                    RGB target = BasicColors.RGB.First(x => x == rgb);
                    this.SelectedBasicColor = target;
                }
                if (this.CustomColors.Any(x => x.RGB == rgb))
                {
                    RGBWithIndexViewModel target = this.CustomColors.First(x => x.RGB == rgb);
                    this.SelectedCutomColor = target;
                }
                this.DisplayRed = rgb.R.ToString();
                this.DisplayGreen = rgb.G.ToString();
                this.DisplayBlue = rgb.B.ToString();
                this.DisplayHue = Math.Round(defaultHSV.H).ToString();
                this.DisplaySaturation = Math.Round(defaultHSV.S * 100).ToString();
                this.DisplayBrightness = Math.Round(defaultHSV.V * 100).ToString();
                this.DisplayColorCode = rgb.ToColorCode();
                this.CurrentRGB = rgb;
                this.currentHSV = defaultHSV;
            }
            else if (parameters.TryGetValue(DialogParameterNames.DefaultColor, out string colorCode) && Converters.ColorConverter.TryStringToRGB(colorCode, out RGB rgb))
            {
                HSV hsv = rgb.ToHSV();
                this.DefaultHSV = hsv;
                if (BasicColors.RGB.Any(x => x == rgb))
                {
                    RGB target = BasicColors.RGB.First(x => x == rgb);
                    this.SelectedBasicColor = target;
                }
                if (this.CustomColors.Any(x => x.RGB == rgb))
                {
                    RGBWithIndexViewModel target = this.CustomColors.First(x => x.RGB == rgb);
                    this.SelectedCutomColor = target;
                }
                this.DisplayRed = rgb.R.ToString();
                this.DisplayGreen = rgb.G.ToString();
                this.DisplayBlue = rgb.B.ToString();
                this.DisplayHue = Math.Round(hsv.H).ToString();
                this.DisplaySaturation = Math.Round(hsv.S * 100).ToString();
                this.DisplayBrightness = Math.Round(hsv.V * 100).ToString();
                this.DisplayColorCode = colorCode;
                this.CurrentRGB = rgb;
                this.currentHSV = hsv;
            }
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.OK"/>.
        /// <br/>RGB, HSV, and ColorCode can be get from DialogParameters.
        /// </summary>
        private void OK()
        {
            var param = new DialogParameters
            {
                { DialogResultParameterNames.RGB, this.CurrentRGB },
                { DialogResultParameterNames.HSV, this.currentHSV },
                { DialogResultParameterNames.ColorCode, this.DisplayColorCode },
            };
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.Cancel"/>.
        /// </summary>
        private void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
        /// <summary>
        /// Add CurrentRGB to CustomColor.
        /// </summary>
        private void AddCustomColor()
        {
            if (this.SelectedCutomColor is null)
            {
                return;
            }
            int index = this.SelectedCutomColor.Index;
            this.CustomColors[index].RGB = this.CurrentRGB;
            Settings settings = Settings.Default;
            if (!settings.EnableCustomColorsStorage)
            {
                return;
            }
            string colorCode = this.CurrentRGB.ToColorCode();
            switch (index)
            {
                case 0:
                    settings.CustomColor01 = colorCode;
                    break;
                case 1:
                    settings.CustomColor02 = colorCode;
                    break;
                case 2:
                    settings.CustomColor03 = colorCode;
                    break;
                case 3:
                    settings.CustomColor04 = colorCode;
                  break;
                case 4:
                    settings.CustomColor05 = colorCode;
                    break;
                case 5:
                    settings.CustomColor06 = colorCode;
                    break;
                case 6:
                    settings.CustomColor07 = colorCode;
                    break;
                case 7:
                    settings.CustomColor08 = colorCode;
                    break;
                case 8:
                    settings.CustomColor09 = colorCode;
                    break;
                case 9:
                    settings.CustomColor10 = colorCode;
                    break;
                case 10:
                    settings.CustomColor11 = colorCode;
                    break;
                case 11:
                    settings.CustomColor12 = colorCode;
                    break;
                case 12:
                    settings.CustomColor13 = colorCode;
                    break;
                case 13:
                    settings.CustomColor14 = colorCode;
                    break;
                case 14:
                    settings.CustomColor15 = colorCode;
                    break;
                case 15:
                    settings.CustomColor16 = colorCode;
                    break;
                default:
                    return;
            }
            settings.Save();
        }
        /// <summary>
        /// Returns whether CustomColor can be added.
        /// </summary>
        /// <returns></returns>
        private bool CanAddCustomColor()
        {
            return this.SelectedCutomColor != null;
        }
        /// <summary>
        /// Set InputHSV.
        /// </summary>
        /// <param name="hsv"></param>
        private void SetInputHSV(HSV hsv)
        {
            this.InputHSV = hsv;
            this.currentHSV = hsv;
            RGB rgb = hsv.ToRGB();
            this.CurrentRGB = rgb;
            this.DisplayRed = rgb.R.ToString();
            this.DisplayGreen = rgb.G.ToString();
            this.DisplayBlue = rgb.B.ToString();
            this.DisplayColorCode = rgb.ToColorCode();
        }
        /// <summary>
        /// Set InputRGB.
        /// </summary>
        /// <param name="rgb"></param>
        private void SetInputRGB(RGB rgb)
        {
            this.CurrentRGB = rgb;
            this.DisplayColorCode = rgb.ToColorCode();
            HSV hsv = rgb.ToHSV();
            this.InputHSV = hsv;
            this.currentHSV = hsv;
            this.DisplayHue = Math.Round(hsv.H).ToString();
            this.DisplaySaturation = Math.Round(hsv.S * 100).ToString();
            this.DisplayBrightness = Math.Round(hsv.V * 100).ToString();
        }
    }
}
