using System;

namespace Prism.CommonDialogPack.Controls
{
    public class BrightnessChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Brightness(明度) 0 ～ 1
        /// </summary>
        public float Brightness { get; }
        /// <summary>
        /// Initialize a new instance of the <see cref="BrightnessChangedEventArgs"/> class.
        /// </summary>
        /// <param name="brightness">Brightness(明度) 0 ～ 1</param>
        public BrightnessChangedEventArgs(float brightness)
        {
            Brightness = brightness;
        }
    }
}
