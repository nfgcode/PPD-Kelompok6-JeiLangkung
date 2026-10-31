# Revisi 02 — Pixel horror grayscale

Tanggal: 12 Oktober 2026. Status: konsep terbaru untuk ditinjau pada [FigJam](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=135-146). Belum diterapkan ke Unity.

## Koreksi arah

Pengguna menyatakan ilustrasi revisi pertama terlalu HD, lalu mengirim contoh layar game horor retro. Revisi ini mengambil bentuk piksel kasar, kontur bertangga, pola dithering kotak-kotak, potret manga, panel padat, dan font pixel dari contoh tersebut. Gambar, karakter, teks, ikon, dan sistem pertarungan referensi tidak disalin.

Grayscale tetap mengikuti permintaan sebelumnya. Empat tingkat abu merupakan acuan visual, bukan klaim bahwa setiap piksel berkas sudah dikuantisasi persis menjadi empat warna. Prompt mengarahkan kesan gambar beresolusi logis kecil; ukuran berkas raster keluaran bukan resolusi native game yang sudah diputuskan.

## Hasil

Section yang sama `135:146` diperbarui menjadi **Revisi visual — pixel horror grayscale / 12 Oktober**.
- Menu: `135:147`, ilustrasi `135:148`.
- Kos: `135:149`, ilustrasi `135:150`.
- Potret Raka: `138:167`.

Menu memakai judul Pixelify Sans Bold dan navigasi VT323. Scene kos memakai panel ilustrasi utama, potret tokoh di samping, tombol pemeriksaan laptop/catatan, serta dialog bawah. Tidak ada mekanik pertarungan atau statistik baru yang ditambahkan. UI dan teks merupakan layer native editable; gambar hanya ilustrasi konten. Dialog contoh tetap usulan.

Tiga gambar memakai filter saturation -1 pada fill FigJam. UI lama disembunyikan untuk menyimpan riwayat tata letak; gambar versi pertama tetap tersedia pada berkas proyek. Materi moodboard tim lainnya tidak diubah. Scene serta build Unity belum diperbarui.

## Aset dan pemeriksaan

- [Menu pixel](first-screen-pixel-v2.png).
- [Kos pixel](scene1-kos-pixel-v2.png).
- [Potret Raka](raka-portrait-pixel-v2.png).
- [Pratinjau komposisi](figjam-pixel-preview-v2.png).

Ketiga upload berhasil. Dua screen tetap 1920 × 1080, dengan 30 node teks yang terlihat setelah memperhitungkan visibilitas parent. Pemeriksaan batas auto-layout tidak menemukan overflow. Pratinjau section diekspor pada 3000 px dan diperiksa: empat mahasiswa terbaca, potret terpisah, tombol dan dialog tidak terpotong, dan wajah tidak tertutup UI.

Pemeriksaan ini untuk konsep FigJam; tidak ada build, playtest, import font, maupun penggantian ilustrasi Unity pada sesi ini.

## Brief ilustrasi

Metode: tool gambar bawaan, tiga ilustrasi baru. Prompt akhir berikut disimpan sebagai dasar variasi selanjutnya.

### Menu

```text
Use case: stylized-concept. Asset type: low-resolution pixel art content background for a retro Indonesian horror game menu, widescreen 16:9. Match the supplied conversation reference's coarse 1-bit horror manga / early monochrome computer adventure graphics language. Very visibly chunky grid-aligned square pixels, approximately 320 by 180 logical pixel canvas enlarged 5x with sharp nearest-neighbor edges. ONLY four neutral gray values: black, dark gray, light gray, white. Binary ordered checkerboard dithering for shadows. Thick stepped pixel contours, flat primitive light shapes, a deliberately rough handmade MS Paint horror aesthetic, simplified economical pixel details. A small Indonesian kos room at night: at RIGHT a single stubby wax candle with a white triangular pixel flame, bamboo-and-cloth Jelangkung doll, a pencil and rough table; black door opening behind, simplified tiled wall. LEFT half is nearly empty black space for later editable title and navigation. Small room, quiet dread, no monster. All shading must be discrete pixel clusters with checkerboard dithering, absolutely no soft gradients, no blur, no antialiasing, no tiny detailed pen strokes, no HD illustration, no smooth rendering, no realistic materials, no photorealism, no 3D. Original illustration only. No text, letters, UI panels, icons, logos, credits, watermark, or borders.
```

### Kos

```text
Use case: illustration-story. Asset type: deliberately LOW RESOLUTION black-white pixel manga scene, 16:9 widescreen. Follow the supplied conversation reference: retro 1-bit computer horror adventure, rough MS Paint pixel manga drawings, human faces with large simply drawn eyes, coarse checkerboard dithering. Think a 320x180 logical pixel drawing enlarged with nearest-neighbor; large crisp visible square pixel clusters throughout, thick stair-step outlines, not smooth pen drawing. Four Indonesian adult university students around a low table doing an assignment in a SMALL cramped kos room at night. EXACTLY FOUR PEOPLE. Left foreground: short-haired male in a hoodie, seated in three-quarter profile. Back left: woman with tied-back long hair and light shirt, holding pen. Center: short wavy-haired man with rectangular glasses using a laptop. Right: shoulder-length-haired woman with round glasses holding a phone. Large readable simple expressive comic faces, relaxed ordinary expressions with slight tiredness, original character designs. Slim mattress and pillow left, standing fan right, simple window with black night outside, bare lamp overhead, notebooks and papers on table, mug, charging cable. Side view slightly elevated, people sitting close because room is cramped. LIMITED FOUR COLORS: pure black, dark neutral gray, light neutral gray, white only, no sepia or warm tint. Mostly flat black shadow shapes and white faces, midtone only coarse checkerboard dither patterns. Radically simplify surfaces and room clutter: the image must look like a vintage small-pixel game panel rather than a detailed illustration rendered at high resolution. Crisp blocky pixels, hard edges, no gradients, no cinematic bloom, no blur, no antialiasing, no photorealism, no 3D, no painting, no fine pen crosshatching. Full illustration only; no UI, no text, no lettering, no watermark or border. No monsters, candles, ritual items or gore in this studying scene.
```

### Potret Raka

```text
Use case: stylized-concept. Asset type: square character bust portrait content for a retro black-white pixel horror manga game's sidebar. Original Indonesian male university student Raka, about 20 years old, short slightly messy black hair, dark zip hoodie over plain white shirt, normal adult proportions, three-quarter view facing left, worried but composed large readable comic eyes, mouth slightly parted, chest-up. VERY COARSE low-resolution 1-bit manga graphics as in the supplied retro horror computer screenshot. Draw as if on a 96x96 logical pixel grid enlarged 10 times using sharp nearest-neighbor: bold stair-step square pixel outlines, coarse checkerboard dither on neck and clothes, few simple flat black shadow masses, pale flat white face. Black solid background. Only black, dark neutral gray, light neutral gray and white. Adult natural manga face, no tiny highly detailed ink strokes, no HD illustration, no realistic texture, no gradients, no smooth lines, no blur, no antialiasing, no 3D or photorealism. Portrait only, no text, no letters, no frame, no HUD, no badges, no weapons, no watermark or credits.
```

