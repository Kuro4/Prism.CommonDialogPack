using System;

namespace Prism.CommonDialogPack.Controls
{
    public class HSChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Hue(色相) 0～360
        /// </summary>
        public float Hue { get; }
        /// <summary>
        /// Saturation(彩度) 0 ～ 1
        /// </summary>
        public float Saturation { get; }
        /// <summary>
        /// Initialize a new instance of the <see cref="HSChangedEventArgs"/> class.
        /// </summary>
        /// <param name="hue">Hue(色相) 0～360</param>
        /// <param name="saturation">Saturation(彩度) 0 ～ 1</param>
        public HSChangedEventArgs(float hue, float saturation)
        {
            Hue = hue;
            Saturation = saturation;
        }
    }
}
