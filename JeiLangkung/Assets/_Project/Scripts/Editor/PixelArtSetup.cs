using System.IO;
using UnityEditor;
using UnityEngine;

namespace JeiLangkung.Editor
{
    /// <summary>
    /// Mengubah ilustrasi konsep pixel revisi 02 (docs/art-direction) menjadi gambar abu-abu kecil pada grid 480 x 270.
    /// Cahaya dan dither ditambahkan saat bermain. Berkas hasil yang sudah ada tidak ditimpa, jadi tim boleh menyuntingnya.
    /// </summary>
    public static class PixelArtSetup
    {
        private const string Folder = "Assets/_Project/Art/Pixel";
        private const string Source = "../docs/art-direction/";
        private const string Menu = "first-screen-pixel-v2.png", Kos = "scene1-kos-pixel-v2.png", Raka = "raka-portrait-pixel-v2.png";

        public static Texture2D MenuArt() => Convert(Menu, "Menu.png", new RectInt(0, 0, 1672, 941), 480, 270);
        public static Texture2D KosArt() => Convert(Kos, "Kos.png", new RectInt(0, 0, 1672, 941), 348, 196);

        /// <summary>Potret 112 x 112. Raka memakai potret khususnya; tokoh lain dipotong dari ilustrasi kos.</summary>
        public static Texture2D Portrait(string speaker)
        {
            switch (speaker)
            {
                case "RAKA": return Convert(Raka, "Potret-Raka.png", new RectInt(77, 0, 1100, 1100), 112, 112);
                case "AYU": return Convert(Kos, "Potret-Ayu.png", new RectInt(500, 160, 290, 290), 112, 112);
                case "DIMAS": return Convert(Kos, "Potret-Dimas.png", new RectInt(820, 150, 290, 290), 112, 112);
                case "NISA": return Convert(Kos, "Potret-Nisa.png", new RectInt(1130, 210, 290, 290), 112, 112);
                default: return null;
            }
        }

        /// <param name="crop">Potongan dalam piksel sumber, diukur dari kiri atas.</param>
        private static Texture2D Convert(string source, string name, RectInt crop, int width, int height)
        {
            string path = Folder + "/" + name;
            if (!File.Exists(path))
            {
                string file = Path.Combine(Source, source);
                if (!File.Exists(file)) throw new FileNotFoundException("Ilustrasi konsep tidak ditemukan: " + Path.GetFullPath(file));
                var input = new Texture2D(2, 2);
                input.LoadImage(File.ReadAllBytes(file));
                Color32[] pixels = input.GetPixels32();
                int sourceWidth = input.width, sourceHeight = input.height;
                var result = new Color32[width * height];
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        // Rata-rata area: titik dither ilustrasi asli melebur menjadi nada, lalu didither ulang pada grid game.
                        int x0 = crop.x + x * crop.width / width, x1 = Mathf.Max(x0 + 1, crop.x + (x + 1) * crop.width / width);
                        int y0 = crop.y + y * crop.height / height, y1 = Mathf.Max(y0 + 1, crop.y + (y + 1) * crop.height / height);
                        float sum = 0;
                        for (int sy = y0; sy < y1; sy++)
                            for (int sx = x0; sx < x1; sx++)
                            {
                                Color32 c = pixels[(sourceHeight - 1 - Mathf.Min(sy, sourceHeight - 1)) * sourceWidth + Mathf.Min(sx, sourceWidth - 1)];
                                sum += (.299f * c.r + .587f * c.g + .114f * c.b) / 255f;
                            }
                        float value = Mathf.Clamp01((sum / ((x1 - x0) * (y1 - y0)) - .04f) / .92f);
                        byte level = (byte)Mathf.RoundToInt(value * 255);
                        result[(height - 1 - y) * width + x] = new Color32(level, level, level, 255);
                    }
                var output = new Texture2D(width, height, TextureFormat.RGB24, false);
                output.SetPixels32(result);
                Directory.CreateDirectory(Folder);
                File.WriteAllBytes(path, output.EncodeToPNG());
                Object.DestroyImmediate(output);
                Object.DestroyImmediate(input);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            if (!importer.isReadable || importer.filterMode != FilterMode.Point || importer.npotScale != TextureImporterNPOTScale.None
                || importer.mipmapEnabled || importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                // Dibaca per piksel saat bermain, jadi ukuran asli dan nilai abu-abu harus utuh.
                importer.textureType = TextureImporterType.Default;
                importer.isReadable = true;
                importer.filterMode = FilterMode.Point;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.mipmapEnabled = false;
                importer.alphaSource = TextureImporterAlphaSource.None;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
    }
}
