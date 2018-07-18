using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Airport.MockApi.ResponseModels;
using System.Net.Http;
using Newtonsoft.Json;

namespace Airport.MockApi
{
  public interface IMockApiConnector
  {
    Task<IList<CrewResponse>> GetCrews(int offset = 0, int limit = 10);
  }

  public class MockApiConnector : IMockApiConnector
  {
    protected readonly string BASE_URL = "http://5b128555d50a5c0014ef1204.mockapi.io";

    public async Task<IList<CrewResponse>> GetCrews(int offset = 0, int limit = 10)
    {
      var crews = await Get<IList<CrewResponse>>("crew");

      return crews.Skip(offset).Take(limit).ToList();
    }

    private async Task<T> Get<T>(string url)
    {
      var client = new HttpClient();
      client.BaseAddress = new Uri(BASE_URL);
      var data = await client.GetStringAsync(url);

      return JsonConvert.DeserializeObject<T>(data);
    }
  }


}
