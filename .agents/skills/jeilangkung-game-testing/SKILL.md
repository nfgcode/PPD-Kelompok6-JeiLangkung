---
name: jeilangkung-game-testing
description: Memverifikasi aturan gameplay dan integrasi Unity JeiLangkung melalui test yang relevan serta playtest pemahaman dan ketegangan pemain. Gunakan setelah perubahan perilaku game atau saat diminta mengevaluasi demo.
---

# Pengujian dan playtest JeiLangkung

Baca kontrak fitur dan keputusan terkait sebelum menentukan hasil yang diharapkan. Aturan yang belum diputuskan tidak boleh dijadikan perilaku final melalui test. Gunakan bahasa Indonesia untuk penjelasan dan laporan.

## Pilih bukti sesuai perubahan

| Jenis | Kebutuhan |
|---|---|
| Pemeriksaan dokumen | Konsistensi keputusan, tautan, dan spesifikasi; tidak perlu membuka Unity |
| EditMode | Timer, batas keseimbangan, aturan jawaban, transisi ritual, pemilihan ending |
| PlayMode | Input, lifecycle komponen, coroutine, integrasi scene, UI, audio/cahaya |
| Playtest manual | Pemahaman tujuan, keterbacaan petunjuk, rasa kontrol, ketegangan, pacing |
| Pemeriksaan build | Startup dan alur yang relevan pada target yang diminta |

Tulis test untuk perilaku penting dan regresi yang masuk akal, bukan sekadar membuktikan detail implementasi. Jangan membuat suite baru untuk perubahan teks atau dokumentasi yang tidak mengubah aturan.

## Kasus yang relevan untuk mekanik ritual

Pilih hanya kasus yang terkait perubahan:

- Timer mencapai nol, tidak menjadi negatif, dan hasil akhir dipicu sekali. Periksa pause atau perawatan hanya jika fitur itu ada pada kontrak.
- Batas zona aman dan durasi di luar zona; hasil pada nilai ambang mengikuti aturan yang disepakati.
- Jawaban/logika cerita selesai sebelum animasi dinyatakan selesai; input berulang tidak menggandakan pertanyaan.
- Penutupan dan habisnya lilin pada saat yang sama mengikuti prioritas yang ditetapkan.
- Ending menghentikan interaksi ritual; restart tidak menyisakan event, timer, atau coroutine sesi sebelumnya.
- Acak memakai seed atau sumber gangguan yang dapat dikontrol dalam test. Jangan mengubah randomness game semata-mata agar satu test kebetulan lulus.

Untuk setup assembly dan pemeriksaan editor aktif, baca `../jeilangkung-unity-development/references/unity-workflow.md` dari folder skill ini.

## Menjalankan Unity Test Framework

Jika Editor membuka project, jalankan dari Test Runner bila akses tersedia. Jika batch diperlukan, minta project ditutup dan lanjutkan setelah kondisi terpenuhi. Jangan menjalankan dua Editor pada project yang sama.

Dari root `PPD/`, setelah editor project tertutup:

```powershell
$taskUnity = Join-Path $env:LOCALAPPDATA 'Unity\bin\unity.exe'
$taskFlags = @('--no-banner', '--non-interactive', '--no-pager')
$taskProject = Join-Path (Get-Location) 'JeiLangkung'
$taskResults = Join-Path (Get-Location) 'test-results'
New-Item -ItemType Directory -Path $taskResults -Force | Out-Null
& $taskUnity @taskFlags test $taskProject --mode EditMode --output (Join-Path $taskResults 'editmode.xml')
```

Gunakan `--filter` untuk test terkait jika sesuai; jalankan PlayMode dengan `--mode PlayMode` dan file hasil berbeda ketika perubahan memerlukannya. Periksa exit code serta NUnit XML: jumlah test, hasil, dan pesan kegagalan. Nol test bukan bukti lolos. Bedakan kegagalan import/kompilasi, kegagalan assertion, dan test yang tidak dijalankan. Jangan melemahkan test untuk menutupi bug.

## Playtest yang memberi informasi desain

Tentukan tujuan sesi, bagian demo, dan pertanyaan yang ingin dijawab. Amati apakah pemain menemukan tindakan utama, memahami aturan lilin, mengenali petunjuk, mengetahui penyebab kalah, dan bisa memulai ulang. Catat titik kebingungan serta kejadian yang menyebabkan ketegangan turun; jangan mengklaim pengalaman pemain dari pembacaan kode saja.

Periksa keterbacaan dalam gelap, ukuran teks, volume cue penting, kontrol yang sudah dipilih, dan kemampuan mengenali informasi penting tanpa mengandalkan satu warna atau suara saja. Setelah visual dan target perangkat dipilih, tambahkan pemeriksaan yang sesuai; jangan menetapkan angka FPS, resolusi, atau tingkat kesulitan tanpa dasar.

## Pelaporan

Sebutkan versi/demo yang diperiksa, langkah atau perintah, hasil aktual, dan batas pemeriksaan. Untuk test otomatis, laporkan jumlah lulus/gagal/skipped dan error yang relevan. Untuk playtest, pisahkan pengamatan dari usulan perbaikan. Jika pemeriksaan tertahan oleh lingkungan atau keputusan terbuka, jelaskan apa yang belum terbukti dan bukti berikutnya yang dibutuhkan.
