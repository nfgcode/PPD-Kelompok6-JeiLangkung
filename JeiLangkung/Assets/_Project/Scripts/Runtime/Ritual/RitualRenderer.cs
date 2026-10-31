using System;
using UnityEngine;
using JeiLangkung.Pixel;
using static JeiLangkung.Pixel.PixelPalette;

namespace JeiLangkung.Ritual
{
    /// <summary>
    /// Meja ritual 480 x 270. Cahaya lilin dihitung per piksel lalu didither ke empat tingkat abu (konsep K-09).
    /// Hanya api lilin dan mata entitas yang berwarna.
    /// </summary>
    public sealed class RitualRenderer : IDisposable
    {
        public const int Width = 480, Height = 270, StripHeight = 12, PanelTop = 234;
        /// <summary>Alas lilin: dua di belakang kertas, dua di depan.</summary>
        public static readonly Vector2Int[] CandleBases = { new Vector2Int(138, 126), new Vector2Int(342, 126), new Vector2Int(132, 226), new Vector2Int(348, 226) };
        /// <summary>Titik mulai pensil di persilangan garis, seperti sketsa storyboard Scene 2.</summary>
        private static readonly Vector2 Start = new Vector2(240, 136);
        private static readonly Vector2 YaWord = new Vector2(204, 153), TidakWord = new Vector2(277, 153);
        private static readonly Rect Glass = new Rect(35, 27, 85, 50);
        private static readonly Vector2 PhoneCenter = new Vector2(70, 204);
        private const float LightRadius = 48, PhoneRadius = 26, BarX = 450, BarY = 92, BarHeight = 124;
        /// <summary>Skala boneka dan jarak ujung pensil ke palang tangan (titik tumpu genggaman).</summary>
        private const float DollScale = 1.25f, Reach = 56 * DollScale;

        // Bagian boneka dalam koordinat lokal sebelum diskalakan; (0, 0) adalah ujung pensil, y negatif ke atas.
        private static readonly float[] PencilTip = { 0, 1, 1.6f, -4, -1.6f, -4 };
        private static readonly float[] PencilBody = { -1.6f, -4, 1.6f, -4, 1.6f, -20, -1.6f, -20 };
        private static readonly float[] Stick = { -2, -19, 2, -19, 2, -24, -2, -24 };
        /// <summary>Kain putih yang digantungkan pada palang, seperti baju.</summary>
        private static readonly float[] Cloth = { -12, -64, 12, -64, 31, -59, 31, -52, 17, -51, 19, -22, -19, -22, -17, -51, -31, -52, -31, -59 };
        private static readonly float[] Neck = { -2, -63, 2, -63, 2, -70, -2, -70 };
        private static readonly float[] Head = BuildHead();

        private readonly PixelCanvas<float> albedo = new PixelCanvas<float>(Width, Height);
        private readonly PixelCanvas<float> work = new PixelCanvas<float>(Width, Height);
        private readonly PixelCanvas<byte> ink = new PixelCanvas<byte>(Width, Height);
        private readonly PixelCanvas<Color32> frame = new PixelCanvas<Color32>(Width, Height);
        private readonly float[][] candleLight = new float[RitualFlow.CandleCount][];
        private readonly float[] phoneLight;
        private readonly float[] intensity = new float[RitualFlow.CandleCount];
        private readonly float[] smokeStart = new float[RitualFlow.CandleCount];
        private readonly Vector2[] scratch = new Vector2[16];
        private Vector2 tip, pivot, restTip, lastInk;
        private float angle, answeredAt, eyesUntil, cos = 1, sin;
        private int answeredCount;
        private bool inking;

        public Texture2D Texture { get; }
        /// <summary>Piksel akhir dengan baris teratas di indeks 0.</summary>
        public Color32[] Pixels => frame.Data;

        public RitualRenderer()
        {
            Texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false) { name = "Meja ritual", filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };
            for (int i = 0; i < candleLight.Length; i++) candleLight[i] = LightMap(FlameOrigin(i), LightRadius);
            phoneLight = LightMap(PhoneCenter, PhoneRadius);
            BuildRoom();
            Reset();
        }

        public void Reset()
        {
            tip = restTip = Start;
            pivot = Start - new Vector2(0, Reach);
            angle = 0;
            answeredAt = eyesUntil = -10;
            answeredCount = 0;
            inking = false;
            Array.Clear(ink.Data, 0, ink.Data.Length);
            for (int i = 0; i < smokeStart.Length; i++) smokeStart[i] = -1;
        }

