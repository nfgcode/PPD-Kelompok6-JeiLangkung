# Log pengerjaan scene JeiLangkung

Gunakan log ini untuk merencanakan pekerjaan dan mencatat hasil setiap sesi. Pembagian mengikuti empat babak cerita pada [konteks pengembangan](game-development.md). Scene di sini berarti adegan atau babak cerita; jumlah file `.unity` ditentukan saat implementasi.

Daftar pekerjaan merupakan rencana awal. Keputusan 2D/3D, mikrofon, kamera, dan versi pembuka tetap mengikuti [daftar keputusan desain](design-decisions.md). Jangan menganggap fitur yang masih diusulkan sudah menjadi kebutuhan final.

## Cara mengisi

1. Pilih satu scene dan satu pekerjaan yang ingin diselesaikan.
2. Periksa keputusan terkait. Jika jawaban diperlukan, tulis kendalanya dan kerjakan bagian lain yang memungkinkan.
3. Setelah bekerja, perbarui status pekerjaan dan tambahkan catatan sesi pada scene tersebut.
4. Catat hasil konkret, lokasi file atau node FigJam, serta pemeriksaan yang benar-benar dilakukan.
5. Tandai selesai setelah hasil memenuhi kriteria yang dipilih. Pekerjaan opsional yang dikeluarkan dari cakupan ditandai **Tidak dipakai** beserta alasannya.

| Status | Arti |
|---|---|
| Belum dicatat | Belum ada laporan pekerjaan pada log ini; bukan pernyataan bahwa seluruh pekerjaan belum pernah dilakukan |
| Direncanakan | Masuk fokus pekerjaan berikutnya, tetapi hasil implementasi belum dilaporkan |
| Dikerjakan | Sedang diproses; tulis bagian yang sudah dan belum selesai |
| Menunggu keputusan | Pekerjaan bergantung pada jawaban desain tertentu |
| Perlu perbaikan | Hasil sudah ada, tetapi pemeriksaan menemukan masalah |
| Selesai | Hasil dan bukti pemeriksaan sudah dicatat |
| Tidak dipakai | Pekerjaan dikeluarkan dari cakupan yang disepakati |

Tanggal dan durasi diisi berdasarkan sesi yang benar-benar dilakukan. Jangan mencentang hasil pengujian hanya karena kode atau desainnya sudah ditulis.

## Ringkasan kemajuan

| Scene | Fokus | Status keseluruhan | Pekerjaan berikutnya |
|---|---|---|---|
| 1 Kilo 15 Padam | Pembuka, pemadaman, dan perpindahan ke ritual | Dikerjakan | Prototype pembuka tersedia dan kini bersambung ke Scene 2; versi kemunculan perlengkapan ritual masih terbuka |
| 2 Datang Tak Dijemput | Pengenalan tindakan utama dan respons ritual | Dikerjakan | Prototype pixel dengan lilin, mantra, bar genggaman boneka, dan dua jawaban YA tersedia; playtest rasa genggaman dan keputusan mantra/perawatan lilin berikutnya |
| 3 Harapan Tak Kunjung Usai | Eskalasi, petunjuk, dan tekanan permainan | Belum dicatat | Pilih kejadian dan fitur yang masuk demo |
| 4 Pulang Tak Diantar | Penutupan, ending, dan restart | Belum dicatat | Tetapkan syarat ending dan prioritas peristiwa |

## Fokus kerja 12 Oktober 2026

Pengguna memilih **first screen/menu judul** dan **layar awal empat mahasiswa di kos**, dengan aset **2D sementara sesuai sketsa FigJam**, lalu meminta kode yang dapat dimainkan untuk first screen dan Scene 1. Sesi persiapan menghasilkan rencana dan kandidat aset; sesi implementasi menghasilkan dua scene, kode pembuka, dan pengujian. Visual serta dialog tetap sementara. Lihat [petunjuk prototype](playable-opening.md).

Istilah “Scene 2 awal” pada fokus ini mengikuti sebutan pengguna. Layar kos terkait pembuka **Scene 1 Kilo 15 Padam** dalam pembagian babak log, bukan **Scene 2 Datang Tak Dijemput** yang membahas ritual. Pada pemeriksaan FigJam 12 Oktober, panel berlabel Scene 2 (`48:253`) mempunyai sketsa lilin/pensil pada grup `53:1711`; penomoran panel dan babak perlu diselaraskan saat storyboard diperbarui. Label tugas **FS** dan **L2A** di bawah menjaga pekerjaan dua layar ini tetap terpisah dari tugas ritual.

