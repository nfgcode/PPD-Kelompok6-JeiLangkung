# Keputusan desain JeiLangkung

Daftar ini mencatat keputusan yang memengaruhi pekerjaan. **Terbuka** berarti belum boleh dianggap final. **Disepakati** hanya digunakan setelah ada keputusan pengguna atau tim yang disampaikan melalui pengguna. Catat tanggal dan alasan singkat saat status berubah.

## Kesepakatan kerja

| ID | Keputusan | Status dan dasar |
|---|---|---|
| K-01 | Panduan dan skill proyek menggunakan bahasa Indonesia | Disepakati, 11 Oktober 2026 |
| K-02 | Tidak menambahkan atribusi asisten atau metadata penulis AI pada hasil kerja | Disepakati, permintaan pengguna |
| K-03 | Visual dan penggunaan mikrofon belum ditentukan | Disepakati untuk tetap terbuka, 11 Oktober 2026 |
| K-04 | Unity saat ini 6000.3.25f1; perubahan versi perlu keputusan tersendiri | Kondisi proyek dan aturan lokal yang dipertahankan |
| K-05 | Fokus kerja berikutnya: menu judul dan layar awal empat mahasiswa di kos; istilah pengguna “Scene 2 bagian awal” mengacu ke layar kos | Disepakati, 12 Oktober 2026, klarifikasi pengguna |
| K-06 | Pencarian aset untuk dua layar tersebut fokus pada 2D sesuai sketsa FigJam sebagai pilihan sementara | Disepakati sementara, 12 Oktober 2026; tidak menutup keputusan visual akhir D-01 atau mikrofon D-02 |
| K-07 | Membuat kode dan versi yang dapat dimainkan untuk first screen dan Scene 1 | Diminta pengguna, 12 Oktober 2026; menggunakan arah prototype sementara K-06 |
| K-08 | Merevisi konsep dua layar pembuka menjadi 2D grayscale mengikuti moodboard, langsung di FigJam | Diminta pengguna, 12 Oktober 2026; ilustrasi geometris prototype belum sesuai. Konsep baru ditambahkan pada node `135:146`; belum diterapkan ke Unity dan belum menetapkan seluruh keputusan visual akhir |
| K-09 | Konsep pembuka menggunakan tampilan pixel horor retro, detail kasar, dithering, dan panel komik mengikuti contoh gambar terbaru | Koreksi pengguna, 12 Oktober 2026: revisi sebelumnya terlalu HD. Node `135:146` diperbarui; grayscale dipertahankan. Konsep belum diterapkan ke Unity atau disahkan sebagai visual final seluruh game |
| K-10 | Scene 2 dibuat sebagai prototype ritual. Bar keseimbangan menjadi genggaman Raka pada boneka Jelangkung, dikendalikan dengan menahan satu tombol (Spasi atau klik kiri) gaya bar memancing | Dipilih pengguna, 31 Oktober 2026. Antislop dipakai dalam mode during. Menutup D-06 untuk prototype Scene 2; ritme mantra (D-02) belum termasuk |
| K-11 | Empat lilin di sudut kertas menjadi satu batas waktu yang padam bergiliran; waktu mulai sejak lilin pertama dan berhenti saat jeda. Nilai sementara 50 detik per lilin dan percepatan 12 detik setiap genggaman lepas | Dipilih pengguna, 31 Oktober 2026 (empat lilin bergiliran). Waktu mulai mengikuti storytelling Scene 1; angka detik masih usulan. Perawatan lilin dan kelanjutan ke Scene 3 tetap terbuka pada D-07 |
| K-12 | Teks UI Scene 2 memakai TextMeshPro dengan font VT323 (SIL OFL), diunduh dari repositori resmi Google Fonts | Diizinkan pengguna, 31 Oktober 2026. Tulisan pada kertas ritual digambar sebagai huruf piksel di dalam gambar karena ikut terkena cahaya lilin |
| K-13 | Menu dan Scene 1 disamakan dengan gaya piksel Scene 2: ilustrasi konsep revisi 02 dikonversi ke empat abu dengan dither, teks VT323, dan tata letak panel FigJam | Diminta pengguna, 31 Oktober 2026 ("samakan"). Desain tokoh dan dialog tetap usulan; D-01 tetap terbuka untuk game akhir |

## Keputusan terbuka

