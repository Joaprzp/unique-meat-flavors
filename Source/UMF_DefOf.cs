using RimWorld;

namespace UniqueMeatFlavors
{
    [DefOf]
    public static class UMF_DefOf
    {
        public static ThoughtDef UMF_AteBlandMeat;
        public static ThoughtDef UMF_AteTastyMeat;
        public static ThoughtDef UMF_AteDeliciousMeat;
        public static ThoughtDef UMF_AteExquisiteMeat;

        static UMF_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(UMF_DefOf));
        }
    }
}