Rujukan: [menu judul pada FigJam](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=54-88), [halaman Storyboard](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=48-236), dan [kandidat aset gratis](free-assets-first-screens.md).

### First screen menu judul

**Target:** judul JeiLangkung, tombol Open, akses pengaturan sesuai sketsa, dan perpindahan ke layar berikutnya. Menu pada board mempunyai judul, Open, dan ikon pengaturan. Label bahasa Indonesia seperti Mulai merupakan pilihan revisi, belum perubahan yang disepakati.

| ID | Pekerjaan | Status |
|---|---|---|
| FS-01 | Susun tata letak judul, Open, dan akses pengaturan; tentukan latar sederhana sementara | Selesai |
| FS-02 | Cari kandidat gratis untuk UI, font judul/teks, suara klik, dan suasana menu; catat lisensi | Selesai |
| FS-03 | Pilih dan import aset yang diperlukan; catat file, sumber, dan lisensinya | Selesai |
| FS-04 | Buat tampilan menu dengan status tombol normal, hover/fokus, dan ditekan | Selesai: gaya piksel revisi 02 diterapkan 31 Oktober (K-13) |
| FS-05 | Hubungkan Open ke layar berikutnya yang dipilih; cegah perpindahan ganda saat klik berulang | Selesai |
| FS-06 | Tentukan isi pengaturan yang benar-benar diperlukan; buat buka/tutup panel dan kontrolnya | Selesai |
| FS-07 | Periksa keterbacaan judul, posisi UI pada ukuran layar uji, klik, panel pengaturan, audio, dan perpindahan | Dikerjakan |
**Kriteria selesai:** menu tampil, Open melakukan perpindahan yang benar, pengaturan dapat dibuka/ditutup, UI tetap terbaca, dan hasil pemeriksaan dicatat. FS-02 yang selesai hanya menyatakan pencarian kandidat telah dilakukan.

#### Sesi 12 Oktober 2026 — persiapan menu

- ID pekerjaan: FS-01, FS-02.
- Target sesi: mencatat fokus menu dan mencari aset gratis.
- Hasil pekerjaan: kebutuhan menu serta daftar kandidat UI, font, suara klik, dan ambience disusun.
- Lokasi hasil: [daftar aset](free-assets-first-screens.md), terutama A-01 sampai A-04 dan A-09.
- Pemeriksaan: struktur board dan halaman sumber/lisensi diperiksa; belum ada import atau pengujian menu di Unity.
- Kendala atau keputusan: 2D sementara; isi pengaturan dan latar menu belum dipilih.
- Status setelah sesi: pencarian kandidat selesai; implementasi direncanakan.
- Langkah berikutnya: pilih font dan bentuk tombol, lalu buat tata letak sementara.

### Scene 2 awal layar empat mahasiswa di kos

**Target sementara:** komposisi kos, empat tokoh yang dapat dibedakan, properti mengerjakan tugas, dan dialog awal. Batas detail bagian awal belum ditetapkan; rencana ini mencakup tampilan/dialog sebelum pemadaman sebagai titik mulai, tanpa mengubah keputusan pembuka ritual.

| ID | Pekerjaan | Status |
|---|---|---|
| L2A-01 | Sketsa posisi empat mahasiswa, meja, bahan tugas, dan area dialog sesuai sudut pandang yang dipilih | Selesai |
| L2A-02 | Cari latar dan karakter gratis; catat keterbatasan pose serta kebutuhan aset tambahan | Selesai |
| L2A-03 | Siapkan latar/karakter sementara melalui aset atau ilustrasi geometris; periksa perspektif, skala, dan grayscale | Selesai |
| L2A-04 | Buat tampilan kos dan empat tokoh; siapkan meja, laptop/buku/kertas jika diperlukan oleh komposisi | Selesai: ilustrasi kos revisi 02 dan potret diterapkan 31 Oktober (K-13); desain tokoh masih usulan |
| L2A-05 | Susun dan pasang dialog awal yang menunjukkan mereka sedang mengerjakan tugas; tandai teks usulan | Selesai |
| L2A-06 | Tentukan titik akhir bagian awal dan titik lanjut ke kabar/pemadaman; jangan memulai timer ritual tanpa aturan | Selesai |
| L2A-07 | Periksa kejelasan empat tokoh, keterbacaan dialog, urutan tampil, input lanjut, dan perpindahan dari menu | Selesai |
**Kriteria selesai:** situasi empat mahasiswa mengerjakan tugas terbaca, dialog awal dapat diikuti, tampilan menyatu, dan batas adegan tercatat. Potret karakter untuk prototype dialog tidak otomatis membuktikan pose duduk dalam ilustrasi ruangan sudah selesai.

