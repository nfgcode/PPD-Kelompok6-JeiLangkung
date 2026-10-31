using System;
using UnityEngine;

namespace JeiLangkung.Ritual
{
    /// <summary>Nilai sementara bar genggaman. Satuan posisi adalah tinggi bar (0 bawah, 1 atas).</summary>
    [Serializable]
    public sealed class GripTuning
    {
        [Range(.1f, .6f)] public float zoneSize = .27f;
        [Tooltip("Percepatan naik per detik saat tombol ditahan.")]
        [Min(.1f)] public float lift = 3.1f;
        [Tooltip("Percepatan turun per detik saat tombol dilepas.")]
        [Min(.1f)] public float gravity = 2.2f;
        [Min(.1f)] public float maxSpeed = 1.35f;
        [Range(0, 1)] public float startProgress = .3f;
        [Min(.01f)] public float fillPerSecond = .22f;
        [Min(.01f)] public float drainPerSecond = .14f;
        [Tooltip("Kecepatan rontaan boneka pada agitasi 0; agitasi 1 menambah 60%.")]
        [Min(.05f)] public float markerSpeed = .5f;
    }
}
