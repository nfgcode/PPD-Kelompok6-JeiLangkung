using System;
using JeiLangkung.Opening;
using JeiLangkung.Pixel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace JeiLangkung.Ritual
{
    /// <summary>Menghubungkan aturan ritual ke input, gambar piksel, teks TextMeshPro, audio, dan perpindahan scene.</summary>
    public sealed class RitualController : MonoBehaviour
    {
        public const string SceneName = OpeningController.RitualScene;
        private const string GripHint = "Tahan Spasi atau klik kiri: angkat genggaman. Lepas: turun.";
        [SerializeField] private RitualConfig config;
        private RitualRenderer pixels;
        private RitualAudio sound;
        private PixelUi ui;
        private RectTransform warning, pause, ending;
        private TMP_Text body, hint, endingText, candleLabel;
        private PixelButton flickerButton, pauseButton;
        private GameObject pendingSelection;
        private string shownState, notice;
        private float time, outSeconds = -1, noticeUntil, selectAt;
        private bool paused, loading, screamed, reducedFlicker;
        private int hoveredCandle = -1;
        public RitualFlow Flow { get; private set; }
        public bool IsPaused => paused;

        private void Awake()
        {
            if (config == null || !config.IsValid)
            {
                Debug.LogError("Konfigurasi ritual belum lengkap. Jalankan JeiLangkung/Siapkan Scene 2 ritual di Editor.");
                enabled = false;
                return;
            }
            Application.targetFrameRate = 60;
            reducedFlicker = PlayerPrefs.GetInt(OpeningController.FlickerKey, 0) == 1;
            Flow = new RitualFlow(config.Steps, config.Tuning, Environment.TickCount);
            pixels = new RitualRenderer();
            sound = new RitualAudio(gameObject, PlayerPrefs.GetFloat(OpeningController.VolumeKey, .55f));
            BuildCanvas();
            sound.Start();
            Refresh();
        }

        private void Update()
        {
            if (loading) return;
            Keyboard keyboard = Keyboard.current;
            Mouse mouse = Mouse.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                if (paused) Resume();
                else ShowPause();
            }
            hoveredCandle = CandleUnderPointer(mouse);
            float delta = paused ? 0 : Mathf.Min(Time.deltaTime, .1f);
            if (!paused)
            {
                bool pressed = keyboard != null && (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame);
                bool clicked = mouse != null && mouse.leftButton.wasPressedThisFrame && !ui.PointerOverButton(mouse.position.ReadValue());
                if (clicked && hoveredCandle >= 0) Flow.LightCandle(hoveredCandle);
                else if (pressed || clicked) Primary();
                bool holding = (keyboard != null && keyboard.spaceKey.isPressed) || (mouse != null && mouse.leftButton.isPressed);
                Flow.Tick(delta, holding);
                time += delta;
                if (outSeconds >= 0) outSeconds += delta;
            }
            HandleEvents();
            if (outSeconds >= 1.2f && !screamed)
            {
                screamed = true;
                sound.Scream(reducedFlicker);
            }
            if (outSeconds >= 2.4f && !ending.gameObject.activeSelf) ShowCandlesOut();
            // Fokus tombol akhir ditunda supaya tekanan Spasi yang menutup dialog tidak langsung memilih "Main ulang".
            if (pendingSelection != null && Time.unscaledTime >= selectAt)
            {
                EventSystem.current.SetSelectedGameObject(pendingSelection);
                pendingSelection = null;
            }
            sound.UpdateLoops(Flow, delta);
            var view = new RitualView { Paused = paused, HoveredCandle = hoveredCandle, OutSeconds = outSeconds, ReducedFlicker = reducedFlicker };
            pixels.Render(Flow, time, delta, view);
            Refresh();
        }

        /// <summary>Tindakan utama Spasi/klik sesuai langkah yang sedang berjalan.</summary>
        public void Primary()
        {
            if (paused || loading) return;
            if (Flow.Stage == RitualStage.Warning) { Flow.Advance(); return; }
            if (Flow.Stage != RitualStage.Playing) return;
            switch (Flow.Step.kind)
            {
                case RitualStepKind.Line: Flow.Advance(); break;
                case RitualStepKind.LightCandles: Flow.LightCandle(); break;
                case RitualStepKind.Question:
                    if (Flow.Phase == QuestionPhase.Waiting) Flow.Ask();
                    else if (Flow.Phase == QuestionPhase.Answered) Flow.Advance();
                    break;
            }
        }

        private void HandleEvents()
        {
            foreach (RitualEvent value in Flow.Events)
            {
                sound.Play(value, Flow);
                if (value == RitualEvent.Slipped)
                {
                    notice = "Genggaman lepas. Lilin terbakar lebih cepat.";
                    noticeUntil = time + 2.5f;
                }
                else if (value == RitualEvent.CandlesOut) outSeconds = 0;
                else if (value == RitualEvent.Completed) ShowComplete();
            }
            Flow.ClearEvents();
        }

        private int CandleUnderPointer(Mouse mouse)
        {
            if (mouse == null || paused || Flow.Stage != RitualStage.Playing || Flow.Step.kind != RitualStepKind.LightCandles) return -1;
            if (!ui.ToPicture(mouse.position.ReadValue(), out Vector2 point)) return -1;
            for (int i = 0; i < RitualRenderer.CandleBases.Length; i++)
            {
                Vector2Int b = RitualRenderer.CandleBases[i];
                if (!Flow.IsLit(i) && Mathf.Abs(point.x - b.x) <= 10 && point.y >= b.y - 32 && point.y <= b.y + 6) return i;
            }
            return -1;
        }

        private void Refresh()
        {
            bool noticeActive = notice != null && time < noticeUntil;
            string state = $"{Flow.Stage}/{Flow.StepIndex}/{Flow.Phase}/{Flow.LitCount}/{noticeActive}/{paused}";
            if (state == shownState) return;
            shownState = state;
            warning.gameObject.SetActive(Flow.Stage == RitualStage.Warning);
            candleLabel.gameObject.SetActive(Flow.Stage != RitualStage.Warning);
            pauseButton.gameObject.SetActive(Flow.Stage == RitualStage.Playing);
            RitualStep step = Flow.Step;
            string who = "", what = "", how = "";
            if (Flow.Stage == RitualStage.Playing)
            {
                switch (step.kind)
                {
                    case RitualStepKind.Line:
                        who = step.speaker; what = step.text; how = "Spasi atau klik: lanjut";
                        break;
                    case RitualStepKind.LightCandles:
                        who = $"Lilin {Flow.LitCount} dari {RitualFlow.CandleCount}"; what = step.text;
                        how = "Klik lilin atau tekan Spasi. Waktu berjalan sejak lilin pertama menyala.";
                        break;
                    case RitualStepKind.Grip:
                        who = "Genggaman"; what = step.text; how = GripHint;
                        break;
                    case RitualStepKind.Question:
                        who = step.speaker; what = step.text;
                        how = Flow.Phase == QuestionPhase.Waiting ? "Spasi atau klik: tanyakan" : GripHint;
                        if (Flow.Phase == QuestionPhase.Answered)
                        {
                            who = "Kertas";
                            what = $"Pensil melingkari {(step.answer == RitualAnswer.Ya ? "YA" : "TIDAK")}.";
                            how = "Spasi atau klik: lanjut";
                        }
                        break;
                }
                if (noticeActive) how = notice;
            }
            else if (Flow.Stage == RitualStage.Complete)
            {
                who = config.Chapter; what = config.CompleteText;
            }
            // Nama pembicara sebaris dengan dialog, lebih redup, supaya panel bawah cukup dua baris.
            body.text = string.IsNullOrEmpty(who) ? what : $"<color={PixelPalette.LightHex}>{who}</color>   {what}";
            hint.text = how;
        }

        private void ShowComplete()
        {
            Refresh();
            var actions = PixelUi.Group("Akhir babak", ui.Root, 1480, 8, 400, 124);
            PixelButton replay = ui.Button(actions, "Replay", "Main ulang", 0, 66, 400, 56, Replay);
            ui.Button(actions, "BackToMenu", "Menu judul", 0, 2, 400, 56, BackToMenu);
            SelectLater(replay.gameObject);
        }

        private void ShowCandlesOut()
        {
            ending.gameObject.SetActive(true);
            endingText.text = config.CandlesOutText;
            SelectLater(ending.GetComponentInChildren<PixelButton>().gameObject);
        }

        private void SelectLater(GameObject target)
        {
            pendingSelection = target;
            selectAt = Time.unscaledTime + .35f;
        }

        public void ShowPause()
        {
            if (loading || paused || Flow.Ended || Flow.Stage == RitualStage.Warning) return;
            paused = true;
            sound.Pause(true);
            pause.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(pause.GetComponentInChildren<PixelButton>().gameObject);
        }

        public void Resume()
        {
            if (!paused) return;
            paused = false;
            sound.Pause(false);
            pause.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void ToggleFlicker()
        {
            reducedFlicker = !reducedFlicker;
            PlayerPrefs.SetInt(OpeningController.FlickerKey, reducedFlicker ? 1 : 0);
            PlayerPrefs.Save();
            flickerButton.SetCaption(FlickerCaption);
        }

        private string FlickerCaption => reducedFlicker ? "Redup halus: aktif" : "Redup halus: nonaktif";
        public void Replay() => Load(SceneName);
        public void BackToMenu() => Load(OpeningController.MenuScene);

        private void Load(string scene)
        {
            if (loading) return;
            loading = true;
            PlayerPrefs.Save();
            SceneManager.LoadScene(scene);
        }

        private void BuildCanvas()
        {
            ui = new PixelUi(config.PixelFont);
            ui.BuildStage(transform, pixels.Texture, "Meja ritual");
            RectTransform root = ui.Root;
            ui.Text(root, config.Location, 32, 1034, 1100, 44, 34, PixelPalette.Light);
            candleLabel = ui.Text(root, "Lilin", 1400, 1034, 256, 44, 34, PixelPalette.Light, TextAlignmentOptions.MidlineRight);
            pauseButton = ui.Button(root, "Pause", "Jeda (Esc)", 1150, 1034, 250, 44, ShowPause, 32);
            body = ui.Text(root, "", 48, 46, 1400, 88, 38, PixelPalette.White, TextAlignmentOptions.TopLeft);
            hint = ui.Text(root, "", 48, 6, 1400, 38, 30, PixelPalette.Light);

            warning = PixelUi.Group("Peringatan", root);
            ui.Text(warning, config.Chapter, 240, 650, 1440, 90, 68, PixelPalette.White);
            ui.Text(warning, config.ContentWarning, 240, 330, 1440, 300, 40, PixelPalette.White, TextAlignmentOptions.TopLeft);
            ui.Text(warning, "Spasi atau klik: lanjut", 240, 240, 1440, 50, 30, PixelPalette.Light);

            pause = PixelUi.Group("Jeda", root);
            PixelUi.Box(pause, 556, 276, 808, 528, PixelPalette.Light);
            PixelUi.Box(pause, 560, 280, 800, 520, PixelPalette.Black);
            ui.Text(pause, "Jeda", 608, 700, 700, 72, 68, PixelPalette.White);
            ui.Button(pause, "Resume", "Lanjut", 608, 610, 704, 64, Resume);
            flickerButton = ui.Button(pause, "ReduceFlicker", FlickerCaption, 608, 530, 704, 64, ToggleFlicker);
            ui.Button(pause, "PauseReplay", "Main ulang", 608, 450, 704, 64, Replay);
            ui.Button(pause, "PauseMenu", "Menu judul", 608, 370, 704, 64, BackToMenu);
            ui.Text(pause, "Esc: kembali bermain", 608, 300, 704, 40, 30, PixelPalette.Light);
            pause.gameObject.SetActive(false);

            ending = PixelUi.Group("Lilin padam", root);
            // Teks dan pilihan di bawah siluet; tidak ada yang menimpa mata entitas.
            endingText = ui.Text(ending, "", 240, 330, 1440, 80, 60, PixelPalette.White, TextAlignmentOptions.Center);
            ui.Button(ending, "Replay", "Main ulang", 760, 236, 400, 60, Replay);
            ui.Button(ending, "BackToMenu", "Menu judul", 760, 164, 400, 60, BackToMenu);
            ending.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            pixels?.Dispose();
            sound?.Dispose();
        }
    }
}
