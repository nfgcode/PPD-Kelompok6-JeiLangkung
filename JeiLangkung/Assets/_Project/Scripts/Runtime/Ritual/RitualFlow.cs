using System;
using System.Collections.Generic;

namespace JeiLangkung.Ritual
{
    public enum RitualStage { Warning, Playing, Complete, CandlesOut }
    public enum QuestionPhase { Waiting, Answering, Answered }
    public enum RitualEvent { Advanced, CandleLit, CandleBurnedOut, GripStarted, Held, Slipped, Asked, Answered, Sign, Completed, CandlesOut }

    /// <summary>Aturan Scene 2; tidak bergantung pada UI, scene, atau waktu Unity.</summary>
    public sealed class RitualFlow
    {
        public const int CandleCount = 4;
        private readonly RitualStep[] steps;
        private readonly RitualTuning tuning;
        private readonly int seed;
        private readonly bool[] lit = new bool[CandleCount];
        private readonly int[] litOrder = new int[CandleCount];
        private readonly float[] signAge = new float[Enum.GetValues(typeof(RitualSign)).Length];
        private readonly List<RitualAnswer> answers = new List<RitualAnswer>();
        private readonly List<RitualEvent> events = new List<RitualEvent>();
        private Random random;
        private int burning;

        public RitualStage Stage { get; private set; }
        public int StepIndex { get; private set; }
        public RitualStep Step => steps[Math.Min(StepIndex, steps.Length - 1)];
        public QuestionPhase Phase { get; private set; }
        public CandleTimer Candles { get; private set; }
        /// <summary>Genggaman yang sedang berlangsung, atau null.</summary>
        public GripBar Grip { get; private set; }
        public int LitCount { get; private set; }
        public int Slips { get; private set; }
        public IReadOnlyList<RitualAnswer> Answers => answers;
        public IReadOnlyList<RitualEvent> Events => events;
        public bool Ended => Stage == RitualStage.Complete || Stage == RitualStage.CandlesOut;
        /// <summary>Perjalanan pensil dari titik mulai ke jawaban, 0 sampai 1.</summary>
        public float PencilTravel => Phase == QuestionPhase.Answered ? 1 :
            Grip == null || Step.kind != RitualStepKind.Question ? 0 :
            Math.Max(0, (Grip.Progress - tuning.grip.startProgress) / (1 - tuning.grip.startProgress));

        public RitualFlow(RitualStep[] steps, RitualTuning tuning, int seed)
        {
            if (steps == null || steps.Length == 0) throw new ArgumentException("Naskah ritual kosong.", nameof(steps));
            this.steps = steps;
            this.tuning = tuning ?? throw new ArgumentNullException(nameof(tuning));
            this.seed = seed;
            Reset();
        }

        public void Reset()
        {
            random = new Random(seed);
            Candles = new CandleTimer(CandleCount, tuning.secondsPerCandle);
            Stage = RitualStage.Warning;
            StepIndex = 0;
            Phase = QuestionPhase.Waiting;
            Grip = null;
            LitCount = 0;
            Slips = 0;
            burning = 0;
            Array.Clear(lit, 0, lit.Length);
            for (int i = 0; i < signAge.Length; i++) signAge[i] = -1;
            answers.Clear();
            events.Clear();
        }

        public void ClearEvents() => events.Clear();
        public bool IsLit(int candle) => lit[candle];
        /// <summary>Urutan menyala sebuah lilin, atau -1 bila belum dinyalakan.</summary>
        public int OrderOf(int candle) => Array.IndexOf(litOrder, candle, 0, LitCount);
        /// <summary>Detik sejak tanda horor muncul, atau -1 bila belum muncul.</summary>
        public float SignAge(RitualSign sign) => signAge[(int)sign];

