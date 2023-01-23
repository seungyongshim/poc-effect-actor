using System.Net;
using Effect.Abstractions;
using Flurl.Http;
using RichardSzalay.MockHttp;
using LanguageExt;
using static LanguageExt.Prelude;

using static Effect.Flurl.IFlurl<Effect.Flurl.Tests.RT>;
using System.Diagnostics;

namespace Effect.Flurl.Tests;
public record Response
{
    public required string Name { get; init; }
};



public class FlurlSpec
{
    [Fact]
    public async Task Get()
    {
        using var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/api/user/*")
                .Respond("application/json", """
                {
                    "name" : "Test McGee"
                }
                """);

        using var http = mockHttp.ToHttpClient();
        using var rest = new FlurlClient(http)
        {
            BaseUrl = "http://localhost"
        };

        using var cts = new CancellationTokenSource();

        var q = GetAff<Response>(new("api/user/1234"));

        var ret = await q.Run(new(cts, rest));

        Assert.Equal(ret.ThrowIfFail(), new()
        {
            Name = "Value",
        });
    }

    [Fact]
    public async Task Get400()
    {
        using var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://localhost/api/user/*")
                .Respond(HttpStatusCode.BadRequest, "application/json", """
                {
                    "name" : "Test McGee"
                }
                """);

        using var http = mockHttp.ToHttpClient();
        using var rest = new FlurlClient(http)
        {
            BaseUrl = "http://localhost"
        };

        using var cts = new CancellationTokenSource();

        var q = from res in GetAff<Response>(new("api/user/1234"))
                select res;

        var ret = await q.Run(new(cts, rest));

        ret.IfFail(err => Debug.WriteLine(err));

        Assert.Equal(ret.ThrowIfFail(), new()
        {
            Name = "Value",
        });
    }
}

public readonly record struct RT
(
    CancellationTokenSource CancellationTokenSource,
    FlurlClient FlurlClient
) : IFlurl<RT>
{
    FlurlClient IHas<RT, FlurlClient>.It => FlurlClient;
}
