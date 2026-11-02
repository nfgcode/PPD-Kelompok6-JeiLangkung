# Panduan kerja proyek JeiLangkung

Proyek mata kuliah Pemrograman Permainan Digital, Kelompok 6, ITK Balikpapan. Game mengangkat horor ritual Jelangkung saat pemadaman listrik di Kilo 15.

## Bahasa dan hasil kerja

- Gunakan bahasa Indonesia untuk komunikasi, dokumentasi, dan penjelasan. Pertahankan nama API, identifier, dan istilah teknis yang diperlukan.
- Jangan menambahkan nama atau atribusi asisten, label hasil AI, maupun keterangan bahwa pekerjaan dibuat atas perintah pengguna pada dokumen, metadata penulis, credits, aset, atau pesan commit. Pertahankan identitas anggota tim yang sudah ada; jangan mengarang nama penulis.
- Kerjakan sesuai permintaan. Jangan commit, push, mempublikasikan build, atau mengubah FigJam tanpa permintaan yang mencakup tindakan tersebut.

## Sumber dan keputusan desain

- Baca [konteks pengembangan](docs/game-development.md) saat memulai pekerjaan desain atau implementasi fitur.
- Periksa [keputusan desain](docs/design-decisions.md) sebelum pekerjaan yang bergantung pada visual, kamera, input mantra, mode pemain, atau versi cerita.
- Instruksi terbaru pengguna mengungguli dokumen lama. GDD dan FigJam adalah sumber desain yang perlu diselaraskan, bukan bukti bahwa semua isinya sudah disetujui.
- Arah 2D/3D dan penggunaan mikrofon **belum diputuskan**. Template Unity 2D yang ada tidak mengunci keputusan produk.
- Ajukan pertanyaan singkat jika keputusan terbuka mengubah pekerjaan yang diminta. Lanjutkan bagian lain yang tidak bergantung pada jawaban. Usulan dan nilai sementara harus diberi status yang jelas.

## Peta proyek

- `JeiLangkung/`: root proyek Unity. Jangan menjalankan Unity dengan `PPD/` sebagai root proyek.
- `GDD_JeiLangkung.docx`: lokasi GDD saat audit awal. File tidak ditemukan lagi pada verifikasi akhir; periksa ketersediaan atau minta sumber terbaru bila detail aslinya diperlukan.
- [PRD](PRD.md): ringkasan kebutuhan produk yang dibagikan di GitHub.
- `docs/`, `.agents/`, dan `.github/`: dokumen kerja lokal, diabaikan Git dan tidak ada di clone anggota lain. Jika tidak tersedia, pakai PRD sebagai acuan.
- [Log pengerjaan scene](docs/scene-work-log.md): rencana dan catatan kemajuan per babak. Saat mengerjakan scene, perbarui status dan catatan hanya berdasarkan pekerjaan serta pemeriksaan yang benar-benar dilakukan.
- `.agents/skills/`: skill proyek yang dapat dibaca bersama repositori.
- `.claude/`: panduan lokal sebelumnya, diabaikan Git. Panduan bersama tidak boleh bergantung pada folder ini.
- `Builds/` dan `test-results/`: hasil build dan pengujian lokal, diabaikan Git.
- [README](README.md) dan `tools/setup-git.*`: instalasi untuk anggota tim dan setup Git (LFS, UnityYAMLMerge).
- Branch: `main` stabil, `develop` gabungan, `gamedev-1` dan `gamedev-2` branch kerja tim. Bekerja di branch tim; perubahan masuk ke `develop` lewat pull request, bukan push langsung ke `main` atau `develop`.

## Skill yang digunakan sesuai tugas

| Tugas | Skill |
|---|---|
| Menyusun cerita, aturan permainan, GDD, scope, storyboard, atau kebutuhan aset | [jeilangkung-game-design](.agents/skills/jeilangkung-game-design/SKILL.md) |
| Menulis C#, mengatur scene/prefab/data, debugging Unity, atau build | [jeilangkung-unity-development](.agents/skills/jeilangkung-unity-development/SKILL.md) |
| Memverifikasi logika game, integrasi scene, atau pengalaman pemain | [jeilangkung-game-testing](.agents/skills/jeilangkung-game-testing/SKILL.md) |

Baca skill yang relevan; tidak perlu memuat seluruh skill untuk setiap tugas. Untuk penulisan langsung ke FigJam, gunakan panduan Figma yang tersedia pada sesi. Untuk mengubah DOCX atau membuat gambar, gunakan skill dokumen atau gambar yang tersedia pada sesi bila sesuai.

## Batas teknis

- Unity Editor: **6000.3.25f1**, sesuai `JeiLangkung/ProjectSettings/ProjectVersion.txt`. Jangan mengganti versi atau renderer untuk menyelesaikan konflik desain tanpa keputusan pengguna.
- Periksa `JeiLangkung/Packages/manifest.json` sebelum mengandalkan package. Jangan menambah package untuk fitur yang belum diperlukan.
- Jangan mengubah atau menghapus cache `Library/`, `Temp/`, `Logs/`, `UserSettings/`, maupun file IDE yang dibuat Unity. Log boleh dibaca untuk diagnosis.
- Pertahankan GUID `.meta`. Jangan membuat GUID atau file `.meta` sendiri; biarkan Unity menghasilkannya. Saat memindahkan aset yang sudah ada, pindahkan pasangan `.meta` juga.
- Utamakan Editor atau script Editor untuk scene, prefab, dan asset. Edit YAML langsung hanya untuk perubahan kecil yang dipahami dan dapat diverifikasi.
- Cek editor aktif sebelum operasi batch. Jangan menutup Editor pengguna secara otomatis. Jika project sedang terbuka, gunakan jalur Editor untuk verifikasi atau minta ditutup hanya bila batch diperlukan.

## Verifikasi dan pelaporan

Pilih pemeriksaan sesuai perubahan. Perubahan dokumentasi cukup diperiksa tautan, struktur, konsistensi, dan frontmatter skill. Perubahan logika perlu pengujian perilaku terkait; perubahan visual perlu pemeriksaan dalam game. Laporkan apa yang diperiksa, hasilnya, dan batas verifikasinya. Jangan menyebut build atau test berhasil jika belum dijalankan.
