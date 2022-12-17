namespace Domain;

public class StockData
{
    public string date { get; set; }
    public string open { get; set; }
    public string high { get; set; }
    public string low { get; set; }
    public string close { get; set; }
    public string volume { get; set; }
    public string Name { get; set; }

    public StockData(string dataLine)
    {
        string[] values = dataLine.Split(',', StringSplitOptions.TrimEntries);

        date = values[0];
        open = values[1];
        high = values[2];
        low = values[3];
        close = values[4];
        volume = values[5];
        Name = values[6];
    }
}