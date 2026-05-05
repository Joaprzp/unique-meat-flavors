using HarmonyLib;
using RimWorld;
using Verse;

namespace UniqueMeatFlavors.HarmonyPatches
{
    // Postfix on Thing.Ingested(Pawn, float). This is the canonical entry
    // point for "a pawn just ate a thing" — both raw meat and meals route
    // through it. We resolve the relevant flavor for the food, look up the
    // matching thought def, and gain it as a memory on the ingester.
    //
    // The mood multiplier is applied per-instance via Thought_Memory.
    // moodPowerFactor (NOT by mutating the def's baseMoodEffect). This makes
    // the multiplier safe under multiplayer — clients with different
    // settings would diverge if we mutated globals; encoding the factor on
    // the memory itself means the value is decided once at gain time and
    // travels with the memory through saves and across clients.
    //
    // Animals are filtered out by checking ingester.needs?.mood; only
    // pawns with a mood need (humanlikes) gain the thought.
    [HarmonyPatch(typeof(Thing), nameof(Thing.Ingested))]
    public static class Patch_Thing_Ingested
    {
        public static void Postfix(Thing __instance, Pawn ingester)
        {
            if (ingester?.needs?.mood?.thoughts?.memories == null) return;

            var game = Current.Game;
            if (game == null) return;

            var comp = game.GetComponent<FlavorGameComponent>();
            if (comp == null) return;

            // Cosmetic-only mode: flavors still randomize and show on
            // inspect strings, but eating doesn't generate a mood thought.
            if (!comp.MoodEffectsEnabled) return;

            var flavor = FlavorResolver.Resolve(__instance, comp);
            if (flavor == null) return;

            var thoughtDef = FlavorResolver.ThoughtFor(flavor.Value);
            if (thoughtDef == null) return; // Common: silent, by design.

            var memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
            memory.moodPowerFactor = comp.MoodMultiplierValue;
            ingester.needs.mood.thoughts.memories.TryGainMemory(memory);
        }
    }
}
