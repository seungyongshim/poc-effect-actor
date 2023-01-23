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

var q = from __ in unitEff
        from _1 in AskAff<string>(pid, "Hello")
        from _2 in AskAff<string>(pid, "Hello1")
        select unit;

using var cts = CancellationTokenSource.CreateLinkedTokenSource(system.Shutdown);
var ret = await q.Run(new(root, cts));

public readonly record struct RT
(
    ISenderContext Context,
    CancellationTokenSource CancellationTokenSource
) : ISender<RT>
{
    ISenderContext IHas<RT, ISenderContext>.It => Context;
}
