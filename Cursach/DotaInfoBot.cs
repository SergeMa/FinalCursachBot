using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InlineQueryResults;
using System.Threading.Tasks;
using Cursach.Client;
using Cursach.Model;

namespace Cursach
{

    public class DotaInfoBot
    {
        TelegramBotClient botClient = new TelegramBotClient("5378861445:AAE89BopRNBfWRex2TOR-x_xA5ZHytNWekI");

        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        public string LastTeam = "eg";

        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Bot {botMe.Username} started!");
            Console.ReadKey();
        }

        private Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Error in API:\n {apiRequestException.ErrorCode}" + $"\n{apiRequestException.Message}", _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessage(botClient, update.Message);
            }
            if (update?.Type == UpdateType.CallbackQuery)
            {
                await HandlerCallbackQuery(botClient, update.CallbackQuery);
            }
        }

        private async Task HandlerCallbackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("ActiveMembers"))
            {
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Getting active team members of team {LastTeam}");
                List<playerModel> activePlayers = new TeamClient().GetActiveTeamMembers(LastTeam).Result;
                foreach (playerModel item in activePlayers)
                {
                    Console.WriteLine(item.name);
                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, ReturnPlayerModel(item));
                }
                return;
            }
            else if (callbackQuery.Data.StartsWith("AddToFav"))
            {
                await new FavClient(callbackQuery.Message).PostFavouriteTeamAsync(LastTeam);
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Team {LastTeam} added  to favorites");
                return;
            }
            else if (callbackQuery.Data.StartsWith("RemoveFromFav"))
            {
                await new FavClient(callbackQuery.Message).DeleteFavoriteTeamAsync(LastTeam);
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Team {LastTeam} deleted from favorites");
                return;
            }
            
        }

        private async Task HandlerMessage(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                string answer = $"Hello, I am Dota Info Bot. I can tell you a lot of statstics about heroes, teams and players\n" +
                    $"How to get information: write the following commands in chat\n" +
                    $"/hero (name) - get hero stats by it's name\n" +
                    $"/allheroes - get the names of all heroes\n" +
                    $"/team(name or tag) - get team stats by it's name\n" +
                    $"/allteams - get the names of all teams\n" +
                    $"/player (name) - get player stats by it's nickname\n" +
                    $"/myfavorites - get stats of your favorite teams";
                
                await botClient.SendTextMessageAsync(message.Chat.Id, answer);
            }
            else if (message.Text == "/myfavorites")
            {
                FavClient fc = new FavClient(message);
                var list = fc.FindFavoriteTeamAsync().Result.ToList();
                if (list.Count >= 2)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Your favourite teams are:");
                    foreach (var item in list)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, ReturnTeamModel(item));
                    }
                }
                else if (list.Count == 1)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Your favourite team is:");
                    foreach (var item in list)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, ReturnTeamModel(item));
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "You have no favourite teams.");
                }


            }
            else if (message.Text == "/allheroes")
            {
                string strHeroes = "";
                var str = new HeroClient().GetHeroesByAttr("str").Result;
                for (int i = 0; i < str.Count; i++)
                {
                    if (i <= str.Count - 2)
                    {
                        strHeroes += str[i] + ", ";
                    }
                    else
                    {
                        strHeroes += str[i];
                    }
                }

                string agiHeroes = "";
                var agi = new HeroClient().GetHeroesByAttr("agi").Result;
                for (int i = 0; i < agi.Count; i++)
                {
                    if (i <= agi.Count - 2)
                    {
                        agiHeroes += str[i] + ", ";
                    }
                    else
                    {
                        agiHeroes += str[i];
                    }
                }

                string intHeroes = "";
                var intH = new HeroClient().GetHeroesByAttr("int").Result;
                for (int i = 0; i < intH.Count; i++)
                {
                    if (i <= intH.Count - 2)
                    {
                        intHeroes += intH[i] + ", ";
                    }
                    else
                    {
                        intHeroes += intH[i];
                    }
                }

                string answer = $"Heroes by attribute:\n" +
                    $"\n" +
                    $"Strengh: {strHeroes}\n" +
                    $"\n" +
                    $"Agility: {agiHeroes}\n" +
                    $"\n" +
                    $"Intelligence: {intHeroes}";
                await botClient.SendTextMessageAsync(message.Chat.Id, answer);
            }
            else if (message.Text == "/allteams")
            {
                string allTeams = "Names of known teams are: ";
                List<string> list = new TeamClient().GetAllTeams().Result;
                for (int i = 0; i < 300; i++)
                {
                    if (i <= 298)
                    {
                        allTeams += list[i] + ", ";
                    }
                    else
                    {
                        allTeams += list[i];
                    }
                }
                await botClient.SendTextMessageAsync(message.Chat.Id, allTeams);
            }
            else if (message.Text == "/hero")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please specify the hero name (/hero *name*). Use /allheroes to get the hero names");
            }
            else if (message.Text == "/team")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please specify the team's name or tag (/hero *name or tag*). Use /allteams to get team names");
            }
            else if (message.Text == "/player")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Please specify the player's nickname");
            }
            else if (message.Text.Split(" ").Length >= 2)
            {
                string[] messageComInfo = message.Text.Split(" ");
                string messageCommand = messageComInfo[0];
                string messageInfo = "";
                for (int i = 1; i < messageComInfo.Length; i++)
                {
                    messageInfo += messageComInfo[i] + " ";
                }
                if (messageCommand == "/hero")
                {
                    HeroClient hc = new HeroClient();
                    heroModel hm = hc.GetHeroByNameAsync(messageInfo).Result;
                    if (hm.localized_name == "Hero not found")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Hero not found! You can view hero names at /allHeroes");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, ReturnHeroModel(hm));
                    }
                }
                else if (messageCommand == "/team")
                {
                    TeamClient tc = new TeamClient();
                    teamModel tm = tc.GetTeamByNameAsync(messageInfo).Result;
                    if (tm.name == "Team not found")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Team not found! You can view hero names at /allTeams");
                    }
                    else
                    {
                        var FavButton = InlineKeyboardButton.WithCallbackData("Remove from favorites", $"RemoveFromFav");
                        if (!new FavClient(message).exists(messageInfo).Result)
                        {
                            FavButton = InlineKeyboardButton.WithCallbackData("Add to favorites", $"AddToFav");
                        }
                        InlineKeyboardMarkup keyboardMarkup = new InlineKeyboardMarkup
                        (
                        new[]
                        {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Get active team players", $"ActiveMembers"),
                                    FavButton
                                }
                        }
                        );
                        await botClient.SendTextMessageAsync(message.Chat.Id, ReturnTeamModel(tm), replyMarkup: keyboardMarkup);
                        LastTeam = tm.name;
                    }
                }
                else if (messageCommand == "/player")
                {
                    PlayerClient pc = new PlayerClient();
                    playerModel pm = pc.GetPlayerByNameAsync(messageInfo).Result;
                    if (pm.name == "Player not found")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Player not found.");
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, ReturnPlayerModel(pm));
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Unknown command.");
                }

            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Sorry, I don't understand that.");
            }

            return;
        }

        public string ReturnHeroModel(heroModel hero)
        {
            string roles = "";
            foreach (var item in hero.roles)
            {
                roles += item + " ";
            }
            roles.Trim();
            string result = "";
            string imageUrl = "https://api.opendota.com";
            result += $"Name:{hero.localized_name}\n" +
                      $"Id: {hero.id}\n" +
                      $"Image: {imageUrl + hero.img.Remove(hero.img.Length -1)}\n" +
                      $"Primary attribute: {hero.primary_attr}\n" +
                      $"Roles: {roles}\n" +
                      $"Hero populatity: {hero.turbo_picks} picks, {hero.turbo_wins} wins\n" +
                      $"Winrate: {Math.Round(Math.Round((double)hero.turbo_wins/(double)hero.turbo_picks, 2) *100, 0)}% \n" +
                      $"Hero populatity (pro level): {hero.pro_pick} picks, {hero.pro_win} wins, {hero.pro_ban} bans\n" +
                      $"Winrate (pro level): {Math.Round(Math.Round((double)hero.pro_win / (double)hero.pro_pick, 2)*100)}%\n" +
                      $"\n" +
                      $"Base attributes: {hero.base_health} ( {hero.base_health_regen} regeneration per sec) health, {hero.base_mana} ( {hero.base_mana_regen} regeneration per sec) mana\n" +
                      $"Base stats: {hero.base_str} strength, {hero.base_agi} agility, {hero.base_int} intelligence\n" +
                      $"Base damage: {(hero.base_attack_max+hero.base_attack_min)/2} with attack range of {hero.attack_range} units\n" +
                      $"Base armor: {hero.base_armor} physical, {hero.base_mr} magical\n" +
                      $"Stats gain: +{hero.str_gain} strength, +{hero.agi_gain} agility, +{hero.int_gain} intelligence\n";

            return result;
        }

        public string ReturnTeamModel(teamModel team)
        {
            string result = "";
            result += $"Name: {team.name} ( Tag: {team.tag}, id: {team.team_id} )\n" +
                $"Wins: {team.wins}. Losses: {team.losses}\n" +
                $"Winrate: {Math.Round(Math.Round((double)team.wins / ((double)team.losses+(double)team.wins) , 2) * 100, 0)}% \n" +
                $"Rating: {team.rating}\n" +
                $"Logo: {team.logo_url}";

            return result;
        }

        public string ReturnPlayerModel(playerModel player)
        {
            string result = "";
            result += $"Name: {player.personaname} ( nickname: {player.name} )\n" +
                $"Account id: {player.account_id}\n" +
                $"Avatar: {player.avatarfull}\n" +
                $"Country of birth: {player.loccountrycode}\n" +
                $"Is part of team: {new TeamClient().GetTeamByNameAsync(player.team_name).Result.name}\n" +
                $"Is a pro player: {player.is_pro}\n" +
                $"Main role in a team: {player.fantasy_role}\n" +
                $"Last match played on {player.last_match_time}\n" +
                $"Link to profile: {player.profileurl}";

            return result;
        }
    }
}