#### Sesi 12 Oktober 2026 — persiapan layar kos

- ID pekerjaan: L2A-01, L2A-02.
- Target sesi: mencatat kebutuhan layar kos dan mencari kandidat latar serta karakter 2D gratis.
- Hasil pekerjaan: Mini Apartment dan Free Backgrounds Set ditemukan sebagai kandidat latar; sprite laki-laki/perempuan sutemo ditemukan sebagai kandidat potret sementara. Kebutuhan pose duduk dan properti terpisah dicatat.
- Lokasi hasil: [daftar aset](free-assets-first-screens.md), A-05 sampai A-08 dan A-10.
- Pemeriksaan: halaman pembuat, bagian file gratis, ketentuan penggunaan, dan keterbatasan umum paket diperiksa; belum ada import, komposisi uji, atau playtest.
- Kendala atau keputusan: paket karakter berupa bust-up, belum sesuai pose duduk mengerjakan tugas; kamera/perspektif dan batas adegan perlu dipilih. D-01, D-04, D-05, dan D-11 tetap mengikuti daftar keputusan.
- Status setelah sesi: pencarian kandidat selesai; implementasi direncanakan.
- Langkah berikutnya: sketsa komposisi terlebih dahulu, pilih satu latar, lalu putuskan potret sementara atau pose khusus.

## Scene 1 Kilo 15 Padam

**Tujuan adegan:** memperkenalkan empat mahasiswa di kos, mengubah situasi biasa menjadi ancaman, dan memberi alasan yang dapat dipahami untuk masuk ke permainan ritual.

**Keputusan terkait:** D-01 visual, D-04 awal cerita, D-05 kamera, D-07 lilin, dan D-11 konten rinci.

**Cabang pembuka yang perlu dipilih:** versi FigJam menampilkan tokoh mengusulkan ritual; alternatif percakapan menampilkan ruangan terasa luas dan perlengkapan muncul sendiri. Pada alternatif kedua, jelaskan motivasi bermain atau alasan mereka tidak dapat pergi.

### Daftar pekerjaan

| ID | Pekerjaan dan hasil yang dituju | Status |
|---|---|---|
| S1-01 | Pilih versi pembuka; tulis urutan kejadian dari mengerjakan tugas sampai masuk ke ritual | Menunggu keputusan |
| S1-02 | Buat sketsa tata ruang dan posisi tokoh, HP, pintu, serta alat ritual sesuai kamera yang dipilih | Dikerjakan |
| S1-03 | Siapkan teks notifikasi pemadaman dan dialog pembuka yang diperlukan | Selesai |
| S1-04 | Susun peristiwa lampu padam, perubahan suara, dan reaksi tokoh; buat daftar aset sementara | Selesai |
| S1-05 | Atur muncul/tersedianya penerangan dan perlengkapan sesuai versi terpilih; jelaskan kapan timer mulai berlaku | Menunggu keputusan |
| S1-06 | Implementasikan pembuka dan transisi ke Scene 2; tetapkan kapan pemain mendapat kontrol | Dikerjakan |
| S1-07 | Periksa urutan kejadian, keterbacaan teks, alasan masuk ritual, dan transisi tanpa pemicu ganda | Dikerjakan |
### Kriteria selesai

- Versi pembuka dan urutan kejadian sudah dicatat serta sesuai keputusan desain.
- Pemain memahami situasi awal dan tindakan berikutnya melalui pembuka yang dapat dijalankan.
- Transisi ke ritual bekerja; waktu mulai timer sesuai aturan yang dipilih.
- Pemeriksaan aktual dan kekurangan yang masih ada tercatat.

### Catatan sesi

#### Sesi 12 Oktober 2026 — implementasi pembuka

