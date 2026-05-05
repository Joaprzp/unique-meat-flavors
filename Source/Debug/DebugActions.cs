using System.Linq;
using System.Text;
using LudeonTK;
using Verse;

namespace UniqueMeatFlavors.Debug
{
    public static class DebugActions
    {
        private const string Category = "Unique Meat Flavors";

        [DebugAction(
            Category,
            "List meat flavors",
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.Playing)]
        private static void ListFlavors()
        {
            var comp = Current.Game?.GetComponent<FlavorGameComponent>();
            if (comp == null)
            {
                Log.Warning("[Unique Meat Flavors] No FlavorGameComponent on current game.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"[Unique Meat Flavors] {comp.AllFlavors.Count} meat def(s):");
            foreach (var kvp in comp.AllFlavors.OrderBy(k => k.Key.label ?? k.Key.defName))
            {
                sb.AppendLine($"  {kvp.Key.label ?? kvp.Key.defName} ({kvp.Key.defName})  →  {kvp.Value}");
            }
            Log.Message(sb.ToString());
        }

        [DebugAction(
            Category,
            "Re-roll all flavors",
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.Playing)]
        private static void RerollAllFlavors()
        {
            var comp = Current.Game?.GetComponent<FlavorGameComponent>();
            if (comp == null)
            {
                Log.Warning("[Unique Meat Flavors] No FlavorGameComponent on current game.");
                return;
            }
            comp.RerollAll();
        }
    }
}
