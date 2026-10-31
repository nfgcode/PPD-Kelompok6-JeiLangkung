# Alur Unity dan CLI lokal

Sintaks berikut diperiksa melalui bantuan CLI yang terpasang pada 11 Oktober 2026. Periksa `--help` bila versi tool berubah. Jangan menganggap Editor atau modul target selalu terpasang pada komputer anggota tim lain.

## Root dan editor aktif

Jalankan dari root `PPD/`:

```powershell
$taskUnity = Join-Path $env:LOCALAPPDATA 'Unity\bin\unity.exe'
$taskFlags = @('--no-banner', '--non-interactive', '--no-pager')
$taskProject = Join-Path (Get-Location) 'JeiLangkung'
if (-not (Test-Path -LiteralPath $taskUnity)) { throw 'Unity CLI tidak ditemukan pada lokasi lokal ini.' }
if (-not (Test-Path -LiteralPath (Join-Path $taskProject 'ProjectSettings\ProjectVersion.txt'))) { throw 'Root proyek Unity tidak ditemukan.' }
& $taskUnity @taskFlags editors running
```

Bandingkan path project pada keluaran dengan `$taskProject`. Jika project terbuka, perubahan C# masih dapat dilakukan. Untuk verifikasi, gunakan Console/Test Runner di Editor. Operasi build/test CLI membuka batch Editor dan tidak boleh menggunakan project yang sama secara bersamaan. Jangan menutup Editor tanpa permintaan; minta ditutup jika batch memang dibutuhkan.

## Assembly dan setup

Saat kode runtime dan test mulai dibuat, gunakan assembly `JeiLangkung.Runtime`, assembly khusus Editor `JeiLangkung.Editor`, serta assembly test EditMode/PlayMode sesuai kebutuhan. Editor assembly hanya untuk platform Editor. Tambahkan referensi package yang benar-benar dipakai, misalnya `Unity.InputSystem`, `Unity.TextMeshPro`, atau `Unity.RenderPipelines.Universal.Runtime` untuk Light 2D. Periksa nama assembly pada package terpasang atau Inspector; jangan menebak dari nama package.

Buat test assembly melalui `Create > Testing > Tests Assembly Folder` atau konfigurasi yang setara. Hubungkan assembly runtime. Test yang dijalankan di Editor dan test dalam player mempunyai batas referensi berbeda; jangan menambahkan `UnityEditor` pada assembly yang perlu berjalan dalam player.

Untuk pembuatan aset otomatis, script di `Scripts/Editor/` dapat menyediakan menu `JeiLangkung/...`. Periksa aset yang sudah ada sebelum membuat baru. Setelah setup dijalankan, verifikasi scene/prefab dan referensinya.

## Build lokal

Setelah editor untuk project ini tertutup, gunakan target dan scene yang disepakati. Pastikan scene game terdaftar pada Build Profiles; snapshot awal hanya memuat SampleScene.

```powershell
$taskBuild = Join-Path (Get-Location) 'Builds\Windows\JeiLangkung.exe'
& $taskUnity @taskFlags build $taskProject --target StandaloneWindows64 --output-path $taskBuild
```

Untuk macOS, ganti target ke `StandaloneOSX` dan output ke `Builds\macOS\JeiLangkung.app` setelah modul dan kebutuhan platform diperiksa. Membuat build macOS di Windows tidak membuktikan perilaku runtime di macOS.

CLI mempunyai pemeriksaan perubahan Git yang belum di-commit. Jika pengguna meminta build lokal dari perubahan saat ini dan CLI menolaknya karena guard tersebut, tambahkan `--allow-dirty-build` dan jelaskan bahwa build berasal dari working tree. Jangan commit otomatis untuk melewati guard. Jangan memakai `--allow-install` atau menerima lisensi untuk mengunduh Editor/modul tanpa permintaan yang mencakup instalasi tersebut.

Baca exit code dan log build. Cek file hasil, lalu lakukan pemeriksaan startup/gameplay yang dibutuhkan. Jangan melaporkan build berhasil hanya karena folder output ada.

## Log dan diagnosis

Console Editor dan `%LOCALAPPDATA%\Unity\Editor\Editor.log` dapat dibaca untuk error kompilasi. Bedakan log terbaru untuk perubahan yang sedang diperiksa dari error sesi lama. CLI menyebutkan lokasi log build/test; baca keluaran dan hasil akhir sebelum menentukan perbaikan.
