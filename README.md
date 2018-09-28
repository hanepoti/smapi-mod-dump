This repository contains all known SMAPI mods as of SMAPI 2.8-beta.4 and Stardew Valley 1.3.31 beta.
This is used to update the [SMAPI compatibility list](https://stardewvalleywiki.com/Modding:SMAPI_compatibility),
find mods using deprecated APIs, etc.

Specifically:

* `compiled` contains the latest available download for each mod, grouped into these categories:

  category       | description
  -------------- | -----------
  abandoned      | Mods which are obsolete, or have been abandoned by their authors and probably won't be updated unofficially. These will likely never be updated again.
  broken in \*   | Mods which broke in a specific game version.
  okay           | Mods which work fine in the latest versions (and don't fit one of the next two categories).
  okay (Harmony) | Mods which work fine in the latest versions, and use Harmony to patch the game code. Using many Harmony mods together often causes conflicts, so these are separate for testing.
  okay (Pong)    | The [Pong mod](https://www.nexusmods.com/stardewvalley/mods/1994). This overrides the entire game, so it's separate for testing.
