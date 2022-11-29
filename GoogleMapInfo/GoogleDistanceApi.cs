using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GoogleMapInfo
{
    public class GoogleDistanceApi
    {
        private readonly IConfiguration _configuration;

        public GoogleDistanceApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleDistanceData> GetMapDistance(string originCity, string destinationCity)
        {
            string? apiKey = _configuration["googleDistanceApi:apiKey"];

            string? googleDistanceApiUrl = _configuration["googleDistanceApi:apiUrl"];

            googleDistanceApiUrl += $"units=imperial&origins={originCity}&destinations={destinationCity}&key={apiKey}";
            
            using HttpClient client = new HttpClient();
            HttpRequestMessage request = 
                new HttpRequestMessage(HttpMethod.Get, new Uri(googleDistanceApiUrl)); 
            
            HttpResponseMessage response = await client.SendAsync(request); 
            
            response.EnsureSuccessStatusCode();
            
            await using Stream data = await response.Content.ReadAsStreamAsync();
            
            GoogleDistanceData? distanceInfo = 
                await JsonSerializer.DeserializeAsync<GoogleDistanceData>(data);
            
            return distanceInfo;
        }
    }
}