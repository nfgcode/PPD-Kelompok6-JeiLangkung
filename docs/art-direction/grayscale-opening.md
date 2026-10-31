# Revisi visual pembuka — 2D grayscale

Tanggal: 12 Oktober 2026. Status: **usulan visual untuk ditinjau**, belum dipasang pada scene Unity.

Riwayat revisi pertama. Pengguna menilai tampilan ini terlalu HD; konsep FigJam pada node yang sama telah diganti dengan [revisi pixel horror](pixel-opening-v2.md). Berkas dan bukti pemeriksaan di bawah tetap menyimpan hasil versi pertama.

Hasil pada [FigJam — Revisi visual](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=135-146):
- First Screen: `135:147`, latar `135:148`.
- Scene 1 pembuka kos: `135:149`, latar `135:150`.
- Section revisi: `135:146`, halaman Moodboard `0:1`.

## Dasar visual

Referensi yang ditinjau pada board: `4:2`, `4:33`, `6:133`, `7:326`, `25:440`, dan `25:683`. Arah yang diambil adalah kontur tinta organik, arsiran rapat, tekstur kertas, ekspresi tokoh, dan kontras hitam-putih. Identitas karakter, panel, watermark, dan judul karya referensi tidak disalin.

Menu menempatkan judul dan pilihan di kiri, lilin serta siluet boneka di kanan. Ilustrasi menu berfungsi sebagai motif judul; kemunculan ritual pada alur Scene 1 tetap mengikuti keputusan cerita terbuka. Kos menampilkan empat mahasiswa duduk berdekatan di meja tugas, laptop, catatan, kasur, kipas, dan jendela malam. Wajah tetap terbaca. Dialog contoh pada FigJam merupakan usulan, bukan perubahan pada `OpeningConfig.asset`.

Palet UI: tinta #121212, abu #696969, kabut #ACACAC, kertas #EDEDED. Fill kedua ilustrasi memakai filter saturation -1 di FigJam untuk tampilan monokrom. Judul memakai Cinzel Decorative Bold sebagai usulan; label memakai Bebas Neue, teks memakai Lato, dan petunjuk memakai Roboto Mono. Pilihan font baru belum diimpor ke Unity.

## Berkas dan struktur

- [Ilustrasi menu](first-screen-ink-v1.png).
- [Ilustrasi kos](scene1-kos-ink-v1.png).
- [Pratinjau komposisi FigJam](figjam-grayscale-preview.png).

Dua ilustrasi adalah aset raster konten. Judul, tiga tombol menu, nama pembicara, dialog, HUD lokasi, dan petunjuk merupakan layer native yang bisa diedit. Ada 28 node teks. Dua screen berukuran 1920 × 1080; section berada di sisi kanan referensi lama tanpa mengubahnya.

Pemeriksaan: kedua upload berhasil, layer gambar terisi, komposisi section diekspor pada 3000 px lalu ditinjau. Empat tokoh terlihat, teks tidak terpotong, panel dialog tidak menutup wajah, dan navigasi menu terbaca. Ini pemeriksaan desain FigJam; tidak ada build atau playtest Unity baru.

## Brief ilustrasi

Metode pengerjaan: tool gambar bawaan, pembuatan dua ilustrasi baru. Prompt akhir disimpan di bawah untuk variasi visual berikutnya.

### First Screen

```text
Use case: illustration-story. Asset type: 16:9 widescreen background illustration for an Indonesian 2D psychological horror game's title menu. Original hand-drawn black-and-white ink manga, dense fine crosshatching, dry brush, stippled photocopy paper texture, gray ink washes, strongly defined organic lines. Strict neutral grayscale only, no sepia, no color. Eye-level intimate view of a weathered wooden table in a pitch-dark Indonesian boarding house at night. On the RIGHT HALF a single short wax candle with a small white flame, melted wax, a wooden pencil and a loosely tied small traditional Jelangkung doll made of bamboo sticks and cloth leaning beyond the candle, quietly ominous without a visible monster. Grimy wall, barely discernible curtain and dark doorframe behind it, atmospheric soft shadow falling toward the viewer, beautiful highly detailed ink craft, tension through darkness and solitude. LEFT HALF contains mostly deep textured charcoal-black empty wall for large title and navigation to be placed later in Figma. Table starts in lower third. Cinematic carefully composed playable-game illustration, not a photograph, not a 3D render, not simple geometric vector shapes, not pixel art. No text, no letters, no UI, no logo, no watermark, no border. Natural 16:9 composition.
```

### Kos

```text
Use case: illustration-story. Asset type: detailed hand-drawn 2D game scene background, wide 16:9. Original black-and-white horror manga ink illustration with natural expressive adult human characters, clean fine pen contours, dense crosshatching in the room and clothing, coarse photocopy paper grain and soft gray wash shading. STRICT neutral grayscale; black ink, gray, off-white only. Four Indonesian university students, all approximately 20 years old, sitting on the floor around ONE low wooden table doing an assignment in a small cramped modest Indonesian kos room at 9pm. Exactly FOUR people: left foreground a short-haired young man in a dark hoodie, three-quarter back/profile with visible natural face, looking toward his friends; back-left a young woman with long dark tied-back hair in a light sweater, holding a pen and glancing toward the others; center-back a wavy-haired young man in a dark t-shirt with rectangular glasses, using an open laptop; right a young woman with shoulder-length dark hair and round glasses, holding a mobile phone and smiling faintly. Convincing anatomy, distinct faces and hairstyles, natural casual poses, original character designs. They are sitting close together because this room is SMALL. Laptop, notebooks, loose assignment papers, a cup, a charging cable on table. Thin mattress along left wall, small standing fan on right, curtained window with black night outside, scuffed plaster wall, Indonesian kos details without decorative Japanese signs. A bare overhead white lamp lights faces and table while edges fall into dark ink shadows. Quiet social warmth underneath subtle impending dread; no monster, no gore, no candle or ritual appearing yet. Camera static cinematic slightly elevated three-quarter wide view, four faces readable, drawing fills upper 78% with unobtrusive dark floor band in bottom 22% for later editable dialogue UI. Premium indie horror visual novel environment with tactile drawn linework, not photographic, not 3D, no geometric silhouettes, no pixel art. No text, no letters, no UI, no watermarks, no credits, no border.
```

## Pekerjaan berikutnya

Tinjau kecocokan ilustrasi dan desain tokoh bersama tim. Setelah arah dipilih, pisahkan kebutuhan animasi, ekspresi, dan hotspot; pasang ilustrasi/UI ke Unity lalu periksa keterbacaan, interaksi, serta pemadaman dalam game. Scene Unity dan executable saat ini masih memakai ilustrasi geometris prototype.

