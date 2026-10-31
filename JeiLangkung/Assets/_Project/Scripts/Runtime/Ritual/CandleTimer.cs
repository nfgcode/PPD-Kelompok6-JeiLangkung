using System;

namespace JeiLangkung.Ritual
{
    /// <summary>Beberapa lilin sebagai satu batas waktu; lilin terbakar bergiliran sesuai urutan dinyalakan.</summary>
    public sealed class CandleTimer
    {
        public int Count { get; }
        public float SecondsPerCandle { get; }
        public float Elapsed { get; private set; }
        public bool Running { get; private set; }
        public bool Expired { get; private set; }
        public float Total => Count * SecondsPerCandle;
        public float Remaining => Math.Max(0, Total - Elapsed);
        /// <summary>Urutan lilin yang sedang terbakar.</summary>
        public int Burning => Math.Min(Count - 1, (int)(Elapsed / SecondsPerCandle));

        public CandleTimer(int count, float secondsPerCandle)
        {
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
            if (secondsPerCandle <= 0) throw new ArgumentOutOfRangeException(nameof(secondsPerCandle));
            Count = count;
            SecondsPerCandle = secondsPerCandle;
        }

        public void Start() { if (!Expired) Running = true; }
        public void Stop() { Running = false; }

        /// <summary>Mengembalikan true tepat sekali, pada pemanggilan yang menghabiskan lilin terakhir.</summary>
        public bool Tick(float seconds) => Running && seconds > 0 && Consume(seconds);

        /// <summary>Lilin dipercepat, misalnya saat genggaman lepas.</summary>
        public bool Penalize(float seconds) => Tick(seconds);

        /// <summary>Sisa lilin berdasarkan urutan menyala, 0 sampai 1.</summary>
        public float Wax(int order)
        {
            if (order < 0 || order >= Count) throw new ArgumentOutOfRangeException(nameof(order));
            float burned = (Elapsed - order * SecondsPerCandle) / SecondsPerCandle;
            return Math.Min(1, Math.Max(0, 1 - burned));
        }

        private bool Consume(float seconds)
        {
            Elapsed = Math.Min(Total, Elapsed + seconds);
            if (Elapsed < Total) return false;
            Expired = true;
            Running = false;
            return true;
        }
    }
}
