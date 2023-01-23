using LanguageExt;
using Proto;
using static Effect.Actor.IActor<ConsoleApp1.Actor.HelloActor.RT>;
using static Effect.Actor.IState<ConsoleApp1.Actor.HelloActor.RT>;
using static LanguageExt.Prelude;

namespace ConsoleApp1.Actor;
public readonly partial record struct HelloActor : IActor
{
    public static Aff<RT, Unit> ReceiveAff =>
        from __1 in ReceiveAff<Started>(static _ => SetEff("Started"))
        from __2 in ReceiveAff<string>(static msg =>
            from get in GetEff<string>()
            from __1 in SetEff(msg)
            from __2 in RespondEff(get)
            select unit)
        select unit;
}
