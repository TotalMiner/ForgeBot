# Total Miner: ForgeBot
> Total Miner: ForgeBot is the official Total Miner Community Wiki Discord bot

## Planned Additions
- Events
- Crafting Help
- Lua Scripting Help
- General Game Tutorials/How-Tos

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
To host ForgeBot locally on your own machine, you will need a Discord Bot Token and a standard `.NET` runtime environment.

### 1. Create a Discord Bot
Before running the code, you must create a bot application on Discord and invite it to your server. 

* Follow the official [Discord Developer Documentation](https://discord.com/developers/docs/getting-started) to create a new application, retrieve your **Bot Token**, and generate an invite link.
* **Important:** Ensure you enable the **Message Content Intent** in the Discord Developer Portal under the "Bot" tab, as ForgeBot requires this to read messages and commands.

### 2. First Boot & Configuration
Run the application for the first time. The bot will automatically create a `Resources` directory and generate a `properties.txt` file, you must edit this file to match your token and debug channel before relaunching.

Open `Resources/properties.txt` in any text editor. You will see several configuration parameters. 

#### Parameter Breakdown
| Parameter | Description |
| :--- | :--- |
| `version` | Internal version tracker for the config file. Do not manually change this. |
| `token` | Your secret Discord Bot Token. Replace `REPLACEME` with your actual token. **Never share this with anyone or commit it to GitHub!** |
| `prefix` (DEPRECIATED) | The character(s) used to trigger standard text commands (default is `?`). *The prefix is no longer used by ForgeBot* |
| `activity` | The custom status message displayed on the bot's profile (e.g., "Playing Total Miner"). |
| `debugChannel` | The 18-19 digit Discord Channel ID where the bot will send its online/offline heartbeat and startup logs. |
| `debugServer` | *(If applicable)* The 18-19 digit Discord Server (Guild) ID for your testing server. *GuildID is not needed to get your debug channel reference* |
| `imageLink` | URL to the raw GitHub directory containing game images. |
| `itemDataLink` | URL to the raw `ItemData.xml` file. |
| `blueprintDataLink` | URL to the raw `BlueprintData.xml` file. |
| `scriptsDataLink` | URL to the raw `ScriptCommands.xml` file. |

*Note: The data links default to the official [ForgeBot-Resources repository](https://github.com/TotalMiner/ForgeBot-Resources). You only need to change these if you are testing custom game data on a private fork.*

### 3. Start the Bot
Once your token and debug IDs are saved in `properties.txt`, launch the application again. You should see the startup sequence begin in your console, data files will automatically download if they are missing or outdated, and the bot will send a `✅ Startup` embed to your designated debug channel!

## Maintaining Truthfulness
To ensure ForgeBot always provides accurate and up-to-date information, it relies on external data files rather than hardcoding game mechanics into the bot's source code. 

All game data (such as items, blueprints, and script commands) and associated images are maintained in the [ForgeBot-Resources repository](https://github.com/TotalMiner/ForgeBot-Resources). 

By decoupling the data from the code, the community can update game files and correct inaccuracies independently. This guarantees the bot remains truthful to the current state of *Total Miner* without requiring a recompilation or redeployment of the bot itself
