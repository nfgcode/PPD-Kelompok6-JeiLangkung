using System;
using UnityEngine;

namespace JeiLangkung.Ritual
{
    /// <summary>Suara Scene 2 yang dibuat prosedural; belum ada sampel audio pihak ketiga.</summary>
    internal sealed class RitualAudio : IDisposable
    {
        private const int Rate = 22050;
        private readonly AudioSource ambience, strain, scratch, effects;
        private readonly AudioClip crickets, strainClip, scratchClip, strike, slip, thud, buzz, puff, whoosh, creak, ripple, scream, softScream, click;
        private float volume, strainLevel, scratchLevel;

        public RitualAudio(GameObject owner, float volume)
        {
            ambience = Source(owner, true);
            strain = Source(owner, true);
            scratch = Source(owner, true);
            effects = Source(owner, false);
            var random = new System.Random(15);
            crickets = Clip("Jangkrik", 3.2f, t => Chirp(t, 4400, .2f, .32f, .44f, 1.5f, 1.62f, 1.74f) + Chirp(t, 3900, .9f, 1f, 2.45f, 2.55f) * .7f, random, .012f);
            strainClip = Clip("Boneka berat", 1, t => (float)(Math.Sin(t * 52 * Math.PI * 2) + .4 * Math.Sin(t * 104 * Math.PI * 2)) * .16f, random, .05f);
            scratchClip = Clip("Goresan pensil", .8f, t => 0, random, .1f, t => Math.Abs((float)Math.Sin(t * 7 * Math.PI * 2)), true);
            strike = Clip("Korek api", .5f, t => 0, random, .3f, t => Math.Min(1, t * 200) * (float)Math.Exp(-t * 8), true);
            slip = Clip("Genggaman lepas", .6f, t => (float)(Math.Sin(t * 90 * Math.PI * 2) * Math.Exp(-t * 7)) * .4f, random, .25f, t => (float)Math.Exp(-t * 12));
            thud = Clip("Jawaban", .35f, t => (float)(Math.Sin(t * 70 * Math.PI * 2) * Math.Exp(-t * 10)) * .35f, random, .05f, t => (float)Math.Exp(-t * 40));
            buzz = Clip("HP bergetar", 2.4f, t => t % .6f < .4f ? Math.Sign(Math.Sin(t * 165 * Math.PI * 2)) * .09f : 0, random, 0);
            puff = Clip("Lilin padam", .4f, t => 0, random, .14f, t => (float)Math.Exp(-t * 9), false, .08f);
            whoosh = Clip("Api condong", 1.6f, t => 0, random, .08f, t => (float)Math.Sin(Math.PI * t / 1.6f), false, .03f);
            creak = Clip("Bayangan", .9f, t => (float)(Math.Sin(t * (300 - t * 130) * Math.PI * 2) * (1 + Math.Sin(t * 40)) * .5) * .05f * (float)Math.Sin(Math.PI * t / .9f), random, 0);
            ripple = Clip("Air bergetar", .7f, t => (float)(Math.Sin(t * 1240 * Math.PI * 2) * Math.Exp(-t * 9) + Math.Sin(t * 1240 * Math.PI * 2) * Math.Exp(-(t - .3) * 9) * (t > .3 ? 1 : 0)) * .05f, random, 0);
            scream = Clip("Kejutan", 1.3f, t => (float)(Math.Sin(t * Math.PI * 2 * (220 + 60 * Math.Sin(t * 37))) * Math.Exp(-t * 1.5)) * .4f, random, .45f, t => (float)Math.Exp(-t * 1.8));
            softScream = Clip("Kejutan halus", 1.3f, t => (float)(Math.Sin(t * Math.PI * 2 * 110) * Math.Exp(-t * 2)) * .2f, random, .08f, t => (float)Math.Exp(-t * 3), false, .1f);
            click = Clip("Klik", .06f, t => (float)Math.Sin(t * 480 * Math.PI * 2) * .1f * Math.Min(1, (.06f - t) * 40), random, 0);
            ambience.clip = crickets;
            strain.clip = strainClip;
            scratch.clip = scratchClip;
            SetVolume(volume);
        }

