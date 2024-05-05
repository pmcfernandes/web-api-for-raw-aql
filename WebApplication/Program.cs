using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.

var app = builder.Build();
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value?.StartsWith("/api/sql") == true)
    {
        if (context.Request.Headers.Authorization.Count == 0 || context.Request.Headers.Authorization[0].Substring("Bearer ".Length) != "34493434nj3kn43knden34jk34sac23c3")
        {
            if (!context.Connection.RemoteIpAddress.MapToIPv4().Equals(IPAddress.Parse("127.0.0.1")))
            {
                context.Response.StatusCode = 404;
                return;
            }
        }
    }

    await next(context);
});
app.MapControllers();
app.Run();


