#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace JeiLangkung.Opening
{
    /// <summary>Pemeriksaan otomatis khusus development build, aktif hanya melalui argumen eksplisit.</summary>
    public sealed class OpeningSmokeRunner : MonoBehaviour
    {
        private string output;
        private bool failed;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, "--smoke-test");
            if (index < 0 || index + 1 >= args.Length) return;
            var obj = new GameObject("Opening smoke verification");
            DontDestroyOnLoad(obj);
            var runner = obj.AddComponent<OpeningSmokeRunner>();
            runner.output = args[index + 1];
            Directory.CreateDirectory(runner.output);
            runner.StartCoroutine(runner.Run());
        }
        private static OpeningController Controller => FindFirstObjectByType<OpeningController>();
        private void Click(string name)
        {
            foreach (Button button in FindObjectsByType<Button>(FindObjectsSortMode.None))
                if (button.name == name && button.isActiveAndEnabled && button.interactable)
                { button.onClick.Invoke(); return; }
            Fail("Tombol tidak tersedia: " + name);
        }
        private void Check(bool value, string message) { if (!value) Fail(message); }
        private void Fail(string message)
        {
            failed = true;
            File.AppendAllText(Path.Combine(output, "report.txt"), "FAIL: " + message + "\n");
            Application.Quit(1);
        }
        private IEnumerator Capture(string name)
        {
            // Render ke texture agar verifikasi tidak bergantung pada jendela desktop yang tersembunyi.
            Camera camera = Camera.main;
            Canvas canvas = FindFirstObjectByType<Canvas>();
            var target = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            target.Create();
            camera.targetTexture = target;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 1;
            Canvas.ForceUpdateCanvases();
            yield return null;
            yield return null;
            Canvas.ForceUpdateCanvases();
            RenderPipeline.SubmitRenderRequest(camera, new UniversalRenderPipeline.SingleCameraRequest { destination = target });
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = target;
            var screenshot = new Texture2D(target.width, target.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
            screenshot.Apply();
            RenderTexture.active = previous;
            camera.targetTexture = null;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = null;
            target.Release();
            Destroy(target);
            Color32[] pixels = screenshot.GetPixels32();
            long brightness = 0;
            for (int i = 0; i < pixels.Length; i += 64) brightness += pixels[i].r + pixels[i].g + pixels[i].b;
            Check(brightness > pixels.Length / 64, "Frame kosong: " + name);
            File.WriteAllBytes(Path.Combine(output, name + ".png"), screenshot.EncodeToPNG());
            Destroy(screenshot);
            yield return null;
        }
        private IEnumerator Run()
        {
            File.WriteAllText(Path.Combine(output, "report.txt"), "START\n");
            yield return new WaitForSecondsRealtime(.5f);
            Check(SceneManager.GetActiveScene().name == OpeningController.MenuScene, "Startup menu");
            yield return Capture("01-menu");
            Click("Settings");
            yield return Capture("02-settings");
            Click("CloseSettings");
            Click("Open");
            yield return new WaitForSecondsRealtime(.4f);
            yield return Capture("03-kos");
            for (int i = 0; i < 3; i++) { Click("Next"); yield return null; }
            Click("Laptop"); yield return null;
            Click("Notes"); yield return null;
            Click("Phone");
            yield return Capture("04-phone");
            Click("ReadMessage"); yield return null;
            Click("Pause");
            yield return new WaitForSecondsRealtime(3.3f);
            Check(Controller.Flow.Stage == OpeningStage.Blackout, "Pause menghentikan pemadaman");
            Click("Resume");
            yield return new WaitForSecondsRealtime(3.5f);
            Check(Controller.Flow.Stage == OpeningStage.FindLight, "Pemadaman mencapai gelap");
            yield return Capture("05-blackout");
            Click("Flashlight"); yield return null;
            yield return Capture("06-flashlight");
            for (int i = 0; i < 4; i++) { Click("Next"); yield return null; }
            Check(Controller.Flow.Stage == OpeningStage.Complete, "Scene 1 selesai");
            yield return Capture("07-complete");
            Click("Replay"); yield return null; yield return null;
            Check(Controller.Flow.Stage == OpeningStage.Intro && !Controller.Flow.MessageOpened, "Main ulang mereset sesi");
            Click("Pause"); Click("PauseMenu"); yield return null; yield return null;
            Check(SceneManager.GetActiveScene().name == OpeningController.MenuScene, "Kembali ke menu");
            Screen.SetResolution(1024, 768, FullScreenMode.Windowed);
            yield return new WaitForSecondsRealtime(.7f);
            yield return Capture("08-menu-4x3");
            if (!failed) File.AppendAllText(Path.Combine(output, "report.txt"), "PASS: menu, settings, assignment, notification, pause, blackout, flashlight, ending, replay, return menu, resize.\n");
            Application.Quit(failed ? 1 : 0);
        }
    }
}
#endif
