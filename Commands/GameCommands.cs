using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ForgeBot.Converters;
using ForgeBot.Tools;
using FuzzySharp;
using FuzzySharp.Extractor;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeBot.Commands
{
    [SlashCommandGroup("Game", "General TotalMiner related commands")]
    internal class GameCommands : ApplicationCommandModule
    {
#if DEBUG
        [SlashCommand("testtest", "TestCommand, Debug Only")]
        internal async Task TestyBoi(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync("YO THAT TEST COMMAND SUCCEEDED YO! THATS CASH MONEY YO!");
        }
#endif
        //[SlashCommand("howto", "How to tutorials")]
        //public async Task HowToCommand(InteractionContext ctx, [Option("Tutorial", "Which tutorial are you trying to view")][RemainingText] Tutorial.Tutorials tutorialName)
        //{
        //    await ctx.CreateResponseAsync($"hello world! {tutorialName}");
        //}

        //[SlashCommand("test", "test command")]
        //public async Task TestyTest(InteractionContext ctx, [Option("message", "message to bot")][RemainingText] string message)
        //{
        //    //BlueprintXML blueprintyBoi = GameItem.Blueprint.GetBlueprint("");
        //    //await ctx.CreateResponseAsync($"{blueprintyBoi.CraftType} {blueprintyBoi} {blueprintyBoi.Material11} {blueprintyBoi.Material12} {blueprintyBoi.Material13} {blueprintyBoi.Material21} {blueprintyBoi.Material22} {blueprintyBoi.Material23} {blueprintyBoi.Material31} {blueprintyBoi.Material32} {blueprintyBoi.Material33}  {blueprintyBoi.ItemID} {blueprintyBoi.Result}  {blueprintyBoi.SortID}");
        //}

        [SlashCommand("script", "Script documentation")]
        internal async Task ScriptDocumentation(InteractionContext ctx, [Option("Script", "Script Command Name")][RemainingText] string scriptname)
        {
            ScriptCommand.Script script = ScriptCommand.GetScript(scriptname.Replace(" ", ""));
            DiscordInteractionResponseBuilder messageBuilder = new();
            DiscordEmbedBuilder embedBuilder = new();
            if (script == null)
            {
                ExtractedResult<string> autoCorrectedNames = Process.ExtractOne(scriptname.Replace(" ", ""), ScriptCommand.ScriptNames, (s) => s, ScorerCache.Get<DefaultRatioScorer>());
                script = ScriptCommand.GetScript(autoCorrectedNames.Value);
            }
            embedBuilder.WithTitle($"🎛 SCRIPT - {script.Name}");
            embedBuilder.WithDescription(script.Description);
            embedBuilder.WithColor(DiscordColor.Goldenrod);

            messageBuilder.AddEmbed(embedBuilder);

            await ctx.CreateResponseAsync(messageBuilder);
        }

        [SlashCommand("item", "Gets info about any in-game item")]
        internal async Task WikiItem(InteractionContext ctx, [Description("ID of in game item")][Option("item", "Item or Block to review")][RemainingText] string itemid)
        {
            ItemDataXML _item = GameItem.GetItem(ItemIDConverter.ConvertBlockIDToItemID(itemid.Replace(" ", "")));
            DiscordInteractionResponseBuilder messageBuilder = new();
            DiscordEmbedBuilder embedBuilder = new();

            if (_item == null)
            {
                ExtractedResult<string> autoCorrectedNames = itemid.Any(char.IsUpper)
                    ? Process.ExtractOne(itemid.Replace(" ", ""), GameItem.ItemList, (s) => s, ScorerCache.Get<DefaultRatioScorer>())
                    : Process.ExtractOne(itemid.Replace(" ", ""), Array.ConvertAll(GameItem.ItemList, i => i.ToLower()), (s) => s, ScorerCache.Get<DefaultRatioScorer>());
                _item = GameItem.GetItem(ItemIDConverter.ConvertBlockIDToItemID(autoCorrectedNames.Value.Replace(" ", "")));
                messageBuilder.WithContent($"I encountered an error; `{itemid.Replace("`", "'")}` doesn't seem to be a valid item id.\nDid you mean `{_item.ItemID}`? Confidence: `{autoCorrectedNames.Score}%`");

            }

            embedBuilder.WithTitle($"📇 INFO - {_item.Name} [{ItemRarity.GetRarity(_item.IDString)}]");
            embedBuilder.WithDescription(_item.Desc);
            embedBuilder.WithColor(ItemColor.GetItemColor(_item.IDString));
            embedBuilder.WithFooter($"id: {_item.IDString} #{(int)_item.ItemID}");
            embedBuilder.WithImageUrl(StringBuilders.GetItemIconLink(_item.IDString));

            // Price
            if (_item.MinCSPrice > 0)
            {
                embedBuilder.AddField(":coin: Price buy/sell", $"{Math.Floor(_item.MinCSPrice * 1.2f)}/{_item.MinCSPrice}", true);
            }
            else
            {
                embedBuilder.AddField(":coin: Price buy/sell", "N/A", true);
            }

            // Durability / Stack Size
            if (_item.Durability > 0)
            {
                embedBuilder.AddField(":tools: Durability", _item.Durability.ToString(), true);
            }
            else
            {
                embedBuilder.AddField(":inbox_tray: Stack Size", _item.StackSize.ToString(), true);
            }

            // Damage
            if (_item.StrikeDamage > 0)
            {
                embedBuilder.AddField(":crossed_swords: Strike Damage", _item.StrikeDamage.ToString(), true);
            }

            // Reach
            if (_item.StrikeReach is > 0 and not 1)
            {
                embedBuilder.AddField(":straight_ruler: Strike Reach", $"{_item.StrikeReach} blocks", true);
            }

            if (_item.StrikeReach == 1)
            {
                embedBuilder.AddField(":straight_ruler: Strike Reach", $"{_item.StrikeReach} block", true);
            }

            // Healing
            if (_item.HealPower > 0)
            {
                embedBuilder.AddField(":heart: Heal Power", _item.HealPower.ToString(), true);
            }

            // Smelting
            if (_item.BurnTime > 0)
            {
                embedBuilder.AddField(":fuelpump: Burn Time", _item.BurnTime.ToString(), true);
            }

            if (_item.SmeltTime > 0)
            {
                embedBuilder.AddField(":fire: Smelt Time", _item.SmeltTime.ToString(), true);
            }

            // Brightness
            if (_item.ParticleLight > 0)
            {
                embedBuilder.AddField(":candle: Particle Light", _item.ParticleLight.ToString(), true);
            }

            // Drop chance
            if (_item.DropChance > 0)
            {
                embedBuilder.AddField(":game_die: Drop Chance", _item.DropChance.ToString(), true);
            }

            embedBuilder.AddField(":closed_lock_with_key: Mob Drop If Locked", _item.CanDropIfLocked.ToString(), true);

            // Plural Name
            if (_item.Plural == PluralType.None)
            {
                embedBuilder.AddField(":pen_ballpoint: Plural", $"{_item.Name}", true);
            }
            else if (_item.Plural == PluralType.ES)
            {
                embedBuilder.AddField(":pen_ballpoint: Plural", $"{_item.Name}es", true);
            }
            else if (_item.Plural == PluralType.S)
            {
                embedBuilder.AddField(":pen_ballpoint: Plural", $"{_item.Name}s", true);
            }

            DiscordButtonComponent infoButton = new(ButtonStyle.Secondary, "Info_Button", "Info", true, new DiscordComponentEmoji("📇"));
            DiscordButtonComponent craftButton = new(ButtonStyle.Primary, "Craft_Button", "Craft [Beta]", true, new DiscordComponentEmoji("⚒️"));

            messageBuilder.AddComponents(infoButton, craftButton);
            messageBuilder.AddEmbed(embedBuilder);


            await ctx.CreateResponseAsync(messageBuilder);

        }
    }
}
