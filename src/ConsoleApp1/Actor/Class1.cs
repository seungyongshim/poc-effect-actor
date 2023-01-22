using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effect.Actor;
using LanguageExt;
using LanguageExt.Effects.Traits;
using Proto;
using static LanguageExt.Prelude;

namespace ConsoleApp1.Actor;

public readonly partial record struct HelloActor
(
    IRootContext RootContext
) : IActor<RT>
{
    public static Aff<RT, Unit> ReceiveAff => 
}

public readonly partial record struct HelloActor
{
    public async Task ReceiveAsync(IContext context)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.System.Shutdown);

        await ReceiveAff.Run();
    }

    public readonly record struct RT
    (
    ) : IActor<RT>
    {
        IContext IHas<RT, IContext>.It { get; }
        CancellationTokenSource HasCancel<RT>.CancellationTokenSource { get; }
    }
}
