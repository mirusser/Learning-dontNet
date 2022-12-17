using System.Text;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IElasticClient client;

    public StockController(IElasticClient client)
    {
        this.client = client;
    }

    [HttpGet("names")]
    public async Task<IActionResult> Get()
    {
        var response = await client.SearchAsync<StockData>(s =>
            s.Aggregations(a =>
                a.Terms("names", t => t.Field(f => f.Name).Size(500))));

        var request = new SearchRequest<StockData>
        {
            Aggregations = new TermsAggregation("name")
            {
                Field = Infer.Field<StockData>(f => f.Name),
                Size = 500
            }
        };

        response = await client.SearchAsync<StockData>(request);

        if (!response.IsValid)
        {
            return NotFound();
        }

        var names = response.Aggregations
            .Terms("name")
            .Buckets.Select(b => b.Key)
            .ToList();

        if (names.Any())
        {
            return Content(string.Join("\r\n", names));
        }

        return NotFound();
    }

    [HttpGet("names/{name}")]
    public async Task<IActionResult> GetName(string name)
    {
        var response = await client.SearchAsync<StockData>(s => s
            .Query(q => q
                .ConstantScore(cs => cs
                    .Filter(f => f
                        .Term(t => t
                            .Field(fld => fld.Name)
                            .Value(name)))))
            .Sort(srt => srt.Descending(f => f.Date))
            .Size(25));
        // TODO: something wrong with sorting, fix later (prolly type of property 'date')

        if (!response.IsValid)
        {
            return NotFound();
        }

        StringBuilder sb = new();
        foreach (var doc in response.Documents)
        {
            sb
                .AppendFormat("{0,-12:d}", doc.Date)
                .AppendFormat("{0,8:F2}", doc.Low)
                .AppendFormat("{0,8:F2}", doc.High)
                .AppendLine();
        }

        return Content(sb.ToString());
    }

    [HttpGet("volumes/{name}")]
    public async Task<IActionResult> GetVolume(string name)
    {
        var response = await client.SearchAsync<StockData>(s => s
            .Size(0)
            .Query(q => q
                .ConstantScore(cs => cs
                    .Filter(f => f
                        .Term(t => t
                            .Field(fld => fld.Name)
                            .Value(name)))))
            .Aggregations(a => a
                .DateHistogram(
                    "by-month",
                    dh => dh
                        .Field(fld => fld.Date)
                        .Order(HistogramOrder.KeyDescending)
                        .CalendarInterval(DateInterval.Month)
                        .Aggregations(a2 => a2
                            .Sum(
                                "trade-volume",
                                sum => sum.Field(fld => fld.Volume))))));
        if (!response.IsValid) { return NotFound(); }

        var montlyVolume = response.Aggregations
            .DateHistogram("by-month").Buckets;

        StringBuilder sb = new();
        foreach (var bucket in montlyVolume)
        {
            var volume = bucket.Sum("trade-volume").Value;
            sb
                .AppendFormat("{0}", bucket.Date.Year)
                .AppendFormat(" {0:MMMM}", bucket.Date)
                .AppendFormat("{0,17:N0}", volume)
                .AppendLine();
        }

        return Content(sb.ToString());
    }

    [HttpGet("search/{searchText}")]
    public async Task<IActionResult> Search(string searchText)
    {
        var response = await client.SearchAsync<StockData>(s => s
            .Query(q => q
                .Match(ma => ma
                    .Field(f => f.Name)
                    .Query(searchText))));
        if (!response.IsValid) { return NotFound(); }
        StringBuilder sb = new();
        foreach (var d in response.Documents)
        {
            sb.AppendLine($"{d.Name,-26}{d.Date,-10:d}{d.Low,10}{d.High,10}");
        }

        return Content(sb.ToString());
    }
}