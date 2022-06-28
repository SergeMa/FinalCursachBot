using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Cursach.Constants;
using Cursach.Model;
using Newtonsoft.Json;


namespace Cursach.Client
{
    public class HeroClient
    {
        private HttpClient _client = new HttpClient();
        private static string _address;
        public HeroClient()
        {
            _address = Constants.Constants.Address + "/Hero";
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }

        public async Task<heroModel> GetHeroByIdAsync(int Id)
        {
            var responce = await _client.GetAsync($"{_address}/Id/{Id}");
            var heroList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<heroModel>(heroList);

            return result;
        }

        public async Task<heroModel> GetHeroByNameAsync(string name)
        {
            var responce = await _client.GetAsync($"{_address}/Name/{name}");
            var heroList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<heroModel>(heroList);

            return result;
        }

        public async Task<List<string>> GetHeroesByAttr(string mainAttr)
        {
            var responce = await _client.GetAsync($"{_address}/Attr/{mainAttr}");
            var heroList = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<string>>(heroList);

            return result;
        }
    }
}
