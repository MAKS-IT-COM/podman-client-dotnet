namespace MaksIT.PodmanClientDotNet {
  public partial class PodmanClient {
    private readonly HttpClient _httpClient;

    public PodmanClient(string baseAddress, int timeOut) {
      _httpClient = new HttpClient();
      _httpClient.BaseAddress = new Uri(baseAddress);
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      _httpClient.Timeout = TimeSpan.FromMinutes(timeOut);
    }    

    public PodmanClient(HttpClient httpClient) {
      _httpClient = httpClient;
      _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      _httpClient.Timeout = TimeSpan.FromMinutes(60);
    }

  }
}
