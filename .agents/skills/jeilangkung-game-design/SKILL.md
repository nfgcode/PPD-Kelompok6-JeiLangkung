---
name: jeilangkung-game-design
description: Merancang cerita, aturan permainan, cakupan demo, storyboard, dan kebutuhan aset JeiLangkung serta menyelaraskan GDD dengan FigJam. Gunakan untuk pekerjaan desain game sebelum atau selama implementasi.
---

# Desain game JeiLangkung

Baca `AGENTS.md`, `docs/game-development.md`, dan bagian relevan dari `docs/design-decisions.md` dari root repositori. Gunakan bahasa Indonesia; istilah API dan identifier boleh dipertahankan.

## Menentukan dasar pekerjaan

- Tentukan apakah tugas meminta konsep, revisi cerita, spesifikasi mekanik, sinkronisasi dokumen, atau rencana demo.
- Baca sumber yang benar-benar terkait. Untuk FigJam, gunakan file dan node dari tautan pengguna. Jangan mengubah board hanya karena diberi tautan untuk diperiksa.
- Bedakan keputusan pengguna, isi dokumen lama, usulan baru, dan fakta kondisi proyek. Template 2D tidak berarti pilihan visual sudah final.
- Bila konflik memengaruhi hasil, tanyakan pilihan beserta dampaknya secara singkat. Jangan menetapkan 2D/3D, mikrofon, co-op, atau versi pembuka sendiri.

## Menghubungkan cerita dengan tindakan pemain

Untuk setiap adegan, jelaskan tujuan tokoh, apa yang pemain lakukan, umpan balik yang terlihat/terdengar, serta perubahan keadaan setelahnya. Ketegangan perlu muncul melalui tindakan dan informasi, bukan hanya dialog atau jumpscare.

Periksa khusus cerita ini:

- Mengapa empat mahasiswa mulai atau terpaksa bermain? Jika alat muncul sendiri, apa yang menghalangi mereka pergi?
- Apa yang membedakan pemadaman biasa dari ruang yang berubah? Setelah kamera dipilih, tentukan cara pemain menyadarinya.
- Bagaimana petunjuk YA/TIDAK atau kebohongan dapat dipelajari sebelum konsekuensi besar?
- Apa hubungan hilangnya Nisa, pesan HP, dan catatan lama dengan pilihan pemain?
- Bagaimana penutupan ritual dan timer menghasilkan ending yang dapat dipahami?

Jangan menganggap ritual fiksi sebagai fakta budaya yang sudah diverifikasi. Untuk detail budaya atau klaim dunia nyata yang diperlukan, periksa sumber yang sesuai.

## Membuat aturan yang dapat diimplementasikan

Gunakan [spesifikasi mekanik](references/mechanic-specification.md) saat menulis aturan fitur. Tetapkan tindakan, keadaan, timer/sumber daya, hasil, dan contoh perilaku yang dapat diuji. Angka tuning yang belum disepakati boleh diusulkan dengan label sementara.

Hindari menambah sistem acak, puzzle, baterai HP, atau malam lanjutan hanya karena tercantum sebagai kemungkinan. Setiap tambahan perlu tujuan gameplay dan prioritas terhadap demo.

## Aset dan rencana produksi

Untuk kebutuhan aset, tulis fungsi, konteks pemakaian, spesifikasi import yang relevan, variasi/animasi, serta status sementara atau final. Tunggu keputusan visual sebelum mengunci pipeline 2D atau 3D. Hubungkan cue audio dan cahaya dengan peristiwa gameplay, serta petunjuk penting dengan tampilan yang tetap terbaca.

Susun milestone berdasarkan hasil yang dapat dimainkan atau ditinjau. Sebutkan dependensi, batas cakupan, dan kriteria selesai. Jangan mengubah estimasi tiga bulan menjadi tanggal deadline tanpa data pengguna.

## Hasil dan pemeriksaan

Simpan spesifikasi yang diperlukan di `docs/` sesuai tugas. Perbarui daftar keputusan hanya untuk jawaban yang benar-benar diterima. Untuk revisi desain, periksa konsistensi awal cerita, aturan, dialog, ending, serta kebutuhan aset dan input. Saat menulis ke FigJam atau mengubah DOCX, gunakan skill alat terkait yang tersedia pada sesi dan ikuti kebutuhan verifikasinya.
