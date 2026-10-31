# Prototype menu dan Scene 1

Sejak 31 Oktober 2026, menu dan Scene 1 memakai gaya piksel yang sama dengan [Scene 2](playable-ritual.md): grid 480 x 270, empat tingkat abu dengan dither, teks VT323, dan panel komik mengikuti konsep FigJam revisi 02 (K-09, K-13).

## Menjalankan

Build Windows berada di `Builds/Windows/JeiLangkung.exe`. Jalankan executable dari folder tersebut; `JeiLangkung_Data`, `UnityPlayer.dll`, dan file pendamping harus tetap bersamanya. Untuk memindahkan game, salin seluruh folder `Windows`.

Di Unity 6000.3.25f1, buka project `JeiLangkung`, lalu buka `Assets/_Project/Scenes/FirstScreen.unity` dan tekan Play. Scene kos juga bisa dibuka langsung melalui `Scene1_Kilo15.unity`.

## Kontrol dan alur

| Tindakan | Kontrol |
|---|---|
| Pilih menu dan tombol | Panah dan Enter, atau mouse; tanda `>` menunjukkan pilihan yang fokus |
| Lanjut dialog | Spasi, klik pada gambar, atau tombol Lanjut |
| Periksa laptop | 1, klik laptop yang disorot sudut putih, atau tombol Laptop |
| Periksa catatan | 2, klik catatan Ayu yang disorot, atau tombol Catatan |
| Buka HP / nyalakan senter setelah gelap | E, klik pada gambar, atau tombol yang tampil |
| Jeda / tutup panel | Esc atau tombol Jeda (Esc) |
| Pilihan setelah Bersambung | Panah dan Enter (fokus otomatis ke Lanjut ke ritual), atau mouse |

Menu menyediakan Open, pengaturan volume, pilihan redup halus, dan Keluar. Pengaturan disimpan lokal melalui PlayerPrefs.

Scene 1: tiga dialog pembuka → cocokkan laptop dan catatan (keduanya wajib, urutan bebas) → baca pesan grup warga di HP → simpan HP → lampu padam → nyalakan senter HP → empat dialog reaksi → Bersambung. Main ulang memulai Scene 1 dari awal. Menu judul mengembalikan pemain ke first screen. **Lanjut ke ritual** membuka Scene 2. Jeda menghentikan urutan pemadaman dan input interaksi.

## Tampilan

- **Menu:** ilustrasi meja dengan boneka dan lilin di kanan, judul dan pilihan di sisi kiri yang gelap. Api lilin satu-satunya warna dan berkedip; cahayanya ikut menggerakkan dither di sekitar lilin.
- **Kos:** strip status (jam, tempat, "Listrik: menyala/padam"), keterangan panel, ilustrasi empat mahasiswa, potret pembicara, panel tindakan, dan kotak dialog, mengikuti tata letak FigJam Scene 1.
- **Pemadaman:** lampu kamar turun, sempat menyala sebentar, lalu padam total lewat perhitungan cahaya yang sama dengan Scene 2. Redup halus menghapus lonjakan itu.
- **Senter:** cahaya keluar dari HP Nisa; Nisa dan Dimas terlihat, Raka dan Ayu tenggelam dalam gelap. Potret pembicara ikut gelap atau tersorot.
- **HP:** tampilan chat buatan sendiri tanpa merek, layar putih keabu-abuan yang dingin.

## Mengubah kode dan isi

- `Assets/_Project/Data/OpeningConfig.asset`: font, gambar menu/kos, potret per pembicara, durasi pemadaman, dialog awal, dan reaksi. Ubah melalui Inspector.
- `Assets/_Project/Art/Pixel/`: gambar abu-abu hasil konversi (`Menu.png` 480 x 270, `Kos.png` 348 x 196, `Potret-*.png` 112 x 112). Boleh disunting tim; setup tidak menimpanya. Hapus sebuah berkas lalu jalankan setup untuk membuatnya ulang dari ilustrasi konsep.
- `Scripts/Runtime/Opening/OpeningFlow.cs`: aturan dan penguncian urutan tindakan.
- `OpeningController.cs`: Input System, panel, HP, transisi scene, suara, dan urutan redup.
- `OpeningRenderer.cs`, `OpeningLook.cs`: cahaya, dither, potret, sorotan benda, dan senter.
- `Scripts/Runtime/Pixel/`: bagian bersama menu, Scene 1, dan Scene 2 (`PixelPalette`, `PixelCanvas`, `PixelImage`, `PixelFont`, `PixelButton`, `PixelUi`, `PixelViewport`).
- `Scripts/Editor/OpeningSetup.cs`: menu **JeiLangkung > Siapkan pembuka**, pembuatan scene/config yang belum ada, pengisian gaya piksel yang masih kosong, render pratinjau, dan build Windows. Setup tidak menimpa scene atau isian config yang sudah ada.
- `Scripts/Editor/PixelArtSetup.cs`: konversi ilustrasi konsep `docs/art-direction/*-pixel-v2.png` ke gambar abu-abu kecil dengan rata-rata area.

Editor membuat seluruh `.meta`, file scene, dan font SDF. Runtime UI dibuat saat Play; nilai cerita tetap tersimpan di asset konfigurasi. Tidak ada package tambahan.

## Aset dan lisensi

Gambar menu, kos, dan potret berasal dari ilustrasi konsep revisi 02 (lihat [brief revisi pixel](art-direction/pixel-opening-v2.md)), diperkecil dan diubah ke abu-abu. Desain tokoh masih usulan. Teks memakai VT323 (SIL OFL). Bebas Neue dan Lato tetap tersimpan di proyek tetapi tidak lagi dipakai oleh UI. Detail ada pada [lisensi aset](asset-licenses.md). Suara klik, notifikasi, dan dengung masih nada prosedural.

## Pemeriksaan 31 Oktober 2026

| Pemeriksaan | Hasil |
|---|---|
| EditMode | 22 lulus, 0 gagal |
| PlayMode | 4 lulus, 0 gagal; test pembuka lama (menu, pengaturan, tugas, HP, jeda saat padam, senter, main ulang, menu judul) tetap lulus tanpa diubah |
| Build Windows development | `BUILD_OK` |
| Executable `--smoke-test` | Keluar 0, PASS, delapan tangkapan di `test-results/opening-smoke/` termasuk menu 1024 x 768 |
| Pratinjau piksel | Sembilan frame di `test-results/opening-preview/` |

Belum diperiksa: playtest dengan pemain, audio dengan pendengaran, klik benda di gambar pada executable (aturannya sama dengan tombol yang diuji), dan macOS. Pemeriksaan otomatis tidak menggantikan playtest rasa horor atau uji pada komputer lain.
