# Aset yang digunakan pada prototype

| Aset | Sumber | Lisensi dan berkas | Pemakaian |
|---|---|---|---|
| Bebas Neue Regular | [Google Fonts / Bebas Neue](https://github.com/google/fonts/tree/main/ofl/bebasneue) | SIL OFL 1.1; `Assets/_Project/Art/Fonts/BebasNeue-OFL.txt` | Tidak dipakai UI sejak 31 Oktober 2026; berkas tetap disimpan |
| Lato Regular | [Google Fonts / Lato](https://github.com/google/fonts/tree/main/ofl/lato) | SIL OFL 1.1; `Assets/_Project/Art/Fonts/Lato-OFL.txt` | Tidak dipakai UI sejak 31 Oktober 2026; berkas tetap disimpan |
| VT323 Regular | [Google Fonts / VT323](https://github.com/google/fonts/tree/main/ofl/vt323) | SIL OFL 1.1, Copyright 2011 The VT323 Project Authors; `Assets/_Project/Art/Fonts/VT323-OFL.txt` | Seluruh teks UI menu, Scene 1, dan Scene 2 |
| TMP Essential Resources | Package uGUI/TextMeshPro yang sudah terpasang | Lisensi dan pemberitahuan yang diimpor bersama resource; folder `Assets/TextMesh Pro` | Shader, pengaturan, dan resource teks |
| Gambar menu, kos, dan potret | Ilustrasi konsep revisi 02 milik proyek, `docs/art-direction/*-pixel-v2.png` ([brief](art-direction/pixel-opening-v2.md)) | Tidak memakai paket gambar pihak ketiga; diperkecil dan diubah ke abu-abu oleh `PixelArtSetup.cs` ke `Assets/_Project/Art/Pixel/` | Menu, empat mahasiswa di kos, potret Raka, Ayu, Dimas, dan Nisa. Desain tokoh masih usulan |
| Gambar piksel Scene 2 | `RitualRenderer.cs`, `PixelFont.cs` | Digambar dari kode; tidak memakai paket gambar pihak ketiga | Meja ritual, boneka, lilin, ruangan, bar genggaman, tulisan YA/TIDAK |
| Nada prosedural | `OpeningController.cs`, `RitualAudio.cs` | Tidak memakai sampel audio pihak ketiga | Klik, notifikasi, dengung listrik; suara ritual Scene 2 |

Bebas Neue dan Lato diunduh 12 Oktober 2026; VT323 diunduh 31 Oktober 2026 (153 KB). Font sumber tidak diubah; Unity menghasilkan atlas SDF untuk rendering. Sertakan semua file OFL ketika membagikan build; salinannya disalin otomatis ke folder `Builds/Windows/Licenses`.

Daftar [kandidat aset gratis](free-assets-first-screens.md) merupakan hasil pencarian. Paket lain pada daftar itu belum diunduh atau dipakai dalam build ini.
