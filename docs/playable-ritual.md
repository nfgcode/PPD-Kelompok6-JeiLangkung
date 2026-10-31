# Prototype Scene 2: Datang Tak Dijemput

Dibuat 31 Oktober 2026. Status: **prototype yang dapat dimainkan**. Visual, angka waktu, dan dialog reaksi masih sementara. Keputusan yang dipakai tercatat sebagai K-10 sampai K-12 di [keputusan desain](design-decisions.md).

## Menjalankan

- Build: jalankan `Builds/Windows/JeiLangkung.exe`, pilih Open, mainkan Scene 1, lalu pilih **Lanjut ke ritual** setelah Bersambung.
- Editor: buka `Assets/_Project/Scenes/Scene2_Ritual.unity`, lalu Play.
- Jika scene atau konfigurasi hilang: menu **JeiLangkung > Siapkan Scene 2 ritual**. Setup tidak menimpa scene atau konfigurasi yang sudah ada.

## Kontrol

| Tindakan | Kontrol |
|---|---|
| Lanjut dialog, menyalakan lilin berikutnya, bertanya | Spasi, Enter, atau klik kiri |
| Menyalakan lilin tertentu | Klik lilinnya; lilin yang disorot diberi sudut putih |
| Menjaga genggaman | Tahan Spasi atau klik kiri untuk mengangkat genggaman, lepas untuk menurunkan |
| Jeda | Esc atau tombol Jeda (Esc) di strip atas |
| Menu jeda dan pilihan akhir | Panah atas/bawah dan Enter, atau mouse; tanda `>` menunjukkan pilihan yang sedang fokus |

## Alur

1. Peringatan konten dan judul babak.
2. Ayu meminta keempat lilin dinyalakan. Waktu mulai berjalan sejak lilin pertama.
3. Ayu meminta boneka dipegang bertiga, lalu membaca mantra dalam dua baris.
4. Tanda kecil: air di gelas bergetar. Boneka berat dan meronta, genggaman pertama.
5. Pertanyaan "Kamu ada di sini?": genggaman berlangsung sambil pensil bergerak, lalu pensil melingkari YA dan mata boneka berkedip merah.
6. Tanda kecil: semua api condong ke satu arah. Dimas mendesak pertanyaan penting.
7. Tanda kecil: HP bergetar dengan layar kosong. Pertanyaan "Apakah lampu akan menyala malam ini?" dijawab YA dengan genggaman yang lebih berat.
8. Nisa lega, bayangan lewat di jendela, Ayu mengingatkan bahwa entitas belum diantar pulang. Bersambung ke Babak 3.

Jika lilin terakhir padam pada langkah mana pun: gelap total, lalu siluet bermata merah dan suara kejut, kemudian pilihan Main ulang atau Menu judul.

## Mekanik bar genggaman

**Objeknya adalah boneka Jelangkung** (gayung batok kelapa berbaju dengan pensil terikat di bawahnya) yang dipegang Raka bersama Ayu dan Dimas. Dasarnya: storytelling Scene 2 menulis "jaga genggaman saat entitas masuk", dan tabel tokoh menetapkan Raka memegang Jelangkung dengan bar keseimbangan dan jawaban YA/TIDAK. Bar tidak ditempel ke pensil atau lilin, karena pensil adalah hasil gerak boneka dan lilin sudah menjadi batas waktu.

Cara kerja, gaya bar memancing:

- Bar tegak di kanan layar. Blok terang adalah genggaman Raka. Ikon kepala batok adalah rontaan boneka yang digerakkan entitas.
- Menahan tombol mempercepat genggaman naik; melepas membuatnya turun.
- Selama ikon berada di dalam genggaman, bar kemajuan di sebelahnya terisi. Di luar genggaman, kemajuan berkurang, boneka bergoyang keras, dan terdengar dengung berat.
- Penuh: boneka tenang, atau pada pertanyaan, pensil sampai di jawaban. Pensil bergerak sejauh kemajuan genggaman.
- Kosong: genggaman lepas, lilin yang sedang terbakar dipercepat 12 detik, dan genggaman diulang dari 30%. Pesan "Genggaman lepas. Lilin terbakar lebih cepat." muncul di baris petunjuk.
- Di layar, boneka berayun di sekitar palang yang digenggam. Kemiringannya mengikuti jarak rontaan dari tengah genggaman.
- Rontaan selalu pindah cukup jauh ke seluruh bar, sehingga pemain yang diam tidak bisa menang dengan menunggu.

