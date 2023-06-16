using Prism.CommonDialogPack.Converters;
using System;
using System.Resources;

namespace Prism.CommonDialogPack.Models
{
    public struct RGB : IEquatable<RGB>
    {
        /// <summary>
        /// Red
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// Blue
        /// </summary>
        public byte B { get; set; }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGB"/> class with the specified Red, Green and Blue.
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        /// <summary>
        /// Convert to <see cref="HSV"/>.
        /// </summary>
        /// <returns></returns>
        public HSV ToHSV()
        {
            return ColorConverter.RGBToHSV(this);
        }
        /// <summary>
        /// Convert to <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToDrawingColor()
        {
            return System.Drawing.Color.FromArgb(this.R, this.G, this.B);
        }
        /// <summary>
        /// Convert to <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Color ToMediaColor()
        {
            return System.Windows.Media.Color.FromRgb(this.R, this.G, this.B);
        }
        /// <summary>
        /// Reurn RGBBytes.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return new byte[] { this.R, this.G, this.B };
        }
        /// <summary>
        /// Convert to ColorCode <see cref="string"/>.
        /// </summary>
        /// <returns></returns>
        public string ToColorCode()
        {
            return ColorConverter.RGBBytesToString(this.GetBytes());
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"R: {this.R}, G: {this.G}, B: {this.B}";
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGB"/> class from <see cref="HSV"/>.
        /// </summary>
        /// <param name="hsv"></param>
        /// <returns></returns>
        public static RGB FromHSV(HSV hsv)
        {
            return ColorConverter.HSVToRGB(hsv);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGB"/> class from HSV.
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="v">Brightness(Value)</param>
        /// <returns></returns>
        public static RGB FromHSV(float h, float s, float v)
        {
            return ColorConverter.HSVToRGB(h, s, v);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGB"/> class from <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static RGB FromDrawingColor(System.Drawing.Color color)
        {
            return new RGB(color.R, color.G, color.B);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGB"/> class from <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static RGB FromMediaColor(System.Windows.Media.Color color)
        {
            return new RGB(color.R, color.G, color.B);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGB"/> class from ColorCode <see cref="string"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static RGB FromString(string colorCode)
        {
            return ColorConverter.StringToRGB(colorCode);
        }

        public override bool Equals(object obj)
        {
            return obj is RGB rGB && Equals(rGB);
        }

        public bool Equals(RGB other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }

        public static bool operator ==(RGB left, RGB right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RGB left, RGB right)
        {
            return !(left == right);
        }
    }
}
