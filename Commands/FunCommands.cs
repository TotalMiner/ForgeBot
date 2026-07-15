using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForgeBot.Commands
{
    [SlashCommandGroup("Fun", "Miscellaneous Commands for fun")]
    internal class FunCommands : ApplicationCommandModule
    {
        [SlashCommand("drift", "Can it drift tho?")]
        internal async Task ItCanDrift(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(":race_car: vrmmmvrmmmmmmm-skrrrrrrrrrrrrrrrrrrr-vrmmmmmmmmmmm");
        }

        [SlashCommand("cat", "Get a random cat picture")]
        internal async Task CatAsync(InteractionContext ctx)
        {
            string imageUrl = await Tools.CatGetter.GetRandomCatImageUrlAsync();

            await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("🐱 Random Cat")
                    .WithImageUrl(imageUrl)
                    .WithColor(DiscordColor.Orange)));
        }
    }
}
