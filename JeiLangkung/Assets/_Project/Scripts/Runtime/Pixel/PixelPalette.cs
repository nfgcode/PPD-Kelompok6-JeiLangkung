using System;
using UnityEngine;

namespace JeiLangkung.Pixel
{
    /// <summary>Empat tingkat abu konsep K-09, ditambah dua warna yang diizinkan cerita: api lilin dan mata entitas.</summary>
    public static class PixelPalette
    {
        public static readonly Color32 Black = new Color32(12, 12, 13, 255);
        public static readonly Color32 Dark = new Color32(70, 70, 70, 255);
        public static readonly Color32 Light = new Color32(150, 150, 148, 255);
        public static readonly Color32 White = new Color32(229, 228, 224, 255);
        public static readonly Color32 FlameOuter = new Color32(240, 166, 44, 255);
        public static readonly Color32 FlameInner = new Color32(255, 236, 166, 255);
        public static readonly Color32 EyeRed = new Color32(214, 34, 40, 255);
        /// <summary>Warna Light untuk rich text TextMeshPro.</summary>
        public const string LightHex = "#969694";
        internal static readonly Color32[] Greys = { Black, Dark, Light, White };
        internal static readonly float[] Bayer = Array.ConvertAll(new[] { 0, 8, 2, 10, 12, 4, 14, 6, 3, 11, 1, 9, 15, 7, 13, 5 }, v => (v + .5f) / 16);

        /// <summary>Nilai terang menjadi salah satu dari empat abu dengan dither Bayer 4 x 4.</summary>
        public static Color32 Quantize(float value, int x, int y)
        {
            int level = (int)(value * 3 + Bayer[(y & 3) * 4 + (x & 3)]);
            return Greys[level < 0 ? 0 : level > 3 ? 3 : level];
        }

        /// <summary>Meredupkan seluruh bidang dengan pola kotak-kotak, dipakai di bawah jendela jeda.</summary>
        public static void Dim(PixelCanvas<Color32> frame)
        {
            Color32[] data = frame.Data;
            for (int y = 0; y < frame.Height; y++)
                for (int x = y & 1; x < frame.Width; x += 2) data[y * frame.Width + x] = Black;
        }
    }
}
