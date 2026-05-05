using System;
using RimWorld;
using Verse;

namespace UniqueMeatFlavors
{
    public static class FlavorResolver
    {
        // Returns the flavor category that applies to the given Thing for a
        // colonist who just ingested it, or null if none applies (Thing has
        // no relevant meat ingredient and isn't itself a known meatDef).
        //
        // - Raw meat eaten directly: returns the meatDef's flavor.
        // - Meal/pemmican/etc. with meat ingredients: equal-weight average
        //   over the distinct meatDefs in CompIngredients that we track.
        //   Quantities aren't preserved on the meal stack (CompIngredients
        //   only records ThingDefs, deduplicated), so this is the closest
        //   approximation. See CLAUDE.md "Meals con múltiples carnes".
        // - Anything else: null.
        public static FlavorCategory? Resolve(Thing thing, FlavorGameComponent comp)
        {
            if (thing?.def == null || comp == null) return null;

            if (comp.AllFlavors.TryGetValue(thing.def, out var directFlavor))
            {
                return directFlavor;
            }

            var ingredientsComp = thing.TryGetComp<CompIngredients>();
            if (ingredientsComp?.ingredients == null || ingredientsComp.ingredients.Count == 0)
            {
                return null;
            }

            int sum = 0;
            int count = 0;
            foreach (var ingredientDef in ingredientsComp.ingredients)
            {
                if (ingredientDef != null && comp.AllFlavors.TryGetValue(ingredientDef, out var ingFlavor))
                {
                    sum += (int)ingFlavor;
                    count++;
                }
            }
            if (count == 0) return null;

            double average = (double)sum / count;
            int rounded = (int)Math.Round(average, MidpointRounding.AwayFromZero);
            if (rounded < 0) rounded = 0;
            if (rounded > 4) rounded = 4;
            return (FlavorCategory)rounded;
        }

        public static ThoughtDef ThoughtFor(FlavorCategory category)
        {
            switch (category)
            {
                case FlavorCategory.Bland:     return UMF_DefOf.UMF_AteBlandMeat;
                case FlavorCategory.Tasty:     return UMF_DefOf.UMF_AteTastyMeat;
                case FlavorCategory.Delicious: return UMF_DefOf.UMF_AteDeliciousMeat;
                case FlavorCategory.Exquisite: return UMF_DefOf.UMF_AteExquisiteMeat;
                default: return null; // Common deliberately has no thought.
            }
        }
    }
}