- ID pekerjaan: S1-02, S1-03, S1-04, S1-06, S1-07, B-02, B-03, B-06; serta FS dan L2A untuk dua layar awal.
- Target sesi: menu dan Scene 1 yang dapat dimainkan dalam Unity serta build Windows lokal.
- Hasil pekerjaan: aturan pembuka terpisah dari UI; dua scene Unity; konfigurasi dialog; kos grayscale dengan empat mahasiswa, meja, laptop, catatan, pintu, dan jendela; notifikasi HP, pemadaman, senter, dialog reaksi, pause, volume, redup halus, main ulang, dan menu judul.
- Lokasi hasil: `JeiLangkung/Assets/_Project`; [petunjuk kode dan kontrol](playable-opening.md), [lisensi](asset-licenses.md).
- Pemeriksaan aturan: EditMode **7 lulus, 0 gagal, 0 skipped**, `test-results/editmode.xml`.
- Pemeriksaan integrasi: PlayMode **1 lulus, 0 gagal, 0 skipped**, `test-results/playmode.xml`. Satu test mencakup alur menu sampai akhir Scene 1, input keyboard, input tertahan saat pause, timer pemadaman berhenti saat pause, main ulang, dan kembali ke menu.
- Perbaikan selama verifikasi: impor resource TMP ditunggu sampai selesai; referensi konfigurasi dimuat setelah membuka scene agar tidak terlepas ketika scene berganti. Test keyboard batch memakai pengaturan fokus sementara yang dikembalikan setelah test; pengaturan input produk tidak diubah untuk melewati test.
- Batas cerita: berhenti setelah pemadaman, senter HP, dan reaksi tokoh. Kemunculan boneka, lilin, tulisan darah, dan transisi gameplay ritual belum dibuat karena versi awal ritual serta aturan masih terbuka.
- Batas aset: memakai ilustrasi geometris sementara serta font Bebas Neue dan Lato dengan OFL. Kandidat latar/potret dari sesi pencarian belum dipakai. Suara berupa nada prosedural.
- Pemeriksaan build: Unity 6000.3.25f1 menghasilkan development build Windows 64-bit di `Builds/Windows/JeiLangkung.exe`; proses build keluar **0** dan `BUILD_OK` tercatat pada `test-results/build-windows.log`.
- Pemeriksaan executable: startup menu → pengaturan → Scene 1 → kedua pemeriksaan tugas → pesan HP → pause saat pemadaman → senter → akhir → main ulang → menu → resize; laporan **PASS**, proses keluar **0** pada `test-results/smoke-render/report.txt` dan `test-results/player-render.log`.
- Pemeriksaan visual: delapan frame UI dirender melalui URP ke RenderTexture dari executable. Menu, kos dengan empat mahasiswa, panel pesan, panel pengaturan, gelap, senter, dan akhir ditinjau pada 1280 × 720; menu ditinjau juga pada 1024 × 768 dengan letterbox. Tangkapan screen dari jendela tersembunyi awalnya gagal/kosong, sehingga alat verifikasi memakai render offscreen dan menolak frame kosong. Hasil gambar: `test-results/smoke-render/`.
- Bukti aset: font sumber, atlas SDF, dan salinan OFL tersedia; folder build menyertakan `Licenses` serta `CARA-BERMAIN.txt`. FS-03 dan L2A-03 selesai untuk aset sementara prototype; kandidat latar/potret final belum diimpor. Akses pengaturan menggunakan tombol teks, belum ikon gear. FS-07 tetap dikerjakan untuk pemeriksaan audio dengan pendengaran. B-05 tetap dikerjakan karena aset final dan seluruh konten game belum selesai.
- Status akhir sesi: **prototype first screen dan pembuka Scene 1 selesai serta dapat dimainkan**. Scene 1 keseluruhan masih dikerjakan karena transisi ke ritual belum ditentukan. Belum ada playtest pemahaman/rasa horor bersama pemain lain, penilaian audio dengan pendengaran, atau uji komputer lain.
- Langkah berikutnya: mainkan build, review dialog/ilustrasi sementara, lalu putuskan versi awal ritual untuk melanjutkan Scene 1 ke Scene 2.

#### Sesi 12 Oktober 2026 — revisi konsep visual di FigJam

