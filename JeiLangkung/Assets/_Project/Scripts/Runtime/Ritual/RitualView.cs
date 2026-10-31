namespace JeiLangkung.Ritual
{
    /// <summary>Keadaan tampilan yang bukan aturan permainan: jeda, sorotan lilin, dan waktu sejak lilin terakhir padam.</summary>
    public struct RitualView
    {
        public bool Paused;
        public int HoveredCandle;
        public float OutSeconds;
        public bool ReducedFlicker;

        public static RitualView Default => new RitualView { HoveredCandle = -1, OutSeconds = -1 };
    }
}
