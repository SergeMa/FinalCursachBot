using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Cursach.Model;
using Cursach.Constants;
using MongoDB.Driver;
using MongoDB.Bson;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;

namespace Cursach.Client
{
    public class FavClient
    {
        public string personName;

        public FavClient(Message message)
        {
            personName = message.Chat.Id.ToString();
        }
        public async Task PostFavouriteTeamAsync(string name)
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://SergiyMak:Mak2506serg@cluster0.dvqbr9x.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(personName);
            string collectionName = "favTeams";

            TeamClient tc = new TeamClient();

            var collection = database.GetCollection<teamModel>(collectionName);

            teamModel favTeam = tc.GetTeamByNameAsync(name).Result;
            var filter = Builders<teamModel>.Filter.Eq("team_id", favTeam.team_id);
            bool exists = await collection.Find(_ => _.team_id == favTeam.team_id).AnyAsync(); ;
            if (!exists && favTeam.team_id != 0)
            {
                await collection.InsertOneAsync(favTeam);
            }
        }

        public async Task DeleteFavoriteTeamAsync(string name)
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://SergiyMak:Mak2506serg@cluster0.dvqbr9x.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(personName);
            string collectionName = "favTeams";

            TeamClient tc = new TeamClient();
            teamModel favTeam = tc.GetTeamByNameAsync(name).Result;
            var collection = database.GetCollection<favTeamModel>(collectionName);
            var filter = Builders<favTeamModel>.Filter.Eq("team_id", favTeam.team_id);
            await collection.DeleteOneAsync(filter);
        }
        
        public async Task<List<teamModel>> FindFavoriteTeamAsync()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://SergiyMak:Mak2506serg@cluster0.dvqbr9x.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(personName);
            string collectionName = "favTeams";
            TeamClient tc = new TeamClient();
            var collection = database.GetCollection<favTeamModel>(collectionName);
            var result = await collection.FindAsync(_ => true);

            List<teamModel> res = new List<teamModel>();

            foreach (var item in result.ToList())
            {
                res.Add(tc.GetTeamByNameAsync(item.tag).Result);
            }
            return res;
        }

        public async Task<bool> exists(string name)
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://SergiyMak:Mak2506serg@cluster0.dvqbr9x.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(personName);
            string collectionName = "favTeams";
            TeamClient tc = new TeamClient();
            var collection = database.GetCollection<favTeamModel>(collectionName);
            bool Exists = await collection.Find(_ => _.team_id == new TeamClient().GetTeamByNameAsync(name).Result.team_id).AnyAsync();
            return Exists;
        }
    }
}
