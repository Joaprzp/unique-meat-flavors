using System.Collections.Generic;
using Multiplayer.API;
using RimWorld;
using Verse;

namespace UniqueMeatFlavors
{
    public class FlavorGameComponent : GameComponent
    {
        private Dictionary<ThingDef, FlavorCategory> meatFlavors
            = new Dictionary<ThingDef, FlavorCategory>();

        // Per-save gameplay settings. Defaults are seeded from ModSettings
        // on StartedNewGame; otherwise loaded from the save. Edited via the
        // [SyncMethod] setters below so changes propagate to all clients in
        // multiplayer (the save travels with the host, so the host's values
        // are authoritative by construction).
        private bool moodEffectsEnabled = true;
        private MoodMultiplierOption moodMultiplier = MoodMultiplierOption.Full;

        public FlavorGameComponent(Game game) { }

        public bool MoodEffectsEnabled => moodEffectsEnabled;
        public MoodMultiplierOption MoodMultiplier => moodMultiplier;
        public float MoodMultiplierValue => moodMultiplier.MultiplierValue();
        public IReadOnlyDictionary<ThingDef, FlavorCategory> AllFlavors => meatFlavors;

        public override void StartedNewGame()
        {
            var defaults = UniqueMeatFlavorsMod.Settings;
            moodEffectsEnabled = defaults?.defaultMoodEffectsEnabled ?? true;
            moodMultiplier = defaults?.defaultMoodMultiplier ?? MoodMultiplierOption.Full;
            AssignMissingFlavors();
        }

        public override void LoadedGame()
        {
            // Catches meatDefs that didn't exist when the save was created
            // (e.g., the user added an animal mod after starting the game).
            AssignMissingFlavors();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(
                ref meatFlavors,
                "meatFlavors",
                LookMode.Def,
                LookMode.Value);
            Scribe_Values.Look(ref moodEffectsEnabled, "moodEffectsEnabled", true);
            Scribe_Values.Look(ref moodMultiplier, "moodMultiplier", MoodMultiplierOption.Full);

            if (Scribe.mode == LoadSaveMode.PostLoadInit && meatFlavors == null)
            {
                meatFlavors = new Dictionary<ThingDef, FlavorCategory>();
            }
        }

        public FlavorCategory GetFlavor(ThingDef meatDef)
        {
            if (meatDef == null) return FlavorCategory.Common;
            return meatFlavors.TryGetValue(meatDef, out var flavor)
                ? flavor
                : FlavorCategory.Common;
        }

        [SyncMethod]
        public void SetMoodEffectsEnabled(bool value)
        {
            moodEffectsEnabled = value;
        }

        [SyncMethod]
        public void SetMoodMultiplier(MoodMultiplierOption value)
        {
            moodMultiplier = value;
        }

        [SyncMethod]
        public void RerollAll()
        {
            meatFlavors.Clear();
            AssignMissingFlavors();
            Log.Message("[Unique Meat Flavors] Re-rolled all flavors.");
        }

        private void AssignMissingFlavors()
        {
            int added = 0;
            foreach (var meatDef in FlavorAssigner.EnumerateAnimalMeatDefs())
            {
                if (!meatFlavors.ContainsKey(meatDef))
                {
                    meatFlavors[meatDef] = FlavorAssigner.RollRandom();
                    added++;
                }
            }
            if (added > 0)
            {
                Log.Message(
                    $"[Unique Meat Flavors] Assigned flavors to {added} meat def(s) " +
                    $"(total tracked: {meatFlavors.Count}).");
            }
        }
    }
}
