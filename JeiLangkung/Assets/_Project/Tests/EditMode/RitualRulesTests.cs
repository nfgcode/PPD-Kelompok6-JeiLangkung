using System;
using System.Linq;
using JeiLangkung.Pixel;
using JeiLangkung.Ritual;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace JeiLangkung.Tests
{
    public sealed class RitualRulesTests
    {
        private const float Frame = 1 / 60f;

        private static RitualStep[] Script() => new[]
        {
            RitualStep.Line("Ayu", "Nyalakan lilinnya."),
            RitualStep.LightCandles("Nyalakan keempat lilin."),
            RitualStep.Grip("Boneka meronta.", .5f),
            RitualStep.Question("Raka", "Kamu ada di sini?", RitualAnswer.Ya, .3f),
            RitualStep.Sign(RitualSign.FlameLean),
            RitualStep.Line("Dimas", "Apinya miring.")
        };

        private static RitualTuning Tuning() => new RitualTuning { secondsPerCandle = 50, slipPenaltySeconds = 12 };

        /// <summary>Pemain yang memperhatikan: menahan tombol saat tengah genggaman di bawah rontaan.</summary>
        private static bool Attentive(GripBar grip) => grip.Zone + grip.ZoneSize / 2 + grip.ZoneVelocity * .25f < grip.Marker;

        private static GripResult Play(GripBar grip, Func<GripBar, bool> policy, float limit, out float seconds)
        {
            for (seconds = 0; seconds < limit; seconds += Frame)
            {
                GripResult result = grip.Tick(Frame, policy(grip));
                if (result != GripResult.None) return result;
            }
            return GripResult.None;
        }

        private static void PlayGrip(RitualFlow flow, float limit = 60)
        {
            for (float t = 0; t < limit && flow.Grip != null; t += Frame) flow.Tick(Frame, Attentive(flow.Grip));
            Assert.That(flow.Grip, Is.Null, "Genggaman harus selesai oleh pemain yang memperhatikan.");
        }

        private static RitualFlow FlowAtGrip()
        {
            var flow = new RitualFlow(Script(), Tuning(), 3);
            flow.Advance();
            flow.Advance();
            for (int i = 0; i < RitualFlow.CandleCount; i++) flow.LightCandle(i);
            Assert.That(flow.Step.kind, Is.EqualTo(RitualStepKind.Grip));
            return flow;
        }

        [Test]
        public void CandlesBurnInOrderAndExpireOnce()
        {
            var timer = new CandleTimer(4, 10);
            Assert.That(timer.Tick(5), Is.False);
            Assert.That(timer.Elapsed, Is.Zero, "Lilin belum berjalan sebelum dinyalakan.");
            timer.Start();
            timer.Tick(15);
            Assert.That(timer.Burning, Is.EqualTo(1));
            Assert.That(timer.Wax(0), Is.Zero);
            Assert.That(timer.Wax(1), Is.EqualTo(.5f).Within(.001f));
            Assert.That(timer.Wax(2), Is.EqualTo(1));
            Assert.That(timer.Tick(24), Is.False);
            Assert.That(timer.Tick(2), Is.True);
            Assert.That(timer.Tick(1), Is.False, "Habis hanya dilaporkan sekali.");
            Assert.That(timer.Remaining, Is.Zero);
            Assert.That(timer.Running, Is.False);
        }

        /// <summary>Agitasi tertinggi pada naskah default adalah 0,7. Lama genggaman dicatat di keluaran test untuk penyetelan.</summary>
        [TestCase(.3f)]
        [TestCase(.5f)]
        [TestCase(.7f)]
        public void AttentivePlayerHoldsTheDoll(float agitation)
        {
            var times = new float[20];
            for (int seed = 1; seed <= times.Length; seed++)
            {
                var grip = new GripBar(new GripTuning(), agitation, new Random(seed));
                GripResult result = Play(grip, Attentive, 30, out times[seed - 1]);
                Assert.That(result, Is.EqualTo(GripResult.Held), $"Seed {seed}: berhenti di {grip.Progress:0.00} setelah {times[seed - 1]:0.0} detik.");
            }
            Array.Sort(times);
            TestContext.Out.WriteLine($"Agitasi {agitation}: tercepat {times[0]:0.0} s, median {times[10]:0.0} s, terlama {times[19]:0.0} s");
            Assert.That(times[10], Is.LessThanOrEqualTo(18), "Median genggaman terlalu lama untuk batas lilin.");
        }

        [Test]
        public void IgnoredGripSlips()
        {
            for (int seed = 1; seed <= 20; seed++)
            {
                var grip = new GripBar(new GripTuning(), .5f, new Random(seed));
                Assert.That(Play(grip, _ => false, 15, out _), Is.EqualTo(GripResult.Slipped), $"Seed {seed}");
            }
        }

        [Test]
        public void GripIsReproducibleWithSeed()
        {
            Play(new GripBar(new GripTuning(), .6f, new Random(9)), Attentive, 30, out float first);
            Play(new GripBar(new GripTuning(), .6f, new Random(9)), Attentive, 30, out float second);
            Assert.That(second, Is.EqualTo(first));
        }

        [Test]
        public void GripZoneStaysOnTrack()
        {
            var pattern = new Random(4);
            for (int seed = 1; seed <= 10; seed++)
            {
                var grip = new GripBar(new GripTuning(), .5f, new Random(seed));
                bool hold = false;
                for (int i = 0; i < 3000 && !grip.Finished; i++)
                {
                    if (i % 20 == 0) hold = pattern.NextDouble() < .55;
                    grip.Tick(Frame, hold);
                    Assert.That(grip.Zone, Is.InRange(0f, 1 - grip.ZoneSize), $"Seed {seed}, frame {i}");
                    Assert.That(grip.Marker, Is.InRange(0f, 1f));
                    Assert.That(grip.Progress, Is.InRange(0f, 1f));
                }
            }
        }

        [Test]
        public void CandlesMustAllBeLitAndTimeStartsAtTheFirst()
        {
            var flow = new RitualFlow(Script(), Tuning(), 3);
            Assert.That(flow.Stage, Is.EqualTo(RitualStage.Warning));
            Assert.That(flow.LightCandle(), Is.False);
            flow.Advance();
            Assert.That(flow.LightCandle(), Is.False, "Lilin hanya dinyalakan pada langkahnya.");
            flow.Advance();
            Assert.That(flow.Candles.Running, Is.False);
            Assert.That(flow.LightCandle(2), Is.True);
            Assert.That(flow.Candles.Running, Is.True);
            Assert.That(flow.LightCandle(2), Is.False);
            flow.Tick(1, false);
            Assert.That(flow.Candles.Elapsed, Is.EqualTo(1).Within(.001f));
            flow.LightCandle();
            flow.LightCandle();
            Assert.That(flow.Step.kind, Is.EqualTo(RitualStepKind.LightCandles));
            flow.LightCandle();
            Assert.That(flow.Step.kind, Is.EqualTo(RitualStepKind.Grip));
            Assert.That(flow.Grip, Is.Not.Null);
            Assert.That(flow.OrderOf(2), Is.Zero);
            Assert.That(flow.OrderOf(0), Is.EqualTo(1));
        }

        [Test]
        public void SlipBurnsCandleFasterAndRestartsGrip()
        {
            RitualFlow flow = FlowAtGrip();
            for (float t = 0; t < 20 && flow.Slips == 0; t += Frame) flow.Tick(Frame, false);
            Assert.That(flow.Slips, Is.EqualTo(1));
            Assert.That(flow.Candles.Elapsed, Is.GreaterThan(12));
            Assert.That(flow.Grip, Is.Not.Null);
            Assert.That(flow.Grip.Progress, Is.EqualTo(new GripTuning().startProgress));
            Assert.That(flow.Events.Count(e => e == RitualEvent.Slipped), Is.EqualTo(1));
        }

        [Test]
        public void QuestionReturnsConfiguredAnswerThenSceneCompletes()
        {
            RitualFlow flow = FlowAtGrip();
            PlayGrip(flow);
            Assert.That(flow.Step.kind, Is.EqualTo(RitualStepKind.Question));
            Assert.That(flow.Advance(), Is.False, "Pertanyaan harus diajukan dulu.");
            Assert.That(flow.Ask(), Is.True);
            Assert.That(flow.Ask(), Is.False, "Pertanyaan tidak boleh terulang saat pensil bergerak.");
            PlayGrip(flow);
            Assert.That(flow.Phase, Is.EqualTo(QuestionPhase.Answered));
            Assert.That(flow.Answers, Is.EqualTo(new[] { RitualAnswer.Ya }));
            Assert.That(flow.PencilTravel, Is.EqualTo(1));
            Assert.That(flow.Advance(), Is.True);
            Assert.That(flow.SignAge(RitualSign.FlameLean), Is.Zero);
            Assert.That(flow.Step.speaker, Is.EqualTo("Dimas"));
            flow.Advance();
            Assert.That(flow.Stage, Is.EqualTo(RitualStage.Complete));
            Assert.That(flow.Candles.Running, Is.False);
            float elapsed = flow.Candles.Elapsed;
            flow.Tick(5, true);
            Assert.That(flow.Candles.Elapsed, Is.EqualTo(elapsed));
            Assert.That(flow.Advance(), Is.False);
        }

        [Test]
        public void LastCandleOutEndsOnceAndRejectsInput()
        {
            RitualFlow flow = FlowAtGrip();
            flow.ClearEvents();
            flow.Tick(500, true);
            Assert.That(flow.Stage, Is.EqualTo(RitualStage.CandlesOut));
            Assert.That(flow.Grip, Is.Null);
            Assert.That(flow.Events.Count(e => e == RitualEvent.CandlesOut), Is.EqualTo(1));
            flow.ClearEvents();
            flow.Tick(1, true);
            Assert.That(flow.Events, Is.Empty);
            Assert.That(flow.Advance() || flow.Ask() || flow.LightCandle(), Is.False);
        }

        [Test]
        public void ExternalPenaltyStillEndsTheRitual()
        {
            RitualFlow flow = FlowAtGrip();
            flow.Candles.Penalize(1000);
            flow.Tick(Frame, false);
            Assert.That(flow.Stage, Is.EqualTo(RitualStage.CandlesOut));
        }

        [Test]
        public void ResetStartsAFreshSession()
        {
            RitualFlow flow = FlowAtGrip();
            flow.Tick(30, false);
            flow.Reset();
            Assert.That(flow.Stage, Is.EqualTo(RitualStage.Warning));
            Assert.That(flow.LitCount, Is.Zero);
            Assert.That(flow.Slips, Is.Zero);
            Assert.That(flow.Candles.Elapsed, Is.Zero);
            Assert.That(flow.IsLit(0), Is.False);
            Assert.That(flow.SignAge(RitualSign.FlameLean), Is.EqualTo(-1));
        }

        [Test]
        public void PixelFontMeasuresWordsOnPaper()
        {
            Assert.That(PixelFont.Measure("YA", 2), Is.EqualTo(22));
            Assert.That(PixelFont.Measure("TIDAK", 2), Is.EqualTo(58));
            Assert.That(PixelFont.Measure(""), Is.Zero);
        }

        [Test]
        public void OnlyFlameAndEyesCarryColour()
        {
            var flow = new RitualFlow(Script(), Tuning(), 3);
            var renderer = new RitualRenderer();
            try
            {
                renderer.Render(flow, 0, Frame, RitualView.Default);
                Assert.That(renderer.Pixels.All(p => p.Equals(PixelPalette.Black)), Is.True, "Layar peringatan gelap penuh.");
                flow.Advance();
                flow.Advance();
                for (int i = 0; i < RitualFlow.CandleCount; i++) flow.LightCandle(i);
                renderer.Render(flow, 1, Frame, RitualView.Default);
                var greys = new[] { PixelPalette.Black, PixelPalette.Dark, PixelPalette.Light, PixelPalette.White };
                Color32[] coloured = renderer.Pixels.Where(p => !greys.Contains(p)).Distinct().ToArray();
                Assert.That(coloured, Is.Not.Empty, "Api lilin harus tampil.");
                Assert.That(coloured.All(p => p.r > 200 && p.b < 180), Is.True, "Warna di luar abu-abu hanya boleh api kuning atau mata merah.");
            }
            finally
            {
                renderer.Dispose();
            }
        }
    }
}
