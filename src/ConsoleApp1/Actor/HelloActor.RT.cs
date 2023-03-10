using Effect.Abstractions;
using Effect.Actor;
using LanguageExt.Effects.Traits;
using Proto;

namespace ConsoleApp1.Actor;

public readonly partial record struct HelloActor(Func<IContext, CancellationTokenSource, HelloActor.RT> FuncRT)
{
    public async Task ReceiveAsync(IContext context)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.System.Shutdown);
        var ret = await ReceiveAff.Run(FuncRT(context, cts));
    }
    public readonly record struct RT
    (
        IContext Context,
        CancellationTokenSource CancellationTokenSource
    ) : IActor<RT>,
        IStore<RT>
    {
        IContext IHas<RT, IContext>.It => Context;

        IContextStore IHas<RT, IContextStore>.It => Context;

        CancellationTokenSource HasCancel<RT>.CancellationTokenSource => CancellationTokenSource;
    }
}
