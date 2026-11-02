# JeiLangkung

Game horor ritual Jelangkung saat pemadaman listrik di Kilo 15, Balikpapan. Proyek mata kuliah Pemrograman Permainan Digital, Kelompok 6, ITK Balikpapan.

Prototype saat ini: menu judul, Scene 1 "Kilo 15 Padam", dan Scene 2 "Datang Tak Dijemput" dengan lilin, mantra, bar genggaman boneka, dan jawaban YA/TIDAK. Kebutuhan produk, alur cerita, dan status fitur ada di [PRD.md](PRD.md).

## Isi repositori

| Folder | Isi |
|---|---|
| `JeiLangkung/` | Project Unity. Buka folder ini di Unity Hub, bukan folder repositori. |
| `PRD.md` | Kebutuhan produk dan rencana kerja |
| `tools/` | Skrip setup Git untuk Windows dan macOS |

`Library/`, `Temp/`, `Logs/`, `UserSettings/`, file IDE, `Builds/`, dan `test-results/` tidak ikut repositori. Unity membuat ulang semuanya saat project dibuka.

## Yang perlu dipasang

1. **Unity Hub**, lalu **Unity 6000.3.25f1** (Unity 6.3 LTS) dengan modul *Windows Build Support*. Anggota yang memakai Mac menambahkan *Mac Build Support (Mono)*. Semua anggota wajib memakai versi yang persis sama; jangan menerima tawaran upgrade dari Unity Hub.
2. **Git**. Di Windows pakai Git for Windows (sudah termasuk Git LFS dan Git Credential Manager). Di macOS: `brew install git git-lfs`.
3. Editor kode: VS Code dengan extension Unity, atau Visual Studio/Rider.

## Instalasi

1. Clone repositori dan masuk ke foldernya:

   ```bash
   git clone <url-repositori> JeiLangkung-PPD
   cd JeiLangkung-PPD
   ```

2. Pindah ke branch kerja tim kamu (`gamedev-1` atau `gamedev-2`):

   ```bash
   git switch gamedev-1
   ```

3. Jalankan setup Git sekali. Skrip ini memasang Git LFS untuk repositori dan menyetel UnityYAMLMerge agar scene dan asset lebih mudah digabung.

   Windows (PowerShell):

   ```bash
   powershell -ExecutionPolicy Bypass -File tools\setup-git.ps1
   ```

   macOS:

   ```bash
   bash tools/setup-git.sh
   ```

4. Di Unity Hub pilih **Add > Add project from disk**, lalu pilih folder `JeiLangkung` di dalam repositori. Pembukaan pertama memakan beberapa menit karena Unity membangun `Library/`.
5. Buka `Assets/_Project/Scenes/FirstScreen.unity` lalu tekan Play.
6. Jika scene atau konfigurasi hilang, jalankan menu **JeiLangkung > Siapkan pembuka** dan **JeiLangkung > Siapkan Scene 2 ritual**.

## Alur branch

| Branch | Fungsi |
|---|---|
| `main` | Versi stabil untuk demo dan pengumpulan. Hanya menerima merge dari `develop`. |
| `develop` | Gabungan pekerjaan semua tim. Harus selalu bisa dibuka dan dimainkan. |
| `gamedev-1` | Branch kerja tim game developer 1 |
| `gamedev-2` | Branch kerja tim game developer 2 |

Pembagian anggota dan scene ke `gamedev-1` dan `gamedev-2` ditentukan tim.

Kerja harian di branch tim:

```bash
git switch gamedev-1
git pull
git merge origin/develop
```

Setelah bekerja, commit dan push ke branch tim:

```bash
git add -A
git commit -m "Scene 3: hilangnya Nisa"
git push
```

Saat bagian sudah bisa dimainkan, buat pull request di GitHub dari `gamedev-1` (atau `gamedev-2`) ke `develop`. Setelah pull request digabung, tim lain menjalankan `git merge origin/develop` di branch mereka. Untuk demo, buat pull request dari `develop` ke `main`.

## Aturan agar project Unity tidak bentrok

- Selalu commit file `.meta` bersama asetnya. Memindah atau mengganti nama aset sebaiknya dilakukan di Unity.
- Satu scene dikerjakan satu orang pada satu waktu. Kabari tim sebelum mengubah `FirstScreen`, `Scene1_Kilo15`, atau `Scene2_Ritual`.
- Simpan scene dan tutup Unity sebelum `git pull`, `git merge`, atau pindah branch.
- Teks, angka waktu, dan dialog ada di `Assets/_Project/Data/*.asset`; ubah lewat Inspector agar perubahan kecil dan mudah digabung.
- Jangan mengubah versi Unity atau menambah package tanpa kesepakatan tim.
- Jika terjadi konflik pada scene atau asset, jalankan `git mergetool --tool=unityyamlmerge`, atau hubungi pemilik scene itu.

## Test dan build

Buka **Window > General > Test Runner** di Unity lalu jalankan EditMode dan PlayMode sebelum membuat pull request ke `develop`. Build Windows dibuat lewat **File > Build Profiles** dengan scene `FirstScreen`, `Scene1_Kilo15`, dan `Scene2_Ritual`, atau lewat metode `JeiLangkung.Editor.OpeningSetup.BuildWindows`.

## Dokumen lanjutan

- [PRD](PRD.md): kebutuhan produk, alur cerita, status fitur, dan keputusan terbuka.
- Lisensi font tersimpan bersama fontnya di `JeiLangkung/Assets/_Project/Art/Fonts/*-OFL.txt`.
