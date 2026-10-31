using TMPro;
using UnityEngine;

namespace JeiLangkung.Ritual
{
    /// <summary>Naskah dan angka Scene 2. Dialog di bawah adalah teks prototype yang boleh diganti tim.</summary>
    [CreateAssetMenu(menuName = "JeiLangkung/Konfigurasi ritual")]
    public sealed class RitualConfig : ScriptableObject
    {
        [SerializeField] private TMP_FontAsset pixelFont;
        [SerializeField] private RitualTuning tuning = new RitualTuning();
        [SerializeField, TextArea(3, 6)] private string contentWarning =
            "JeiLangkung adalah cerita fiksi. Ritual Jelangkung di sini ditampilkan sebagai bagian dari cerita horor budaya lokal, bukan ajakan untuk mencobanya.\n\nBabak ini memuat gelap, cahaya berkedip, dan suara kejut.";
        [SerializeField] private string location = "Kos Km 15, listrik padam";
        [SerializeField] private string chapter = "Babak 2: Datang Tak Dijemput";
        [SerializeField] private string completeText = "Bersambung ke Babak 3: Harapan Tak Kunjung Usai.";
        [SerializeField] private string candlesOutText = "Lilin terakhir padam.";
        [SerializeField] private RitualStep[] steps = {
            RitualStep.Line("Ayu", "Kata nenekku, keempat lilin harus menyala dulu sebelum kita mulai."),
            RitualStep.LightCandles("Nyalakan keempat lilin di sudut kertas."),
            RitualStep.Line("Ayu", "Pegang bonekanya bertiga. Apa pun yang terjadi, jangan dilepas."),
            RitualStep.Line("Ayu", "Jelangkung, jelangkung, di sini ada pesta kecil-kecilan."),
            RitualStep.Line("Ayu", "Datang tak dijemput, pulang tak diantar."),
            RitualStep.Sign(RitualSign.GlassRipple),
            RitualStep.Grip("Boneka tiba-tiba berat dan meronta. Jaga boneka tetap di dalam genggaman sampai ia tenang.", .5f),
            RitualStep.Line("Nisa", "Dia... sudah datang kah?"),
            RitualStep.Question("Raka", "Kamu ada di sini?", RitualAnswer.Ya, .3f),
            RitualStep.Sign(RitualSign.FlameLean),
            RitualStep.Line("Dimas", "Bah, apinya miring semua. Padahal kipas mati."),
            RitualStep.Line("Dimas", "Tanya yang penting, Rak."),
            RitualStep.Sign(RitualSign.PhoneBuzz),
            RitualStep.Question("Raka", "Apakah lampu akan menyala malam ini?", RitualAnswer.Ya, .7f),
            RitualStep.Line("Nisa", "Katanya menyala! Syukurlah..."),
            RitualStep.Sign(RitualSign.WindowShadow),
            RitualStep.Line("Ayu", "Jangan senang dulu. Dia belum kita antar pulang.")
        };

        public TMP_FontAsset PixelFont => pixelFont;
        public RitualTuning Tuning => tuning;
        public string ContentWarning => contentWarning;
        public string Location => location;
        public string Chapter => chapter;
        public string CompleteText => completeText;
        public string CandlesOutText => candlesOutText;
        public RitualStep[] Steps => steps;
        public bool IsValid => pixelFont != null && tuning != null && tuning.grip != null && steps != null && steps.Length > 0;
    }
}
