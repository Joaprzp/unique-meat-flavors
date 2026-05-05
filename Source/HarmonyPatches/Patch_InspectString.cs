using HarmonyLib;
using Verse;

namespace UniqueMeatFlavors.HarmonyPatches
{
    // Postfix on ThingWithComps.GetInspectString() so every meat stack the
    // player hovers/selects appends a "Flavor: X" line. Meat in vanilla
    // and modded RimWorld is a ThingWithComps (it carries CompForbiddable
    // and others), so this hits the right surface without us having to
    // inject a ThingComp into every meat def at load time.
    [HarmonyPatch(typeof(ThingWithComps), nameof(ThingWithComps.GetInspectString))]
    public static class Patch_ThingWithComps_GetInspectString
    {
        public static void Postfix(ThingWithComps __instance, ref string __result)
        {
            if (__instance?.def == null) return;

            var game = Current.Game;
            if (game == null) return;

            var comp = game.GetComponent<FlavorGameComponent>();
            if (comp == null) return;

            // Fast path: most ThingWithComps aren't meat. Dict lookup is O(1).
            if (!comp.AllFlavors.ContainsKey(__instance.def)) return;

            var flavor = comp.GetFlavor(__instance.def);
            string line = "UMF.InspectFlavor".Translate(flavor.TranslatedLabel());

            if (string.IsNullOrEmpty(__result))
            {
                __result = line;
            }
            else
            {
                __result = __result.TrimEnd('\n', '\r') + "\n" + line;
            }
        }
    }
}
