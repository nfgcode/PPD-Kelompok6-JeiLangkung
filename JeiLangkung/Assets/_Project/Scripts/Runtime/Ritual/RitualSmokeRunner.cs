#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JeiLangkung.Ritual
{
    /// <summary>Pemeriksaan Scene 2 khusus development build, aktif hanya melalui argumen --ritual-smoke folder.</summary>
    public sealed class RitualSmokeRunner : MonoBehaviour
    {
        private string output;
        private bool failed;
        private Keyboard keyboard;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, "--ritual-smoke");
            if (index < 0 || index + 1 >= args.Length) return;
            var obj = new GameObject("Ritual smoke verification");
            DontDestroyOnLoad(obj);
            var runner = obj.AddComponent<RitualSmokeRunner>();
            runner.output = args[index + 1];
            Directory.CreateDirectory(runner.output);
            runner.StartCoroutine(runner.Run());
        }

        private static RitualController Controller => FindFirstObjectByType<RitualController>();
        private static RitualFlow Flow => Controller.Flow;

        private void Check(bool value, string message)
        {
            if (value) return;
            failed = true;
            File.AppendAllText(Path.Combine(output, "report.txt"), "FAIL: " + message + "\n");
        }

        private void Click(string name)
        {
            foreach (Button button in FindObjectsByType<Button>(FindObjectsSortMode.None))
                if (button.name == name && button.isActiveAndEnabled && button.interactable) { button.onClick.Invoke(); return; }
            Check(false, "Tombol tidak tersedia: " + name);
        }

        private IEnumerator Press()
        {
            keyboard.MakeCurrent();
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
        }

        private IEnumerator Hold(Func<bool> done, float seconds = 60)
        {
            float until = Time.realtimeSinceStartup + seconds;
            while (!done() && Time.realtimeSinceStartup < until)
            {
                GripBar grip = Flow.Grip;
                bool hold = grip != null && grip.Zone + grip.ZoneSize / 2 + grip.ZoneVelocity * .25f < grip.Marker;
                InputSystem.QueueStateEvent(keyboard, hold ? new KeyboardState(Key.Space) : new KeyboardState());
                yield return null;
            }
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            Check(done(), "Genggaman selesai");
        }

        private IEnumerator Lines()
        {
            for (int i = 0; i < 12 && Flow.Stage == RitualStage.Playing && Flow.Step.kind == RitualStepKind.Line; i++) yield return Press();
        }

        private IEnumerator Capture(string name)
        {
            // Render ke texture agar verifikasi tidak bergantung pada jendela desktop yang tersembunyi.
            Camera camera = Camera.main;
            Canvas canvas = Controller.GetComponentInChildren<Canvas>();
            var target = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            target.Create();
            camera.targetTexture = target;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 1;
            Canvas.ForceUpdateCanvases();
            yield return null;
            Canvas.ForceUpdateCanvases();
            RenderPipeline.SubmitRenderRequest(camera, new UniversalRenderPipeline.SingleCameraRequest { destination = target });
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = target;
            var shot = new Texture2D(target.width, target.height, TextureFormat.RGB24, false);
            shot.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
            shot.Apply();
            RenderTexture.active = previous;
            camera.targetTexture = null;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = null;
            target.Release();
            Destroy(target);
            File.WriteAllBytes(Path.Combine(output, name + ".png"), shot.EncodeToPNG());
            Destroy(shot);
            yield return null;
        }

        private IEnumerator Run()
        {
            File.WriteAllText(Path.Combine(output, "report.txt"), "START\n");
            keyboard = InputSystem.AddDevice<Keyboard>();
            SceneManager.LoadScene(RitualController.SceneName);
            yield return new WaitForSecondsRealtime(.5f);
            Check(Controller != null && Flow.Stage == RitualStage.Warning, "Scene 2 dimulai dari peringatan konten");
            yield return Capture("01-peringatan");
            yield return Press();
            yield return Capture("02-dialog");
            yield return Press();
            Flow.LightCandle(0);
            yield return new WaitForSecondsRealtime(.4f);
            yield return Capture("03-lilin-pertama");
            for (int i = 0; i < 3; i++) yield return Press();
            Check(Flow.Candles.Running && Flow.LitCount == RitualFlow.CandleCount, "Empat lilin menyala dan waktu berjalan");
            yield return Lines();
            Check(Flow.Step.kind == RitualStepKind.Grip, "Mantra berlanjut ke genggaman");
            yield return Hold(() => Flow.Grip != null && Flow.Grip.Progress > .5f, 20);
            yield return Capture("04-genggaman");
            yield return Hold(() => Flow.Step.kind != RitualStepKind.Grip);
            yield return Lines();
            yield return Press();
            yield return Hold(() => Flow.PencilTravel > .4f, 20);
            yield return Capture("05-pensil-bergerak");
            yield return Hold(() => Flow.Phase == QuestionPhase.Answered);
            yield return new WaitForSecondsRealtime(1.4f);
            yield return Capture("06-jawaban-ya");
            Click("Pause");
            Check(Controller.IsPaused, "Jeda terbuka");
            yield return Capture("07-jeda");
            Click("Resume");
            yield return Press();
            yield return Lines();
            yield return Press();
            yield return Hold(() => Flow.Phase == QuestionPhase.Answered);
            yield return Press();
            yield return Lines();
            Check(Flow.Stage == RitualStage.Complete, "Scene 2 selesai");
            yield return new WaitForSecondsRealtime(.6f);
            yield return Capture("08-bersambung");
            RitualController previous = Controller;
            Click("Replay");
            // Tunggu scene yang dimuat ulang, bukan jeda tetap; komputer yang sibuk bisa lebih lambat.
            for (float until = Time.realtimeSinceStartup + 3; (Controller == null || Controller == previous) && Time.realtimeSinceStartup < until;) yield return null;
            yield return null;
            Check(Flow.Stage == RitualStage.Warning && Flow.LitCount == 0, "Main ulang mereset sesi");
            yield return Press();
            yield return Press();
            for (int i = 0; i < RitualFlow.CandleCount; i++) yield return Press();
            Flow.Candles.Penalize(Flow.Candles.Total);
            yield return new WaitForSecondsRealtime(1.5f);
            yield return Capture("09-mata");
            yield return new WaitForSecondsRealtime(1.6f);
            Check(GameObject.Find("Lilin padam") != null, "Pilihan setelah lilin padam");
            yield return Capture("10-lilin-padam");
            Screen.SetResolution(1024, 768, FullScreenMode.Windowed);
            yield return new WaitForSecondsRealtime(.7f);
            yield return Capture("11-lilin-padam-4x3");
            InputSystem.RemoveDevice(keyboard);
            if (!failed) File.AppendAllText(Path.Combine(output, "report.txt"), "PASS: peringatan, dialog, lilin, genggaman, pertanyaan, jeda, bersambung, main ulang, lilin padam, resize.\n");
            Application.Quit(failed ? 1 : 0);
        }
    }
}
#endif
