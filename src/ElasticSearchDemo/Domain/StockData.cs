namespace Domain;

public class StockData
{
    public DateTime Date { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public int Volume { get; set; }
    public string Name { get; set; }

    public StockData(string dataLine)
    {
        string[] values = dataLine.Split(',', StringSplitOptions.TrimEntries);

        if (DateTime.TryParse(values[0], out var tempDate))
            Date = tempDate;

        if (double.TryParse(values[1], out var tempOpen))
            Open = tempOpen;

        if (double.TryParse(values[2], out var tempHigh))
            High = tempHigh;

        if (double.TryParse(values[3], out var tempLow))
            Low = tempLow;

        if (double.TryParse(values[4], out var tempClose))
            Close = tempClose;

        if (int.TryParse(values[5], out var tempVolume))
            Volume = tempVolume;

        Name = values[6];
    }
}