using ConsoleApp1.Actor;
using Effect.Actor;
using LanguageExt;
using Proto;
using static LanguageExt.Prelude;
using static Effect.Actor.ISender<RT>;
using Effect.Abstractions;

var system = new ActorSystem();
var root = system.Root;

var pid = root.Spawn(Props.FromProducer(() => new HelloActor()));

var q = from __ in SuccessAff<RT, Unit>(unit)
        let f1 = AskAff<string>(pid, "Hello")
        let f2 = AskAff<string>(pid, "Hello1")
        from _1 in f1
        from _2 in Eff(fun(() => Console.WriteLine(_1)))
        from _3 in f1
        from _4 in Eff(fun(() => Console.WriteLine(_3)))
        select unit;

using var cts = CancellationTokenSource.CreateLinkedTokenSource(system.Shutdown);
var ret = await q.Run(new RT(root, cts));
ret.ThrowIfFail();

public readonly record struct RT
(
    ISenderContext Context,
    CancellationTokenSource CancellationTokenSource
) : ISender<RT>
{
    ISenderContext IHas<RT, ISenderContext>.It => Context;
}