Agitasi naskah default: masuknya entitas 0,5, pertanyaan pertama 0,3, pertanyaan utama 0,7. Pada simulasi pemain yang memperhatikan, median lama genggaman 4,3 / 5,8 / 8,1 detik untuk agitasi 0,3 / 0,5 / 0,7. Pemain manusia kemungkinan lebih lambat; angka ini perlu dicek lewat playtest.

## Lilin

- Empat lilin di sudut kertas adalah satu batas waktu yang terbakar bergiliran sesuai urutan dinyalakan.
- Sementara: 50 detik per lilin, 200 detik total.
- Jeda menghentikan lilin. Lilin yang tersisa di bawah 20% meredup dan berkedip lebih keras. Lilin yang habis mengeluarkan asap, dan ruangan menjadi lebih gelap.
- Strip atas menampilkan sisa keempat lilin sesuai urutan menyala.
- Perawatan lilin belum dibuat, dan waktu belum dibawa ke Scene 3.

## Visual, teks, dan suara

- Gambar dirender pada 480 x 270 lalu diperbesar tanpa penghalusan. Cahaya dihitung per piksel dari keempat lilin dan didither Bayer 4 x 4 ke empat tingkat abu, mengikuti konsep pixel horror K-09.
- Warna hanya pada api lilin (kuning) dan mata entitas (merah). Aturan ini diperiksa oleh test `OnlyFlameAndEyesCarryColour`.
- Detail cerita tanpa dialog: pengisi daya masih tertancap di stopkontak yang mati dengan kabel menuju HP, kilang menyala di cakrawala jendela sementara Km 15 gelap, pintu sedikit terbuka, korek api, dan gelas air.
- Bagian bawah kertas sengaja kosong sebagai bidang tulisan pensil untuk Babak 3 dan 4.
- Teks UI memakai TextMeshPro dengan font VT323. Tulisan YA/TIDAK di kertas memakai huruf piksel 5 x 7 di dalam gambar (`PixelFont`) karena ikut terkena cahaya lilin; huruf yang sama bisa dipakai untuk tulisan BELUM, TUNGGU, atau TINGGAL.
- Pengaturan **Redup halus** di menu jeda mengurangi kedip lilin dan menghilangkan guncangan serta suara keras pada kejutan.
- Suara masih prosedural: jangkrik, korek api, dengung boneka berat, goresan pensil, getar HP, air bergetar, api condong, bayangan, lilin padam, dan kejutan.

## Lokasi kode dan data

| File | Isi |
|---|---|
| `Scripts/Runtime/Ritual/CandleTimer.cs` | Empat lilin sebagai satu batas waktu |
| `Scripts/Runtime/Ritual/GripBar.cs`, `GripTuning.cs` | Aturan bar genggaman |
| `Scripts/Runtime/Ritual/RitualFlow.cs` | Urutan langkah, lilin, genggaman, pertanyaan, tanda horor, akhir |
| `Scripts/Runtime/Ritual/RitualStep.cs`, `RitualTuning.cs`, `RitualConfig.cs` | Naskah dan angka yang diatur lewat Inspector |
| `Scripts/Runtime/Ritual/RitualRenderer.cs`; `Scripts/Runtime/Pixel/` (palet, kanvas, huruf, tombol, panggung UI bersama) | Gambar piksel, cahaya, boneka, bar |
| `Scripts/Runtime/Ritual/RitualController.cs`, `RitualAudio.cs` | Input System, TextMeshPro, menu, audio, perpindahan scene |
| `Scripts/Runtime/Ritual/RitualSmokeRunner.cs` | Pemeriksaan executable, hanya development build dengan `--ritual-smoke <folder>` |
| `Scripts/Editor/RitualSetup.cs` | Setup scene/config dan render pratinjau piksel |
| `Data/RitualConfig.asset` | Naskah dan angka Scene 2 |

Naskah terdiri dari langkah `Line`, `LightCandles`, `Grip`, `Question`, dan `Sign`, sehingga tim bisa mengubah urutan, teks, jawaban per pertanyaan, dan agitasi tanpa menyentuh kode.

## Pemeriksaan 31 Oktober 2026

