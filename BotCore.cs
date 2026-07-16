using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using ForgeBot.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ForgeBot
{
    public enum ResponseKey
    {
        OneDriveError,
    }
    public static class BotCore
    {
        public static Stopwatch WakeTime = new Stopwatch();
        public static bool IsVerbose = false;
        public static DiscordChannel DebugChannel;
        public static string ImagePath;
        public static string ItemDataPath;
        public static string BlueprintDataPath;
        public static string ScriptsDataPath;
        public static ulong DebugChannelId = 0;        
        public static ulong DebugGuildId = 0; // possibly unneeded, but keeping it for now

        public static Dictionary<ResponseKey, bool> AllowResponse = new()
        {
            { ResponseKey.OneDriveError, true }
        };

        private static DiscordClient discord;
        private static bool HasStarted = false;

        private static readonly string OneDriveError = @"## My game crashes on startup!!!

If your **Total Miner** game crashes on startup or when saving, it may be due to your save folder being located in **OneDrive**, which can restrict the game's access to necessary files. This can happen if your Documents folder is synced to OneDrive by default.

### Solution 1: Give Total Miner Access to OneDrive
If you want to keep your saves in OneDrive, you'll need to ensure the game has the right permissions:

**Manually Grant Permission to Total Miner:**
   - Navigate to your **OneDrive folder** where `TotalMiner` is located. Typically `OneDrive/Documents/My Games/TotalMiner`.
   - Right-click the `TotalMiner` folder → Select **Properties**.
   - Go to the **Security** tab → Click **Edit**.
   - Find your **user account** and ensure it has **Full Control**.
   - Click **Apply** and **OK** to save changes.

### Solution 2: Change the Save Location in the Launcher
If you prefer to avoid OneDrive altogether, you can move your save location:

1. **Open the Total Miner Launcher**.
2. Click on **Settings**.
3. Look for the **Save Folder Location** option.
4. Change it to a different location outside of OneDrive, such as:
   - `C:\Users\YourName\Documents\TotalMiner`
   - `D:\TotalMinerSaves` (if you have a second drive)
5. Launch the game.";


        public static async Task MainAsync(string token, string prefix, string activity)
        {
            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            if (IsVerbose)
            {
                discord.Ready += DiscordReady;
            }

            var slashCommands = discord.UseSlashCommands();

            discord.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis
            });

            discord.MessageCreated += MessageCreatedOverride;

            Console.WriteLine(discord.Intents.ToString());

            slashCommands.RegisterCommands<GameCommands>();
            slashCommands.RegisterCommands<AdminCommands>();
            slashCommands.RegisterCommands<FunCommands>();

            await discord.ConnectAsync(new DiscordActivity(activity));

            await Task.Delay(-1);
        }

        private static async Task MessageCreatedOverride(DiscordClient sender, MessageCreateEventArgs args)
        {
            if (args.Author.IsBot)
            {
                return;
            }

            if (args.Message.Content.Contains("System.IO.IOException", StringComparison.OrdinalIgnoreCase) && AllowResponse[ResponseKey.OneDriveError])
            {
                await args.Message.RespondAsync(OneDriveError);
            }
        }

        public static async Task ClosingClient()
        {
            if (discord == null) return;

            if (HasStarted && DebugChannel != null)
            {
                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithTitle($":x:  Shutting Down")
                .WithDescription("Application Offline")
                .AddField("System", $"{Environment.MachineName}", false)
                .WithColor(DiscordColor.Red);

                await discord.SendMessageAsync(DebugChannel, embedBuilder);
            }

            await discord.UpdateStatusAsync(new("Shutting Down"), UserStatus.DoNotDisturb); 
            
            await discord.DisconnectAsync();
            discord.Dispose();
        }

        private static async Task DiscordReady(DiscordClient sender, ReadyEventArgs args)
        {
            if (HasStarted) return;

            DebugChannel = await discord.GetChannelAsync(DebugChannelId);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithTitle($":white_check_mark:  Startup")
                .WithDescription("Application Online")
                .AddField("Startup Time", $"{WakeTime.ElapsedMilliseconds}ms", false)
                .AddField("Ping", $"{sender.Ping}ms", false)
                .AddField("System", $"{Environment.MachineName}", false)
                .WithColor(DiscordColor.Green);

            if (IsVerbose)
            {
                embedBuilder.AddField("`-v`", IsVerbose.ToString(), false);
            }

            HasStarted = true;

            WakeTime.Restart();

            if (DebugChannel == null)
            {
                Console.WriteLine("!!! DebugChannel is null. Cannot send startup message.");
            }
            else
            {
                await discord.SendMessageAsync(DebugChannel, embedBuilder);
            }
        }
    }
}
