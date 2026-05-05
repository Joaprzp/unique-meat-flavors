using HarmonyLib;
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
        }
    }
}
