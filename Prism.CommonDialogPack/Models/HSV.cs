using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Models
{
    public struct HSV
    {
        /// <summary>
        /// Hue(色相) 0°～360°
        /// </summary>
        public float H { get; set; }
        /// <summary>
        /// Saturation(彩度) 0 ～ 100%
        /// </summary>
        public float S { get; set; }
        /// <summary>
        /// Value(明度) 0 ～ 100%
        /// </summary>
        public float V { get; set; }

        public HSV(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

        public static HSV FromRGB(byte r, byte g, byte b)
        {
            // RGB値を0 ～ 1.0 の値に変換
            float fr = r / 255f;
            float fg = g / 255f;
            float fb = b / 255f;

            float min = Math.Min(fr, Math.Min(fg, fb));
            float max = Math.Max(fr, Math.Max(fg, fb));

            float value = max;
            float hue;
            float saturation;
            if (max == min)
            {
                // Undefined
                hue = 0f;
                saturation = 0f;
                return new HSV(hue, saturation, value);
            }
            else
            {
                float c = max - min;
                if (max == r)
                {
                    hue = (fg - fb) / c;
                }
                else if (max == g)
                {
                    hue = (fb - fr) / c + 2f;
                }
                else
                {
                    hue = (fr - fg) / c + 4f;
                }
                hue *= 60f;
                if (hue < 0f)
                {
                    hue += 360f;
                }
                saturation = c / max;
            }



            // wiki
            float h = max - min;
            if (0.0f < h)
            {
                if (max == fr)
                {
                    h = (fg - fb) / h;
                    if (h < 0.0f)
                    {
                        h += 6.0f;
                    }
                    else if (max == g)
                    {
                        h = 2.0f + (fb - fr) / h;
                    }
                    else
                    {
                        h = 4.0f + (fr - fg) / h;
                    }
                }
            }
            h /= 6.0f;
            saturation = max - min;
            if (max != 0.0f)
            {
                saturation /= max;
            }
            value = max;


            //var min = Math.Min(Math.Min(r, g), b);
            //var max = Math.Max(Math.Max(r, g), b);
            //float h = max - min;
            //if (0 < h)
            //{
            //    if ( max == r)
            //    {
            //        h = (g - b) / h;
            //        if (h < 0)
            //        {
            //            h += 6;
            //        }
            //    }
            //    else if (max == g)
            //    {
            //        h = 2 + (b - r) / h;
            //    }
            //    else
            //    {
            //        h = 4 + (r - g) / h;
            //    }
            //}
            //h /= 6;
            //var s = (max - min);
            //if (0 < max)
            //{
            //    s /= max;
            //}
            //var v = max;
            return new HSV();
        }
    }
}
