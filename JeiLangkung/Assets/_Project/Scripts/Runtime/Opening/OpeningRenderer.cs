using System;
using System.Collections.Generic;
using JeiLangkung.Pixel;
using UnityEngine;
using static JeiLangkung.Pixel.PixelPalette;

namespace JeiLangkung.Opening
{
    /// <summary>
    /// Menu dan kos dalam gaya piksel Scene 2: ilustrasi konsep revisi 02 diberi cahaya per piksel
    /// lalu didither ke empat abu. Api lilin menu satu-satunya warna.
    /// </summary>
    public sealed class OpeningRenderer : IDisposable
    {
        public const int Width = 480, Height = 270, StripHeight = 12;
        /// <summary>Panel ilustrasi kos dan potret, mengikuti tata letak FigJam Scene 1.</summary>
        public static readonly RectInt KosPanel = new RectInt(6, 26, 348, 196);
        public static readonly RectInt PortraitPanel = new RectInt(362, 38, 112, 112);
        /// <summary>Bingkai teks: tombol tindakan di kanan bawah, dialog di bawah ilustrasi.</summary>
        public static readonly RectInt ActionPanel = new RectInt(362, 158, 112, 106), DialogPanel = new RectInt(6, 230, 348, 34);
        /// <summary>Laptop Dimas dan catatan Ayu pada ilustrasi kos, dalam koordinat gambar penuh.</summary>
        public static readonly RectInt Laptop = new RectInt(166, 118, 57, 34), Notes = new RectInt(128, 132, 45, 15);
        private static readonly Vector2 MenuFlame = new Vector2(454, 175);
        /// <summary>HP Nisa di ilustrasi kos, sumber cahaya senter setelah lampu padam.</summary>
        private static readonly Vector2 PhoneLight = new Vector2(255, 120);

        private readonly PixelImage menu, kos;
        private readonly Dictionary<string, PixelImage> portraits = new Dictionary<string, PixelImage>();
        private readonly float[] candleMap = new float[Width * Height];
        private readonly float[] flashMap;
        private readonly PixelCanvas<Color32> frame = new PixelCanvas<Color32>(Width, Height);

        public Texture2D Texture { get; }
        /// <summary>Piksel akhir dengan baris teratas di indeks 0.</summary>
        public Color32[] Pixels => frame.Data;

