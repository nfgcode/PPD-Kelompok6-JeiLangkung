using System.Collections;
using JeiLangkung.Opening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace JeiLangkung.Tests
{
    public sealed class OpeningIntegrationTests
    {
        private Keyboard keyboard;
        private InputSettings.BackgroundBehavior oldBackground;
#if UNITY_EDITOR
        private InputSettings.EditorInputBehaviorInPlayMode oldEditorInput;
#endif
        [UnitySetUp]
        public IEnumerator Setup()
        {
            oldBackground = InputSystem.settings.backgroundBehavior;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            oldEditorInput = InputSystem.settings.editorInputBehaviorInPlayMode;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            keyboard = InputSystem.AddDevice<Keyboard>();
            SceneManager.LoadScene(OpeningController.MenuScene);
            yield return null;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Cleanup()
        {
            InputSystem.RemoveDevice(keyboard);
            InputSystem.settings.backgroundBehavior = oldBackground;
#if UNITY_EDITOR
            InputSystem.settings.editorInputBehaviorInPlayMode = oldEditorInput;
#endif
            yield return null;
        }

        private static OpeningController Controller => Object.FindFirstObjectByType<OpeningController>();
        private static void Click(string name)
        {
            foreach (Button button in Object.FindObjectsByType<Button>(FindObjectsSortMode.None))
                if (button.name == name && button.isActiveAndEnabled && button.interactable)
                { button.onClick.Invoke(); return; }
            Assert.Fail("Tombol tidak tersedia: " + name);
        }
        private IEnumerator Press(Key key)
        {
            keyboard.MakeCurrent();
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
        }

        [UnityTest]
        public IEnumerator MenuSettingsAndKeyboardPlaythroughWithReplay()
        {
            Click("Settings");
            Assert.That(Controller.IsPaused, Is.True);
            Click("CloseSettings");
            Click("Open");
            yield return null;
            yield return null;
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(OpeningController.StoryScene));
            for (int i = 0; i < 3; i++) yield return Press(Key.Space);
            Assert.That(Controller.Flow.Stage, Is.EqualTo(OpeningStage.Assignment));
            Click("Pause");
            yield return Press(Key.Digit1);
            Assert.That(Controller.Flow.LaptopChecked, Is.False, "Input tugas wajib berhenti selama jeda.");
            Click("Resume");
            yield return Press(Key.Digit2);
            yield return Press(Key.Digit1);
            Assert.That(Controller.Flow.Stage, Is.EqualTo(OpeningStage.Notification));
            yield return Press(Key.E);
            Click("ReadMessage");
            Click("Pause");
            yield return new WaitForSecondsRealtime(3.3f);
            Assert.That(Controller.Flow.Stage, Is.EqualTo(OpeningStage.Blackout));
            Click("Resume");
            yield return new WaitForSecondsRealtime(3.3f);
            Assert.That(Controller.Flow.Stage, Is.EqualTo(OpeningStage.FindLight));
            yield return Press(Key.E);
            for (int i = 0; i < 4; i++) yield return Press(Key.Space);
            Assert.That(Controller.Flow.Stage, Is.EqualTo(OpeningStage.Complete));
            Click("Replay");
            yield return null;
            yield return null;
            Assert.That(Controller.Flow.Stage, Is.EqualTo(OpeningStage.Intro));
            Assert.That(Controller.Flow.MessageOpened, Is.False);
            Assert.That(Controller.Flow.LaptopChecked, Is.False);
            Click("Pause");
            Click("PauseMenu");
            yield return null;
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(OpeningController.MenuScene));
        }
    }
}
