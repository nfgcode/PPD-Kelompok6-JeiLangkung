---
name: jeilangkung-unity-development
description: Mengimplementasikan dan memperbaiki C#, scene, prefab, data, input, dan build proyek Unity JeiLangkung. Gunakan saat bekerja pada proyek Unity, setelah keputusan desain yang memengaruhi fitur diperiksa.
---

# Pengembangan Unity JeiLangkung

Baca `AGENTS.md` dan keputusan yang berkaitan dengan fitur di `docs/design-decisions.md`. Root Unity adalah `JeiLangkung/`; versi Editor mengikuti `ProjectSettings/ProjectVersion.txt`, saat ini 6000.3.25f1.

## Sebelum implementasi

Periksa file yang sudah ada, manifest package, dan editor aktif. Gunakan [alur Unity](references/unity-workflow.md) saat perlu CLI, setup assembly, atau build. Jangan mengandalkan kondisi proyek awal setelah fitur mulai dibuat.

Jika fitur bergantung pada visual 2D/3D, kamera, mikrofon, multiplayer, atau aturan yang belum diputuskan, tanyakan keputusan itu. Kerjakan logika atau dokumentasi independen yang termasuk permintaan sambil menunggu. Jangan mengubah renderer untuk menjadikan usulan visual sebagai keputusan.

## Organisasi yang disarankan saat kode mulai dibuat

```text
JeiLangkung/Assets/_Project/
  Scripts/
    Runtime/     logika dan penghubung scene per fitur
    Editor/      alat pembuatan dan konfigurasi khusus Editor
  Tests/
    EditMode/    aturan yang tidak memerlukan scene berjalan
    PlayMode/    integrasi yang memerlukan frame dan komponen
  Data/          konfigurasi pertanyaan, ritual, dan kesulitan
  Scenes/        scene yang benar-benar diperlukan
  Prefabs/
  Art/
  Audio/
```

Buat folder hanya ketika digunakan. Pertahankan aset bawaan di `Assets/Settings/`. `Art/` belum menetapkan sprite atau model sebagai pilihan akhir.

## Konvensi dan pemisahan tanggung jawab

- Namespace `JeiLangkung.<Fitur>`; satu class publik per file, nama file mengikuti class.
- PascalCase untuk class, method, property, dan event; camelCase untuk field privat.
- Field Inspector menggunakan `[SerializeField] private`. Data yang perlu di-tuning disimpan sebagai konfigurasi atau ScriptableObject sesuai kebutuhan, bukan angka tersebar di kode.
- Pisahkan timer, aturan keseimbangan, keputusan jawaban, dan transisi ritual dari input, tampilan, audio, serta MonoBehaviour. Pilih interface atau sistem event hanya bila kebutuhan nyata membenarkannya.
- Input memakai Input System yang sudah tersedia. Teks UI memakai TextMeshPro. Periksa action yang ada sebelum menambah atau mengganti binding.
- Cache komponen yang digunakan berulang. Hindari pencarian scene dan alokasi besar pada setiap `Update()`.
- Animasi berurutan dapat memakai coroutine atau `Awaitable` Unity. Jangan memblokir thread utama. Hentikan proses dan langganan event dengan benar ketika sesi berakhir atau objek dinonaktifkan.
- Validasi konfigurasi yang menentukan aturan: durasi, rentang, referensi komponen, dan jumlah resource. Contoh kode lama merupakan referensi, bukan implementasi yang boleh disalin tanpa memeriksa kontrak dan nilai batas.

## Scene dan aset

Gunakan Editor atau script Editor yang terbatas pada aset tujuan. Jangan membuat ulang seluruh scene saat diminta mengubah satu bagian. Operasi setup harus dapat dijalankan ulang tanpa menggandakan objek atau menimpa hasil kerja tim.

Biarkan Unity membuat `.meta`. Pertahankan GUID saat rename atau move. Untuk asset yang sudah ada, periksa pemakai sebelum memindahkan atau menghapusnya. Jika alat untuk menjalankan script Editor belum tersedia, berikan menu dan langkah minimal yang perlu dilakukan pengguna; jangan mengklaim scene sudah dibuat hanya karena script generator sudah ditulis.

Light 2D, Sprite-Lit material, rig sprite, dan renderer 2D digunakan bila fitur atau arah 2D sudah dipilih. Untuk 3D, tentukan pipeline setelah keputusan visual dibuat. Jangan memasang jaringan atau integrasi speech untuk mode yang masih terbuka.

## Verifikasi hasil

Untuk perubahan aturan, gunakan `jeilangkung-game-testing`. Untuk perubahan komponen, verifikasi kompilasi dan integrasi terkait. Untuk visual/audio, periksa hasil dalam game. Build dilakukan bila diperlukan oleh tugas atau milestone; keberhasilan kompilasi saja bukan bukti game dapat dimainkan.
