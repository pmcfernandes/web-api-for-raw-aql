using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace WebApplication.AskSql.Models;

public class CommandInput
{
    [JsonPropertyName("sql")]
    public string Sql
    {
        get;
        set;
    } = "";

    [JsonPropertyName("args")]
    public dynamic Arguments
    {
        get;
        set;
    } = new JObject();
}

