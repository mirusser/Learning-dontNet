
//API docs:  https://beta.openai.com/docs/guides/completion
// Single-file deployment: https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview?WT.mc_id=DX-MVP-5004571&tabs=cli

using System.Text;
using Newtonsoft.Json;
using TextCopy;

if (args.Length > 0)
{
    HttpClient client = new();

    client.DefaultRequestHeaders.Add("authorization", "Bearer API_KEY");
    //client.DefaultRequestHeaders.Add("content", "application/json");

    string contentString = $$"""{"model": "text-davinci-001", "prompt": "{{args[0]}}", "temperature": 1, "max_tokens": 100}""";
    StringContent content = new(contentString, Encoding.UTF8, "application/json");

    HttpResponseMessage reponse = await client.PostAsync("https://api.openai.com/v1/completions", content);
    string responseString = await reponse.Content.ReadAsStringAsync();

    try
    {
        dynamic dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

        string guess = dyData.choices[0].text;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"---> My guess at the command prompt is: {guess}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"---> Could not deserialize the Json: {ex.Message}");
    }

    //Console.WriteLine(responseString);
}
else
{
    Console.WriteLine("---> You need to provide some input");
}

static string GuessCommand(string text)
{
    int lastIndex = text.LastIndexOf(Environment.NewLine);
    string guess = text.Substring(lastIndex + 1);


    ClipboardService.SetText(guess);
    return guess;
}