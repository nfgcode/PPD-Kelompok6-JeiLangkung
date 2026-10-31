using System;
using TMPro;
using UnityEngine;

namespace JeiLangkung.Opening
{
    [CreateAssetMenu(menuName = "JeiLangkung/Konfigurasi pembuka")]
    public sealed class OpeningConfig : ScriptableObject
    {
        [Serializable]
        public struct DialogueLine
        {
            public string speaker;
            [TextArea(2, 4)] public string text;
            public DialogueLine(string speaker, string text) { this.speaker = speaker; this.text = text; }
        }

        /// <summary>Potret pembicara; nama mengikuti kolom speaker pada dialog.</summary>
        [Serializable]
        public struct Portrait
        {
            public string speaker;
            public Texture2D image;
        }

        [SerializeField] private TMP_FontAsset pixelFont;
        [Tooltip("Gambar abu-abu 480 x 270 dengan Read/Write aktif, dibuat dari ilustrasi konsep menu.")]
        [SerializeField] private Texture2D menuArt;
        [Tooltip("Gambar abu-abu 348 x 196 dengan Read/Write aktif, dibuat dari ilustrasi konsep kos.")]
        [SerializeField] private Texture2D kosArt;
        [SerializeField] private Portrait[] portraits;
        [SerializeField, Min(0.5f)] private float blackoutSeconds = 3;
        [SerializeField] private DialogueLine[] intro = {
            new DialogueLine("RAKA", "Jam sembilan lewat. Kalau bagian terakhir selesai, kita bisa pulang sebelum terlalu malam."),
            new DialogueLine("DIMAS", "Data di laptop sudah masuk. Coba cocokkan lagi dengan catatan Ayu."),
            new DialogueLine("NISA", "Sampai jam berapa kah ini? WA-ku centang satu terus, ih.")
        };
        [SerializeField] private DialogueLine[] aftermath = {
            new DialogueLine("DIMAS", "Satu gang ikut padam. Ini bukan cuma lampu kamar kita."),
            new DialogueLine("NISA", "Tadi dekat sekali dindingnya… kenapa sekarang terasa jauh?"),
            new DialogueLine("AYU", "Nenekku bilang, kalau gelap begini jangan main yang aneh-aneh."),
            new DialogueLine("RAKA", "Tetap di sini dulu. Kita cari penerangan bersama.")
        };
        public TMP_FontAsset PixelFont => pixelFont;
        public Texture2D MenuArt => menuArt;
        public Texture2D KosArt => kosArt;
        public Portrait[] Portraits => portraits ?? Array.Empty<Portrait>();
        public float BlackoutSeconds => Mathf.Max(0.5f, blackoutSeconds);
        public DialogueLine[] Intro => intro;
        public DialogueLine[] Aftermath => aftermath;
        public bool IsValid => pixelFont != null && menuArt != null && menuArt.isReadable && kosArt != null && kosArt.isReadable
            && intro != null && intro.Length > 0 && aftermath != null && aftermath.Length > 0;
    }
}
