using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Cursach.Model;
using Cursach.Client;
using Cursach.Constants;

namespace Cursach.Client
{
    public class TeamClient
    {
        private HttpClient _client = new HttpClient();
        private static string _address;
        public TeamClient()
        {
            _address = Constants.Constants.Address + "/Team";
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }

        public async Task<teamModel> GetTeamByIdAsync(double Id)
        {
            var responce = await _client.GetAsync($"{_address}/Id/{Id}");
            var playerList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<teamModel>(playerList);
            if (result.team_id != 0)
            {
                return result;
            }
            else return new teamModel() { name = "Team not found" };
        }

        public async Task<teamModel> GetTeamByNameAsync(string name)
        {
            var responce = await _client.GetAsync($"{_address}/Name/{name}");
            var heroList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<teamModel>(heroList);
            if (result.team_id != 0)
            {
                return result;
            }
            else return new teamModel() { name = "Team not found" };
        }

        public async Task<List<playerModel>> GetActiveTeamMembers(string name)
        {
            var responce = await _client.GetAsync($"{_address}/{name}/ActivePlayers");
            var playerList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<playerModel>>(playerList);

            return result;
        }

        public async Task<List<string>> GetAllTeams()
        {
            var responce = await _client.GetAsync($"{_address}/All");
            var playerList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<string>>(playerList);

            return result;
        }
    }
}
