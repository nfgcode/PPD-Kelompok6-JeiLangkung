using UnityEngine;

namespace JeiLangkung.Pixel
{
    /// <summary>Gambar abu-abu kecil (Read/Write aktif) sebagai nilai terang, baris teratas di indeks 0.</summary>
    public sealed class PixelImage
    {
        public int Width { get; }
        public int Height { get; }
        public float[] Values { get; }

        public PixelImage(Texture2D texture)
        {
            Width = texture.width;
            Height = texture.height;
            Values = new float[Width * Height];
            Color32[] pixels = texture.GetPixels32();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Values[y * Width + x] = pixels[(Height - 1 - y) * Width + x].r / 255f;
        }

        public float this[int x, int y] => Values[y * Width + x];
    }
}
