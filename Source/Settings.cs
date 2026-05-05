using Verse;

namespace UniqueMeatFlavors
{
    public enum MoodMultiplierOption
    {
        Half = 0,
        Full = 1,
        Double = 2,
    }

    public static class MoodMultiplierOptionExtensions
    {
        public static float MultiplierValue(this MoodMultiplierOption option)
        {
            switch (option)
            {
                case MoodMultiplierOption.Half:   return 0.5f;
                case MoodMultiplierOption.Double: return 2f;
                default:                          return 1f;
            }
        }
    }

    // Per-client defaults used to seed a new save's per-save values
    // (FlavorGameComponent.moodEffectsEnabled / moodMultiplier). Once a save
    // is loaded, the in-game settings UI edits the per-save values directly,
    // not these — that way the host's choices are authoritative in
    // multiplayer (the FlavorGameComponent travels with the save).
    public class UniqueMeatFlavorsSettings : ModSettings
    {
        public bool defaultMoodEffectsEnabled = true;
        public MoodMultiplierOption defaultMoodMultiplier = MoodMultiplierOption.Full;

        public override void ExposeData()
        {
            base.ExposeData();
            // Field names in the saved XML are stable across the rework so
            // existing players don't lose their previously chosen values.
            Scribe_Values.Look(ref defaultMoodEffectsEnabled, "moodEffectsEnabled", true);
            Scribe_Values.Look(ref defaultMoodMultiplier, "moodMultiplier", MoodMultiplierOption.Full);
        }
    }
}
