using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace UniqueMeatFlavors
{
    public static class FlavorAssigner
    {
        // Pico en Common, tail más larga hacia los positivos. Suma = 1.0.
        private static readonly (FlavorCategory category, float weight)[] Distribution =
        {
            (FlavorCategory.Bland,     0.15f),
            (FlavorCategory.Common,    0.35f),
            (FlavorCategory.Tasty,     0.25f),
            (FlavorCategory.Delicious, 0.15f),
            (FlavorCategory.Exquisite, 0.10f),
        };

        // Usa Rand.Value (RNG sincronizado de RimWorld), nunca System.Random:
        // requisito para compat con Zetrith's Multiplayer.
        public static FlavorCategory RollRandom()
        {
            float r = Rand.Value;
            float cumulative = 0f;
            foreach (var bucket in Distribution)
            {
                cumulative += bucket.weight;
                if (r < cumulative) return bucket.category;
            }
            return FlavorCategory.Common;
        }

        public static IEnumerable<ThingDef> EnumerateAnimalMeatDefs()
        {
            var seen = new HashSet<ThingDef>();
            foreach (var def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (def?.race == null || !def.race.Animal) continue;
                var meatDef = def.race.meatDef;
                if (meatDef == null) continue;
                if (seen.Add(meatDef)) yield return meatDef;
            }
        }
    }
}