- ID pekerjaan: FS-01, FS-04, L2A-01, L2A-03, L2A-04, S1-02.
- Target sesi: menanggapi penilaian pengguna bahwa visual prototype belum sesuai moodboard, dengan konsep 2D grayscale langsung di FigJam.
- Hasil pekerjaan: dua ilustrasi tinta/arsir untuk menu dan kos; judul, tombol, HUD, serta dialog editable; catatan palet dan arah visual. Referensi lama dipertahankan.
- Lokasi hasil: [section revisi FigJam](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=135-146), [brief dan aset](art-direction/grayscale-opening.md), serta `docs/art-direction/figjam-grayscale-preview.png`.
- Pemeriksaan: kedua gambar berhasil ditempatkan, 28 node teks terbuat, dua layar 1920 × 1080. Tangkapan komposisi section ditinjau; menu terbaca, empat wajah terlihat, dan dialog tidak menutupi wajah.
- Kendala atau keputusan: K-08. Konsep ini belum diterapkan pada scene atau build Unity; dialog dan desain tokoh masih usulan.
- Status setelah sesi: konsep FigJam selesai untuk ditinjau; tampilan Unity FS-04 dan L2A-04 perlu perbaikan. Pengujian fungsi sebelumnya tetap tercatat sebagai hasil sesi sebelumnya.
- Langkah berikutnya: review konsep bersama tim, lalu implementasikan ilustrasi dan UI yang dipilih pada menu serta kos; periksa kembali di dalam game.

#### Sesi 12 Oktober 2026 — koreksi visual menjadi pixel horror

- ID pekerjaan: FS-01, FS-04, L2A-01, L2A-03, L2A-04, S1-02.
- Target sesi: mengikuti koreksi pengguna bahwa konsep pertama terlalu HD, memakai contoh gambar game horor retro yang dikirim.
- Hasil pekerjaan: tiga ilustrasi pixel untuk menu, kos, dan potret Raka; tipografi Pixelify Sans/VT323; panel komik, objek tugas, dan dialog editable pada section revisi yang sama. UI revisi pertama disembunyikan; referensi tim dipertahankan.
- Lokasi hasil: [FigJam revisi pixel](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=135-146), [brief dan aset revisi 02](art-direction/pixel-opening-v2.md).
- Pemeriksaan: tiga upload berhasil; dua layar 1920 × 1080; 30 teks terlihat; auto-layout tanpa overflow. Pratinjau komposisi ditinjau: empat tokoh, potret, menu, dan dialog terbaca tanpa potongan teks.
- Kendala atau keputusan: K-09. Resolusi native game dan palet berkas final belum ditetapkan. Tidak ada perubahan atau build Unity pada sesi ini.
- Status setelah sesi: revisi konsep FigJam selesai untuk ditinjau; FS-04 dan L2A-04 pada Unity tetap perlu perbaikan.
- Langkah berikutnya: tinjau arah pixel ini, lalu terapkan visual yang dipilih ke prototype dan periksa di dalam game.

#### Sesi 31 Oktober 2026: menu dan Scene 1 disamakan dengan gaya piksel Scene 2

- ID pekerjaan: FS-04, FS-07, L2A-04, S1-07, B-05, B-06.
- Target sesi: permintaan pengguna untuk menyamakan menu dan Scene 1 dengan gaya piksel Scene 2.
- Hasil pekerjaan: ilustrasi konsep revisi 02 dikonversi ke abu-abu pada grid 480 x 270 lalu diberi cahaya per piksel dan dither empat abu saat bermain. Menu memakai api lilin kuning yang berkedip; kos memakai panel ilustrasi, potret pembicara, panel tindakan, dan kotak dialog sesuai FigJam; pemadaman dan senter HP Nisa dihitung lewat cahaya yang sama; HP tampil sebagai layar chat putih keabu-abuan tanpa merek. Laptop dan catatan disorot dan bisa diklik di gambar. Setelah Bersambung, fokus otomatis diberikan ke Lanjut ke ritual agar pemain keyboard bisa memilih. Bagian piksel bersama dipindah ke `Scripts/Runtime/Pixel/`; ilustrasi geometris lama dihapus.
- Lokasi hasil: `Scripts/Runtime/Opening/`, `Scripts/Runtime/Pixel/`, `Scripts/Editor/PixelArtSetup.cs`, `Art/Pixel/`, `Data/OpeningConfig.asset`; [petunjuk pembuka](playable-opening.md).
- Pemeriksaan: EditMode 22 lulus, PlayMode 4 lulus (test pembuka lama tidak diubah). Build Windows development `BUILD_OK`. Executable `--smoke-test` dan `--ritual-smoke` keduanya keluar 0 dengan PASS. Pratinjau piksel di `test-results/opening-preview/`, tangkapan lengkap di `test-results/opening-smoke/`.
- Perbaikan selama verifikasi: nama `Light` bentrok dengan `UnityEngine.Light`; sudut sorotan diberi kontur hitam; panel tindakan dan dialog diberi bingkai; runner Scene 2 kini menunggu scene yang dimuat ulang, bukan jeda tetap 0,3 detik.
- Kendala atau keputusan: K-13. Desain tokoh dan dialog masih usulan. Audio belum didengarkan.
- Status setelah sesi: FS-04 dan L2A-04 selesai untuk prototype. FS-07 tetap dikerjakan karena audio belum dinilai.
- Langkah berikutnya: review tampilan bersama tim, lalu playtest dari menu sampai Bersambung Scene 2.

