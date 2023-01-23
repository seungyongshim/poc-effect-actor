using Effect.Abstractions;
using Effect.Actor;
using LanguageExt.Effects.Traits;
using Proto;

namespace ConsoleApp1.Actor;

public readonly partial record struct HelloActor
{
    public async Task ReceiveAsync(IContext context)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.System.Shutdown);
        var ret = await ReceiveAff.Run(new(context, cts));
    }
    public readonly record struct RT
    (
        IContext Context,
        CancellationTokenSource CancellationTokenSource
    ) : IActor<RT>,
        IState<RT>
    {
        IContext IHas<RT, IContext>.It => Context;
        CancellationTokenSource HasCancel<RT>.CancellationTokenSource => CancellationTokenSource;
    }
}
