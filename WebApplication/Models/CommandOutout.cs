using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication.AskSql.Models;

public class CommandOutput
{

    [JsonPropertyName("results")]
    public IEnumerable<dynamic> Results
    {
        get;
        set;
    } = new List<dynamic>();

    [JsonPropertyName("hasMore")]
    public bool HasMore
    {
        get;
        set;
    }

    [JsonPropertyName("responseTime")]
    public double ResponseTime
    {
        get;
        set;
    }
}