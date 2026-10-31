using System;
using UnityEngine;

namespace JeiLangkung.Ritual
{
    /// <summary>Angka aturan ritual. Semua nilai sementara sampai D-07 diputuskan tim.</summary>
    [Serializable]
    public sealed class RitualTuning
    {
        [Tooltip("Lama satu lilin menyala; empat lilin terbakar bergiliran.")]
        [Min(5)] public float secondsPerCandle = 50;
        [Tooltip("Lilin yang sedang terbakar dipercepat sebanyak ini setiap genggaman lepas.")]
        [Min(0)] public float slipPenaltySeconds = 12;
        public GripTuning grip = new GripTuning();
    }
}
