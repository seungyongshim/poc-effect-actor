using Effect.Abstractions;
using Proto;


namespace Effect.Actor.Tests;

using static IActor<RT>;

public class ActorSpec
{
    [Fact]
    public async Task Respond()
    {
        var system = new ActorSystem();
        var root = system.Root;

        var pid = root.Spawn(Props.FromFunc(async ctx =>
        {
            var q = from ___ in unitAff
                    from __1 in RespondEff(ctx.Message)
                    select unit;

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ctx.System.Shutdown);
            await q.Run(new(cts, ctx));
        }));

        var ret = await root.RequestAsync<string>(pid, "Hello");

        Assert.Equal("Hello", ret);

        await root.PoisonAsync(pid);
    }
}

public readonly record struct RT
(
    CancellationTokenSource CancellationTokenSource,
    IContext Context
) : IActor<RT>
{
    IContext IHas<RT, IContext>.It => Context;


}
