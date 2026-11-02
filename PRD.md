# PRD: JeiLangkung

Dokumen kebutuhan produk untuk game JeiLangkung, Kelompok 6, mata kuliah Pemrograman Permainan Digital, ITK Balikpapan. Status dokumen: draf kerja. Bagian bertanda **Terbuka** belum diputuskan tim.

## 1. Ringkasan

JeiLangkung adalah game horor ritual 2D dengan judul cerita **Dikala Malam, Masa Kelam**. Kilo 15, Balikpapan, gelap total tanpa kabar kapan listrik menyala. Empat mahasiswa di kamar kos memanggil Jelangkung untuk bertanya: "Apakah lampu akan menyala malam ini?" Jawabannya selalu YA, sampai lilin terakhir padam.

Tema: harapan yang tak kunjung usai. Menunggu lampu menyala sama seperti menunggu janji yang tidak ditepati. Penyebab pemadaman (kebijakan) disampaikan tersirat lewat HP dan grup chat warga, tanpa menyebut perusahaan atau tokoh nyata. Tema dikaitkan dengan SDG 7, 11, dan 13.

## 2. Tujuan

| Tujuan | Ukuran keberhasilan |
|---|---|
| Menyampaikan cerita horor lokal yang utuh dari awal sampai ending | Pemain bisa memainkan Scene 1 sampai Scene 4 dan mencapai minimal satu ending |
| Mekanik ritual mudah dipahami tanpa penjelasan panjang | Pada playtest, pemain mengerti cara memakai bar genggaman dan arti lilin tanpa bantuan |
| Ketegangan terjaga dengan kamera diam di satu ruangan | Pemain merasakan tekanan dari lilin, tanda horor kecil, dan jumpscare |
| Pesan sosial tersampaikan secara tersirat | Pemain mengaitkan pemadaman dengan kabar di HP tanpa tokoh mengucapkannya |

## 3. Pemain dan platform

- **Pemain:** remaja dan dewasa yang menyukai horor psikologis, khususnya cerita horor lokal Indonesia.
- **Platform utama:** desktop Windows. macOS sebagai target tambahan. Web (WebGL) mungkin ditambahkan; **Terbuka** (D-12).
- **Input:** keyboard dan mouse.
- **Durasi demo:** **Terbuka** (D-10).

## 4. Tokoh

| Tokoh | Peran cerita | Peran gameplay |
|---|---|---|
| Raka (pemain) | Memegang Jelangkung | Bar genggaman dan jawaban YA/TIDAK |
| Ayu | Tahu mantra dari neneknya | Petunjuk tata cara ritual |
| Dimas | Anak teknik yang skeptis | Minigame puzzle (sekring, senter) |
| Nisa | Paling penakut, memantau HP dan sinyal | Tokoh pertama yang "hilang" |
| Si Penunggu | Arwah penghuni lama kos yang tidak pernah dipulangkan; hidup dari harapan orang lain, karena itu selalu menjawab YA | Mata merah, tulisan pensil sendiri, jumpscare |

## 5. Alur cerita

| Scene | Isi | Mekanik | Status |
|---|---|---|---|
| 1 Kilo 15 Padam | Empat mahasiswa mengerjakan tugas, kabar pemadaman di grup warga, lampu padam | Periksa laptop dan catatan, buka HP, senter HP | Prototype dapat dimainkan |
| 2 Datang Tak Dijemput | Lilin dinyalakan, mantra, Jelangkung "masuk", dua pertanyaan dijawab YA | Empat lilin sebagai waktu, bar genggaman, YA/TIDAK, tanda horor kecil | Prototype dapat dimainkan |
| 3 Harapan Tak Kunjung Usai | Pemadaman diperpanjang, Nisa hilang, pesan dari nomornya, pensil menulis "BELUM... TUNGGU", buku catatan penghuni lama | Puzzle Dimas, lilin cepat meredup, jawaban saling bertentangan | Belum dibuat |
| 4 Pulang Tak Diantar | Menutup ritual sebelum lilin terakhir padam | Penutupan ritual, pemilihan ending | Belum dibuat |

**Ending:**
1. Lilin terakhir padam: gelap total, mata merah, jumpscare. (Sudah ada di prototype Scene 2.)
2. Ritual tidak ditutup atau entitas diusir kasar: pensil menulis "TINGGAL", jumpscare.
3. Ending terbaik yang tetap pahit: ritual ditutup saat azan Subuh, lampu dan sinyal kembali, lalu semua HP bergetar: "pemadaman bergilir akan kembali dilaksanakan malam ini." Pensil bergerak sendiri ke YA. Kembali ke Malam 2.

## 6. Kebutuhan fitur

Prioritas: **W** wajib untuk demo, **P** penting, **O** opsional.

