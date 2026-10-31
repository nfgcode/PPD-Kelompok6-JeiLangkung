using System.Collections;
using JeiLangkung.Pixel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JeiLangkung.Opening
{
    /// <summary>Menu judul dan Scene 1 dalam gaya piksel yang sama dengan Scene 2; aturan urutan ada di OpeningFlow.</summary>
    public sealed class OpeningController : MonoBehaviour
    {
        public const string MenuScene = "FirstScreen";
        public const string StoryScene = "Scene1_Kilo15";
        public const string RitualScene = "Scene2_Ritual";
        public const string VolumeKey = "JeiLangkung.Volume";
        public const string FlickerKey = "JeiLangkung.ReducedFlicker";
        [SerializeField] private OpeningConfig config;
        [SerializeField] private bool menuScreen;
        private OpeningRenderer pixels;
        private PixelUi ui;
        private RectTransform hud;
        private RectTransform popup;
        private TMP_Text electricity;
        private AudioSource ambience;
        private AudioSource effects;
        private AudioClip humClip;
        private AudioClip clickClip;
        private AudioClip notificationClip;
        private Coroutine blackout;
        private bool paused;
        private bool loading;
        private string popupKind;
        private bool reducedFlicker;
        private float volume;
        private float lamp = 1;
        private float time;
        private string assignmentHint;
        public OpeningFlow Flow { get; private set; }
        public bool IsPaused => paused;

        private void Awake()
        {
            if (config == null || !config.IsValid)
            {
                Debug.LogError("Konfigurasi pembuka belum lengkap. Jalankan JeiLangkung/Siapkan pembuka di Editor.");
                enabled = false;
                return;
            }
            Application.targetFrameRate = 60;
            volume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKey, .55f));
            reducedFlicker = PlayerPrefs.GetInt(FlickerKey, 0) == 1;
            pixels = new OpeningRenderer(config);
            ui = new PixelUi(config.PixelFont);
            ui.BuildStage(transform, pixels.Texture, menuScreen ? "Menu" : "Kos");
            BuildAudio();
            if (menuScreen) DrawMenu();
            else
            {
                Flow = new OpeningFlow(config.Intro.Length, config.Aftermath.Length);
                DrawKosFrame();
                ambience.Play();
                RefreshHud();
            }
        }

        private void DrawMenu()
        {
            RectTransform root = ui.Root;
            ui.Text(root, "Dikala Malam, Masa Kelam", 32, 1034, 900, 44, 34, PixelPalette.Light);
            ui.Text(root, "Kilo 15, Balikpapan", 1000, 1034, 888, 44, 34, PixelPalette.Light, TextAlignmentOptions.MidlineRight);
            ui.Text(root, "JeiLangkung", 116, 640, 1000, 160, 156, PixelPalette.White);
            ui.Text(root, "Ada yang menunggu dalam gelap.", 124, 576, 1000, 60, 46, PixelPalette.Light);
            PixelButton start = ui.Button(root, "Open", "Open", 124, 424, 540, 76, () => LoadScene(StoryScene), 46);
            ui.Button(root, "Settings", "Pengaturan", 124, 336, 540, 76, ShowSettings, 46);
            ui.Button(root, "Quit", "Keluar", 124, 248, 540, 76, Quit, 46);
            ui.Text(root, "Panah: pilih   Enter: buka   Esc: pengaturan", 124, 150, 1000, 40, 30, PixelPalette.Light);
            EventSystem.current.SetSelectedGameObject(start.gameObject);
        }

        /// <summary>Bagian kos yang tidak berubah: strip status dan tombol jeda.</summary>
        private void DrawKosFrame()
        {
            RectTransform root = ui.Root;
            ui.Text(root, "21.06 WITA, Kos Km 15", 32, 1034, 900, 44, 34, PixelPalette.Light);
            electricity = ui.Text(root, "", 1420, 1034, 468, 44, 34, PixelPalette.Light, TextAlignmentOptions.MidlineRight);
            ui.Button(root, "Pause", "Jeda (Esc)", 1150, 1034, 250, 44, ShowPause, 32);
        }

        private void Update()
        {
            if (loading || ui == null) return;
            Keyboard keyboard = Keyboard.current;
            Mouse mouse = Mouse.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                if (popup != null) ClosePopup();
                else if (menuScreen) ShowSettings();
                else ShowPause();
            }
            else if (!menuScreen && !paused && popup == null)
            {
                if (keyboard != null)
                {
                    if (keyboard.spaceKey.wasPressedThisFrame) Advance();
                    if (keyboard.digit1Key.wasPressedThisFrame) Inspect(true);
                    if (keyboard.digit2Key.wasPressedThisFrame) Inspect(false);
                    if (keyboard.eKey.wasPressedThisFrame) Interact();
                }
                if (mouse != null && mouse.leftButton.wasPressedThisFrame && !ui.PointerOverButton(mouse.position.ReadValue())) ClickPicture(mouse.position.ReadValue());
            }
            if (!paused) time += Time.deltaTime;
            if (menuScreen) pixels.RenderMenu(time, new OpeningLook { Dim = popup != null, ReducedFlicker = reducedFlicker });
            else pixels.RenderKos(time, Look());
        }

        private OpeningLook Look() => new OpeningLook
        {
            Lamp = lamp,
            Flashlight = Flow.Stage == OpeningStage.Aftermath || Flow.Stage == OpeningStage.Complete,
            Speaker = Speaker(),
            Pending = Flow.Stage == OpeningStage.Assignment ? (Flow.LaptopChecked ? 0 : 1) | (Flow.NotesChecked ? 0 : 2) : 0,
            Dim = popup != null,
            ReducedFlicker = reducedFlicker
        };

        private string Speaker()
        {
            if (Flow.Stage == OpeningStage.Intro) return config.Intro[Flow.LineIndex].speaker;
            if (Flow.Stage == OpeningStage.Aftermath) return config.Aftermath[Flow.LineIndex].speaker;
            return "RAKA";
        }

        private static string DisplayName(string speaker) =>
            string.IsNullOrEmpty(speaker) ? "" : char.ToUpperInvariant(speaker[0]) + speaker.Substring(1).ToLowerInvariant();

        /// <summary>Klik di gambar: benda yang disorot diperiksa, selain itu menjadi tindakan utama tahap ini.</summary>
        private void ClickPicture(Vector2 screen)
        {
            if (!ui.ToPicture(screen, out Vector2 point)) return;
            if (Flow.Stage == OpeningStage.Assignment)
            {
                if (OpeningRenderer.Laptop.Contains(Vector2Int.FloorToInt(point))) Inspect(true);
                else if (OpeningRenderer.Notes.Contains(Vector2Int.FloorToInt(point))) Inspect(false);
            }
            else if (Flow.Stage == OpeningStage.Intro || Flow.Stage == OpeningStage.Aftermath) Advance();
            else Interact();
        }

        private void Interact()
        {
            if (Flow.Stage == OpeningStage.Notification) ShowPhone();
            else if (Flow.Stage == OpeningStage.FindLight) Flashlight();
        }

        private void RefreshHud()
        {
            ClearGroup(ref hud);
            hud = PixelUi.Group("HUD", ui.Root);
            electricity.text = Flow.Stage >= OpeningStage.Blackout ? "Listrik: padam" : "Listrik: menyala";
            string speaker = Speaker(), who = "", what = "", how = "", caption = "Kos, tugas belum selesai", panel = "Percakapan";
            // Isi panel tindakan memakai bingkai OpeningRenderer.ActionPanel: x 1448 sampai 1896, y 24 sampai 448.
            switch (Flow.Stage)
            {
                case OpeningStage.Intro:
                    who = speaker; what = config.Intro[Flow.LineIndex].text; how = "Spasi atau klik: lanjut";
                    ui.Button(hud, "Next", "Lanjut (Spasi)", 1464, 40, 416, 64, Advance);
                    break;
                case OpeningStage.Assignment:
                    panel = "Objek di meja";
                    who = "RAKA"; what = assignmentHint ?? "Cocokkan data laptop dan catatan sebelum mengumpulkan tugas.";
                    how = "Klik benda yang disorot, atau tekan 1 dan 2.";
                    TaskButtons();
                    break;
                case OpeningStage.Notification:
                    caption = "Kos, satu pesan masuk";
                    panel = "Pesan baru";
                    who = "HP"; what = "HP Raka bergetar. Ada kabar dari grup warga kos."; how = "E atau klik: buka HP";
                    ui.Button(hud, "Phone", "Buka HP (E)", 1464, 40, 416, 64, ShowPhone);
                    break;
                case OpeningStage.Blackout:
                    caption = "Kos, lampu meredup";
                    panel = "Listrik padam";
                    what = "Lampu meredup. Dengung listrik berhenti.";
                    break;
                case OpeningStage.FindLight:
                    caption = "Kos, gelap total";
                    panel = "Penerangan";
                    who = "RAKA"; what = "Gelap sekali. Nyalakan senter HP supaya kita bisa melihat satu sama lain."; how = "E atau klik: nyalakan senter";
                    ui.Button(hud, "Flashlight", "Senter HP (E)", 1464, 40, 416, 64, Flashlight);
                    break;
                case OpeningStage.Aftermath:
                    caption = "Kos, cahaya senter";
                    who = speaker; what = config.Aftermath[Flow.LineIndex].text; how = "Spasi atau klik: lanjut";
                    ui.Button(hud, "Next", "Lanjut (Spasi)", 1464, 40, 416, 64, Advance);
                    break;
                case OpeningStage.Complete:
                    caption = "Kos, cahaya senter";
                    panel = "Pilihan";
                    who = "Bersambung"; what = "Empat mahasiswa. Satu kamar. Malam baru saja dimulai.";
                    how = "Panah dan Enter, atau klik";
                    PixelButton next = ui.Button(hud, "ToRitual", "Lanjut ke ritual", 1464, 192, 416, 60, () => LoadScene(RitualScene));
                    ui.Button(hud, "Replay", "Main ulang", 1464, 120, 416, 60, () => LoadScene(StoryScene));
                    ui.Button(hud, "BackToMenu", "Menu judul", 1464, 48, 416, 60, () => LoadScene(MenuScene));
                    // Akhir babak tanpa tombol pintas: fokus diberikan supaya pemain keyboard bisa memilih.
                    StartCoroutine(SelectSoon(next.gameObject));
                    break;
            }
            ui.Text(hud, caption, 24, 986, 1392, 36, 30, PixelPalette.Light);
            ui.Text(hud, DisplayName(speaker), 1448, 938, 448, 38, 34, PixelPalette.White);
            ui.Text(hud, panel, 1464, 392, 416, 40, 32, PixelPalette.Light);
            string name = who == "HP" || who == "Bersambung" ? who : DisplayName(who);
            ui.Text(hud, string.IsNullOrEmpty(name) ? what : $"<color={PixelPalette.LightHex}>{name}</color>   {what}", 44, 72, 1356, 82, 38,
                PixelPalette.White, TextAlignmentOptions.TopLeft);
            ui.Text(hud, how, 44, 30, 1356, 36, 30, PixelPalette.Light);
        }

        /// <summary>Ditunda sedikit supaya tekanan Spasi yang menutup dialog terakhir tidak ikut memilih tombol.</summary>
        private IEnumerator SelectSoon(GameObject target)
        {
            yield return new WaitForSecondsRealtime(.35f);
            if (target != null && popup == null) EventSystem.current.SetSelectedGameObject(target);
        }

        private void TaskButtons()
        {
            Button laptop = ui.Button(hud, "Laptop", Flow.LaptopChecked ? "Laptop: cocok" : "Laptop (1)", 1464, 300, 416, 60, () => Inspect(true));
            Button notes = ui.Button(hud, "Notes", Flow.NotesChecked ? "Catatan: cocok" : "Catatan (2)", 1464, 228, 416, 60, () => Inspect(false));
            laptop.interactable = !Flow.LaptopChecked;
            notes.interactable = !Flow.NotesChecked;
            ui.Text(hud, $"{(Flow.LaptopChecked ? 1 : 0) + (Flow.NotesChecked ? 1 : 0)} dari 2 diperiksa", 1464, 168, 416, 40, 32, PixelPalette.Light);
        }

        public void Advance()
        {
            if (paused || loading || popup != null || Flow == null || !Flow.Advance()) return;
            Click();
            RefreshHud();
        }

        public void Inspect(bool laptop)
        {
            if (paused || loading || popup != null || Flow == null || !Flow.InspectAssignment(laptop)) return;
            assignmentHint = laptop ? "Data laptop sudah cocok. Sekarang periksa catatan Ayu." : "Catatan sudah cocok. Sekarang periksa data laptop.";
            Click();
            if (Flow.Stage == OpeningStage.Notification) effects.PlayOneShot(notificationClip);
            RefreshHud();
        }

        /// <summary>Jendela di atas gambar; gambar piksel di belakangnya diredupkan oleh renderer.</summary>
        private RectTransform BeginPopup(string kind)
        {
            ClearGroup(ref popup);
            popupKind = kind;
            paused = kind != "Phone";
            popup = PixelUi.Group(kind, ui.Root);
            Image blocker = PixelUi.Box(popup, 0, 0, 1920, 1080, Color.clear);
            blocker.raycastTarget = true;
            return popup;
        }

        private RectTransform Panel(string kind, float x, float y, float w, float h)
        {
            RectTransform panel = BeginPopup(kind);
            PixelUi.Box(panel, x - 4, y - 4, w + 8, h + 8, PixelPalette.Light);
            PixelUi.Box(panel, x, y, w, h, PixelPalette.Black);
            return panel;
        }

        /// <summary>Tampilan chat buatan sendiri tanpa merek: layar HP putih keabu-abuan yang dingin.</summary>
        public void ShowPhone()
        {
            if (paused || loading || Flow == null || Flow.Stage != OpeningStage.Notification) return;
            Flow.OpenMessage();
            Click();
            RectTransform phone = BeginPopup("Phone");
            PixelUi.Box(phone, 676, 66, 568, 948, PixelPalette.Light);
            PixelUi.Box(phone, 680, 70, 560, 940, PixelPalette.Black);
            PixelUi.Box(phone, 704, 150, 512, 800, PixelPalette.White);
            PixelUi.Box(phone, 900, 100, 120, 12, PixelPalette.Dark);
            ui.Text(phone, "21.06", 728, 900, 220, 36, 30, PixelPalette.Dark);
            ui.Text(phone, "73%", 980, 900, 212, 36, 30, PixelPalette.Dark, TextAlignmentOptions.MidlineRight);
            ui.Text(phone, "Warga Kos Km 15", 728, 830, 464, 56, 46, PixelPalette.Black);
            ui.Text(phone, "Pak RT, sekarang", 728, 784, 464, 40, 30, PixelPalette.Dark);
            PixelUi.Box(phone, 728, 476, 464, 290, PixelPalette.Light);
            ui.Text(phone, "Info untuk warga: malam ini ada pemadaman listrik bergilir di wilayah Kilo 15.\n\nMohon siapkan penerangan dan simpan pekerjaan masing-masing.",
                748, 492, 424, 258, 32, PixelPalette.Black, TextAlignmentOptions.TopLeft);
            ui.Text(phone, "Pesan diterima", 728, 424, 464, 40, 30, PixelPalette.Dark);
            ui.Button(phone, "ReadMessage", "Simpan HP", 728, 300, 464, 64, ReadMessage);
            ui.Button(phone, "ClosePhone", "Tutup", 728, 224, 464, 64, ClosePopup);
        }

        public void ReadMessage()
        {
            if (loading || paused || Flow == null || !Flow.ReadMessage()) return;
            ClosePopup();
            RefreshHud();
            blackout = StartCoroutine(BlackoutSequence());
        }

        private IEnumerator BlackoutSequence()
        {
            float elapsed = 0;
            while (elapsed < config.BlackoutSeconds)
            {
                if (!paused)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float progress = Mathf.Clamp01(elapsed / config.BlackoutSeconds);
                    // Lampu turun, menyala sebentar, lalu padam; redup halus menghapus lonjakan itu.
                    float darkness = reducedFlicker ? progress : (progress < .48f ? Mathf.Lerp(0, .65f, progress / .48f) : Mathf.Lerp(.76f, 1, (progress - .48f) / .52f));
                    lamp = 1 - darkness;
                    ambience.volume = volume * (1 - progress);
                }
                yield return null;
            }
            ambience.Stop();
            // Nol penuh; sisa terang yang sangat kecil akan didither menjadi kisi titik.
            lamp = 0;
            Flow.FinishBlackout();
            blackout = null;
            RefreshHud();
        }

        public void Flashlight()
        {
            if (paused || loading || popup != null || Flow == null || !Flow.TurnOnFlashlight()) return;
            Click();
            RefreshHud();
        }

        public void ShowPause()
        {
            if (loading || menuScreen || popup != null) return;
            Click();
            RectTransform panel = Panel("Pause", 560, 290, 800, 500);
            ui.Text(panel, "Jeda", 608, 690, 704, 72, 68, PixelPalette.White);
            ui.Text(panel, "Tarik napas. Mereka masih di sana.", 608, 630, 704, 48, 34, PixelPalette.Light);
            PixelButton resume = ui.Button(panel, "Resume", "Lanjut", 608, 530, 704, 64, ClosePopup);
            ui.Button(panel, "PauseSettings", "Pengaturan", 608, 450, 704, 64, ShowSettings);
            ui.Button(panel, "PauseMenu", "Menu judul", 608, 370, 704, 64, () => LoadScene(MenuScene));
            ui.Text(panel, "Esc: kembali bermain", 608, 310, 704, 40, 30, PixelPalette.Light);
            EventSystem.current.SetSelectedGameObject(resume.gameObject);
        }

        public void ShowSettings()
        {
            if (loading) return;
            Click();
            RectTransform panel = Panel("Settings", 490, 190, 940, 700);
            ui.Text(panel, "Pengaturan", 550, 770, 820, 72, 68, PixelPalette.White);
            ui.Text(panel, "Volume suara", 550, 680, 600, 52, 38, PixelPalette.White);
            TMP_Text percent = ui.Text(panel, $"{Mathf.RoundToInt(volume * 100)}%", 1170, 680, 200, 52, 38, PixelPalette.Light, TextAlignmentOptions.MidlineRight);
            Slider slider = ui.Slider(panel, "Volume", 550, 610, 820, 48, volume, value =>
            {
                volume = value;
                effects.volume = value;
                ambience.volume = value * (Flow != null && Flow.Stage >= OpeningStage.Blackout ? 0 : 1);
                PlayerPrefs.SetFloat(VolumeKey, value);
                percent.text = $"{Mathf.RoundToInt(value * 100)}%";
            });
            ui.Button(panel, "ReduceFlicker", reducedFlicker ? "Redup halus: aktif" : "Redup halus: nonaktif", 550, 500, 820, 64, () =>
            {
                reducedFlicker = !reducedFlicker;
                PlayerPrefs.SetInt(FlickerKey, reducedFlicker ? 1 : 0);
                ShowSettings();
            });
            ui.Text(panel, "Spasi: dialog   1 / 2: tugas   E: HP / senter   Esc: jeda\nKlik benda yang disorot untuk memeriksanya.", 550, 340, 820, 120, 32, PixelPalette.Light, TextAlignmentOptions.TopLeft);
            ui.Button(panel, "CloseSettings", "Kembali", 550, 230, 820, 64, ClosePopup);
            EventSystem.current.SetSelectedGameObject(slider.gameObject);
        }

        public void ClosePopup()
        {
            string previous = popupKind;
            ClearGroup(ref popup);
            popupKind = null;
            paused = false;
            PlayerPrefs.Save();
            if (!menuScreen && previous == "Settings") ShowPause();
            else if (menuScreen) EventSystem.current.SetSelectedGameObject(ui.Root.Find("Open").gameObject);
            else EventSystem.current.SetSelectedGameObject(null);
        }

        public void LoadScene(string scene)
        {
            if (loading) return;
            loading = true;
            paused = false;
            if (blackout != null) StopCoroutine(blackout);
            PlayerPrefs.Save();
            SceneManager.LoadScene(scene);
        }

        private static void ClearGroup(ref RectTransform group)
        {
            if (group == null) return;
            group.gameObject.SetActive(false);
            Destroy(group.gameObject);
            group = null;
        }

        private void BuildAudio()
        {
            ambience = gameObject.AddComponent<AudioSource>();
            effects = gameObject.AddComponent<AudioSource>();
            ambience.playOnAwake = effects.playOnAwake = false;
            ambience.loop = true;
            ambience.volume = effects.volume = volume;
            humClip = Tone("Dengung kamar", 2, 60, .045f, true);
            clickClip = Tone("Klik", .06f, 480, .12f, false);
            notificationClip = Tone("Notifikasi", .55f, 880, .14f, false);
            ambience.clip = humClip;
        }

        private static AudioClip Tone(string name, float duration, float frequency, float amplitude, bool loop)
        {
            const int rate = 22050;
            var samples = new float[Mathf.CeilToInt(rate * duration)];
            for (int i = 0; i < samples.Length; i++)
            {
                float t = (float)i / rate;
                float envelope = loop ? 1 : Mathf.Min(1, t * 100) * Mathf.Clamp01((duration - t) * 12);
                if (name == "Notifikasi" && t > .16f && t < .27f) envelope = 0;
                samples[i] = Mathf.Sin(t * frequency * 2 * Mathf.PI) * amplitude * envelope;
            }
            AudioClip clip = AudioClip.Create(name, samples.Length, 1, rate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private void Click() { effects.PlayOneShot(clickClip); }
        private static void Quit()
        {
            PlayerPrefs.Save();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnDestroy()
        {
            if (blackout != null) StopCoroutine(blackout);
            pixels?.Dispose();
            if (humClip != null) Destroy(humClip);
            if (clickClip != null) Destroy(clickClip);
            if (notificationClip != null) Destroy(notificationClip);
        }
    }
}