| Pemeriksaan | Hasil |
|---|---|
| EditMode | 22 lulus, 0 gagal, `test-results/editmode.xml` (15 test baru untuk lilin, genggaman, alur, huruf, dan warna) |
| PlayMode | 4 lulus, 0 gagal, `test-results/playmode.xml`: alur keyboard sampai Bersambung dengan genggaman lewat perangkat keyboard, jeda, Redup halus, main ulang, lilin padam, Menu judul, dan tombol Lanjut ke ritual dari Scene 1 |
| Build Windows development | `Build Finished, Result: Success` dan `BUILD_OK`, `test-results/build-windows.log` |
| Executable | `--ritual-smoke` keluar 0, laporan PASS, 11 tangkapan 1280 x 720 dan satu 1024 x 768 di `test-results/ritual-smoke/` |
| Pratinjau piksel | 16 frame di `test-results/ritual-preview/` |

Belum diperiksa: playtest dengan pemain (rasa genggaman, pemahaman bar, ketegangan), audio dengan pendengaran, navigasi panah pada menu (mengandalkan navigasi otomatis EventSystem), klik mouse pada lilin di executable (aturannya diuji lewat test), dan macOS.

## Gate antislop (mode during)

Arah: konsep pixel horror K-09 dan aturan visual game. Pembacaan desain: layar ritual game horor untuk pemain desktop, bahasa visual pixel grayscale dengan dither, dial ENERGY 2 / RHYTHM 1 / MOTION 2. Skill antislop dan tasteskill ditulis untuk halaman web, jadi aturan khusus web (mobile, tema, Core Web Vitals) tidak berlaku. Skrip `draw.mjs` dan `inspect.mjs` milik tasteskill tidak dijalankan karena layar ini bukan halaman web; aturan rumah dan hukum tipografi/warna dipakai sebagai daftar periksa.

| Butir | Status | Bukti |
|---|---|---|
| R-02 em dash | PASS | Tidak ada em dash di naskah dan teks baru |
| R-17, R-18, R-36, R-38 data dan klaim | PASS | Tidak ada angka atau klaim; dialog reaksi ditandai usulan |
| R-23 aset | PASS | Tidak ada logo atau foto tokoh; boneka dan ruangan digambar dari kode sebagai aset sementara |
| R-25 kontras | PASS | Putih di hitam sekitar 15:1, abu terang di hitam 6,6:1, putih di latar fokus 7,5:1 |
| R-26, R-35 kontrol berfungsi | PASS | Lanjut, Jeda, Redup halus, Main ulang, Menu judul, dan Lanjut ke ritual diuji di PlayMode atau executable |
| R-32 keyboard | PASS dengan catatan | Seluruh permainan bisa dengan keyboard; fokus menu terlihat; navigasi panah belum diuji otomatis |
| R-01, R-13 gradien dan glow | PASS | Tidak ada; bayangan dibuat dengan dither |
| R-06 tipografi | PASS | VT323 dipilih karena konsep pixel K-09 dan selaras dengan grid piksel 4 x 4; huruf kalimat, tanpa label kapital berjarak lebar |
| R-07 pola latar | PASS | Kisi titik saat gelap total dihapus setelah pratinjau |
| R-08 panah | PASS | `>` hanya muncul sebagai kursor fokus |
| Fokus per layar | PASS | Boneka di tengah, diterangi empat lilin |
| Satu aksen | PASS | Kuning api; merah mata hanya saat entitas menjawab dan pada akhir buruk |
| Motif identitas | PASS | Wajah kapur di batok kelapa, kertas YA/TIDAK, dan meter lilin |
| R-29 palet | PASS | Empat abu ditambah kuning dan merah sesuai aturan game |

## Masih terbuka

- D-02 input mantra: mantra masih dua baris dialog Ayu yang dilanjutkan dengan Spasi. Ritme mantra belum dibuat.
- D-07: perawatan lilin dan biayanya, serta apakah sisa waktu berlanjut ke Scene 3.
- D-08: jawaban diatur per pertanyaan (keduanya YA sesuai storytelling). Sistem mood dari GDD tidak dibuat.
- D-11: dialog reaksi Nisa, Dimas, dan Ayu adalah usulan prototype. Tanda kebohongan entitas masih "Ruang kreasimu"; empat tanda kecil di sini hanya suasana, belum menjadi petunjuk kebohongan.