## Scene 2 Datang Tak Dijemput

**Tujuan adegan:** mengajarkan tindakan utama ritual, menunjukkan respons pertama, dan membuat pemain memahami hubungan tindakan, petunjuk, serta ancaman.

**Keputusan terkait:** D-02 input mantra, D-03 mode pemain, D-06 tindakan inti, D-07 lilin, D-08 jawaban entitas, dan D-11 konten rinci.

Mantra, putaran pensil, pilihan pertanyaan, dan keseimbangan belum otomatis menjadi empat mekanik wajib. Pilih tindakan yang masuk demo sebelum membuat tutorialnya.

### Daftar pekerjaan

| ID | Pekerjaan dan hasil yang dituju | Status |
|---|---|---|
| S2-01 | Tentukan tindakan utama, kontrol, tujuan pemain, syarat berhasil/gagal, dan umpan balik | Selesai untuk prototype: bar genggaman boneka, tahan satu tombol (K-10) |
| S2-02 | Susun tutorial singkat dan urutan pertanyaan awal; tandai teks yang masih perlu keputusan | Dikerjakan: urutan dan petunjuk ada; dialog reaksi masih usulan (D-11) |
| S2-03 | Implementasikan tindakan inti yang dipilih; mantra atau keseimbangan ditambahkan jika masuk cakupan | Dikerjakan: genggaman selesai; ritme mantra menunggu D-02 |
| S2-04 | Hubungkan aturan jawaban dengan gerakan pensil/boneka dan tampilan hasil | Selesai untuk prototype |
| S2-05 | Hubungkan lilin dengan tekanan waktu; jelaskan apakah waktu berjalan selama tutorial dan animasi | Selesai untuk prototype: empat lilin bergiliran, mulai sejak lilin pertama, berhenti saat jeda (K-11) |
| S2-06 | Tambahkan gangguan awal yang dipilih serta perpindahan ke Scene 3 | Dikerjakan: empat tanda kecil ada; Scene 3 belum dibuat sehingga akhir menampilkan Bersambung |
| S2-07 | Periksa pemahaman tutorial, input berulang, hasil jawaban, timer, dan transisi | Dikerjakan: test otomatis dan executable lulus; playtest pemain belum |

### Kriteria selesai

- Pemain bisa melakukan tindakan utama dan mengenali hasilnya.
- Jawaban mengikuti aturan cerita yang sudah dipilih, termasuk batas makna jawaban YA.
- Tutorial dan animasi tidak menghasilkan input atau peristiwa berulang yang tidak diinginkan.
- Aturan tekanan waktu dan transisi ke eskalasi sudah diperiksa.

### Catatan sesi

#### Sesi 31 Oktober 2026: prototype ritual dan bar genggaman

