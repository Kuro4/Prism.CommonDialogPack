using Prism.CommonDialogPack.Converters;
using System;

namespace Prism.CommonDialogPack.Models
{
    public struct HSV : IEquatable<HSV>
    {
        /// <summary>
        /// Hue(色相) 0～360
        /// </summary>
        public float H { get; set; }
        /// <summary>
        /// Saturation(彩度) 0 ～ 1
        /// </summary>
        public float S { get; set; }
        /// <summary>
        /// Brightness(明度) 0 ～ 1
        /// </summary>
        public float V { get; set; }
        /// <summary>
        /// Initialize a new instance of the <see cref="HSV"/> class with the specified Hue, Saturation and Brightness(Value).
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="v">Brightness(Value)</param>
        public HSV(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }
        /// <summary>
        /// Convert to <see cref="RGB"/>.
        /// </summary>
        /// <returns></returns>
        public RGB ToRGB()
        {
            return ColorConverter.HSVToRGB(this);
        }
        /// <summary>
        /// Convert to <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToDrawingColor()
        {
            RGB rgb = this.ToRGB();
            return rgb.ToDrawingColor();
        }
        /// <summary>
        /// Convert to <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Color ToMediaColor()
        {
            RGB rgb = this.ToRGB();
            return rgb.ToMediaColor();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"H: {this.H}, S: {this.S}, V: {this.V}";
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="HSV"/> class from <see cref="RGB"/>.
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static HSV FromRGB(RGB rgb)
        {
            return ColorConverter.RGBToHSV(rgb);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="HSV"/> class from RGB.
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns></returns>
        public static HSV FromRGB(byte r, byte g, byte b)
        {
            return ColorConverter.RGBToHSV(r, g, b);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="HSV"/> class from <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static HSV FromDrawingColor(System.Drawing.Color color)
        {
            return ColorConverter.RGBToHSV(color.R, color.G, color.B);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="HSV"/> class from <see cref="System.Windows.Media.Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static HSV FromMediaColor(System.Windows.Media.Color color)
        {
            return ColorConverter.RGBToHSV(color.R, color.B, color.B);
        }

        public override bool Equals(object obj)
        {
            return obj is HSV hSV && Equals(hSV);
        }

        public bool Equals(HSV other)
        {
            return H == other.H &&
                   S == other.S &&
                   V == other.V;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(H, S, V);
        }

        public static bool operator ==(HSV left, HSV right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HSV left, HSV right)
        {
            return !(left == right);
        }
    }
}
