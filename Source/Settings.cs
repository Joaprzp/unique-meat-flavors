using Verse;

namespace UniqueMeatFlavors
{
    public enum MoodMultiplierOption
    {
        Half = 0,
        Full = 1,
        Double = 2,
    }

    public class UniqueMeatFlavorsSettings : ModSettings
    {
        public bool moodEffectsEnabled = true;
        public MoodMultiplierOption moodMultiplier = MoodMultiplierOption.Full;

        public float MoodMultiplierValue
        {
            get
            {
                switch (moodMultiplier)
                {
                    case MoodMultiplierOption.Half:   return 0.5f;
                    case MoodMultiplierOption.Double: return 2f;
                    default:                          return 1f;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref moodEffectsEnabled, "moodEffectsEnabled", true);
            Scribe_Values.Look(ref moodMultiplier, "moodMultiplier", MoodMultiplierOption.Full);
        }
    }
}
