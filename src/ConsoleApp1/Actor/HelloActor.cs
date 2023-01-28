using LanguageExt;
using Proto;
using Effect.Actor;
using static Effect.Actor.IActor<ConsoleApp1.Actor.HelloActor.RT>;
using static Effect.Actor.IStore<ConsoleApp1.Actor.HelloActor.RT>;
using static LanguageExt.Prelude;

namespace ConsoleApp1.Actor;
public readonly partial record struct HelloActor : IActor
{
    public static Aff<RT, Unit> ReceiveAff =>
        //from __1 in ReceiveAff<Started>(static _ => SetStoreEff("Started"))
        from __2 in ReceiveAndResponseResultOrErrorAff<string, string>(static msg =>
            from get in GetStoreEff<string>()
            from __1 in SetStoreEff(msg)
            select get)
        select unit;
}
