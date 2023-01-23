using Effect.Abstractions;
using LanguageExt;
using RestSharp;
using static LanguageExt.Prelude;

using static Effect.RestSharp.IRest<Effect.RestSharp.Tests.RT>;
using RichardSzalay.MockHttp;

namespace Effect.RestSharp.Tests;
public record Response
{
    public required string Name { get; init; }
};

public class RestSpec
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
        using var rest = new RestClient(http, new RestClientOptions()
        {
            BaseUrl = new("http://localhost")
        });

        using var cts = new CancellationTokenSource();

        var q = GetAff<Response>(new("api/user/1234"));

        var ret = await q.Run(new(cts, rest));

        Assert.Equal(ret.ThrowIfFail(), new()
        {
            Name = "Value",
        });
    }
}

public readonly record struct RT
(
    CancellationTokenSource CancellationTokenSource,
    RestClient RestClient
) : IRest<RT>
{
    RestClient IHas<RT, RestClient>.It => RestClient;
}
