﻿using OpenMod.UnityEngine.Extensions;
using System.Drawing;
using UColor = UnityEngine.Color;

namespace Dummy.Extensions
{
    public static class ColorExtensions
    {
        public static UColor ToColor(this string color)
        {
            var SColor = ColorTranslator.FromHtml(color);
            return SColor.IsEmpty ? UColor.white : SColor.ToUnityColor();
        }
    }
}
