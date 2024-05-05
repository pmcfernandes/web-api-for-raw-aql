using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.AskSql.Models;

namespace WebApplication.AskSql.Controllers;

[ApiController]
public class AskSqlController : ControllerBase
{
    public readonly IConfiguration _configuration;

    public AskSqlController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [Route("/api/sql")]
    [Route("/api/sql/{Database}")]
    public async Task<IActionResult> Index([FromBody] CommandInput input, [FromRoute] string Database = "master", [FromQuery(Name = "page")] int Page = 1, [FromQuery(Name = "t")] string ReturnType = "JSON")
    {
        DataSet ds = new DataSet();

        dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(input.Arguments.ToString(), new ExpandoObjectConverter());

        var output = new CommandOutput();
        var startTime = Stopwatch.GetTimestamp();

        using (var conn = new SqlConnection() { ConnectionString = GetConnectionString(Database) })
        {
            switch (ReturnType.ToUpper())
            {
                case "JSON":
                    IEnumerable<dynamic> query;

                    try
                    {
                        query = await conn.QueryAsync(input.Sql, (object)data);
                    }
                    catch (Exception ex)
                    {
                        return Problem(ex.Message, null, 500);
                    }

                    int intTotal = query.Count();

                    if (intTotal == 0)
                    {
                        return NoContent();
                    }

                    if (intTotal > 200)
                    {
                        query = query.Skip(200 * (Page - 1)).Take(200);
                    }

                    output.Results = query;
                    output.HasMore = query.Count() == 200 && intTotal > 200;
                    break;
                case "DATASET":
                    using (var reader = conn.ExecuteReader(input.Sql, (object)data))
                    {
                        int i = 0;

                        do
                        {
                            DataTable dt = new DataTable();
                            dt.TableName = $"Table{i}";
                            dt.Load(reader);
                            ds.Tables.Add(dt);

                            i++;
                        }
                        while (reader.NextResult());
                    }

                    break;
                default:
                    break;
            }
        }

        var endTime = Stopwatch.GetTimestamp();
        output.ResponseTime = TimeSpan.FromTicks(endTime - startTime).TotalSeconds;

        if (ReturnType.ToUpper() == "DATASET")
        {
            return Ok(ds.GetXml());
        }

        return Ok(output);
    }

    #region Private methods

    private string GetConnectionString(string Database)
    {
        if (_configuration["ConnectionStrings:DefaultConnection"] == null)
        {
            throw new NullReferenceException("ConnectionStrings:DefaultConnection");
        }

        return _configuration["ConnectionStrings:DefaultConnection"].Replace("=master;", $"={Database};");
    }

    #endregion
}
