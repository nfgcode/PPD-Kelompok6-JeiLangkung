using System.IO;
using System.Linq;
using JeiLangkung.Opening;
using TMPro;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;

namespace JeiLangkung.Editor
{
    public static class OpeningSetup
    {
        private const string Root = "Assets/_Project";
        public const string MenuPath = Root + "/Scenes/FirstScreen.unity";
        public const string StoryPath = Root + "/Scenes/Scene1_Kilo15.unity";

        [MenuItem("JeiLangkung/Siapkan pembuka")]
        public static void Setup()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            if (!File.Exists("Assets/TextMesh Pro/Resources/TMP Settings.asset"))
            {
                AssetDatabase.importPackageCompleted += ResourcesImported;
                string package = Directory.GetFiles("Library/PackageCache", "TMP Essential Resources.unitypackage", SearchOption.AllDirectories).First();
                AssetDatabase.ImportPackage(package, false);
                return;
            }
            Directory.CreateDirectory(Root + "/Data");
            Directory.CreateDirectory(Root + "/Scenes");
            AssetDatabase.Refresh();
            TMP_FontAsset font = GetFont("VT323-Regular");
            string path = Root + "/Data/OpeningConfig.asset";
            OpeningConfig config = AssetDatabase.LoadAssetAtPath<OpeningConfig>(path);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<OpeningConfig>();
                AssetDatabase.CreateAsset(config, path);
            }
            FillPixelStyle(config, font);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            config = AssetDatabase.LoadAssetAtPath<OpeningConfig>(path);
            if (config == null) throw new System.Exception("OpeningConfig tidak bisa dibaca sebagai asset.");
            CreateScene(MenuPath, true);
            CreateScene(StoryPath, false);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(MenuPath, true), new EditorBuildSettingsScene(StoryPath, true) }
                .Concat(EditorBuildSettings.scenes.Where(scene => scene.path != MenuPath && scene.path != StoryPath)).ToArray();
            PlayerSettings.productName = "JeiLangkung";
            PlayerSettings.companyName = "Kelompok 6 ITK";
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.runInBackground = true;
            AssetDatabase.SaveAssets();
            Debug.Log("Pembuka siap: FirstScreen dan Scene1_Kilo15. Scene yang sudah ada dipertahankan.");
            if (!Application.isBatchMode) EditorSceneManager.OpenScene(MenuPath);
        }

        private static void ResourcesImported(string package)
        {
            AssetDatabase.importPackageCompleted -= ResourcesImported;
            try
            {
                Setup();
                if (Application.isBatchMode) EditorApplication.Exit(0);
            }
            catch (System.Exception error)
            {
                Debug.LogException(error);
                if (Application.isBatchMode) EditorApplication.Exit(1);
            }
        }

        public static void PrepareBatch()
        {
            bool ready = File.Exists("Assets/TextMesh Pro/Resources/TMP Settings.asset");
            Setup();
            if (ready) EditorApplication.Exit(0);
        }

        /// <summary>Batch: render lapisan piksel menu dan kos ke test-results/opening-preview untuk ditinjau.</summary>
        public static void RenderPreviews()
        {
            Setup();
            OpeningConfig config = AssetDatabase.LoadAssetAtPath<OpeningConfig>(Root + "/Data/OpeningConfig.asset");
            string folder = Path.GetFullPath("../test-results/opening-preview");
            Directory.CreateDirectory(folder);
            var renderer = new OpeningRenderer(config);
            try
            {
                void Kos(string name, OpeningLook look)
                {
                    renderer.RenderKos(1, look);
                    RitualSetup.Save(renderer.Pixels, Path.Combine(folder, name + ".png"));
                }
                renderer.RenderMenu(1, new OpeningLook());
                RitualSetup.Save(renderer.Pixels, Path.Combine(folder, "01-menu.png"));
                Kos("02-kos-raka", new OpeningLook { Lamp = 1, Speaker = "RAKA" });
                Kos("03-kos-dimas-tugas", new OpeningLook { Lamp = 1, Speaker = "DIMAS", Pending = 3 });
                Kos("04-kos-ayu", new OpeningLook { Lamp = 1, Speaker = "AYU" });
                Kos("05-kos-nisa", new OpeningLook { Lamp = 1, Speaker = "NISA" });
                Kos("06-meredup", new OpeningLook { Lamp = .35f, Speaker = "RAKA" });
                Kos("07-gelap", new OpeningLook { Lamp = 0, Speaker = "RAKA" });
                Kos("08-senter", new OpeningLook { Lamp = 0, Flashlight = true, Speaker = "NISA" });
                Kos("09-jeda", new OpeningLook { Lamp = 1, Speaker = "RAKA", Dim = true });
                Debug.Log("PREVIEW_OK: " + folder);
            }
            finally
            {
                renderer.Dispose();
            }
        }

        /// <summary>Mengisi font dan gambar gaya piksel yang masih kosong; isian yang sudah diubah tim dipertahankan.</summary>
        private static void FillPixelStyle(OpeningConfig config, TMP_FontAsset font)
        {
            var data = new SerializedObject(config);
            void Fill(string property, Object value)
            {
                SerializedProperty field = data.FindProperty(property);
                if (field.objectReferenceValue == null) field.objectReferenceValue = value;
            }
            Fill("pixelFont", font);
            Fill("menuArt", PixelArtSetup.MenuArt());
            Fill("kosArt", PixelArtSetup.KosArt());
            SerializedProperty portraits = data.FindProperty("portraits");
            if (portraits.arraySize == 0)
            {
                string[] speakers = { "RAKA", "AYU", "DIMAS", "NISA" };
                portraits.arraySize = speakers.Length;
                for (int i = 0; i < speakers.Length; i++)
                {
                    portraits.GetArrayElementAtIndex(i).FindPropertyRelative("speaker").stringValue = speakers[i];
                    portraits.GetArrayElementAtIndex(i).FindPropertyRelative("image").objectReferenceValue = PixelArtSetup.Portrait(speakers[i]);
                }
            }
            data.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static TMP_FontAsset GetFont(string name)
        {
            string path = Root + "/Art/Fonts/" + name + " SDF.asset";
            TMP_FontAsset asset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            if (asset != null) return asset;
            Font source = AssetDatabase.LoadAssetAtPath<Font>(Root + "/Art/Fonts/" + name + ".ttf");
            asset = TMP_FontAsset.CreateFontAsset(source, 80, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic);
            asset.name = name + " SDF";
            string characters = new string(Enumerable.Range(32, 224).Select(i => (char)i).ToArray()) + "…–—•";
            asset.TryAddCharacters(characters, out string missing);
            asset.atlasPopulationMode = AtlasPopulationMode.Static;
            AssetDatabase.CreateAsset(asset, path);
            foreach (Texture2D texture in asset.atlasTextures) AssetDatabase.AddObjectToAsset(texture, asset);
            AssetDatabase.AddObjectToAsset(asset.material, asset);
            EditorUtility.SetDirty(asset);
            if (!string.IsNullOrEmpty(missing)) Debug.Log("Glyph di luar teks pembuka tidak tersedia pada " + name + ": " + missing);
            return asset;
        }

        private static void CreateScene(string path, bool menu)
        {
            if (File.Exists(path))
            {
                Scene existing = EditorSceneManager.OpenScene(path);
                OpeningController opening = existing.GetRootGameObjects().SelectMany(obj => obj.GetComponentsInChildren<OpeningController>()).FirstOrDefault();
                if (opening == null) return;
                SerializedObject reference = new SerializedObject(opening);
                if (reference.FindProperty("config").objectReferenceValue != null) return;
                // Pergantian scene dapat melepas asset yang belum direferensikan scene.
                reference.FindProperty("config").objectReferenceValue = AssetDatabase.LoadAssetAtPath<OpeningConfig>(Root + "/Data/OpeningConfig.asset");
                reference.ApplyModifiedPropertiesWithoutUndo();
                EditorSceneManager.MarkSceneDirty(existing);
                EditorSceneManager.SaveScene(existing);
                return;
            }
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var camera = new GameObject("Camera", typeof(Camera), typeof(AudioListener));
            camera.tag = "MainCamera";
            camera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            camera.GetComponent<Camera>().backgroundColor = Color.black;
            var controller = new GameObject("Opening", typeof(OpeningController));
            SerializedObject data = new SerializedObject(controller.GetComponent<OpeningController>());
            data.FindProperty("config").objectReferenceValue = AssetDatabase.LoadAssetAtPath<OpeningConfig>(Root + "/Data/OpeningConfig.asset");
            data.FindProperty("menuScreen").boolValue = menu;
            data.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.SaveScene(scene, path);
        }

        public static void BuildWindows()
        {
            Setup();
            RitualSetup.Setup();
            string output = Path.GetFullPath("../Builds/Windows/JeiLangkung.exe");
            Directory.CreateDirectory(Path.GetDirectoryName(output));
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { MenuPath, StoryPath, RitualSetup.ScenePath },
                locationPathName = output,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            });
            if (report.summary.result != BuildResult.Succeeded) throw new System.Exception("Build gagal: " + report.summary.result);
            string licenses = Path.Combine(Path.GetDirectoryName(output), "Licenses");
            Directory.CreateDirectory(licenses);
            foreach (string license in Directory.GetFiles(Root + "/Art/Fonts", "*-OFL.txt"))
                File.Copy(license, Path.Combine(licenses, Path.GetFileName(license)), true);
            string tmpLicense = "Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt";
            if (File.Exists(tmpLicense)) File.Copy(tmpLicense, Path.Combine(licenses, "LiberationSans-OFL.txt"), true);
            string emojiNotice = "Assets/TextMesh Pro/Sprites/EmojiOne Attribution.txt";
            if (File.Exists(emojiNotice)) File.Copy(emojiNotice, Path.Combine(licenses, "EmojiOne Attribution.txt"), true);
            string packageRoot = Directory.GetDirectories("Library/PackageCache", "com.unity.ugui@*").First();
            File.Copy(Path.Combine(packageRoot, "LICENSE.md"), Path.Combine(licenses, "uGUI-LICENSE.md"), true);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(output), "CARA-BERMAIN.txt"),
                "JEILANGKUNG - MENU, SCENE 1, DAN SCENE 2\n\nJalankan JeiLangkung.exe. Semua file pendamping harus tetap berada di folder yang sama.\n\nScene 1: klik tombol untuk memilih. Spasi: lanjut dialog. 1: laptop. 2: catatan. E: HP/senter. Esc: jeda. Setelah Bersambung, pilih Lanjut ke ritual.\n\nScene 2: Spasi atau klik untuk lanjut, menyalakan lilin, dan bertanya. Saat boneka meronta, tahan Spasi atau klik kiri untuk mengangkat genggaman dan lepas untuk menurunkan; jaga ikon boneka di dalam genggaman sampai bar di sebelahnya penuh. Empat lilin terbakar bergiliran; genggaman lepas mempercepat lilin. Lilin terakhir padam berarti kalah.\n\nPengaturan volume ada di menu. Redup halus tersedia pada pause Scene 1 dan Scene 2.\n\nVisual, angka waktu, dan dialog masih sementara. Lisensi font dan resource terdapat di folder Licenses.\n");
            Debug.Log("BUILD_OK: " + output);
        }
    }
}
