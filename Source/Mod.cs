using UnityEngine;
using Verse;

namespace UniqueMeatFlavors
{
    public class UniqueMeatFlavorsMod : Mod
    {
        public static UniqueMeatFlavorsSettings Settings { get; private set; }

        public UniqueMeatFlavorsMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<UniqueMeatFlavorsSettings>();
            Log.Message("[Unique Meat Flavors] loaded");
        }

        public override string SettingsCategory() => "Unique Meat Flavors";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.Begin(inRect);

            var comp = Current.Game?.GetComponent<FlavorGameComponent>();
            bool gameLoaded = comp != null;

            // Header explains which scope the user is editing right now.
            listing.Label(gameLoaded
                ? "UMF.Settings.HeaderInGame".Translate()
                : "UMF.Settings.HeaderDefaults".Translate());
            listing.Gap(4f);

            // Mood effects toggle.
            bool moodEnabled = gameLoaded
                ? comp.MoodEffectsEnabled
                : Settings.defaultMoodEffectsEnabled;
            bool moodEnabledNew = moodEnabled;
            listing.CheckboxLabeled(
                "UMF.Settings.MoodEffectsEnabled".Translate(),
                ref moodEnabledNew,
                "UMF.Settings.MoodEffectsEnabledTooltip".Translate());
            if (moodEnabledNew != moodEnabled)
            {
                if (gameLoaded) comp.SetMoodEffectsEnabled(moodEnabledNew);
                else Settings.defaultMoodEffectsEnabled = moodEnabledNew;
            }

            listing.Gap();
            listing.GapLine();
            listing.Gap();

            // Multiplier radio buttons.
            var currentMultiplier = gameLoaded
                ? comp.MoodMultiplier
                : Settings.defaultMoodMultiplier;
            listing.Label("UMF.Settings.MoodMultiplier".Translate());

            DrawMultiplierOption(listing, currentMultiplier,
                MoodMultiplierOption.Half, "UMF.Settings.MoodMultiplier.Half", comp);
            DrawMultiplierOption(listing, currentMultiplier,
                MoodMultiplierOption.Full, "UMF.Settings.MoodMultiplier.Full", comp);
            DrawMultiplierOption(listing, currentMultiplier,
                MoodMultiplierOption.Double, "UMF.Settings.MoodMultiplier.Double", comp);

            listing.Gap();
            listing.GapLine();
            listing.Gap();

            // Re-roll button. Only meaningful with a save loaded; in-game
            // it routes through the GameComponent's [SyncMethod] so MP
            // clients all execute the re-roll on the same tick.
            listing.Label("UMF.Settings.RerollSection".Translate());
            listing.Gap(4f);
            if (gameLoaded)
            {
                if (listing.ButtonText("UMF.Settings.RerollButton".Translate()))
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "UMF.Settings.RerollConfirmText".Translate(),
                        () => comp.RerollAll(),
                        destructive: true));
                }
            }
            else
            {
                GUI.color = Color.gray;
                listing.Label("UMF.Settings.RerollNoGame".Translate());
                GUI.color = Color.white;
            }

            listing.End();
        }

        private static void DrawMultiplierOption(
            Listing_Standard listing,
            MoodMultiplierOption current,
            MoodMultiplierOption option,
            string labelKey,
            FlavorGameComponent comp)
        {
            if (listing.RadioButton(labelKey.Translate(), current == option))
            {
                if (comp != null) comp.SetMoodMultiplier(option);
                else Settings.defaultMoodMultiplier = option;
            }
        }
    }
}
