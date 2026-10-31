using System;

namespace JeiLangkung.Ritual
{
    public enum GripResult { None, Held, Slipped }

    /// <summary>
    /// Genggaman Raka pada boneka, gaya bar memancing. Pemain mengangkat zona genggaman,
    /// entitas menggerakkan rontaan boneka. Rontaan di dalam zona mengisi kemajuan.
    /// </summary>
    public sealed class GripBar
    {
        private readonly GripTuning tuning;
        private readonly Random random;
        private float target;
        private float retarget;
        public float Agitation { get; }
        public float Zone { get; private set; }
        public float ZoneVelocity { get; private set; }
        public float Marker { get; private set; }
        public float Progress { get; private set; }
        public bool Finished { get; private set; }
        public float ZoneSize => tuning.zoneSize;
        public bool Inside => Marker >= Zone && Marker <= Zone + tuning.zoneSize;
        /// <summary>Jarak rontaan dari tengah genggaman, -1 sampai 1. Dipakai untuk kemiringan boneka.</summary>
        public float Strain => Math.Max(-1, Math.Min(1, (Marker - Zone - tuning.zoneSize / 2) / (1 - tuning.zoneSize / 2)));

        public GripBar(GripTuning tuning, float agitation, Random random)
        {
            this.tuning = tuning ?? throw new ArgumentNullException(nameof(tuning));
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            Agitation = Math.Max(0, Math.Min(1, agitation));
            Marker = tuning.zoneSize / 2;
            target = Marker;
            Progress = tuning.startProgress;
        }

        public GripResult Tick(float seconds, bool holding)
        {
            if (Finished || seconds <= 0) return GripResult.None;
            MoveZone(seconds, holding);
            MoveMarker(seconds);
            Progress += (Inside ? tuning.fillPerSecond : -tuning.drainPerSecond) * seconds;
            if (Progress >= 1) { Progress = 1; Finished = true; return GripResult.Held; }
            if (Progress <= 0) { Progress = 0; Finished = true; return GripResult.Slipped; }
            return GripResult.None;
        }

        private void MoveZone(float seconds, bool holding)
        {
            float speed = ZoneVelocity + (holding ? tuning.lift : -tuning.gravity) * seconds;
            ZoneVelocity = Math.Max(-tuning.maxSpeed, Math.Min(tuning.maxSpeed, speed));
            Zone += ZoneVelocity * seconds;
            float top = 1 - tuning.zoneSize;
            if (Zone < 0)
            {
                Zone = 0;
                // Pantulan kecil di dasar supaya zona tidak terasa menempel.
                if (ZoneVelocity < 0) ZoneVelocity *= -.2f;
            }
            else if (Zone > top)
            {
                Zone = top;
                if (ZoneVelocity > 0) ZoneVelocity = 0;
            }
        }

        private void MoveMarker(float seconds)
        {
            retarget -= seconds;
            if (retarget <= 0)
            {
                // Rontaan selalu pindah cukup jauh ke seluruh bar, jadi pemain yang diam tidak bisa menunggu di dasar.
                for (int attempt = 0; attempt < 8; attempt++)
                {
                    target = .08f + (float)random.NextDouble() * .87f;
                    if (Math.Abs(target - Marker) >= .2f) break;
                }
                retarget = (.5f + (float)random.NextDouble() * .9f) / (.8f + .6f * Agitation);
            }
            float step = tuning.markerSpeed * (1 + .6f * Agitation) * seconds;
            float delta = target - Marker;
            Marker += Math.Abs(delta) <= step ? delta : Math.Sign(delta) * step;
        }
    }
}
