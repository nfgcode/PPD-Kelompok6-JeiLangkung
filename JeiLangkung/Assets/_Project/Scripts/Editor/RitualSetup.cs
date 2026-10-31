using System.IO;
using System.Linq;
using JeiLangkung.Ritual;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JeiLangkung.Editor
{
    public static class RitualSetup
    {
        public const string ScenePath = "Assets/_Project/Scenes/Scene2_Ritual.unity";
        private const string ConfigPath = "Assets/_Project/Data/RitualConfig.asset";

        [MenuItem("JeiLangkung/Siapkan Scene 2 ritual")]
        public static void Setup()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            TMP_FontAsset font = OpeningSetup.GetFont("VT323-Regular");
            RitualConfig config = AssetDatabase.LoadAssetAtPath<RitualConfig>(ConfigPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<RitualConfig>();
                AssetDatabase.CreateAsset(config, ConfigPath);
            }
            var data = new SerializedObject(config);
            if (data.FindProperty("pixelFont").objectReferenceValue == null)
            {
                data.FindProperty("pixelFont").objectReferenceValue = font;
                data.ApplyModifiedPropertiesWithoutUndo();
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(ConfigPath, ImportAssetOptions.ForceSynchronousImport);
            CreateScene();
            if (EditorBuildSettings.scenes.All(scene => scene.path != ScenePath))
                EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(ScenePath, true)).ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("Scene 2 ritual siap. Scene dan konfigurasi yang sudah ada dipertahankan.");
        }

        /// <summary>Config dimuat setelah scene dibuka; asset yang dimuat sebelum pergantian scene bisa terlepas.</summary>
        private static void CreateScene()
        {
            if (File.Exists(ScenePath))
            {
                Scene existing = EditorSceneManager.OpenScene(ScenePath);
                RitualController current = existing.GetRootGameObjects().SelectMany(obj => obj.GetComponentsInChildren<RitualController>()).FirstOrDefault();
                if (current == null) return;
                var reference = new SerializedObject(current);
                if (reference.FindProperty("config").objectReferenceValue != null) return;
                reference.FindProperty("config").objectReferenceValue = AssetDatabase.LoadAssetAtPath<RitualConfig>(ConfigPath);
                reference.ApplyModifiedPropertiesWithoutUndo();
                EditorSceneManager.MarkSceneDirty(existing);
                EditorSceneManager.SaveScene(existing);
                return;
            }
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RitualConfig config = AssetDatabase.LoadAssetAtPath<RitualConfig>(ConfigPath);
            var camera = new GameObject("Camera", typeof(Camera), typeof(AudioListener));
            camera.tag = "MainCamera";
            camera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            camera.GetComponent<Camera>().backgroundColor = Color.black;
            var controller = new GameObject("Ritual", typeof(RitualController));
            var serialized = new SerializedObject(controller.GetComponent<RitualController>());
            serialized.FindProperty("config").objectReferenceValue = config;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.SaveScene(scene, ScenePath);
        }

        /// <summary>Batch: render lapisan piksel pada momen penting ke test-results/ritual-preview untuk ditinjau.</summary>
        public static void RenderPreviews()
        {
            RitualConfig config = AssetDatabase.LoadAssetAtPath<RitualConfig>(ConfigPath);
            string folder = Path.GetFullPath("../test-results/ritual-preview");
            Directory.CreateDirectory(folder);
            var flow = new RitualFlow(config.Steps, config.Tuning, 7);
            var renderer = new RitualRenderer();
            float time = 0;
            void Run(float seconds, bool bot = true)
            {
                for (float t = 0; t < seconds && !flow.Ended; t += 1 / 30f)
                {
                    GripBar grip = flow.Grip;
                    bool hold = bot && grip != null && grip.Zone + grip.ZoneSize / 2 + grip.ZoneVelocity * .25f < grip.Marker;
                    flow.Tick(1 / 30f, hold);
                    time += 1 / 30f;
                    renderer.Render(flow, time, 1 / 30f, RitualView.Default);
                    flow.ClearEvents();
                }
            }
            void Shot(string name, RitualView view)
            {
                renderer.Render(flow, time, 0, view);
                Save(renderer.Pixels, Path.Combine(folder, name + ".png"));
            }
            void Lines() { while (flow.Stage == RitualStage.Playing && flow.Step.kind == RitualStepKind.Line) { flow.Advance(); Run(.2f); } }
            try
            {
                Shot("01-peringatan", RitualView.Default);
                flow.Advance();
                Run(.6f);
                Shot("02-dialog-ayu", RitualView.Default);
                flow.Advance();
                flow.LightCandle(0);
                Run(.4f);
                Shot("03-lilin-pertama", new RitualView { HoveredCandle = 2, OutSeconds = -1 });
                flow.LightCandle(1); flow.LightCandle(2); flow.LightCandle(3);
                Run(.6f);
                Shot("04-empat-lilin", RitualView.Default);
                Lines();
                Run(2.2f, false);
                Shot("05-genggaman-lepas-kendali", RitualView.Default);
                Run(1.6f);
                Shot("06-genggaman-dijaga", RitualView.Default);
                Run(30);
                Lines();
                flow.Ask();
                Run(3);
                Shot("07-pensil-bergerak", RitualView.Default);
                while (flow.Phase == QuestionPhase.Answering && !flow.Ended) Run(.5f);
                Run(.6f);
                Shot("08-pensil-melingkari", RitualView.Default);
                Run(1);
                flow.Advance();
                Run(1.6f);
                Shot("09-api-condong", RitualView.Default);
                Lines();
                Run(.3f);
                Shot("10-hp-bergetar", RitualView.Default);
                Shot("11-jeda", new RitualView { Paused = true, HoveredCandle = -1, OutSeconds = -1 });
                flow.Ask();
                while (flow.Phase == QuestionPhase.Answering && !flow.Ended) Run(.5f);
                Run(1.6f);
                flow.Advance();
                Run(.9f);
                Shot("12-bayangan-jendela", RitualView.Default);
                Lines();
                Run(.5f);
                Shot("13-selesai", RitualView.Default);

                flow.Reset();
                renderer.Reset();
                flow.Advance();
                flow.Advance();
                for (int i = 0; i < RitualFlow.CandleCount; i++) flow.LightCandle(i);
                flow.Candles.Penalize(config.Tuning.secondsPerCandle * 3.9f);
                Run(.5f);
                Shot("14-lilin-terakhir-redup", RitualView.Default);
                flow.Candles.Penalize(config.Tuning.secondsPerCandle);
                flow.Tick(.01f, false);
                Shot("15-gelap", new RitualView { HoveredCandle = -1, OutSeconds = .6f });
                Shot("16-mata", new RitualView { HoveredCandle = -1, OutSeconds = 1.3f });
                Debug.Log("PREVIEW_OK: " + folder);
            }
            finally
            {
                renderer.Dispose();
            }
        }

        /// <summary>Menyimpan bidang 480 x 270 (baris teratas di indeks 0) sebagai PNG tiga kali lebih besar.</summary>
        internal static void Save(Color32[] pixels, string path)
        {
            const int scale = 3;
            int w = RitualRenderer.Width, h = RitualRenderer.Height;
            var output = new Color32[w * h * scale * scale];
            for (int y = 0; y < h * scale; y++)
                for (int x = 0; x < w * scale; x++)
                    output[y * w * scale + x] = pixels[(h - 1 - y / scale) * w + x / scale];
            var texture = new Texture2D(w * scale, h * scale, TextureFormat.RGBA32, false);
            texture.SetPixels32(output);
            texture.Apply();
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
        }
    }
}
