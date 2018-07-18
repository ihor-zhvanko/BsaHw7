using System;
using System.Threading.Task;

namespace Airport.MockApiConnector
{
    public interface IMockApiConnector
    {
        Task<CrewResponse> GetCrews(int offset = 0, int limit = 10);
    }

    public class MockApiConnector : IMockApiConnector
    {
        protected readonly string BASE_URL = "http://5b128555d50a5c0014ef1204.mockapi.io";

        

        private async Task<T> Get<T>(string url)
    {
      var client = new HttpClient();
      client.BaseAddress = new Uri(BASE_URL);
      var data = await client.GetStringAsync(url);

      return JsonConvert.DeserializeObject<T>(data);
    }
    }

    
}
