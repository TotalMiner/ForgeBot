# Total Miner: ForgeBot
> Total Miner: ForgeBot is the official Total Miner Community Wiki Discord bot

## Planned Additions
- Placeholder

## Dependencies
### NuGet Packages
- `DSharpPlus.CommandsNext` (v4.4.6)
- `DSharpPlus.Interactivity` (v4.4.6)
- `DSharpPlus.SlashCommands` (v4.4.6)
- `FuzzySharp` (v2.0.2)
- `MonoGame.Framework.DesktopGL` (v3.8.1.303)

### Library References (.dll)
Local assembly references required for game API integration (located in `libs/`):
- `StudioForge.Engine.Game.dll`
- `StudioForge.TotalMiner.dll`
- `StudioForge.TotalMiner.API.dll`

### Resource Dependencies
Data files required at runtime for bot functionality:
- `Resources/BlueprintData.xml`
- `Resources/ItemData.xml`
- `Resources/ScriptCommands.xml`
- `Resources/properties.txt`

## Getting Started
- Placeholder

## Maintaining Truthfulness
To ensure ForgeBot always provides accurate and up-to-date information, it relies on external data files rather than hardcoding game mechanics into the bot's source code. 

All game data (such as items, blueprints, and script commands) and associated images are maintained in the [ForgeBot-Resources repository](https://github.com/TotalMiner/ForgeBot-Resources). 

By decoupling the data from the code, the community can update game files and correct inaccuracies independently. This guarantees the bot remains truthful to the current state of *Total Miner* without requiring a recompilation or redeployment of the bot itself