| ID | Fitur | Prioritas | Kriteria penerimaan | Status |
|---|---|---|---|---|
| F-01 | Menu judul: Open, pengaturan (volume, redup halus), keluar | W | Semua tombol bekerja dengan mouse dan keyboard; pengaturan tersimpan | Selesai |
| F-02 | Scene 1: dialog, tugas laptop/catatan, HP, pemadaman, senter | W | Alur bisa diselesaikan; jeda menghentikan pemadaman | Selesai |
| F-03 | Lilin sebagai batas waktu: empat lilin padam bergiliran, mulai sejak lilin pertama | W | Waktu berhenti saat jeda; lilin terakhir padam memicu ending 1 tepat sekali | Selesai (angka sementara) |
| F-04 | Bar genggaman boneka gaya game memancing | W | Tahan Spasi/klik kiri menaikkan genggaman; pemain yang diam gagal; genggaman lepas mempercepat lilin | Selesai (angka sementara) |
| F-05 | Pertanyaan YA/TIDAK dengan pensil bergerak di kertas | W | Jawaban mengikuti naskah; pertanyaan tidak bisa diulang saat pensil bergerak | Selesai |
| F-06 | Tanda horor kecil gaya FNAF (air bergetar, api condong, HP bergetar, bayangan) | P | Tiap tanda muncul sekali pada momen naskahnya | Selesai untuk Scene 2 |
| F-07 | Mantra dengan ritme | P | Bentuk input diputuskan (D-02), lalu dapat diselesaikan tanpa membaca panduan | Terbuka |
| F-08 | Perawatan lilin | P | Pemain bisa memperpanjang lilin dengan biaya yang jelas | Terbuka (D-07) |
| F-09 | HP sebagai monitor: chat warga, baterai, sinyal | P | Cek HP mengurangi baterai; pesan baru muncul sesuai alur | Sebagian (chat Scene 1) |
| F-10 | Minigame puzzle Dimas (sekring/senter) | P | Bisa diselesaikan tanpa meninggalkan meja; gagal atau berhasil memengaruhi ritual | Belum |
| F-11 | Penutupan ritual dan pemilihan ending 2 dan 3 | W | Setiap sesi memicu satu ending; prioritas bila waktu habis bersamaan sudah ditetapkan | Belum (D-09) |
| F-12 | Main ulang dan kembali ke menu tanpa sisa state | W | Lilin, genggaman, dan audio kembali ke awal | Selesai |
| F-13 | Peringatan konten sebelum ritual | W | Tampil sebelum Scene 2 dan bisa dilewati | Selesai |

## 7. Arah visual dan audio

- 2D grayscale dalam satu ruangan sempit, dirender pada 480 x 270 dengan empat tingkat abu dan dither. Hanya api lilin (kuning) dan mata entitas (merah) yang berwarna. Cahaya layar HP putih keabu-abuan dan dingin.
- Kamera diam. Pemain tidak berjalan; memantau, merapal, dan bereaksi.
- Referensi: Last Report, World of Horror, Manus Sors, The Long-Necked Woman, manga horor Junji Ito, dan workflow tim "pixel dari foto atau video".
- Teks UI memakai font VT323 dengan panel bergaya komik retro.
- Audio: dengung listrik yang melemah, jangkrik, angin, getar HP, goresan pensil, azan Subuh untuk ending, dan jumpscare. Prototype masih memakai suara prosedural.
- Dialog dan chat memakai logat Balikpapan ("kah", "ih", "bah").
- **Perlu revisi:** gaya 2D Scene 2 dan gambar tangan belum sesuai gambaran tim di FigJam. Arah final (foto jadi pixel, manga pixel, atau 1-bit) **Terbuka** (D-01).

## 8. Kebutuhan non-fungsional

- Unity **6000.3.25f1** (Unity 6.3 LTS), Universal 2D/URP. Seluruh tim memakai versi yang sama.
- Package: Input System, uGUI dan TextMeshPro, 2D Animation, PSD Importer, Aseprite Importer, Test Framework. Package baru hanya dengan kesepakatan tim.
- Logika inti (lilin, genggaman, alur ritual) terpisah dari tampilan dan diuji dengan test EditMode; integrasi diuji dengan PlayMode.
- Aksesibilitas: semua menu bisa dipakai dengan keyboard, fokus tombol terlihat, kontras teks memenuhi WCAG AA, pengaturan "redup halus" mengurangi kedip dan guncangan.
- Data naskah, dialog, dan angka waktu disimpan di ScriptableObject agar bisa diubah lewat Inspector.
- Aset pihak ketiga dicatat sumber dan lisensinya; font memakai SIL OFL.

## 9. Konten dan etika

- Konten horor okultisme lokal ditampilkan sebagai fiksi dan budaya, dengan peringatan konten.
- Tidak memakai merek WhatsApp, nama PLN, atau tokoh nyata di dalam game. Nama grup, berita, dan instansi dibuat fiktif.
- Ironi Balikpapan sebagai Kota Minyak cukup ditampilkan lewat gambar.

## 10. Keputusan terbuka

| ID | Pertanyaan |
|---|---|
| D-01 | Bentuk visual akhir: gaya 2D yang mana, atau 3D |
| D-02 | Input mantra: tombol ritme, mengetik, mikrofon, atau gabungan |
| D-03 | Satu pemain dengan tokoh pendamping, atau co-op |
| D-04 | Awal ritual: dipilih tokoh atau perlengkapan muncul sendiri |
| D-07 | Perawatan lilin dan kelanjutan waktu ke Scene 3 |
| D-08 | Si Penunggu selalu YA, atau mood acak sesuai GDD; tanda saat entitas berbohong |
| D-09 | Cara menutup ritual, prioritas ending, nasib Nisa, fungsi Malam 2 |
| D-10 | Durasi demo, tenggat, dan pembagian kerja |
| D-11 | Isi pesan Nisa, buku catatan, kalimat perpisahan |
| D-12 | Target perangkat demo (Windows, macOS, Web) |

## 11. Rencana kerja

| Tahap | Hasil |
|---|---|
| 1 | Revisi visual Scene 2 sesuai FigJam, putuskan D-01 |
| 2 | Scene 3: kabar diperpanjang, hilangnya Nisa, puzzle Dimas |
| 3 | Scene 4: penutupan ritual dan tiga ending |
| 4 | Audio final, playtest, perbaikan, build demo |

Pembagian tahap ke branch `gamedev-1` dan `gamedev-2` ditentukan tim. Alur branch dan instalasi ada di [README](README.md).
