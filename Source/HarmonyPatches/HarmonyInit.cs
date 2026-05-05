using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace UniqueMeatFlavors.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            var harmony = new Harmony("joa.uniquemeatflavors");
            harmony.PatchAll();
            Log.Message("[Unique Meat Flavors] Harmony patches applied.");

            // Register all [SyncMethod]-annotated methods so Zetrith's
            // Multiplayer mod intercepts and broadcasts them to all clients.
            // MP.enabled is false when Multiplayer isn't loaded, so this is
            // a no-op for single-player users.
            if (MP.enabled)
            {
                MP.RegisterAll();
                Log.Message("[Unique Meat Flavors] Multiplayer sync handlers registered.");
            }
        }
    }
}
