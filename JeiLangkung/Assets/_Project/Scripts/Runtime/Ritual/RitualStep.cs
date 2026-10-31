using System;
using UnityEngine;

namespace JeiLangkung.Ritual
{
    public enum RitualStepKind { Line, LightCandles, Grip, Question, Sign }
    public enum RitualAnswer { Ya, Tidak }
    public enum RitualSign { GlassRipple, FlameLean, PhoneBuzz, WindowShadow }

    /// <summary>Satu langkah naskah ritual. Urutan langkah diatur pada RitualConfig.</summary>
    [Serializable]
    public struct RitualStep
    {
        public RitualStepKind kind;
        [Tooltip("Nama pembicara untuk Line dan Question.")]
        public string speaker;
        [TextArea(2, 4)] public string text;
        [Tooltip("Kekuatan rontaan boneka untuk Grip dan Question.")]
        [Range(0, 1)] public float agitation;
        public RitualAnswer answer;
        public RitualSign sign;

        public static RitualStep Line(string speaker, string text) => new RitualStep { kind = RitualStepKind.Line, speaker = speaker, text = text };
        public static RitualStep LightCandles(string text) => new RitualStep { kind = RitualStepKind.LightCandles, text = text };
        public static RitualStep Grip(string text, float agitation) => new RitualStep { kind = RitualStepKind.Grip, text = text, agitation = agitation };
        public static RitualStep Question(string speaker, string text, RitualAnswer answer, float agitation) =>
            new RitualStep { kind = RitualStepKind.Question, speaker = speaker, text = text, answer = answer, agitation = agitation };
        public static RitualStep Sign(RitualSign sign) => new RitualStep { kind = RitualStepKind.Sign, sign = sign };
    }
}
