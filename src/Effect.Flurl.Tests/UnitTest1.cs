using Effect.Abstractions;
using Flurl.Http;
using RichardSzalay.MockHttp;

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

        var q = IFlurl<RT>.GetAff<Response>(new("api/user/1234"));

        var ret = await q.Run(new(cts, rest));

        Assert.Equal(ret.ThrowIfFail(), new()
        {
            Name = "Value",
        });
    }

    [Fact]
    public async Task Get400()
    {

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
