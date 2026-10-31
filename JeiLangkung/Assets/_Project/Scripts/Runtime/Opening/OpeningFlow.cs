using System;

namespace JeiLangkung.Opening
{
    public enum OpeningStage { Intro, Assignment, Notification, Blackout, FindLight, Aftermath, Complete }

    /// <summary>Aturan pembuka; tidak bergantung pada UI, scene, atau waktu Unity.</summary>
    public sealed class OpeningFlow
    {
        private readonly int introCount;
        private readonly int aftermathCount;
        private int inspectedItems;
        public OpeningStage Stage { get; private set; }
        public int LineIndex { get; private set; }
        public bool MessageOpened { get; private set; }
        public bool LaptopChecked => (inspectedItems & 1) != 0;
        public bool NotesChecked => (inspectedItems & 2) != 0;

        public OpeningFlow(int introCount, int aftermathCount)
        {
            if (introCount < 1 || aftermathCount < 1) throw new ArgumentOutOfRangeException(nameof(introCount));
            this.introCount = introCount;
            this.aftermathCount = aftermathCount;
            Reset();
        }

        public void Reset()
        {
            Stage = OpeningStage.Intro;
            LineIndex = 0;
            inspectedItems = 0;
            MessageOpened = false;
        }

        public bool Advance()
        {
            int count;
            if (Stage == OpeningStage.Intro) count = introCount;
            else if (Stage == OpeningStage.Aftermath) count = aftermathCount;
            else return false;
            if (++LineIndex < count) return true;
            LineIndex = 0;
            Stage = Stage == OpeningStage.Intro ? OpeningStage.Assignment : OpeningStage.Complete;
            return true;
        }

        public bool InspectAssignment(bool laptop)
        {
            if (Stage != OpeningStage.Assignment) return false;
            int bit = laptop ? 1 : 2;
            if ((inspectedItems & bit) != 0) return false;
            inspectedItems |= bit;
            if (inspectedItems == 3) Stage = OpeningStage.Notification;
            return true;
        }

        public bool OpenMessage()
        {
            if (Stage != OpeningStage.Notification || MessageOpened) return false;
            MessageOpened = true;
            return true;
        }

        public bool ReadMessage()
        {
            if (Stage != OpeningStage.Notification || !MessageOpened) return false;
            Stage = OpeningStage.Blackout;
            return true;
        }

        public bool FinishBlackout()
        {
            if (Stage != OpeningStage.Blackout) return false;
            Stage = OpeningStage.FindLight;
            return true;
        }

        public bool TurnOnFlashlight()
        {
            if (Stage != OpeningStage.FindLight) return false;
            Stage = OpeningStage.Aftermath;
            LineIndex = 0;
            return true;
        }
    }
}
