# Unique Meat Flavors

A RimWorld mod that gives each animal species a randomized "flavor" at the
start of every new game. Colonists eating meat get an extra mood thought based
on the flavor of the species. Re-randomized per save, so chicken might be a
delicacy in one playthrough and Thrumbo bland in the next.

> **Status:** in development. This README will be rewritten for the public
> Workshop release.

## Supported versions

- RimWorld 1.6 (target).

## Building

Requires the .NET SDK (6.0 or newer). All other references — RimWorld's
`Assembly-CSharp.dll`, Harmony, and the .NET Framework 4.7.2 reference
assemblies — are pulled from NuGet, so you do **not** need to install the
.NET Framework 4.7.2 developer pack or copy DLLs out of your RimWorld install.

```
dotnet build Source/UniqueMeatFlavors.csproj -c Release
```

The build drops `UniqueMeatFlavors.dll` into `Assemblies/`, which is the
folder RimWorld loads at runtime.

## Testing locally

Copy (or symlink) the entire repository folder into RimWorld's local mods
directory so the game can find it. On Windows, with the default Steam install:

```
mklink /D "G:\SteamLibrary\steamapps\common\RimWorld\Mods\UniqueMeatFlavors" "C:\code\unique-meat-flavors"
```

(Adjust the RimWorld path for your install. Run from an elevated `cmd`. The
symlink lets you rebuild and reload the mod without copying files each time.)

Then enable **Unique Meat Flavors** in the in-game mod manager. On startup
you should see `[Unique Meat Flavors] loaded` in the log.

## Dependencies

- [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)
  by Andreas Pardeike (`brrainz.harmony`). Required.

## License

MIT — see [LICENSE](LICENSE).

## Future Ideas

Out of scope for V1. Tracked here so they don't sneak into the initial
release:

- Effects on market value of meat / meals.
- Recipes that require a minimum flavor.
- A "Gourmet" trait that amplifies flavor mood effects.
- Reactions from visitors and traders to the flavor of food served.
- Hidden flavors that only become known after tasting (discovery system).
- Custom textures or icons per flavor tier.
- Sound effects on eating.
