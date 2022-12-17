using Domain;

namespace Ingest;

public class StockDataReader
{
    internal IEnumerable<StockData?> StockData()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Data\\all_stocks_5yr.csv");

        if (!File.Exists(filePath))
        {
            yield return default;
        }

        var file = new StreamReader(filePath);
        string? line;
        var i = -1;

        while ((line = file.ReadLine()) != null)
        {
            i++;
            if (i == 0)
            {
                //first line of csv file consists of column names
                //skip
                continue;
            }

            yield return new(line);
        }
    }
}