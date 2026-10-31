# Aset gratis untuk dua layar awal JeiLangkung

Diperiksa pada **12 Oktober 2026** dari halaman pembuat atau halaman unggahan aslinya. Fokus: menu judul dan layar awal empat mahasiswa di kos, dengan **2D sebagai pilihan sementara**. Semua pilihan di bawah adalah kandidat; belum diunduh atau diimpor ke Unity. Audio belum didengarkan dan komposisi gabungan belum diuji dalam game.

Daftar pekerjaan ada pada [log scene](scene-work-log.md). Arah visual akhir masih mengikuti [keputusan desain](design-decisions.md).

## Pilihan untuk mulai bekerja

Untuk menu, mulai dari UI Pack, Bebas Neue, Inter, dan Interface Sounds. Untuk kos, coba Mini Apartment sebagai latar dan sprite sutemo untuk prototype dialog. Ini adalah saran pemakaian berdasarkan fungsi aset, bukan keputusan final tentang gaya karakter.

| ID | Kebutuhan | Kandidat dan sumber | Bagian gratis serta ketentuan yang diperiksa | Kesesuaian untuk layar |
|---|---|---|---|---|
| A-01 | Tombol Open, panel pengaturan, slider | [UI Pack — Kenney](https://kenney.nl/assets/ui-pack) | Gratis, 430 elemen, CC0 | Dasar UI; pilih bentuk sederhana dan sesuaikan warna dengan tampilan grayscale |
| A-02 | Suara klik dan konfirmasi | [Interface Sounds — Kenney](https://kenney.nl/assets/interface-sounds) | Gratis, 100 suara, CC0 | Pilih satu klik dan satu konfirmasi; dengarkan sebelum dipakai agar tidak terlalu ceria |
| A-03 | Font judul | [Bebas Neue — Google Fonts](https://fonts.google.com/specimen/Bebas+Neue), [lisensi asli](https://raw.githubusercontent.com/google/fonts/main/ofl/bebasneue/OFL.txt) | Gratis, SIL OFL 1.1; sertakan copyright dan lisensi jika file font didistribusikan | Kandidat judul yang tegas dan sederhana; belum merupakan logo final |
| A-04 | Font tombol, pengaturan, dan dialog | [Inter — situs resmi](https://rsms.me/inter/), [lisensi asli](https://raw.githubusercontent.com/rsms/inter/master/LICENSE.txt) | Gratis, SIL OFL 1.1; sertakan copyright dan lisensi jika file font didistribusikan | Kandidat teks yang mudah dibaca pada panel gelap |
| A-05 | Latar kos | [VN Background Pack — Mini Apartment, Katelynn Parsec](https://katelynn-parsec.itch.io/mini-aprtment) | Gratis; `VN Backgrounds - Mini Apartment.zip`; PNG 1920 × 1080; CC BY 4.0 | Pilihan pertama: apartemen kecil dengan versi bergaris/tanpa garis, kamar, ruang duduk, dan pintu. Perlu penyesuaian untuk terlihat seperti kos dan cocok dengan perspektif tokoh |
| A-06 | Alternatif latar kamar | [Free Backgrounds Set — dicortesia](https://dicortesia.itch.io/free-backgrounds) | Gratis melalui Name your own price; PNG 1920 × 1080; kamar dengan tiga pencahayaan; CC BY 4.0 | Alternatif untuk membandingkan kamar terang/malam. Pilih dari pratinjau, jangan menebak isi hanya dari nomor file |
| A-07 | Potret Raka dan Dimas | [Male Character Sprite for Visual Novel — sutemo](https://sutemo.itch.io/male-character-sprite-for-visual-novel) | Gratis melalui Name your own price; `Male Sprite by Sutemo.zip`; PSD 1158 × 1287. Halaman mengizinkan proyek pribadi/komersial dan melarang penjualan sprite sebagai aset tersendiri | Potret bust-up dengan variasi rambut, pakaian, dan ekspresi; dapat dipakai sebagai dua tokoh sementara yang dibedakan. Tidak menyediakan pose duduk mengerjakan tugas |
| A-08 | Potret Ayu dan Nisa | [Female Character Sprite for Visual Novel — sutemo](https://sutemo.itch.io/female-character) | **Versi dasar** `Female Sprite by Sutemo.zip` gratis; PSD 1011 × 1145. `Anime Girl Sprite V2.zip` memerlukan pembayaran dan tidak masuk pilihan gratis ini. Ketentuan penggunaan pribadi/komersial sama dengan halaman pembuat | Untuk prototype dialog dan pembeda dua tokoh sementara; bukan pose tubuh penuh atau animasi duduk |
| A-09 | Suasana menu horor | [Ambient Horror Track 01 — Cleyton Kauffman](https://opengameart.org/content/ambient-horror-track-01) | Gratis; `ambient_horror_track_01.zip`; WAV/OGG; CC0. Pembuat menyebut loop 48 detik | Kandidat musik menu; dengarkan dan uji sambungan loop. Tidak otomatis cocok untuk adegan kos sebelum ancaman muncul |
| A-10 | Suara ruangan/peralatan | [The Shop — LEGIT Audio](https://opengameart.org/content/the-shop) | **Sampel gratis pada OpenGameArt** berlisensi CC0; `legit_audio_-_the_shop_free_sfx_wav.zip` atau versi MP3. Lisensi ini tidak mencakup paket lengkap yang dijual di tempat lain | Kandidat room tone atau bunyi peralatan. Pilih setelah mendengarkan; belum dipastikan ada suara gardu/kipas yang tepat |

Kesesuaian pada kolom terakhir merupakan pertimbangan desain. Halaman sumber membuktikan isi umum dan ketentuan paket, bukan bahwa aset tersebut sudah cocok dalam komposisi game kita.

## Cara memperoleh versi gratis

- Kenney: gunakan **Download** lalu **Continue without donating** bila penawaran donasi muncul. Bundle All-in-1 tidak diperlukan.
- itch.io dengan Name your own price: gunakan jalur **No thanks, just take me to the downloads** bila muncul, lalu pilih file gratis yang disebutkan di atas. Hindari file tambahan yang mempunyai harga minimum.
- Mini Apartment dan OpenGameArt: gunakan bagian Download/File(s) pada halaman paket yang ditautkan.
- Font: unduh dari halaman resmi; simpan file lisensi bersama font yang dipakai dalam distribusi.

## Kebutuhan yang belum ditutup oleh kandidat ini

| Kebutuhan | Kondisi saat pencarian | Langkah kerja |
|---|---|---|
| Empat mahasiswa duduk mengerjakan tugas dalam satu perspektif | Belum ditemukan satu paket gratis terverifikasi yang tepat untuk keempat tokoh dan pose itu | Gunakan potret sementara untuk menguji dialog, lalu siapkan ilustrasi/pose khusus atau cari paket lanjutan setelah komposisi dipilih |
| Meja, laptop, buku, dan kertas sebagai lapisan terpisah | Latar kamar mungkin sudah memuat furnitur, tetapi kandidat latar tidak menjamin tiap benda terpisah | Tentukan benda yang harus bergerak/berinteraksi; siapkan lapisan terpisah hanya untuk benda tersebut |
| Ikon pengaturan sesuai sketsa | Board menyebut `grommet-icons:settings-option`, tetapi lisensi paket ikon tersebut belum diperiksa dalam daftar ini | Buat ikon sederhana sendiri atau pilih ikon dari sumber yang lisensinya diverifikasi sebelum import |
| Tampilan chat dan notifikasi | Isi dan desainnya khusus cerita kita | Buat panel dan teks sendiri dengan komponen UI; suara notifikasi dapat diseleksi dari kandidat audio |
| Kesesuaian grayscale dan suasana horor | Paket visual belum dinilai sebagai gabungan | Buat satu komposisi uji, periksa kontras, skala tokoh, dan perspektif sebelum memfinalkan aset |

## Pencatatan lisensi saat aset benar-benar dipakai

- **CC0:** sumber dicatat untuk pelacakan. Kredit tidak menjadi syarat CC0; apresiasi kepada pembuat boleh ditambahkan.
- **CC BY 4.0:** sertakan nama pembuat, tautan sumber, tautan lisensi, dan keterangan perubahan jika ada. Ketentuan ini tercantum pada [lisensi Creative Commons](https://creativecommons.org/licenses/by/4.0/).
- **SIL OFL 1.1:** simpan copyright dan teks lisensi dengan font yang dibagikan bersama game.
- **sutemo:** simpan ketentuan dari halaman dan paket yang diunduh. Pembuat menyatakan kredit boleh diberikan atau tidak pada komentar aslinya; sumber tetap dicatat secara internal. Larangan penjualan sprite tersendiri tetap berlaku.

Jika latar Mini Apartment digunakan dan diubah, contoh catatan kredit: “Latar Mini Apartment — Katelynn Parsec; CC BY 4.0; diubah ke grayscale,” disertai tautan halaman paket dan lisensi. Contoh tersebut baru dipakai bila perubahan dan pemakaian itu benar-benar terjadi.

## Catatan pemilihan

| ID aset | Dipilih? | File yang diambil | Lokasi import | Penyesuaian | Pemeriksaan |
|---|---|---|---|---|---|
| A-03 Bebas Neue | Ya, font judul prototype | `BebasNeue-Regular.ttf`, `OFL.txt` | `Assets/_Project/Art/Fonts` | Atlas SDF dibuat Unity; font sumber tetap | Impor dan referensi font diuji dalam PlayMode |
| Lato Regular, tambahan saat implementasi | Ya, font teks prototype | `Lato-Regular.ttf`, `OFL.txt` dari [Google Fonts](https://github.com/google/fonts/tree/main/ofl/lato) | `Assets/_Project/Art/Fonts` | Atlas SDF dibuat Unity; font sumber tetap | Impor dan referensi font diuji dalam PlayMode |

Tambahkan baris ketika aset dipilih. Pencarian kandidat tidak dihitung sebagai import, pembuatan scene, atau pengujian game.

Prototype 12 Oktober memakai ilustrasi geometris dan nada prosedural untuk menu/kos. Paket latar, sprite potret, UI, dan audio lain di daftar kandidat belum diimpor. Lato dipilih sebagai font teks statis; Inter tetap kandidat. Catatan aset aktual serta salinan lisensi tersedia pada [lisensi prototype](asset-licenses.md).