- ID pekerjaan: S2-01 sampai S2-07, B-04, B-05, B-06; S1-06 untuk tombol lanjut dari Scene 1.
- Target sesi: memeriksa game dan FigJam, menentukan objek untuk mekanik bar, lalu membuat Scene 2 dengan skill antislop (mode during) dan tasteskill sebagai filter desain.
- Pemeriksaan awal: FigJam masih memuat konsep pixel horror `135:146` dari sesi 12 Oktober. Tim menambahkan catatan "Resolusi frame 270 × 480" (`144:240`) dan workflow pixel dari foto (`168:147`). Sketsa Scene 2 (`53:1711`) menunjukkan empat lilin di sudut kertas bersilang dengan pensil.
- Keputusan pengguna: bar ditempel ke boneka Jelangkung yang dipegang Raka; kontrol tahan satu tombol; empat lilin padam bergiliran; unduh font VT323 untuk teks TextMeshPro.
- Hasil pekerjaan: scene `Scene2_Ritual` 480 x 270 dengan cahaya lilin per piksel dan dither empat abu; peringatan konten; menyalakan empat lilin; mantra; bar genggaman; dua pertanyaan yang dijawab YA dengan pensil melingkari jawaban; empat tanda horor kecil; jeda dengan Redup halus; Bersambung; akhir buruk lilin padam dengan mata merah. Scene 1 kini punya tombol Lanjut ke ritual.
- Lokasi hasil: `JeiLangkung/Assets/_Project/Scripts/Runtime/Ritual/`, `Scripts/Editor/RitualSetup.cs`, `Data/RitualConfig.asset`, `Scenes/Scene2_Ritual.unity`; [petunjuk Scene 2](playable-ritual.md).
- Pemeriksaan: EditMode 22 lulus, 0 gagal. PlayMode 4 lulus, 0 gagal. Build Windows development berhasil (`BUILD_OK`). Executable dengan `--ritual-smoke` keluar 0 dengan laporan PASS dan 11 tangkapan di `test-results/ritual-smoke/`. Pratinjau piksel di `test-results/ritual-preview/` ditinjau dan diperbaiki tiga putaran.
- Perbaikan selama verifikasi: boneka diperbesar dan diberi kontur agar terbaca di atas kertas; lengan dibuat bersiku; baris YA/TIDAK dinaikkan agar tangan tidak menyentuh api lilin; kisi titik saat gelap total dihapus; rontaan diubah agar pemain yang diam tidak bisa menang; agitasi disetel ulang; referensi config yang terlepas saat scene dibuat diperbaiki pada script setup.
- Kendala atau keputusan: D-02, D-07 (perawatan lilin), D-08, dan D-11 tetap terbuka. Dialog reaksi adalah usulan. Teks di kertas memakai huruf piksel di dalam gambar; teks UI tetap TextMeshPro.
- Status setelah sesi: Dikerjakan. Prototype dapat dimainkan dari menu sampai Bersambung Scene 2.
- Langkah berikutnya: playtest rasa dan keterbacaan bar bersama tim, dengarkan audio, putuskan input mantra dan perawatan lilin, lalu susun Scene 3.

## Scene 3 Harapan Tak Kunjung Usai

**Tujuan adegan:** meningkatkan tekanan, membuka informasi tentang ancaman, dan memberi pemain petunjuk yang berguna untuk mengambil keputusan penutupan.

**Keputusan terkait:** D-05 kamera, D-07 lilin, D-08 entitas, D-10 cakupan demo, dan D-11 konten rinci.

Materi FigJam memuat pemadaman diperpanjang, Nisa hilang, pesan dari nomornya, siluet di jendela, catatan penghuni lama, dan puzzle. Pilih bagian yang masuk demo; tidak semua harus dibuat sekaligus.

### Daftar pekerjaan

| ID | Pekerjaan dan hasil yang dituju | Status |
|---|---|---|
| S3-01 | Pilih kejadian eskalasi; tentukan urutan, pemicu, dan informasi yang diterima pemain | Belum dicatat |
| S3-02 | Siapkan kabar pemadaman lanjutan serta dialog/reaksi yang dipakai | Belum dicatat |
| S3-03 | Jika dipilih: susun dan implementasikan hilangnya Nisa, pesan dari nomornya, serta petunjuk visual/audio | Belum dicatat |
| S3-04 | Jika dipilih: siapkan catatan penghuni lama dan hubungkan isinya dengan petunjuk penutupan | Belum dicatat |
| S3-05 | Jika dipilih: buat puzzle sekring/senter atau pengelolaan baterai HP; jelaskan pengaruhnya pada ritual dan timer | Belum dicatat |
| S3-06 | Atur peningkatan tekanan dan kesempatan pemain memahami petunjuk; hubungkan ke Scene 4 | Belum dicatat |
| S3-07 | Periksa urutan peristiwa, akses petunjuk, interaksi panel/puzzle, tingkat kesulitan, dan transisi | Belum dicatat |

### Kriteria selesai

- Kejadian yang dipilih terpicu pada kondisi yang jelas dan tidak berulang tanpa aturan.
- Petunjuk yang diperlukan tersedia sebelum pemain harus menggunakannya.
- Jika fitur opsional dipakai, pemain dapat menyelesaikan atau meninggalkannya sesuai aturan tanpa kehilangan kontrol utama.
- Eskalasi dan transisi dapat dimainkan; fitur yang dikeluarkan dari demo dicatat.

### Catatan sesi

Belum ada sesi yang dicatat. Salin format di bagian akhir ke sini setelah mengerjakan scene ini.

## Scene 4 Pulang Tak Diantar

**Tujuan adegan:** menyelesaikan ritual melalui aturan yang dapat dipahami, menampilkan akibat pilihan pemain, dan memungkinkan permainan dicoba kembali.

**Keputusan terkait:** D-07 lilin, D-09 penutupan/ending, D-10 cakupan demo, dan D-11 konten rinci.

