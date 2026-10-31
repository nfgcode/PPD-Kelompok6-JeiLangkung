# Spesifikasi mekanik yang dapat dibuat dan diuji

Gunakan bagian yang relevan dengan fitur. Dokumen singkat untuk satu mekanik tidak perlu mengikuti seluruh format GDD.

## Isi spesifikasi

| Bagian | Informasi yang diperlukan |
|---|---|
| Status | Usulan, sementara, atau disepakati; sumber keputusan |
| Tujuan pemain | Apa yang ingin dicapai dan mengapa tindakan ini menarik |
| Awal dan akhir | Pemicu fitur, keadaan yang diizinkan, kondisi selesai |
| Input | Tindakan pemain, kontrol, batas input selama animasi atau dialog |
| Aturan | Perubahan keadaan, syarat transisi, biaya tindakan, prioritas peristiwa |
| Umpan balik | Tampilan, animasi, suara, tanda bahaya, dan cara mengenali keberhasilan |
| Data | Parameter yang bisa diatur dan satuannya; nilai sementara ditandai |
| Hasil | Berhasil, gagal, batal, restart; hubungan dengan cerita dan ending |
| Kriteria penerimaan | Contoh tindakan dan hasil yang bisa diperiksa |

## Pertanyaan khusus mekanik ritual

- Lilin: kapan waktu berjalan, apakah pause menghentikannya, bagaimana merawatnya, dan apa yang terjadi tepat saat nol? Perawatan tanpa biaya bisa menghilangkan ancaman waktu; jelaskan batasnya jika fitur ini dipakai.
- Empat lilin: apakah padam pertama atau terakhir memicu kalah? Apakah tiap lilin terkait tokoh? Jangan mengasumsikan hubungan ini hanya dari jumlah tokoh.
- Pensil: siapa menentukan jawaban dan kapan hasil dianggap sah? Apakah pemain memutar untuk bertanya, memilih jawaban, atau menstabilkan boneka? Animasi penunjuk tidak boleh tanpa sengaja menjadi aturan hasil.
- Keseimbangan: bagaimana gangguan datang, berapa lama boleh di luar zona, dan apakah kembali aman mereset waktu? Jika acak dipakai, perilaku harus bisa direproduksi untuk pengujian.
- Penutupan: apa syarat ritual boleh diakhiri? Jika penutupan benar dan lilin habis pada frame yang sama, hasil mana menang? Keputusan ini harus eksplisit.
- HP dan puzzle: apakah timer ritual terus berjalan saat panel terbuka? Bagaimana pemain kembali ke permainan utama tanpa kehilangan kontrol?
- Ending dan restart: apakah input, audio, coroutine, timer, dan event direset? Apakah pemain bisa mencoba lagi tanpa state sesi sebelumnya?

## Bentuk kriteria penerimaan

Tuliskan contoh perilaku yang terlihat, misalnya: “Ketika batas waktu yang disepakati habis dalam fase ritual aktif, permainan memicu satu hasil kekalahan dan menolak input ritual berikutnya.” Sesuaikan dengan aturan yang sudah diputuskan; contoh ini tidak menetapkan kapan lilin mulai menyala atau bagaimana penutupan bersamaan diprioritaskan.

Untuk playtest, tambahkan pertanyaan pengalaman: apakah pemain tahu tindakan selanjutnya, mengenali ancaman, memahami alasan gagal, dan ingin mencoba lagi? Bedakan hasil pengamatan pemain dari dugaan desainer.