        public bool Advance()
        {
            if (Stage == RitualStage.Warning)
            {
                Stage = RitualStage.Playing;
                Raise(RitualEvent.Advanced);
                EnterStep(0);
                return true;
            }
            if (Stage != RitualStage.Playing) return false;
            bool finishedQuestion = Step.kind == RitualStepKind.Question && Phase == QuestionPhase.Answered;
            if (Step.kind != RitualStepKind.Line && !finishedQuestion) return false;
            Raise(RitualEvent.Advanced);
            EnterStep(StepIndex + 1);
            return true;
        }

        /// <summary>Menyalakan lilin tertentu, atau lilin berikutnya bila index negatif.</summary>
        public bool LightCandle(int index = -1)
        {
            if (Stage != RitualStage.Playing || Step.kind != RitualStepKind.LightCandles) return false;
            if (index < 0) index = Array.IndexOf(lit, false);
            if (index < 0 || index >= CandleCount || lit[index]) return false;
            lit[index] = true;
            litOrder[LitCount++] = index;
            // Waktu mulai berjalan sejak lilin pertama, sesuai storytelling Scene 1.
            if (LitCount == 1) Candles.Start();
            Raise(RitualEvent.CandleLit);
            if (LitCount == CandleCount) EnterStep(StepIndex + 1);
            return true;
        }

        public bool Ask()
        {
            if (Stage != RitualStage.Playing || Step.kind != RitualStepKind.Question || Phase != QuestionPhase.Waiting) return false;
            Phase = QuestionPhase.Answering;
            StartGrip();
            Raise(RitualEvent.Asked);
            return true;
        }

        public void Tick(float seconds, bool holding)
        {
            if (Stage != RitualStage.Playing || seconds <= 0) return;
            for (int i = 0; i < signAge.Length; i++) if (signAge[i] >= 0) signAge[i] += seconds;
            if (Candles.Tick(seconds) || Candles.Expired) { BurnOut(); return; }
            NoteBurnedCandles();
            if (Grip == null) return;
            GripResult result = Grip.Tick(seconds, holding);
            if (result == GripResult.Held)
            {
                Grip = null;
                Raise(RitualEvent.Held);
                if (Step.kind == RitualStepKind.Question)
                {
                    Phase = QuestionPhase.Answered;
                    answers.Add(Step.answer);
                    Raise(RitualEvent.Answered);
                }
                else EnterStep(StepIndex + 1);
            }
            else if (result == GripResult.Slipped)
            {
                Slips++;
                Raise(RitualEvent.Slipped);
                if (Candles.Penalize(tuning.slipPenaltySeconds)) { BurnOut(); return; }
                NoteBurnedCandles();
                StartGrip();
            }
        }

        private void EnterStep(int index)
        {
            // Tanda horor tidak menunggu input; langsung lanjut ke langkah berikutnya.
            while (index < steps.Length && steps[index].kind == RitualStepKind.Sign)
            {
                signAge[(int)steps[index].sign] = 0;
                Raise(RitualEvent.Sign);
                index++;
            }
            Grip = null;
            Phase = QuestionPhase.Waiting;
            if (index >= steps.Length)
            {
                StepIndex = steps.Length - 1;
                Stage = RitualStage.Complete;
                Candles.Stop();
                Raise(RitualEvent.Completed);
                return;
            }
            StepIndex = index;
            if (Step.kind == RitualStepKind.LightCandles && LitCount == CandleCount) EnterStep(index + 1);
            else if (Step.kind == RitualStepKind.Grip) StartGrip();
        }

        private void StartGrip()
        {
            Grip = new GripBar(tuning.grip, Step.agitation, random);
            Raise(RitualEvent.GripStarted);
        }

        private void NoteBurnedCandles()
        {
            while (burning < Candles.Burning) { burning++; Raise(RitualEvent.CandleBurnedOut); }
        }

        private void BurnOut()
        {
            Grip = null;
            Stage = RitualStage.CandlesOut;
            Raise(RitualEvent.CandlesOut);
        }

        private void Raise(RitualEvent value) => events.Add(value);
    }
}
