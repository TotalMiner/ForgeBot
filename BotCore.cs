using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using ForgeBot.Commands;
using System;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using DSharpPlus.EventArgs;
using System.Diagnostics;
using System.Collections.Generic;

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

            discord.Heartbeated += DiscordReady;

            var slashCommands = discord.UseSlashCommands();

            discord.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis
            });

            discord.MessageCreated += MessageCreatedOverride;

            Console.WriteLine(discord.Intents.ToString());

            slashCommands.RegisterCommands<SlashGeneral>();
            slashCommands.RegisterCommands<AdminCommands>();
#if DEBUG
            await discord.ConnectAsync(new DiscordActivity(activity, ActivityType.Competing));
#else
            await discord.ConnectAsync(new DiscordActivity(activity));
#endif


            await Task.Delay(-1);
        }

        private static async Task MessageCreatedOverride(DiscordClient sender, MessageCreateEventArgs args)
        {
            if (args.Message.Content.Contains("System.IO.IOException", StringComparison.OrdinalIgnoreCase) && AllowResponse[ResponseKey.OneDriveError])
            {
                await args.Message.RespondAsync(OneDriveError);
            }
        }

        public static async Task ClosingClient()
        {
            await discord.UpdateStatusAsync(new("Shutting Down"), UserStatus.DoNotDisturb);
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            if (HasStarted)
            {
                embedBuilder.WithTitle($":x:  Shutting Down");
                embedBuilder.WithDescription("Application Offline");
                embedBuilder.WithColor(DiscordColor.Red);

                await discord.SendMessageAsync(DebugChannel, embedBuilder);
            }
        }
            private static async Task DiscordReady(DiscordClient sender, HeartbeatEventArgs args)
        {

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();

            if (!HasStarted)
            {
                DebugChannel = discord.GetGuildAsync(284460570948665354).Result.GetChannel(284460570948665354);
                embedBuilder.WithTitle($":white_check_mark:  Startup");
                embedBuilder.WithDescription("Application Online");
                embedBuilder.AddField("Startup Time", $"{WakeTime.ElapsedMilliseconds}ms", false);
                embedBuilder.AddField("Ping", $"{args.Ping}", false);
                if (IsVerbose)
                    embedBuilder.AddField("`-v`", IsVerbose.ToString(), false);
                embedBuilder.WithColor(DiscordColor.Green);
                HasStarted = true;
            }
            else
            {
                TimeSpan timeSpan = WakeTime.Elapsed;
                string elapsedTime = System.String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
            timeSpan.Milliseconds / 10);
                DebugChannel = discord.GetGuildAsync(284460570948665354).Result.GetChannel(284460570948665354);
                embedBuilder.WithTitle($":heart:  Heartbeat");
                embedBuilder.WithDescription("Heartbeat Recorded\nApplication Reconnected");
                embedBuilder.AddField("Last Heartbeat", $"{elapsedTime}", false);
                embedBuilder.AddField("Ping", $"{args.Ping}", false);
                embedBuilder.WithColor(DiscordColor.Goldenrod);
                HasStarted = true;
            }

            WakeTime.Restart();

            await discord.SendMessageAsync(DebugChannel, embedBuilder);
        }
    }
}
