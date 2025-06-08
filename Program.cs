using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    private static readonly string catUrl = "https://api.thecatapi.com/v1/images/search";
    public static async Task Main(string[] args)
    {
        CatFetcher catFetcher = new();
        Console.WriteLine("Fetching a cat picture.");
        try
        {
            string catPictureUrl = await catFetcher.Fetch(catUrl);
            Console.WriteLine("Look at this cat!");
            Process.Start(new ProcessStartInfo(catPictureUrl)
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

public class CatFetcher
{
    public async Task<string> Fetch(string catUrl)
    {
        using HttpClient client = new HttpClient();
        await SimulateDelay();

        var data = await client.GetAsync(catUrl);
        data.EnsureSuccessStatusCode();
        var dataJson = await data.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(dataJson);
        var firstObj = jsonDoc.RootElement[0];
        string pictureUrl = firstObj.GetProperty("url").GetString() ?? throw new Exception("No URL found");

        return pictureUrl;
    }

    public async Task SimulateDelay()
    {
        Console.WriteLine("Give me one second...");
        await Task.Delay(1000);
    }
}