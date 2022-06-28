using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Cursach.Model;
using Cursach.Constants;

namespace Cursach.Client
{
    public class PlayerClient
    {
        private HttpClient _client = new HttpClient();
        private static string _address;
        public PlayerClient()
        {
            _address = Constants.Constants.Address + "/Player";
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }

        public async Task<playerModel> GetPlayerByIdAsync(double Id)
        {
            var responce = await _client.GetAsync($"{_address}/Id/{Id}");
            var playerList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<playerModel>(playerList);
            if (result.account_id != 0)
            {
                return result;
            }
            else return new playerModel() { name = "Player not found" };
        }

        public async Task<playerModel> GetPlayerByNameAsync(string name)
        {
            var responce = await _client.GetAsync($"{_address}/Name/{name}");
            var playerList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<playerModel>(playerList);
            if (result.account_id != 0)
            {
                return result;
            }
            else return new playerModel() { name = "Player not found" };
        }
    }
}