        public OpeningRenderer(OpeningConfig config)
        {
            Texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false) { name = "Pembuka", filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };
            menu = new PixelImage(config.MenuArt);
            kos = new PixelImage(config.KosArt);
            foreach (OpeningConfig.Portrait portrait in config.Portraits)
                if (portrait.image != null && portrait.image.isReadable) portraits[portrait.speaker.ToUpperInvariant()] = new PixelImage(portrait.image);
            // Api putih pada ilustrasi diganti api kuning yang berkedip; sisanya menjadi pendar abu-abu.
            for (int y = (int)MenuFlame.y - 24; y < MenuFlame.y + 2; y++)
                for (int x = (int)MenuFlame.x - 7; x <= MenuFlame.x + 7; x++)
                    if (menu.Values[y * Width + x] > .8f) menu.Values[y * Width + x] = .55f;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    float dx = x - MenuFlame.x, dy = (y - MenuFlame.y + 8) * 1.2f;
                    candleMap[y * Width + x] = 3600 / (3600 + dx * dx + dy * dy);
                }
            flashMap = new float[KosPanel.width * KosPanel.height];
            for (int y = 0; y < KosPanel.height; y++)
                for (int x = 0; x < KosPanel.width; x++)
                {
                    float dx = x + KosPanel.x - PhoneLight.x, dy = (y + KosPanel.y - PhoneLight.y) * 1.3f;
                    flashMap[y * KosPanel.width + x] = 3000 / (3000 + dx * dx + dy * dy);
                }
        }

        public void RenderMenu(float time, OpeningLook look)
        {
            float amplitude = look.ReducedFlicker ? .05f : .2f;
            float flicker = 1 - amplitude * Mathf.PerlinNoise(time * 6.5f, 2.3f);
            float[] values = menu.Values;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    int i = y * Width + x;
                    frame.Data[i] = Quantize(values[i] * (.8f + .32f * candleMap[i] * flicker), x, y);
                }
            DrawMenuFlame(time, look.ReducedFlicker);
            frame.Rect(0, 0, Width, StripHeight, Black);
            Finish(look.Dim);
        }

        public void RenderKos(float time, OpeningLook look)
        {
            frame.Fill(Black);
            float[] values = kos.Values;
            float flash = look.Flashlight ? 1 - (look.ReducedFlicker ? .02f : .06f) * Mathf.PerlinNoise(time * 9, 4.1f) : 0;
            for (int y = 0; y < KosPanel.height; y++)
                for (int x = 0; x < KosPanel.width; x++)
                {
                    int i = y * KosPanel.width + x;
                    float light = look.Lamp + (flash > 0 ? .04f + 1.3f * flashMap[i] * flash : 0);
                    int fx = KosPanel.x + x, fy = KosPanel.y + y;
                    frame.Data[fy * Width + fx] = Quantize(values[i] * light, fx, fy);
                }
            Border(KosPanel);
            if ((look.Pending & 1) != 0) Brackets(Laptop);
            if ((look.Pending & 2) != 0) Brackets(Notes);
            string speaker = string.IsNullOrEmpty(look.Speaker) ? "RAKA" : look.Speaker.ToUpperInvariant();
            if (portraits.TryGetValue(speaker, out PixelImage portrait))
            {
                // Potret ikut gelap saat lampu padam dan tersorot senter sesudahnya.
                float light = Mathf.Max(look.Lamp, look.Flashlight ? .8f : 0);
                for (int y = 0; y < PortraitPanel.height; y++)
                    for (int x = 0; x < PortraitPanel.width; x++)
                    {
                        int fx = PortraitPanel.x + x, fy = PortraitPanel.y + y;
                        frame.Data[fy * Width + fx] = Quantize(portrait[x * portrait.Width / PortraitPanel.width, y * portrait.Height / PortraitPanel.height] * light, fx, fy);
                    }
            }
            Border(PortraitPanel);
            Border(ActionPanel);
            Border(DialogPanel);
            frame.Rect(0, 0, Width, StripHeight, Black);
            Finish(look.Dim);
        }

        private void DrawMenuFlame(float time, bool reducedFlicker)
        {
            int height = 14 + (Mathf.PerlinNoise(time * 9, 1) > .6f ? 1 : 0) - (reducedFlicker ? 0 : (Mathf.PerlinNoise(time * 7, 5) < .25f ? 1 : 0));
            float sway = (Mathf.PerlinNoise(time * 4, 3.3f) - .5f) * (reducedFlicker ? 1 : 3);
            int baseX = (int)MenuFlame.x, baseY = (int)MenuFlame.y;
            for (int r = 0; r < height; r++)
            {
                float t = r / (float)height;
                int half = r < 2 ? 1 : t < .62f ? 2 : t < .88f ? 1 : 0;
                int cx = baseX + Mathf.RoundToInt(sway * t * t);
                frame.Rect(cx - half, baseY - r, half * 2 + 1, 1, FlameOuter);
                if (r >= 2 && t < .55f) frame.Rect(cx - half + 1, baseY - r, Math.Max(1, half * 2 - 1), 1, FlameInner);
            }
            frame.Rect(baseX, baseY + 1, 1, 2, Black);
        }

        private void Border(RectInt panel)
        {
            frame.Rect(panel.x - 1, panel.y - 1, panel.width + 2, 1, PixelPalette.Light);
            frame.Rect(panel.x - 1, panel.yMax, panel.width + 2, 1, PixelPalette.Light);
            frame.Rect(panel.x - 1, panel.y, 1, panel.height, PixelPalette.Light);
            frame.Rect(panel.xMax, panel.y, 1, panel.height, PixelPalette.Light);
        }

        /// <summary>
        /// Sudut putih di sekitar benda yang masih bisa diperiksa, sama seperti sorotan lilin di Scene 2.
        /// Kontur hitam menjaga sudut tetap terlihat di atas kertas dan baju yang terang.
        /// </summary>
        private void Brackets(RectInt area)
        {
            int left = area.x - 2, right = area.xMax + 1, top = area.y - 2, bottom = area.yMax + 1;
            for (int pass = 0; pass < 2; pass++)
            {
                int grow = pass == 0 ? 1 : 0;
                Color32 colour = pass == 0 ? Black : White;
                frame.Rect(left - grow, top - grow, 5 + grow * 2 - 1, 1 + grow * 2, colour);
                frame.Rect(left - grow, top - grow, 1 + grow * 2, 5 + grow * 2 - 1, colour);
                frame.Rect(right - 3 - grow, top - grow, 4 + grow * 2, 1 + grow * 2, colour);
                frame.Rect(right - grow, top - grow, 1 + grow * 2, 4 + grow * 2, colour);
                frame.Rect(left - grow, bottom - grow, 4 + grow * 2, 1 + grow * 2, colour);
                frame.Rect(left - grow, bottom - 3 - grow, 1 + grow * 2, 4 + grow * 2, colour);
                frame.Rect(right - 3 - grow, bottom - grow, 4 + grow * 2, 1 + grow * 2, colour);
                frame.Rect(right - grow, bottom - 3 - grow, 1 + grow * 2, 4 + grow * 2, colour);
            }
        }

        private void Finish(bool dim)
        {
            if (dim) Dim(frame);
            Texture.SetPixelData(frame.Data, 0);
            Texture.Apply(false);
        }

        public void Dispose()
        {
            if (Texture == null) return;
            if (Application.isPlaying) UnityEngine.Object.Destroy(Texture);
            else UnityEngine.Object.DestroyImmediate(Texture);
        }
    }
}
