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

namespace ForgeBot
{
    public static class BotCore
    {
        public static Stopwatch WakeTime = new Stopwatch();
        public static bool IsVerbose = false;
        public static DiscordChannel DebugChannel;
        private static DiscordClient discord;
        private static bool HasStarted = false;
        public static async Task MainAsync(string token, string prefix, string activity)
        {
            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            discord.Heartbeated += DiscordReady;

            var slashCommands = discord.UseSlashCommands();

            discord.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = PollBehaviour.KeepEmojis
            });

            slashCommands.RegisterCommands<SlashGeneral>();
#if DEBUG
            await discord.ConnectAsync(new DiscordActivity(activity, ActivityType.Competing));
#else
            await discord.ConnectAsync(new DiscordActivity(activity));
#endif

            await Task.Delay(-1);
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