        public void Render(RitualFlow flow, float time, float deltaTime, RitualView view)
        {
            if (flow.Stage == RitualStage.Warning)
            {
                frame.Fill(Black);
                Upload();
                return;
            }
            bool dark = flow.Stage == RitualStage.CandlesOut;
            UpdateDoll(flow, time, deltaTime);
            Array.Copy(albedo.Data, work.Data, albedo.Data.Length);
            DrawCandles(flow);
            DrawGlass(flow, time);
            float buzz = Buzz(flow);
            DrawPhone(buzz, time);
            DrawDoll();
            UpdateIntensity(flow, time, view.ReducedFlicker, dark);
            // Saat semua lilin padam, ambien nol: sisa terang dither membentuk kisi titik yang terlihat buatan.
            Shade(dark ? 0 : .03f, buzz > 0 ? .7f : 0);
            DrawWindow(flow, time, dark);
            if (buzz > 0) DrawPhoneScreen(buzz, time);
            if (!dark) DrawFlames(flow, time);
            DrawSmoke(flow, time);
            if (time < eyesUntil && !dark && Mathf.Repeat(time * 5, 1) < .7f) DrawDollEyes();
            if (dark) DrawEyes(view);
            DrawStrip(flow);
            frame.Rect(0, PanelTop, Width, Height - PanelTop, Black);
            if (flow.Grip != null) DrawBar(flow.Grip, time);
            if (flow.Stage == RitualStage.Playing && flow.Step.kind == RitualStepKind.LightCandles) DrawCandleFocus(flow, view.HoveredCandle);
            if (view.Paused) Dim(frame);
            Upload();
        }

        public static Vector2 CandleTop(int candle, float wax)
        {
            Vector2Int b = CandleBases[candle];
            return new Vector2(b.x, b.y - BodyHeight(candle, wax));
        }

        private static bool IsFront(int candle) => candle >= 2;
        private static int BodyHeight(int candle, float wax) => wax <= 0 ? 2 : Math.Max(3, Mathf.RoundToInt((IsFront(candle) ? 21 : 15) * wax));
        private static Vector2 FlameOrigin(int candle) => CandleTop(candle, .8f) + new Vector2(0, IsFront(candle) ? -4 : -3);

        private void Upload()
        {
            Texture.SetPixelData(frame.Data, 0);
            Texture.Apply(false);
        }