| ID | Hal yang perlu diputuskan | Pilihan atau pertanyaan | Pekerjaan yang bergantung |
|---|---|---|---|
| D-01 | Bentuk visual | 2D grayscale, 3D, atau arah lain? | Aset, renderer, pencahayaan, kamera |
| D-02 | Input mantra | Tombol timing, mengetik, mikrofon, atau kombinasi? | Input, tutorial, kebutuhan audio |
| D-03 | Mode pemain | Singleplayer dengan tokoh pendamping atau co-op? | Arsitektur pemain dan jaringan |
| D-04 | Awal cerita | Ritual dipilih tokoh atau perlengkapan muncul sendiri? | Scene pembuka, motivasi, tutorial |
| D-05 | Kamera dan gerak | Kamera diam, perpindahan sudut, atau bebas bergerak? | Kontrol, tata ruang, interaksi |
| D-06 | Tindakan inti | Memutar pensil, memilih pertanyaan, menjaga keseimbangan, atau gabungan? Apa tujuan putarannya? | Prototype dan aturan interaksi |
| D-07 | Lilin | Satu atau empat? Timer bersama/terpisah? Mulai kapan? Bisa dirawat dengan biaya apa? | Batas waktu, UI, kondisi kalah |
| D-08 | Entitas dan jawaban | Si Penunggu tetap atau mood acak? YA selalu untuk pertanyaan tertentu atau semua pertanyaan? | Logika respons dan petunjuk kebohongan |
| D-09 | Penutupan dan ending | Cara menutup, prioritas saat timer habis, nasib Nisa, fungsi Malam 2? | State permainan, kemenangan, kekalahan |
| D-10 | Cakupan demo | Durasi sesi, deadline kalender, fitur minimum, pembagian kerja? | Milestone dan perkiraan pekerjaan |
| D-11 | Konten rinci | Pesan Nisa, isi catatan lama, petunjuk, mantra, dan kalimat perpisahan? | Dialog, chat, puzzle naratif |
| D-12 | Target perangkat | Windows/macOS untuk demo? Resolusi, input, dan spesifikasi minimum? | UI, performa, verifikasi build |

Ajukan pertanyaan yang berhubungan dengan tugas berikutnya. Tidak perlu meminta seluruh keputusan sekaligus. Jika tugas hanya menyusun konsep, tampilkan pilihan dan dampaknya. Jika tugas membutuhkan implementasi keputusan terbuka, tunggu jawaban untuk bagian tersebut dan kerjakan bagian lain yang tidak bergantung.

## Mencatat keputusan baru

### Pilihan implementasi prototype 12 Oktober 2026

Pembuka memakai komposisi kamera tetap 2D dan satu pemain lokal, dengan interaksi laptop/catatan, HP, dialog, serta senter setelah pemadaman. Ini merupakan pilihan sementara untuk menghasilkan dua layar yang dapat dimainkan; D-01, D-03, D-04, dan D-05 tetap terbuka untuk game akhir. Dialog tambahan merupakan teks prototype yang dapat diubah pada `OpeningConfig.asset`. Demo berhenti sebelum perlengkapan ritual diperkenalkan, sehingga belum menentukan pilihan D-04 atau aturan D-07.

Build lokal memakai Windows 64-bit sesuai lingkungan kerja saat ini. Hal ini belum menetapkan spesifikasi minimum atau menutup dukungan perangkat lain pada D-12. Petunjuk alur dan lokasi kode ada pada [prototype pembuka](playable-opening.md).

### Pilihan implementasi prototype Scene 2, 31 Oktober 2026

Scene 2 dirender sebagai gambar piksel 480 x 270 dengan empat tingkat abu, mengikuti konsep K-09. Ini pilihan sementara untuk prototype; D-01 tetap terbuka untuk game akhir. Kamera tetap di meja ritual (D-05 tetap terbuka untuk game akhir). Pemain hanya mengendalikan genggaman Raka, sedangkan Ayu dan Dimas tampil sebagai tangan pendamping (D-03 tetap terbuka).

Jawaban diatur per pertanyaan di `RitualConfig.asset`. Kedua pertanyaan default dijawab YA sesuai storytelling; sistem mood acak dari GDD tidak dibuat (D-08 tetap terbuka). Mantra tampil sebagai dua baris dialog Ayu tanpa mekanik ritme (D-02 tetap terbuka). Dialog reaksi tokoh dan empat tanda horor kecil adalah usulan prototype (D-11). Petunjuk alur, angka sementara, dan bukti pemeriksaan ada pada [prototype Scene 2](playable-ritual.md).

Saat jawaban datang, perbarui baris terkait dengan hasil, tanggal, alasan, dan sumber keputusan. Jika jawaban mengganti desain sebelumnya, catat dampak pada GDD, storyboard, mekanik, aset, dan test. Jangan menandai dokumen eksternal sudah diperbarui jika perubahan belum dilakukan.