Materi FigJam memuat kegagalan karena lilin padam, kegagalan karena penutupan salah, serta ending terbaik yang tetap pahit. Tetapkan ending yang masuk demo, nasib Nisa, dan apakah Malam 2 hanya ditunjukkan atau benar-benar dimainkan.

### Daftar pekerjaan

| ID | Pekerjaan dan hasil yang dituju | Status |
|---|---|---|
| S4-01 | Tetapkan cara menutup ritual, petunjuk yang dibutuhkan, syarat tiap ending, dan hasil bila waktu habis bersamaan | Belum dicatat |
| S4-02 | Siapkan teks, visual, dan audio penutupan serta ending yang dipilih | Belum dicatat |
| S4-03 | Implementasikan penutupan dan pemilihan hasil; setiap sesi hanya memicu satu ending | Belum dicatat |
| S4-04 | Implementasikan akibat lilin padam atau penutupan salah jika termasuk ending demo | Belum dicatat |
| S4-05 | Implementasikan ending terbaik sesuai keputusan; kabar berikutnya dan Malam 2 hanya sesuai cakupan | Belum dicatat |
| S4-06 | Tambahkan restart atau kembali ke menu; reset timer, input, event, tampilan, dan audio sesi sebelumnya | Belum dicatat |
| S4-07 | Periksa semua jalur ending yang dipilih, prioritas peristiwa, penghentian interaksi, dan restart | Belum dicatat |

### Kriteria selesai

- Setiap ending yang dipilih dapat dicapai melalui aturan yang tercatat.
- Pemain menerima petunjuk yang diperlukan dan dapat memahami akibat tindakannya.
- Peristiwa yang terjadi bersamaan menghasilkan satu hasil sesuai prioritas yang disepakati.
- Restart atau kembali ke menu bekerja tanpa membawa state sesi sebelumnya.

### Catatan sesi

Belum ada sesi yang dicatat. Salin format di bagian akhir ke sini setelah mengerjakan scene ini.

## Pekerjaan bersama antar scene

Kerjakan bagian ini saat dibutuhkan oleh scene atau persiapan demo. Jangan menganggap menu, pengaturan, dan fitur tambahan sudah menjadi lingkup wajib.

| ID | Pekerjaan | Status |
|---|---|---|
| B-01 | Pastikan keputusan desain dan aturan yang dipakai konsisten antar scene | Belum dicatat |
| B-02 | Buat komponen/data bersama yang diperlukan agar timer, input, dan state tidak terduplikasi | Selesai |
| B-03 | Jika dibutuhkan: menu awal, pause, pengaturan audio, dan petunjuk kontrol | Selesai |
| B-04 | Jalankan alur Scene 1 sampai ending tanpa melompati bagian; catat masalah transisi | Dikerjakan: Scene 1 ke Scene 2 diuji di PlayMode, Scene 2 sampai Bersambung diuji terpisah; Scene 3 dan 4 belum ada |
| B-05 | Periksa teks, aset sementara/final, audio, dan sumber/lisensi aset sebelum demo dibagikan | Dikerjakan: VT323 dan lisensinya ditambahkan 31 Oktober |
| B-06 | Buat dan periksa build pada target yang dipilih; catat perangkat dan hasilnya | Selesai: build Windows development 31 Oktober memuat Scene 2 |
## Format catatan sesi

Salin format berikut ke bagian **Catatan sesi** scene yang dikerjakan. Tambahkan entri baru untuk sesi berikutnya agar riwayat perbaikan tetap terlihat.

```markdown
#### Sesi [tanggal dan jam]

- ID pekerjaan: [misalnya S1-03]
- Target sesi: [hasil yang ingin dicapai]
- Durasi aktual: [isi jika dicatat]
- Hasil pekerjaan: [apa yang dibuat atau diubah]
- Lokasi hasil: [file proyek, nama scene/prefab, atau tautan node FigJam]
- Pemeriksaan: [langkah/perintah yang dijalankan dan hasil aktual; tulis belum diperiksa bila belum]
- Kendala atau keputusan: [masalah dan ID keputusan terkait, atau tidak ada]
- Status setelah sesi: [pilih status]
- Langkah berikutnya: [tindakan konkret untuk sesi selanjutnya]
```

Perubahan desain yang disepakati juga dicatat di [design-decisions.md](design-decisions.md). Log ini mencatat pekerjaan dan bukti hasil; keputusan desain tetap disimpan pada daftar keputusan agar mudah ditemukan.
