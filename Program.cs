using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    public static async Task Main(string[] args)
    {
        using var httpClient = new HttpClient();
        Picture picture = new(httpClient);
        Console.WriteLine("Fetching a cat picture.");
        try
        {
            string pictureUrl = await picture.Fetch();
            Console.WriteLine("Look at this cat!");
             Process.Start(new ProcessStartInfo(pictureUrl) 
            { 
                UseShellExecute = true  // Open the URL
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong: ", ex.Message);
        }
    }
}

public class Picture
{
    private readonly HttpClient _httpClient;
    public Picture(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> Fetch()
    {
        Console.WriteLine("Give me one second...");
        await Task.Delay(1000);
        var data = await _httpClient.GetAsync("https://api.thecatapi.com/v1/images/search");
        data.EnsureSuccessStatusCode();
        var dataJson = await data.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(dataJson);
        var firstCat = jsonDoc.RootElement[0];
        string pictureUrl = firstCat.GetProperty("url").GetString() ?? throw new Exception("No URL found");
        return pictureUrl;
    }
}