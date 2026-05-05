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

            listing.CheckboxLabeled(
                "UMF.Settings.MoodEffectsEnabled".Translate(),
                ref Settings.moodEffectsEnabled,
                "UMF.Settings.MoodEffectsEnabledTooltip".Translate());

            listing.Gap();
            listing.GapLine();
            listing.Gap();

            listing.Label("UMF.Settings.MoodMultiplier".Translate());
            if (listing.RadioButton(
                    "UMF.Settings.MoodMultiplier.Half".Translate(),
                    Settings.moodMultiplier == MoodMultiplierOption.Half))
            {
                Settings.moodMultiplier = MoodMultiplierOption.Half;
            }
            if (listing.RadioButton(
                    "UMF.Settings.MoodMultiplier.Full".Translate(),
                    Settings.moodMultiplier == MoodMultiplierOption.Full))
            {
                Settings.moodMultiplier = MoodMultiplierOption.Full;
            }
            if (listing.RadioButton(
                    "UMF.Settings.MoodMultiplier.Double".Translate(),
                    Settings.moodMultiplier == MoodMultiplierOption.Double))
            {
                Settings.moodMultiplier = MoodMultiplierOption.Double;
            }

            // Re-apply on every render so multiplier changes take effect
            // immediately, not just when the settings window closes.
            FlavorMoodMultiplierApplier.Apply();

            listing.Gap();
            listing.GapLine();
            listing.Gap();

            listing.Label("UMF.Settings.RerollSection".Translate());
            listing.Gap(4f);

            bool gameLoaded = Current.Game?.GetComponent<FlavorGameComponent>() != null;
            if (gameLoaded)
            {
                if (listing.ButtonText("UMF.Settings.RerollButton".Translate()))
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "UMF.Settings.RerollConfirmText".Translate(),
                        () => Current.Game.GetComponent<FlavorGameComponent>().RerollAll(),
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

        public override void WriteSettings()
        {
            base.WriteSettings();
            FlavorMoodMultiplierApplier.Apply();
        }
    }
}