        private static float[] LightMap(Vector2 origin, float radius)
        {
            var map = new float[Width * Height];
            float r2 = radius * radius;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    float dx = x + .5f - origin.x, dy = (y + .5f - origin.y) * 1.15f;
                    map[y * Width + x] = 1.15f * r2 / (r2 + dx * dx + dy * dy);
                }
            return map;
        }

        private static float[] BuildHead()
        {
            var points = new float[28];
            for (int i = 0; i < 14; i++)
            {
                float a = i / 14f * Mathf.PI * 2;
                points[i * 2] = Mathf.Cos(a) * 14;
                points[i * 2 + 1] = -80 + Mathf.Sin(a) * 12;
            }
            return points;
        }

        private void BuildRoom()
        {
            PixelCanvas<float> c = albedo;
            c.Fill(.07f);
            for (int y = 0; y < 96; y++)
                for (int x = 0; x < Width; x++)
                    c.Data[y * Width + x] = .29f + (Mathf.PerlinNoise(x * .035f + 3, y * .07f) - .5f) * .11f - (y < 26 ? (26 - y) * .005f : 0);
            c.Rect(0, 90, Width, 4, .21f);
            // Jendela: kaca dibiarkan 0 supaya diisi langit malam setelah pencahayaan.
            c.Rect(26, 82, 102, 3, .6f);
            c.Rect(30, 22, 94, 60, .52f);
            c.Rect((int)Glass.x, (int)Glass.y, (int)Glass.width, (int)Glass.height, 0);
            c.Rect(76, 27, 3, 50, .52f);
            c.Rect(35, 51, 85, 2, .52f);
            for (int x = 120; x < 137; x++)
                for (int y = 16; y < 94 + (x % 3); y++)
                    c.Set(x, y, x % 3 == 0 ? .3f : .42f + (Mathf.PerlinNoise(x * .3f, y * .05f) - .5f) * .08f);
            // Pintu sedikit terbuka; celahnya gelap pekat.
            c.Rect(398, 18, 60, 78, .46f);
            c.Rect(403, 23, 50, 73, .24f);
            c.Rect(408, 28, 40, 26, .2f);
            c.Rect(410, 30, 36, 22, .25f);
            c.Rect(408, 60, 40, 30, .2f);
            c.Rect(410, 62, 36, 28, .25f);
            c.Rect(403, 23, 2, 73, -1);
            c.Ellipse(447, 58, 1.6f, 1.6f, .62f);
            // Pengisi daya masih tertancap di stopkontak yang mati, kabelnya menjulur ke HP.
            c.Rect(176, 58, 10, 12, .62f);
            c.Rect(178, 64, 7, 9, .78f);
            float[] cable = { 181, 73, 180, 96, 168, 106, 116, 116, 90, 138, 76, 168, 72, 186 };
            for (int i = 0; i + 3 < cable.Length; i += 2) c.Line(cable[i], cable[i + 1], cable[i + 2], cable[i + 3], .17f);

            for (int y = 96; y < Height; y++)
            {
                float left = 14 - (y - 96) * .5f, right = 466 + (y - 96) * .5f;
                for (int x = Math.Max(0, (int)left); x < Math.Min(Width, (int)right); x++)
                    c.Data[y * Width + x] = .42f + (Mathf.PerlinNoise(x * .035f, y * .55f) - .5f) * .12f;
            }
            for (int k = 1; k < 7; k++)
            {
                float y = 96 + 174 * Mathf.Pow(k / 7f, 1.25f);
                c.Line(14 - (y - 96) * .5f, y, 466 + (y - 96) * .5f, y, .3f);
            }
            c.Rect(14, 96, 452, 2, .3f);

            // Kertas: baris YA/TIDAK di atas, bidang kosong di bawah untuk tulisan pensil pada babak berikutnya.
            Fill(c, .27f, 174, 118, 310, 118, 326, 216, 158, 216);
            Fill(c, .9f, 172, 116, 308, 116, 324, 214, 156, 214);
            for (int y = 116; y < 215; y++)
                for (int x = 156; x < 325; x++)
                    if (c.Data[y * Width + x] > .85f) c.Data[y * Width + x] = .9f + (Mathf.PerlinNoise(x * .21f, y * .21f) - .5f) * .05f;
            for (int x = 172; x < 309; x++) c.Set(x, 136 + Mathf.RoundToInt((Mathf.PerlinNoise(x * .08f, 4) - .5f) * 1.6f), .34f);
            for (int y = 120; y < 170; y++) c.Set(240 + Mathf.RoundToInt((Mathf.PerlinNoise(9, y * .08f) - .5f) * 1.6f), y, .34f);
            c.Ellipse(Start.x, Start.y, 5, 3.5f, .34f);
            c.Ellipse(Start.x, Start.y, 3.5f, 2.2f, .9f);
            PixelFont.Draw(c, "YA", 193, 146, .16f, 2);
            PixelFont.Draw(c, "TIDAK", 248, 146, .16f, 2);

            for (int i = 0; i < CandleBases.Length; i++)
            {
                Vector2Int b = CandleBases[i];
                float rx = IsFront(i) ? 11 : 8, ry = IsFront(i) ? 4 : 3;
                c.Ellipse(b.x + 1, b.y + 1.5f, rx + 1, ry + 1, .26f);
                c.Ellipse(b.x, b.y, rx, ry, .6f);
                c.Ellipse(b.x, b.y - .5f, rx * .6f, ry * .55f, .7f);
            }
            // Korek api dan batang bekas di dekat lilin kiri belakang.
            c.Rect(84, 134, 15, 8, .58f);
            c.Rect(84, 134, 15, 2, .3f);
            c.Line(103, 141, 116, 144, .62f);
            c.Rect(101, 140, 2, 2, .1f);
        }

        private void Fill<T>(PixelCanvas<T> canvas, T value, params float[] xy) where T : struct
        {
            int count = xy.Length / 2;
            for (int i = 0; i < count; i++) scratch[i] = new Vector2(xy[i * 2], xy[i * 2 + 1]);
            canvas.Polygon(scratch, count, value);
        }

        /// <summary>
        /// Tangan memegang palang, jadi boneka berayun di sekitar palang itu. Untuk mencapai jawaban,
        /// boneka sedikit turun lalu condong sehingga ujung pensil tiba di kata yang dituju.
        /// </summary>
        private static void PoseFor(Vector2 target, out Vector2 pivotTarget, out float angleTarget)
        {
            float dx = target.x - Start.x;
            angleTarget = Mathf.Asin(Mathf.Clamp(-.7f * dx / Reach, -.6f, .6f));
            pivotTarget = target - new Vector2(-Reach * Mathf.Sin(angleTarget), Reach * Mathf.Cos(angleTarget));
        }

        private void UpdateDoll(RitualFlow flow, float time, float deltaTime)
        {
            if (flow.Answers.Count > answeredCount)
            {
                answeredCount = flow.Answers.Count;
                answeredAt = time;
                eyesUntil = time + .9f;
            }
            Vector2 target = restTip;
            bool writing = false;
            RitualStep step = flow.Step;
            if (flow.Stage == RitualStage.Playing && step.kind == RitualStepKind.Question)
            {
                Vector2 word = step.answer == RitualAnswer.Ya ? YaWord : TidakWord;
                Vector2 restPoint = word + new Vector2(0, -12);
                if (flow.Phase == QuestionPhase.Waiting) target = Start;
                else if (flow.Phase == QuestionPhase.Answering)
                {
                    target = Vector2.Lerp(Start, restPoint, flow.PencilTravel);
                    target += new Vector2(Mathf.PerlinNoise(time * 7, 2) - .5f, Mathf.PerlinNoise(3, time * 7) - .5f) * 2;
                    writing = true;
                }
                else
                {
                    // Pensil melingkari jawaban, lalu diam di atasnya.
                    float loop = (time - answeredAt) / 1.2f;
                    if (loop < 1)
                    {
                        float a = (loop * 2 - .5f) * Mathf.PI;
                        target = word + new Vector2(Mathf.Cos(a) * 18, Mathf.Sin(a) * 10);
                        writing = true;
                    }
                    else target = restPoint;
                    restTip = restPoint;
                }
            }
            PoseFor(target, out Vector2 pivotTarget, out float angleTarget);
            if (flow.Grip != null)
            {
                // Rontaan memutar boneka di sekitar palang yang digenggam.
                angleTarget += flow.Grip.Strain * .38f;
                if (!flow.Grip.Inside) angleTarget += (Mathf.PerlinNoise(time * 9, 1.7f) - .5f) * .24f;
            }
            else if (!writing) angleTarget += Mathf.Sin(time * 1.3f) * .03f;
            float follow = 1 - Mathf.Exp(-deltaTime * (writing ? 10 : 5));
            pivot += (pivotTarget - pivot) * follow;
            angle += (angleTarget - angle) * (1 - Mathf.Exp(-deltaTime * 10));
            cos = Mathf.Cos(angle);
            sin = Mathf.Sin(angle);
            tip = pivot + new Vector2(-Reach * sin, Reach * cos);
            if (writing) Ink(tip);
            inking = writing;
        }

        private void Ink(Vector2 point)
        {
            if (inking) ink.Line(lastInk.x, lastInk.y, point.x, point.y, (byte)1);
            lastInk = point;
        }

        private Vector2 Local(float x, float y)
        {
            x *= DollScale;
            y *= DollScale;
            return new Vector2(tip.x + x * cos - y * sin, tip.y + x * sin + y * cos);
        }

        /// <summary>Bagian boneka dengan kontur gelap satu piksel supaya kain putih tetap terbaca di atas kertas putih.</summary>
        private void Part(float[] shape, float value, bool outline = true)
        {
            int count = shape.Length / 2;
            if (outline)
                for (int side = 0; side < 4; side++)
                {
                    Vector2 offset = side == 0 ? Vector2.left : side == 1 ? Vector2.right : side == 2 ? Vector2.up : Vector2.down;
                    for (int i = 0; i < count; i++) scratch[i] = Local(shape[i * 2], shape[i * 2 + 1]) + offset;
                    work.Polygon(scratch, count, .14f);
                }
            for (int i = 0; i < count; i++) scratch[i] = Local(shape[i * 2], shape[i * 2 + 1]);
            work.Polygon(scratch, count, value);
        }

        private void LocalLine(float x0, float y0, float x1, float y1, float value, float thickness = 1)
        {
            Vector2 a = Local(x0, y0), b = Local(x1, y1);
            work.Line(a.x, a.y, b.x, b.y, value, thickness);
        }

        private void DrawDoll()
        {
            Vector2 ayu = Local(-35, -56), dimas = Local(35, -56), raka = Local(0, -36);
            Arm(new Vector2(-30, 96), ayu, 18, 14, 9, .7f);
            Arm(new Vector2(510, 96), dimas, 18, 14, 9, .26f);
            Part(PencilBody, .64f);
            Part(PencilTip, .2f, false);
            LocalLine(-2, -15, 2, -16, .92f);
            LocalLine(-2, -17, 2, -18, .92f);
            Part(Stick, .36f, false);
            LocalLine(-36, -56, 36, -56, .14f, 5);
            LocalLine(-36, -56, 36, -56, .42f, 3);
            Part(Cloth, .82f);
            LocalLine(-15, -50, -12, -24, .6f);
            LocalLine(15, -50, 12, -24, .6f);
            LocalLine(-2, -60, 0, -24, .66f);
            LocalLine(-18, -23, 18, -23, .62f);
            LocalLine(-10, -63, 10, -63, .5f, 2);
            Part(Neck, .36f);
            Part(Head, .5f);
            // Serat batok kelapa dan pinggir yang tertimpa cahaya lilin.
            for (int i = 0; i < 10; i++) LocalLine(-11 + i * 2.4f, -89 + (i % 3), -10 + i * 2.4f, -87 + (i % 3), .38f);
            LocalLine(-12, -86, -9, -91, .64f);
            LocalLine(-9, -91, 2, -92, .64f);
            // Wajah kapur pada batok kelapa.
            LocalLine(-8, -82, -3, -81, .96f);
            LocalLine(3, -81, 8, -82, .96f);
            LocalLine(-5, -74, -2, -75, .96f);
            LocalLine(-2, -75, 2, -74, .96f);
            LocalLine(2, -74, 5, -75, .96f);
            Hand(ayu, .58f);
            Hand(dimas, .56f);
            Arm(new Vector2(480, 220), raka, 14, 22, 12, .17f);
            Hand(raka, .62f);
        }

        /// <summary>Lengan dua ruas; siku jatuh ke bawah layar supaya terbaca sebagai lengan, bukan papan.</summary>
        private void Arm(Vector2 shoulder, Vector2 hand, float bend, float startWidth, float endWidth, float value)
        {
            Vector2 along = hand - shoulder, normal = new Vector2(-along.y, along.x).normalized;
            if (normal.y < 0) normal = -normal;
            Vector2 elbow = Vector2.Lerp(shoulder, hand, .5f) + normal * bend;
            float middle = (startWidth + endWidth) / 2;
            Vector2 toElbow = (elbow - hand).normalized;
            Vector2 wrist = hand + toElbow * 4, cuff = hand + toElbow * 6;
            work.Quad(shoulder.x, shoulder.y, elbow.x, elbow.y, startWidth, middle, value);
            work.Ellipse(elbow.x, elbow.y, middle / 2, middle / 2, value);
            work.Quad(elbow.x, elbow.y, wrist.x, wrist.y, middle, endWidth, value);
            work.Quad(cuff.x, cuff.y, wrist.x, wrist.y, endWidth + 1, endWidth + 1, value * .75f);
        }

        private void Hand(Vector2 at, float value)
        {
            work.Ellipse(at.x, at.y, 5.5f, 4.5f, value);
            Vector2 across = new Vector2(cos, sin), down = new Vector2(-sin, cos);
            for (int i = -1; i <= 2; i++)
            {
                Vector2 a = at + across * (i * 2.4f - 1) - down * 3, b = a + down * 6;
                work.Line(a.x, a.y, b.x, b.y, value * .74f);
            }
        }

        private void DrawCandles(RitualFlow flow)
        {
            for (int i = 0; i < CandleBases.Length; i++)
            {
                int order = flow.OrderOf(i);
                float wax = order < 0 ? 1 : flow.Candles.Wax(order);
                Vector2Int b = CandleBases[i];
                int half = IsFront(i) ? 3 : 2, height = BodyHeight(i, wax);
                int top = b.y - height;
                work.Rect(b.x - half, top, half * 2 + 1, height, .84f);
                work.Rect(b.x - half, top, 1, height, .95f);
                work.Rect(b.x + half, top, 1, height, .66f);
                work.Rect(b.x - half, top, half * 2 + 1, 1, .92f);
                if (wax > 0) work.Rect(b.x - half + 1, top + 2, 1, Math.Min(height - 2, 4), .97f);
                work.Rect(b.x, top - 2, 1, 2, .08f);
            }
        }

        private void DrawGlass(RitualFlow flow, float time)
        {
            const float x = 102, top = 160;
            work.Ellipse(x + 1, top + 21, 8, 2.5f, .26f);
            work.Rect((int)x - 6, (int)top, 13, 21, .5f);
            work.Rect((int)x - 6, (int)top + 6, 13, 15, .36f);
            work.Rect((int)x - 6, (int)top, 1, 21, .72f);
            float age = flow.SignAge(RitualSign.GlassRipple);
            work.Ellipse(x, top + 6, 6, 2, .58f);
            if (age >= 0 && age < 3.5f)
            {
                float phase = Mathf.Repeat(time * 6, 1);
                work.Ellipse(x, top + 6, 6 * phase, 2 * phase, .3f);
                work.Ellipse(x, top + 6, 4 * phase, 1.2f * phase, .72f);
            }
            work.Ellipse(x, top, 6, 2, .7f);
            work.Ellipse(x, top, 4.5f, 1.2f, .45f);
        }

        private static float Buzz(RitualFlow flow)
        {
            float age = flow.SignAge(RitualSign.PhoneBuzz);
            if (age < 0 || age > 2.4f) return 0;
            return Mathf.Repeat(age, .6f) < .4f ? 1 : .35f;
        }

        private static void PhoneAxes(float shake, out Vector2 center, out Vector2 up, out Vector2 side)
        {
            const float tilt = -.26f;
            center = PhoneCenter + new Vector2(shake, 0);
            up = new Vector2(Mathf.Sin(tilt), -Mathf.Cos(tilt));
            side = new Vector2(Mathf.Cos(tilt), Mathf.Sin(tilt));
        }

        private static float PhoneShake(float buzz, float time) => buzz >= 1 ? (Mathf.Repeat(time * 30, 1) < .5f ? -1 : 1) : 0;

        private void DrawPhone(float buzz, float time)
        {
            PhoneAxes(PhoneShake(buzz, time), out Vector2 c, out Vector2 up, out Vector2 side);
            Box(work, c + new Vector2(1.5f, 1.5f), up, side, 10.5f, 17.5f, .2f);
            Box(work, c, up, side, 10, 17, .13f);
            Box(work, c, up, side, 8, 14.5f, .07f);
        }

        /// <summary>HP menyala tanpa notifikasi apa pun: layar kosong yang dingin.</summary>
        private void DrawPhoneScreen(float buzz, float time)
        {
            PhoneAxes(PhoneShake(buzz, time), out Vector2 c, out Vector2 up, out Vector2 side);
            Box(frame, c, up, side, 8, 14.5f, PixelPalette.Light);
            Box(frame, c + up * 12, up, side, 8, 1.5f, White);
        }

        private void Box<T>(PixelCanvas<T> canvas, Vector2 c, Vector2 up, Vector2 side, float halfWidth, float halfHeight, T value) where T : struct
        {
            scratch[0] = c - side * halfWidth + up * halfHeight;
            scratch[1] = c + side * halfWidth + up * halfHeight;
            scratch[2] = c + side * halfWidth - up * halfHeight;
            scratch[3] = c - side * halfWidth - up * halfHeight;
            canvas.Polygon(scratch, 4, value);
        }

        private void UpdateIntensity(RitualFlow flow, float time, bool reducedFlicker, bool dark)
        {
            for (int i = 0; i < intensity.Length; i++)
            {
                int order = flow.OrderOf(i);
                float wax = order < 0 ? 0 : flow.Candles.Wax(order);
                if (dark || order < 0 || wax <= 0) { intensity[i] = 0; continue; }
                float amplitude = reducedFlicker ? .05f : .2f, level = 1;
                // Lilin yang hampir habis meredup dan berkedip lebih keras sebagai peringatan.
                if (wax < .2f) { level = .45f + .55f * wax / .2f; amplitude *= 2; }
                intensity[i] = level * (1 - amplitude * Mathf.PerlinNoise(time * 6.5f + i * 7.3f, i * 3.1f));
            }
        }

        private void Shade(float ambient, float phone)
        {
            float[] a = work.Data, l0 = candleLight[0], l1 = candleLight[1], l2 = candleLight[2], l3 = candleLight[3];
            byte[] k = ink.Data;
            Color32[] f = frame.Data;
            float i0 = intensity[0], i1 = intensity[1], i2 = intensity[2], i3 = intensity[3];
            for (int y = StripHeight; y < PanelTop; y++)
            {
                int row = y * Width, bayer = (y & 3) * 4;
                for (int x = 0; x < Width; x++)
                {
                    int i = row + x;
                    float light = ambient + i0 * l0[i] + i1 * l1[i] + i2 * l2[i] + i3 * l3[i] + phone * phoneLight[i];
                    float value = (k[i] != 0 && a[i] > .85f ? .16f : a[i]) * light;
                    int level = (int)(value * 3 + Bayer[bayer + (x & 3)]);
                    f[i] = Greys[level < 0 ? 0 : level > 3 ? 3 : level];
                }
            }
        }

        private void DrawWindow(RitualFlow flow, float time, bool dark)
        {
            for (int y = (int)Glass.yMin; y < (int)Glass.yMax; y++)
                for (int x = (int)Glass.xMin; x < (int)Glass.xMax; x++)
                {
                    if (albedo.Data[y * Width + x] != 0) continue;
                    // Langit malam dengan cahaya kilang di cakrawala: kota minyak tetap terang, Km 15 gelap.
                    bool glow = y >= 69 || (y >= 64 && ((x + y) & 1) == 0) || (y >= 59 && ((x + y) & 3) == 0);
                    frame.Data[y * Width + x] = glow && !dark ? Dark : Black;
                }
            if (dark) return;
            Fill(frame, Black, 35, 77, 35, 70, 44, 65, 53, 70, 58, 70, 58, 67, 70, 67, 70, 71, 76, 77);
            Fill(frame, Black, 79, 77, 79, 69, 88, 63, 97, 69, 97, 77);
            frame.Rect(108, 53, 2, 24, Black);
            frame.Rect(113, 62, 2, 15, Black);
            frame.Set(101, 67, PixelPalette.Light);
            frame.Set(116, 65, PixelPalette.Light);
            frame.Set(104, 72, PixelPalette.Light);
            int flare = Mathf.FloorToInt(Mathf.PerlinNoise(time * 5, 8) * 3);
            frame.Rect(108, 51 - flare, 2, flare + 2, White);
            frame.Set(108 + (flare & 1), 50 - flare, PixelPalette.Light);
            float age = flow.SignAge(RitualSign.WindowShadow);
            if (age >= 0 && age < 2.2f)
            {
                float x = Mathf.Lerp(126, 22, age / 2.2f);
                frame.Ellipse(x, 55, 5, 6, Black);
                frame.Rect((int)x - 2, 60, 4, 7, Black);
                Fill(frame, Black, x - 11, 77, x - 8, 67, x + 8, 67, x + 11, 77);
            }
        }

        private void DrawFlames(RitualFlow flow, float time)
        {
            float leanAge = flow.SignAge(RitualSign.FlameLean);
            float lean = leanAge >= 0 && leanAge < 4 ? Mathf.Sin(Mathf.Clamp01(leanAge / 4) * Mathf.PI) * 3 : 0;
            for (int i = 0; i < CandleBases.Length; i++)
            {
                if (intensity[i] <= 0) continue;
                float wax = flow.Candles.Wax(flow.OrderOf(i));
                Vector2 top = CandleTop(i, wax);
                bool weak = wax < .2f;
                int x = (int)top.x, y = (int)top.y - 2;
                int height = (IsFront(i) ? 6 : 5) - (weak ? 2 : 0) + (Mathf.PerlinNoise(time * 9, i) > .62f ? 1 : 0);
                float sway = (Mathf.PerlinNoise(time * 4, i * 5.1f) - .5f) * 2;
                for (int r = 0; r < height; r++)
                {
                    float t = r / (float)height;
                    int cx = x + Mathf.RoundToInt((sway - lean) * t * t);
                    int half = r > 0 && r < height - 2 && IsFront(i) ? 1 : 0;
                    frame.Rect(cx - half, y - r, half * 2 + 1, 1, FlameOuter);
                }
                if (!weak) frame.Rect(x, y - 2, 1, 2, FlameInner);
            }
        }

        private void DrawSmoke(RitualFlow flow, float time)
        {
            for (int i = 0; i < CandleBases.Length; i++)
            {
                int order = flow.OrderOf(i);
                bool burned = order >= 0 && (flow.Candles.Wax(order) <= 0 || flow.Stage == RitualStage.CandlesOut);
                if (!burned) { smokeStart[i] = -1; continue; }
                if (smokeStart[i] < 0) smokeStart[i] = time;
                Vector2 top = CandleTop(i, 0);
                for (int p = 0; p < 6; p++)
                {
                    float age = time - smokeStart[i] - p * .45f;
                    if (age < 0 || age > 3) continue;
                    int x = (int)(top.x + Mathf.Sin(age * 3 + p) * 2), y = (int)(top.y - 4 - age * 9);
                    frame.Set(x, y, age < 1.5f ? PixelPalette.Light : Dark);
                }
            }
        }

        private void DrawDollEyes()
        {
            Vector2 left = Local(-5.5f, -81.5f), right = Local(5.5f, -81.5f);
            frame.Rect((int)left.x - 1, (int)left.y, 3, 2, EyeRed);
            frame.Rect((int)right.x - 1, (int)right.y, 3, 2, EyeRed);
        }

        private void DrawEyes(RitualView view)
        {
            float t = view.OutSeconds - 1.2f;
            if (t < 0) return;
            if (view.ReducedFlicker && t < .3f) return;
            bool burst = !view.ReducedFlicker && t < .45f;
            int scale = burst ? 3 : 2;
            int ox = burst ? Mathf.RoundToInt(Mathf.Sin(t * 90) * 3) : 0, oy = burst ? Mathf.RoundToInt(Mathf.Cos(t * 70) * 2) : 0;
            int y = 104 + oy;
            for (int row = -30; row < 52; row++)
                for (int col = -24; col <= 24; col++)
                    if ((col * col) / 576f + (row * row) / (row < 0 ? 900f : 4900f) < 1 && ((row + col) % 3 == 0))
                        frame.Set(240 + ox + col, y + row, Dark);
            frame.Rect(240 + ox - 6 * scale, y, 4 * scale, 2 * scale, EyeRed);
            frame.Rect(240 + ox + 2 * scale, y, 4 * scale, 2 * scale, EyeRed);
        }

        private void DrawStrip(RitualFlow flow)
        {
            frame.Rect(0, 0, Width, StripHeight, Black);
            for (int order = 0; order < RitualFlow.CandleCount; order++)
            {
                int x = 418 + order * 15;
                frame.Rect(x, 4, 12, 4, Dark);
                float wax = order < flow.LitCount ? flow.Candles.Wax(order) : 0;
                if (order < flow.LitCount && flow.Stage != RitualStage.CandlesOut)
                    frame.Rect(x, 4, Mathf.CeilToInt(12 * wax), 4, wax < .2f ? PixelPalette.Light : White);
            }
        }

        private void DrawBar(GripBar grip, float time)
        {
            int x = (int)BarX, y = (int)BarY, h = (int)BarHeight;
            frame.Rect(x - 2, y - 2, 18, h + 4, PixelPalette.Light);
            frame.Rect(x, y, 14, h, Black);
            int zoneTop = y + Mathf.RoundToInt((1 - grip.Zone - grip.ZoneSize) * h), zoneHeight = Mathf.RoundToInt(grip.ZoneSize * h);
            if (grip.Inside) frame.Rect(x + 1, zoneTop, 12, zoneHeight, PixelPalette.Light);
            else frame.Checker(x + 1, zoneTop, 12, zoneHeight, Dark, Black);
            int jitter = grip.Inside ? 0 : (Mathf.Repeat(time * 24, 1) < .5f ? -1 : 1);
            float my = y + (1 - grip.Marker) * h;
            frame.Ellipse(x + 7 + jitter, my, 5, 4, White);
            frame.Rect(x + 4 + jitter, (int)my - 1, 2, 1, Black);
            frame.Rect(x + 8 + jitter, (int)my - 1, 2, 1, Black);
            frame.Rect(x + 18, y - 2, 8, h + 4, PixelPalette.Light);
            frame.Rect(x + 20, y, 4, h, Black);
            int fill = Mathf.RoundToInt(grip.Progress * h);
            frame.Rect(x + 20, y + h - fill, 4, fill, White);
        }

        private void DrawCandleFocus(RitualFlow flow, int hovered)
        {
            int candle = hovered >= 0 && !flow.IsLit(hovered) ? hovered : -1;
            for (int i = 0; candle < 0 && i < RitualFlow.CandleCount; i++) if (!flow.IsLit(i)) candle = i;
            if (candle < 0) return;
            Vector2Int b = CandleBases[candle];
            int height = BodyHeight(candle, 1), left = b.x - 8, right = b.x + 8, top = b.y - height - 9, bottom = b.y + 5;
            Corner(left, top, 1, 1);
            Corner(right, top, -1, 1);
            Corner(left, bottom, 1, -1);
            Corner(right, bottom, -1, -1);
        }

        private void Corner(int x, int y, int dx, int dy)
        {
            frame.Rect(dx > 0 ? x : x - 3, y, 4, 1, White);
            frame.Rect(x, dy > 0 ? y : y - 3, 1, 4, White);
        }

        public void Dispose()
        {
            if (Texture == null) return;
            if (Application.isPlaying) UnityEngine.Object.Destroy(Texture);
            else UnityEngine.Object.DestroyImmediate(Texture);
        }
    }
}
