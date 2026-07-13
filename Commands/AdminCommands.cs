using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ForgeBot.Tools;
using System.Threading.Tasks;
using static ForgeBot.Tools.Permissions;

namespace ForgeBot.Commands
{
    [SlashCommandGroup("Admin", "Admin and Developer Commands")]
    internal class AdminCommands : ApplicationCommandModule
    {
        [SlashCommand("setresponse", "Enable or disable auto responses by key name")]
        [UserPermissionLevel(Permission.Admin)]
        public async Task SetResponseAsync(InteractionContext ctx,
            [Option("key", "Name of the response key (case-sensitive)")] ResponseKey key,
            [Option("enabled", "True to enable, false to disable")] bool enabled)
        {
            if (!BotCore.AllowResponse.ContainsKey(key))
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"❌ Key `{key}` not found in AllowResponse.")
                        .AsEphemeral(true));
                return;
            }

            BotCore.AllowResponse[key] = enabled;

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .WithContent($"✅ Key `{key}` is now set to `{enabled}`.")
                    .AsEphemeral(true));
        }

        [SlashCommand("setperm", "Sets a user's permission level (Admin-only or higher)")]
        [UserPermissionLevel(Permission.Admin)]
        public async Task SetPermissionAsync(
            InteractionContext ctx,
            [Option("user", "User to modify")] DiscordUser targetUser,
            [Option("level", "New permission level (must be ≤ your own)")] Permission newLevel)
        {
            var callerLevel = GetUserLevel(ctx.User.Id);
            var targetLevel = GetUserLevel(targetUser.Id);
            if (ctx.User == targetUser)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent($"❌ You cannot assign permission to yourself.")
                .AsEphemeral(true));
            }

            if (callerLevel > targetLevel)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent($"❌ You cannot assign permission level **{newLevel}**, to {targetUser.Username} (**{targetLevel}**) which is higher than your own (**{callerLevel}**).")
                .AsEphemeral(true));
            }
            else if (newLevel > callerLevel)
            {
                AddUser(targetUser.Id, newLevel);

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"✅ Set `{targetUser.Username}` to **{newLevel}**.")
                        .AsEphemeral(true));
                FileSaveManager.SaveFile("users/userops.json", Users);
            }
            else
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"❌ You cannot assign permission level **{newLevel}**, which is higher than your own (**{callerLevel}**).")
                        .AsEphemeral(true));
            }
        }
    }

}
