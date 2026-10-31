using JeiLangkung.Opening;
using NUnit.Framework;

namespace JeiLangkung.Tests
{
    public sealed class OpeningFlowTests
    {
        private static OpeningFlow Assignment()
        {
            var flow = new OpeningFlow(2, 2);
            flow.Advance(); flow.Advance();
            return flow;
        }

        [Test]
        public void NotificationRequiresTwoDifferentInspections()
        {
            OpeningFlow flow = Assignment();
            Assert.That(flow.InspectAssignment(true), Is.True);
            Assert.That(flow.InspectAssignment(true), Is.False);
            Assert.That(flow.Stage, Is.EqualTo(OpeningStage.Assignment));
            Assert.That(flow.Advance(), Is.False);
            flow.InspectAssignment(false);
            Assert.That(flow.Stage, Is.EqualTo(OpeningStage.Notification));
        }

        [Test]
        public void BlackoutCannotStartUntilMessageOpenedAndOnlyStartsOnce()
        {
            OpeningFlow flow = Assignment();
            flow.InspectAssignment(false); flow.InspectAssignment(true);
            Assert.That(flow.ReadMessage(), Is.False);
            flow.OpenMessage();
            Assert.That(flow.ReadMessage(), Is.True);
            Assert.That(flow.ReadMessage(), Is.False);
            Assert.That(flow.TurnOnFlashlight(), Is.False);
            Assert.That(flow.Stage, Is.EqualTo(OpeningStage.Blackout));
        }

        [Test]
        public void EndingRequiresLightAndDialogueAndCannotAdvanceAgain()
        {
            OpeningFlow flow = Assignment();
            flow.InspectAssignment(true); flow.InspectAssignment(false); flow.OpenMessage(); flow.ReadMessage();
            flow.FinishBlackout();
            Assert.That(flow.Advance(), Is.False);
            Assert.That(flow.TurnOnFlashlight(), Is.True);
            Assert.That(flow.TurnOnFlashlight(), Is.False);
            flow.Advance(); flow.Advance();
            Assert.That(flow.Stage, Is.EqualTo(OpeningStage.Complete));
            Assert.That(flow.Advance(), Is.False);
        }

        [Test]
        public void ResetClearsProgressMessageAndDialogue()
        {
            OpeningFlow flow = Assignment();
            flow.InspectAssignment(true); flow.InspectAssignment(false); flow.OpenMessage(); flow.ReadMessage();
            flow.Reset();
            Assert.That(flow.Stage, Is.EqualTo(OpeningStage.Intro));
            Assert.That(flow.LineIndex, Is.Zero);
            Assert.That(flow.MessageOpened, Is.False);
            Assert.That(flow.LaptopChecked || flow.NotesChecked, Is.False);
        }

        [Test]
        public void EarlyActionsCannotSkipOpening()
        {
            var flow = new OpeningFlow(2, 2);
            Assert.That(flow.InspectAssignment(true), Is.False);
            Assert.That(flow.OpenMessage(), Is.False);
            Assert.That(flow.FinishBlackout(), Is.False);
            Assert.That(flow.TurnOnFlashlight(), Is.False);
            Assert.That(flow.Stage, Is.EqualTo(OpeningStage.Intro));
        }

        [TestCase(0, 2)] [TestCase(2, 0)]
        public void EmptyDialogueRejected(int intro, int after)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new OpeningFlow(intro, after));
        }
    }
}
