using Prism.CommonDialogPack.Models;
using System;

namespace Prism.CommonDialogPack.Converters
{
    /// <summary>
    /// Class providing the ability to convert various color types.
    /// </summary>
    public static class ColorConverter
    {
        /// <summary>
        /// Convert <see cref="RGB"/> to <see cref="HSV"/>.
        /// </summary>
        /// <param name="rgb">RGB</param>
        /// <returns></returns>
        public static HSV RGBToHSV(RGB rgb)
        {
            return RGBToHSV(rgb.R, rgb.G, rgb.B);
        }
        /// <summary>
        /// Convert <see cref="RGB"/> to <see cref="HSV"/>.
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns></returns>
        public static HSV RGBToHSV(byte r, byte g, byte b)
        {
            float rRatio = r / 255f;
            float gRatio = g / 255f;
            float bRatio = b / 255f;
            float max = Math.Max(rRatio, Math.Max(gRatio, bRatio));
            float min = Math.Min(rRatio, Math.Min(gRatio, bRatio));
            float brightness = max;
            float hue = 0f;
            float saturation = 0f;
            if (max != min)
            {
                float diff = max - min;
                if (max == rRatio)
                {
                    hue = (gRatio - bRatio) / diff;
                }
                else if (max == gRatio)
                {
                    hue = (bRatio - rRatio) / diff + 2f;
                }
                else
                {
                    hue = (rRatio - gRatio) / diff + 4f;
                }
                hue *= 60f;
                if (hue < 0f)
                {
                    hue += 360f;
                }
                saturation = diff / max;
            }
            return new HSV(hue, saturation, brightness);
        }
        /// <summary>
        /// Convert <see cref="HSV"/> to <see cref="RGB"/>.
        /// </summary>
        /// <param name="hsv">HSV</param>
        /// <returns></returns>
        public static RGB HSVToRGB(HSV hsv)
        {
            return HSVToRGB(hsv.H, hsv.S, hsv.V);
        }
        /// <summary>
        /// Convert <see cref="HSV"/> to <see cref="RGB"/>.
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="v">Brightness(Value)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static RGB HSVToRGB(float h, float s, float v)
        {
            float rRatio = v;
            float gRatio = v;
            float bRatio = v;
            if (s != 0f)
            {
                // 区分け(Hueを60で割り、小数点以下を四捨五入)
                float section = h / 60f;
                int sectionInt = (int)Math.Floor(section);
                float diff = section - sectionInt;
                float res1 = v * (1f - s);
                float res2;
                if (sectionInt % 2 == 0)
                {
                    res2 = v * (1f - ((1f - diff) * s));
                }
                else
                {
                    res2 = v * (1f - (diff * s));
                }
                switch (sectionInt)
                {
                    //  0° <= x < 60°
                    case 0:
                        gRatio = res2;
                        bRatio = res1;
                        break;
                    // 60° <= x < 120°
                    case 1:
                        rRatio = res2;
                        bRatio = res1;
                        break;
                    // 120° <= x < 180°
                    case 2:
                        rRatio = res1;
                        bRatio = res2;
                        break;
                    // 180° <= x < 240°
                    case 3:
                        rRatio = res1;
                        gRatio = res2;
                        break;
                    // 240° <= x < 300°
                    case 4:
                        rRatio = res2;
                        gRatio = res1;
                        break;
                    // 300° <= x < 360°
                    case 5:
                        gRatio = res1;
                        bRatio = res2;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            byte r = (byte)Math.Round(rRatio * 255f);
            byte g = (byte)Math.Round(gRatio * 255f);
            byte b = (byte)Math.Round(bRatio * 255f);
            return new RGB(r, g, b);
        }
        /// <summary>
        /// Try to convert from ColorCode <see cref="string"/> to <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool TryStringToMediaColor(string colorCode, out System.Windows.Media.Color color)
        {
            try
            {
                color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorCode);
                return true;
            }
            catch (FormatException _)
            {
                return false; ;
            }
        }
        /// <summary>
        /// Try to convert from ColorCode <see cref="string"/> to <see cref="RGB"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static bool TryStringToRGB(string colorCode, out RGB rgb)
        {
            bool res = TryStringToMediaColor(colorCode, out System.Windows.Media.Color color);
            rgb = RGB.FromMediaColor(color);
            return res;
        }
        /// <summary>
        /// Try to convert from ColorCode <see cref="string"/> to <see cref="HSV"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <param name="hsv"></param>
        /// <returns></returns>
        public static bool TryStringToHSV(string colorCode, out HSV hsv)
        {
            bool res = TryStringToMediaColor(colorCode, out System.Windows.Media.Color color);
            hsv = HSV.FromMediaColor(color);
            return res;
        }
        /// <summary>
        /// Convert ColorCode <see cref="string"/> to <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color StringToMediaColor(string colorCode)
        {
            try
            {
                return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorCode);
            }
            catch (FormatException _)
            {
                return default;
            }
        }
        /// <summary>
        /// Convert ColorCode <see cref="string"/> to <see cref="RGB"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static RGB StringToRGB(string colorCode)
        {
            return RGB.FromMediaColor(StringToMediaColor(colorCode));   
        }
        /// <summary>
        /// Convert ColorCode <see cref="string"/> to <see cref="HSV"/>.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static HSV StringToHSV(string colorCode)
        {
            return HSV.FromMediaColor(StringToMediaColor(colorCode));
        }
        /// <summary>
        /// Convert <see cref="byte[]"/> RGBBytes to ColorCode <see cref="string"/>.
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static string RGBBytesToString(byte[] rgb)
        {
            return $"#{BitConverter.ToString(rgb).Replace("-", string.Empty)}";
        }
    }
}
