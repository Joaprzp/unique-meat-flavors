using System.Collections.Generic;
using RimWorld;
using Verse;

namespace UniqueMeatFlavors
{
    // Applies the current mood multiplier from settings directly to the
    // flavor ThoughtDefs by mutating stages[0].baseMoodEffect. We do this
    // instead of Harmony-patching Thought.MoodOffset because:
    //   - It's a one-shot write, not a per-tick recalculation, so it's
    //     effectively zero cost on the mood evaluation hot path.
    //   - Pawns' existing memory thoughts read from the def's CurStage
    //     each time mood is computed, so a settings change applies
    //     retroactively to thoughts already in their memory stack.
    //   - We avoid the virtual-dispatch question of which Thought subclass
    //     overrides MoodOffset.
    [StaticConstructorOnStartup]
    public static class FlavorMoodMultiplierApplier
    {
        private static readonly Dictionary<ThoughtDef, float> originalEffects
            = new Dictionary<ThoughtDef, float>();

        static FlavorMoodMultiplierApplier()
        {
            CaptureOriginal(UMF_DefOf.UMF_AteBlandMeat);
            CaptureOriginal(UMF_DefOf.UMF_AteTastyMeat);
            CaptureOriginal(UMF_DefOf.UMF_AteDeliciousMeat);
            CaptureOriginal(UMF_DefOf.UMF_AteExquisiteMeat);
            Apply();
        }

        private static void CaptureOriginal(ThoughtDef def)
        {
            if (def?.stages == null || def.stages.Count == 0) return;
            originalEffects[def] = def.stages[0].baseMoodEffect;
        }

        public static void Apply()
        {
            float multiplier = UniqueMeatFlavorsMod.Settings?.MoodMultiplierValue ?? 1f;
            foreach (var kvp in originalEffects)
            {
                kvp.Key.stages[0].baseMoodEffect = kvp.Value * multiplier;
            }
        }
    }
}
