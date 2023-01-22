using LanguageExt;
using Proto;
using static Effect.Actor.IActor<ConsoleApp1.Actor.HelloActor.RT>;
using static Effect.Actor.IState<ConsoleApp1.Actor.HelloActor.RT>;
using static LanguageExt.Prelude;

namespace ConsoleApp1.Actor;
public readonly partial record struct HelloActor : IActor
{
    public static Aff<RT, Unit> ReceiveAff =>
        from __ in unitEff
        //from _1 in ReceiveAff<Started>(_ => SetEff("Started"))
        from _2 in ReceiveAff<string>(msg =>
            from __ in unitEff
            from _1 in GetEff<string>()
            from _2 in SetEff(msg)
            from _3 in RespondEff(_1)
            select unit)
        select unit;
}
