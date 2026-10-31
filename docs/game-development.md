# Konteks pengembangan JeiLangkung

Dokumen ini menghubungkan kondisi proyek dengan kebutuhan desain dan implementasi. Keputusan yang belum disepakati dicatat di [design-decisions.md](design-decisions.md). Dokumen ini tidak menggantikan GDD atau mengesahkan seluruh usulan cerita.

## Sumber yang diperiksa

- `GDD_JeiLangkung.docx`, dibaca dari root repositori saat audit awal. Pada verifikasi akhir 11 Oktober 2026, file tidak ditemukan lagi di proyek; ringkasan di sini merupakan hasil pembacaan sebelumnya. Periksa lokasi atau minta sumber terbaru bila perlu mengubah GDD atau memeriksa detail aslinya.
- [FigJam Game Developer, halaman Storyboard](https://www.figma.com/board/AUruJk2evwmw7jQT47g2CQ/Game-Developer?node-id=48-236), dibaca 11 Oktober 2026.
- Aturan dan skill Unity lokal yang sudah ada di `.claude/`.
- `JeiLangkung/ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, dan isi `Assets/`.
- Percakapan tim tentang alternatif awal cerita: empat mahasiswa mengerjakan tugas, notifikasi pemadaman, ruangan terasa luas, lalu perlengkapan ritual muncul sendiri.

## Kondisi proyek saat diperiksa

| Bagian | Temuan |
|---|---|
| Editor | Unity 6000.3.25f1 |
| Template | Universal 2D, URP dengan Renderer 2D |
| Aset | Scene bawaan, konfigurasi renderer, dan Input System |
| Gameplay | Belum ditemukan script C#, prefab game, atau assembly definition di `Assets/` |
| Build Settings | Hanya `Assets/Scenes/SampleScene.unity` |
| Panduan lama | Tiga aturan proyek dan dua skill di `.claude/`; folder tersebut diabaikan Git |
| Git | File proyek yang diperiksa belum dilacak; jangan menganggap sudah tersimpan sebagai commit |

Kondisi di atas adalah hasil pemeriksaan, bukan janji bahwa proyek akan selalu demikian. Periksa ulang bagian yang relevan sebelum bekerja.

Package yang tercatat: URP 17.3.0, Input System 1.20.0, uGUI 2.0.0, Unity Test Framework 1.6.0, 2D Animation 13.0.6, Aseprite 3.0.2, PSD Importer 12.0.2, dan Timeline 1.8.13. Versi aktual mengikuti manifest proyek. Aturan lokal menyebut Windows sebagai target utama dan macOS sebagai tambahan; kemampuan build tiap platform perlu diperiksa saat diperlukan. Jangan menganggap distribusi Steam sudah menjadi pekerjaan yang diminta.

## Perbedaan antar sumber

| Topik | GDD | FigJam dan panduan lokal | Status |
|---|---|---|---|
| Visual | Realistis gelap; jadwal menyebut pemodelan 3D | 2D grayscale, cahaya lilin kuning, mata merah | Terbuka |
| Mantra | Mengetik, mikrofon, atau tombol timing | Ritme tanpa voice pada panduan lokal | Terbuka |
| Pemain | Solo dengan rekan atau co-op | Raka sebagai pemain bersama tiga tokoh | Terbuka; jumlah tokoh tidak berarti multiplayer |
| Respons entitas | Kepribadian acak | Si Penunggu hidup dari harapan dan selalu menjawab YA pada premis | Terbuka; juga perlu menjelaskan jawaban yang bertentangan pada eskalasi |
| Awal ritual | Kelompok memilih mencoba ritual | FigJam: Dimas mengusulkan; alternatif baru: alat muncul sendiri | Terbuka |
| Bentuk jawaban | Coretan atau tulisan bebas | Pensil menunjuk YA/TIDAK, kemudian menulis | Terbuka untuk aturan gameplay |
| Kekalahan | Fase chase/terror | Jumpscare dan bad ending; kamera diam | Terbuka |

Pengguna menetapkan pada 11 Oktober 2026 bahwa arah visual dan mikrofon tetap terbuka. Jangan memilih salah satunya karena template sudah tersedia atau karena lebih mudah dikerjakan.

Pada 12 Oktober 2026, pengguna memilih pencarian aset **2D sementara** untuk menu judul dan layar awal empat mahasiswa di kos. Ini merupakan arah prototype dua layar tersebut; visual akhir dan mikrofon tetap terbuka. Rencana serta hasil pencarian tercatat pada [log scene](scene-work-log.md) dan [daftar aset gratis](free-assets-first-screens.md).

Pada sesi implementasi berikutnya, dibuat kode menu dan Scene 1 pada `Assets/_Project`, dua scene Unity, konfigurasi dialog, ilustrasi geometris sementara, input, suara prosedural, dan pengujian. Kondisi “belum ada gameplay” pada tabel audit di atas merupakan snapshot awal. Untuk kondisi hasil implementasi dan cara menjalankan, lihat [prototype pembuka](playable-opening.md) serta bukti pengujian pada log scene.

## Materi cerita pada FigJam

Bagian berikut merangkum materi yang sudah ada untuk menjaga kesinambungan saat membahas desain. Perlakukan sebagai sumber cerita saat ini, bukan keputusan final atas konflik di atas.

Judul cerita: **Dikala Malam, Masa Kelam**. Empat mahasiswa berada di kamar kos sempit di Km 15, Balikpapan, saat pemadaman. Mereka menanyakan apakah listrik akan menyala malam itu. Jawaban YA menjadi harapan yang terus tertunda. HP berfungsi sebagai penghubung ke luar melalui berita dan chat warga; baterai menurun, sinyal terputus, dan pesan hanya centang satu.

| Tokoh | Peran pada materi cerita | Kaitan gameplay yang diusulkan |
|---|---|---|
| Raka | Tokoh pemain yang memegang boneka | Keseimbangan dan pertanyaan |
| Ayu | Mengetahui mantra dari neneknya | Petunjuk ritual dan penutupan |
| Dimas | Mahasiswa teknik yang skeptis | Puzzle sekring atau senter |
| Nisa | Takut dan mencari sinyal | Hilangnya tokoh, pesan misterius |
| Si Penunggu | Penghuni lama yang tidak dipulangkan dari ritual | Jawaban, gangguan, dan ancaman |

| Babak | Isi pada FigJam |
|---|---|
| 1 Kilo 15 Padam | Tugas kuliah, kabar pemadaman, lampu padam, pengenalan lilin |
| 2 Datang Tak Dijemput | Ritual, pertanyaan awal, harapan listrik menyala, gangguan kecil |
| 3 Harapan Tak Kunjung Usai | Pemadaman diperpanjang, Nisa hilang, pesan dari nomornya, catatan penghuni lama |
| 4 Pulang Tak Diantar | Penutupan sebelum lilin habis, hasil ritual dan ending |

Ending pada FigJam: lilin padam menghasilkan teror; salah menutup ritual menghasilkan teror; penutupan berhasil saat Subuh memberi kelegaan singkat sebelum kabar pemadaman berikutnya. Kertas kembali menunjuk YA. Nasib Nisa dan hubungan ending dengan Malam 2 belum dirinci.

Materi visual FigJam: grayscale, kuning api lilin, merah mata entitas, dan cahaya HP putih dingin. Referensi yang tercatat meliputi FNAF untuk kamera, game memancing untuk keseimbangan, task Among Us untuk puzzle, serta manga horor untuk suasana. Gunakan referensi untuk membahas fungsi dan rasa permainan, tanpa menganggap aset atau sistemnya boleh disalin.

## Alternatif awal cerita yang sedang dibahas

Empat mahasiswa sedang mengerjakan tugas di kos kecil. Notifikasi chat mengabarkan pemadaman bergilir; beberapa detik kemudian lampu mati. Saat mencari penerangan, ruangan terasa kosong dan lebih luas. Lilin menyala sendiri, menampakkan kertas YA/TIDAK, pensil, dan boneka. Tulisan darah memerintahkan pemain memutar pensil sebelum lilin habis. Pesan lanjutan menyebut empat lilin.

Alternatif ini belum menggantikan alur FigJam. Sebelum dipakai, jelaskan alasan tokoh ikut bermain, usaha mereka keluar, apa yang dicapai dengan memutar pensil, kapan timer dimulai, dan apakah empat lilin merupakan satu batas waktu bersama atau sumber daya terpisah. Perubahan ruangan perlu diterjemahkan menjadi pengalaman kamera, suara, atau interaksi setelah bentuk game diputuskan.

## Batas lingkup yang perlu ditentukan

Belum ada target durasi demo, tenggat final, spesifikasi perangkat minimum, atau pembagian tugas tim yang terkonfirmasi dalam pengecekan ini. Estimasi tiga bulan pada GDD adalah rencana dokumen, bukan tenggat kalender yang boleh ditetapkan otomatis.

Saat menyusun rencana, bedakan kebutuhan minimum demo dari tambahan seperti puzzle, baterai HP, variasi entitas, beberapa malam, atau multiplayer. Mulai dari satu alur yang dapat dimainkan: pemicu ritual, tindakan utama, umpan balik, tekanan waktu, dan hasil akhir. Daftar ini adalah cara menentukan cakupan; mekanik final tetap mengikuti keputusan tim.

## Visual audio dan produksi aset

Brief aset perlu mencatat fungsi dalam game, scene atau peristiwa pemakai, ukuran atau kebutuhan import, gaya yang sudah dipilih, variasi/animasi, dan hak penggunaan. Gunakan aset sementara bila gaya belum diputuskan dan catat statusnya.

Untuk horor, hubungkan bunyi dengan tindakan atau perubahan keadaan: listrik padam, pensil bergerak, lilin melemah, notifikasi, dan penutupan. Tentukan petunjuk visual yang menyertai informasi penting bila audio dimatikan. Jangan menentukan pipeline sprite 2D, model 3D, rekaman mikrofon, atau audio spasial sebagai kebutuhan final sebelum keputusan terkait dibuat.

Materi lama mengusulkan chat dan instansi fiktif serta pesan SDG 7, 11, dan 13 melalui lingkungan. Relevansi setiap SDG perlu dijelaskan dalam GDD; pemadaman saja tidak membuktikan klaim tentang kebijakan nyata atau perubahan iklim. Unsur ritual ditulis sebagai aturan fiksi permainan. Konten, credits anggota tim, dan lisensi aset diperiksa sesuai kebutuhan hasil akhir.

## Susunan kerja yang disarankan

Gunakan [log pengerjaan scene](scene-work-log.md) untuk memilih pekerjaan berikutnya dan mencatat hasil per babak.

1. Putuskan hal yang memengaruhi pekerjaan berikutnya; catat di daftar keputusan.
2. Susun aturan mekanik dan kriteria keberhasilannya dalam bahasa Indonesia.
3. Buat prototype dengan aset sementara untuk membuktikan tindakan utama terasa jelas dan menegangkan.
4. Satukan satu bagian cerita, gameplay, visual, dan audio menjadi cuplikan demo utuh.
5. Uji pemahaman pemain, kesulitan, ending, restart, dan build pada perangkat target.

Setiap tahap menghasilkan sesuatu yang dapat dinilai. Tambahkan fitur hanya jika membantu tujuan demo yang sudah disepakati.
