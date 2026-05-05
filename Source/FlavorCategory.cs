using Verse;

namespace UniqueMeatFlavors
{
    public enum FlavorCategory
    {
        Bland = 0,
        Common = 1,
        Tasty = 2,
        Delicious = 3,
        Exquisite = 4,
    }

    public static class FlavorCategoryExtensions
    {
        public static int MoodOffset(this FlavorCategory category)
        {
            switch (category)
            {
                case FlavorCategory.Bland:     return -3;
                case FlavorCategory.Common:    return 0;
                case FlavorCategory.Tasty:     return 2;
                case FlavorCategory.Delicious: return 4;
                case FlavorCategory.Exquisite: return 6;
                default: return 0;
            }
        }

        public static string TranslatedLabel(this FlavorCategory category)
        {
            return ("UMF.Flavor." + category.ToString()).Translate();
        }
    }
}