        public void Start()
        {
            ambience.Play();
            strain.volume = scratch.volume = 0;
            strain.Play();
            scratch.Play();
        }

        public void SetVolume(float value)
        {
            volume = Mathf.Clamp01(value);
            effects.volume = volume;
            ambience.volume = volume * .8f;
        }

        public void Pause(bool paused)
        {
            foreach (AudioSource source in new[] { ambience, strain, scratch, effects })
                if (paused) source.Pause(); else source.UnPause();
        }

        public void Play(RitualEvent value, RitualFlow flow)
        {
            switch (value)
            {
                case RitualEvent.Advanced:
                case RitualEvent.Asked: effects.PlayOneShot(click); break;
                case RitualEvent.CandleLit: effects.PlayOneShot(strike); break;
                case RitualEvent.CandleBurnedOut: effects.PlayOneShot(puff); break;
                case RitualEvent.Held: effects.PlayOneShot(thud, .5f); break;
                case RitualEvent.Slipped: effects.PlayOneShot(slip); break;
                case RitualEvent.Answered: effects.PlayOneShot(thud); break;
                case RitualEvent.CandlesOut:
                    effects.PlayOneShot(puff);
                    ambience.Stop();
                    break;
                case RitualEvent.Sign:
                    if (flow.SignAge(RitualSign.GlassRipple) == 0) effects.PlayOneShot(ripple);
                    if (flow.SignAge(RitualSign.FlameLean) == 0) effects.PlayOneShot(whoosh);
                    if (flow.SignAge(RitualSign.PhoneBuzz) == 0) effects.PlayOneShot(buzz);
                    if (flow.SignAge(RitualSign.WindowShadow) == 0) effects.PlayOneShot(creak);
                    break;
            }
        }

        public void Scream(bool soft) => effects.PlayOneShot(soft ? softScream : scream);

        public void UpdateLoops(RitualFlow flow, float deltaTime)
        {
            GripBar grip = flow.Grip;
            float strainTarget = grip == null ? 0 : grip.Inside ? .2f : 1;
            float scratchTarget = flow.Stage == RitualStage.Playing && flow.Step.kind == RitualStepKind.Question && flow.Phase == QuestionPhase.Answering ? .8f : 0;
            float follow = 1 - Mathf.Exp(-deltaTime * 8);
            strainLevel += (strainTarget - strainLevel) * follow;
            scratchLevel += (scratchTarget - scratchLevel) * follow;
            strain.volume = strainLevel * volume;
            scratch.volume = scratchLevel * volume;
        }

        private static AudioSource Source(GameObject owner, bool loop)
        {
            AudioSource source = owner.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = loop;
            return source;
        }

        private static float Chirp(float t, float frequency, params float[] starts)
        {
            float value = 0;
            foreach (float start in starts)
            {
                float local = t - start;
                if (local >= 0 && local < .06f) value += (float)Math.Sin(local * frequency * Math.PI * 2) * (float)Math.Sin(local / .06f * Math.PI) * .05f;
            }
            return value;
        }

        /// <summary>Gelombang ditambah derau. Derau diberi selubung, opsional dibuat tajam (high-pass) atau lembut (low-pass).</summary>
        private static AudioClip Clip(string name, float duration, Func<float, float> wave, System.Random random, float noise,
            Func<float, float> noiseEnvelope = null, bool sharp = false, float smooth = 1)
        {
            var samples = new float[Mathf.CeilToInt(Rate * duration)];
            float low = 0, previous = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                float t = (float)i / Rate;
                float raw = (float)random.NextDouble() * 2 - 1;
                low += (raw - low) * smooth;
                float shaped = sharp ? raw - previous : low;
                previous = raw;
                samples[i] = Mathf.Clamp(wave(t) + shaped * noise * (noiseEnvelope?.Invoke(t) ?? 1), -1, 1);
            }
            AudioClip clip = AudioClip.Create(name, samples.Length, 1, Rate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public void Dispose()
        {
            foreach (AudioClip clip in new[] { crickets, strainClip, scratchClip, strike, slip, thud, buzz, puff, whoosh, creak, ripple, scream, softScream, click })
                if (clip != null) UnityEngine.Object.Destroy(clip);
        }
    }
}
