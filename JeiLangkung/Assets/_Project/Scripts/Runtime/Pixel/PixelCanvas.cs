using System;
using UnityEngine;

namespace JeiLangkung.Pixel
{
    /// <summary>Bidang piksel kecil dengan sumbu y ke bawah. T berupa nilai terang (float) atau warna akhir (Color32).</summary>
    public sealed class PixelCanvas<T> where T : struct
    {
        private readonly float[] crossings = new float[32];
        private readonly Vector2[] quad = new Vector2[4];
        public int Width { get; }
        public int Height { get; }
        public T[] Data { get; }

        public PixelCanvas(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new T[width * height];
        }

        public void Fill(T value) => Array.Fill(Data, value);

        public void Set(int x, int y, T value)
        {
            if ((uint)x < (uint)Width && (uint)y < (uint)Height) Data[y * Width + x] = value;
        }

        public void Rect(int x, int y, int w, int h, T value)
        {
            int x0 = Math.Max(0, x), x1 = Math.Min(Width, x + w);
            for (int row = Math.Max(0, y); row < Math.Min(Height, y + h); row++)
                for (int col = x0; col < x1; col++) Data[row * Width + col] = value;
        }

        /// <summary>Pola kotak-kotak dua nilai, ciri bayangan dithering.</summary>
        public void Checker(int x, int y, int w, int h, T even, T odd)
        {
            int x0 = Math.Max(0, x), x1 = Math.Min(Width, x + w);
            for (int row = Math.Max(0, y); row < Math.Min(Height, y + h); row++)
                for (int col = x0; col < x1; col++) Data[row * Width + col] = ((row + col) & 1) == 0 ? even : odd;
        }

        public void Ellipse(float cx, float cy, float rx, float ry, T value)
        {
            for (int row = (int)Math.Floor(cy - ry); row <= (int)Math.Ceiling(cy + ry); row++)
            {
                float dy = (row + .5f - cy) / ry;
                if (dy * dy > 1) continue;
                float half = rx * (float)Math.Sqrt(1 - dy * dy);
                Span(row, cx - half, cx + half, value);
            }
        }

        public void Polygon(Vector2[] points, T value) => Polygon(points, points.Length, value);

        /// <summary>Isi poligon dengan aturan genap-ganjil pada pusat piksel.</summary>
        public void Polygon(Vector2[] points, int count, T value)
        {
            float top = float.MaxValue, bottom = float.MinValue;
            for (int i = 0; i < count; i++) { top = Math.Min(top, points[i].y); bottom = Math.Max(bottom, points[i].y); }
            for (int row = Math.Max(0, (int)Math.Floor(top)); row <= Math.Min(Height - 1, (int)Math.Ceiling(bottom)); row++)
            {
                float y = row + .5f;
                int found = 0;
                for (int i = 0, j = count - 1; i < count && found < crossings.Length; j = i++)
                {
                    Vector2 a = points[i], b = points[j];
                    if ((a.y > y) == (b.y > y)) continue;
                    crossings[found++] = a.x + (y - a.y) / (b.y - a.y) * (b.x - a.x);
                }
                Array.Sort(crossings, 0, found);
                for (int k = 0; k + 1 < found; k += 2) Span(row, crossings[k], crossings[k + 1], value);
            }
        }

        /// <summary>Garis setebal thickness piksel; garis tipis memakai langkah Bresenham.</summary>
        public void Line(float x0, float y0, float x1, float y1, T value, float thickness = 1)
        {
            if (thickness > 1.5f) { Quad(x0, y0, x1, y1, thickness, thickness, value); return; }
            int ax = (int)Math.Floor(x0), ay = (int)Math.Floor(y0), bx = (int)Math.Floor(x1), by = (int)Math.Floor(y1);
            int dx = Math.Abs(bx - ax), dy = -Math.Abs(by - ay), sx = ax < bx ? 1 : -1, sy = ay < by ? 1 : -1, error = dx + dy;
            while (true)
            {
                Set(ax, ay, value);
                if (ax == bx && ay == by) return;
                int twice = 2 * error;
                if (twice >= dy) { error += dy; ax += sx; }
                if (twice <= dx) { error += dx; ay += sy; }
            }
        }

        /// <summary>Batang dengan lebar berbeda di kedua ujung, misalnya lengan yang mengecil ke pergelangan.</summary>
        public void Quad(float x0, float y0, float x1, float y1, float startWidth, float endWidth, T value)
        {
            float dx = x1 - x0, dy = y1 - y0, length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length < .01f) return;
            float nx = -dy / length * .5f, ny = dx / length * .5f;
            quad[0] = new Vector2(x0 + nx * startWidth, y0 + ny * startWidth);
            quad[1] = new Vector2(x1 + nx * endWidth, y1 + ny * endWidth);
            quad[2] = new Vector2(x1 - nx * endWidth, y1 - ny * endWidth);
            quad[3] = new Vector2(x0 - nx * startWidth, y0 - ny * startWidth);
            Polygon(quad, 4, value);
        }

        private void Span(int row, float left, float right, T value)
        {
            if ((uint)row >= (uint)Height) return;
            int x0 = Math.Max(0, (int)Math.Ceiling(left - .5f)), x1 = Math.Min(Width - 1, (int)Math.Floor(right - .5f));
            for (int col = x0; col <= x1; col++) Data[row * Width + col] = value;
        }
    }
}
