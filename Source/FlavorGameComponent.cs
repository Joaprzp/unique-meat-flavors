using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace UniqueMeatFlavors
{
    public class FlavorGameComponent : GameComponent
    {
        private Dictionary<ThingDef, FlavorCategory> meatFlavors
            = new Dictionary<ThingDef, FlavorCategory>();

        public FlavorGameComponent(Game game) { }

        public override void StartedNewGame()
        {
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

        public IReadOnlyDictionary<ThingDef, FlavorCategory> AllFlavors => meatFlavors;

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

        public void RerollAll()
        {
            meatFlavors.Clear();
            AssignMissingFlavors();
            Log.Message("[Unique Meat Flavors] Re-rolled all flavors.");
        }
    }
}
