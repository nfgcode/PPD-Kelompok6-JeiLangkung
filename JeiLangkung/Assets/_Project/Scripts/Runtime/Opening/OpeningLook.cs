namespace JeiLangkung.Opening
{
    /// <summary>Keadaan tampilan pembuka yang dihitung controller setiap frame.</summary>
    public struct OpeningLook
    {
        /// <summary>Terang lampu kamar, 1 menyala sampai 0 padam.</summary>
        public float Lamp;
        public bool Flashlight;
        public string Speaker;
        /// <summary>Benda tugas yang belum diperiksa: bit 1 laptop, bit 2 catatan.</summary>
        public int Pending;
        public bool Dim;
        public bool ReducedFlicker;
    }
}
