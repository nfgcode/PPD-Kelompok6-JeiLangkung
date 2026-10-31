using System;
using System.Collections;
using JeiLangkung.Opening;
using JeiLangkung.Ritual;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace JeiLangkung.Tests
{
    public sealed class RitualIntegrationTests
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
            SceneManager.LoadScene(RitualController.SceneName);
            yield return null;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Cleanup()
        {
            Time.timeScale = 1;
            InputSystem.RemoveDevice(keyboard);
            InputSystem.settings.backgroundBehavior = oldBackground;
#if UNITY_EDITOR
            InputSystem.settings.editorInputBehaviorInPlayMode = oldEditorInput;
#endif
            yield return null;
        }

        private static RitualController Controller => Object.FindFirstObjectByType<RitualController>();
        private static RitualFlow Flow => Controller.Flow;

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

        private IEnumerator PressWhile(Func<bool> condition, int limit = 12)
        {
            for (int i = 0; i < limit && condition(); i++) yield return Press(Key.Space);
        }

        /// <summary>Menahan Spasi lewat perangkat keyboard seperti pemain yang memperhatikan bar.</summary>
        private IEnumerator HoldGrip(Func<bool> done, float realSeconds = 60)
        {
            float until = Time.realtimeSinceStartup + realSeconds;
            while (!done() && Time.realtimeSinceStartup < until)
            {
                GripBar grip = Flow.Grip;
                bool hold = grip != null && grip.Zone + grip.ZoneSize / 2 + grip.ZoneVelocity * .25f < grip.Marker;
                InputSystem.QueueStateEvent(keyboard, hold ? new KeyboardState(Key.Space) : new KeyboardState());
                yield return null;
            }
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            Assert.That(done(), Is.True, "Genggaman tidak selesai dalam batas waktu.");
        }

        [UnityTest]
        public IEnumerator KeyboardPlaythroughWithPauseAndReplay()
        {
            Time.timeScale = 2;
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.Warning));
            yield return Press(Key.Space);
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.Playing));
            yield return Press(Key.Space);
            Assert.That(Flow.Step.kind, Is.EqualTo(RitualStepKind.LightCandles));
            for (int i = 0; i < RitualFlow.CandleCount; i++) yield return Press(Key.Space);
            Assert.That(Flow.LitCount, Is.EqualTo(RitualFlow.CandleCount));
            Assert.That(Flow.Candles.Running, Is.True);

            Click("Pause");
            Assert.That(Controller.IsPaused, Is.True);
            float burned = Flow.Candles.Elapsed;
            int step = Flow.StepIndex;
            yield return new WaitForSecondsRealtime(.4f);
            yield return Press(Key.Space);
            Assert.That(Flow.Candles.Elapsed, Is.EqualTo(burned), "Lilin berhenti selama jeda.");
            Assert.That(Flow.StepIndex, Is.EqualTo(step), "Dialog tidak maju selama jeda.");
            int flicker = PlayerPrefs.GetInt(OpeningController.FlickerKey, 0);
            Click("ReduceFlicker");
            Assert.That(PlayerPrefs.GetInt(OpeningController.FlickerKey, 0), Is.EqualTo(1 - flicker), "Redup halus tersimpan.");
            Click("ReduceFlicker");
            Assert.That(PlayerPrefs.GetInt(OpeningController.FlickerKey, 0), Is.EqualTo(flicker));
            Click("Resume");
            Assert.That(Controller.IsPaused, Is.False);

            yield return PressWhile(() => Flow.Step.kind == RitualStepKind.Line);
            Assert.That(Flow.Step.kind, Is.EqualTo(RitualStepKind.Grip));
            yield return HoldGrip(() => Flow.Step.kind != RitualStepKind.Grip);
            for (int question = 0; question < 2; question++)
            {
                yield return PressWhile(() => Flow.Step.kind == RitualStepKind.Line);
                Assert.That(Flow.Step.kind, Is.EqualTo(RitualStepKind.Question));
                yield return Press(Key.Space);
                Assert.That(Flow.Phase, Is.EqualTo(QuestionPhase.Answering));
                yield return HoldGrip(() => Flow.Phase == QuestionPhase.Answered);
                Assert.That(Flow.Answers[question], Is.EqualTo(RitualAnswer.Ya));
                yield return Press(Key.Space);
            }
            Assert.That(Flow.SignAge(RitualSign.PhoneBuzz), Is.GreaterThanOrEqualTo(0));
            yield return PressWhile(() => Flow.Stage == RitualStage.Playing);
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.Complete));
            yield return new WaitForSecondsRealtime(.5f);
            Click("Replay");
            yield return null;
            yield return null;
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.Warning));
            Assert.That(Flow.LitCount, Is.Zero);
        }

        [UnityTest]
        public IEnumerator LastCandleOutShowsEndingOnce()
        {
            yield return Press(Key.Space);
            yield return Press(Key.Space);
            for (int i = 0; i < RitualFlow.CandleCount; i++) yield return Press(Key.Space);
            Flow.Candles.Penalize(Flow.Candles.Total);
            yield return null;
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.CandlesOut));
            yield return Press(Key.Space);
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.CandlesOut), "Input ritual ditolak setelah lilin padam.");
            Controller.ShowPause();
            Assert.That(Controller.IsPaused, Is.False, "Jeda tidak dibuka setelah ritual berakhir.");
            yield return new WaitForSecondsRealtime(2.8f);
            Assert.That(GameObject.Find("Lilin padam"), Is.Not.Null, "Pilihan akhir tampil setelah kejutan.");
            Click("BackToMenu");
            yield return null;
            yield return null;
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(OpeningController.MenuScene));
        }

        [UnityTest]
        public IEnumerator SceneOneEndingOpensRitual()
        {
            SceneManager.LoadScene(OpeningController.StoryScene);
            yield return null;
            yield return null;
            var opening = Object.FindFirstObjectByType<OpeningController>();
            for (int i = 0; i < 3; i++) opening.Advance();
            opening.Inspect(true);
            opening.Inspect(false);
            opening.ShowPhone();
            opening.ReadMessage();
            yield return new WaitUntil(() => opening.Flow.Stage == OpeningStage.FindLight);
            opening.Flashlight();
            for (int i = 0; i < 4; i++) opening.Advance();
            Assert.That(opening.Flow.Stage, Is.EqualTo(OpeningStage.Complete));
            Click("ToRitual");
            yield return null;
            yield return null;
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(RitualController.SceneName));
            Assert.That(Flow.Stage, Is.EqualTo(RitualStage.Warning));
        }
    }
}
