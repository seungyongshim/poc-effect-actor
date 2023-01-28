using Effect.Abstractions;
using LanguageExt;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public partial interface IActor<RT> : IHas<RT, IContext> where RT : struct, IActor<RT>
{
    public static Eff<RT, Unit> RespondEff(object message)  =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Respond(message)))
        select unit;

    public static Aff<RT, Option<R>> ReceiveAff<T, R>(Func<T, Aff<RT, R>> func) =>
        from ctx in Eff
        from __1 in ctx.Message switch
        {
            T m => from ret in func.Invoke(m)
                   select Option<R>.Some(ret),
            _ => Eff(() => Option<R>.None),
        }
        select __1;

    public static Aff<RT, Unit> ReceiveAff<T>(Func<T, Aff<RT, Unit>> func) =>
        from ctx in Eff
        from __1 in ctx.Message switch
        {
            T m => func.Invoke(m),
            _ => unitAff
        }
        select unit;
}


public static class ActorExtention
{
    public static Aff<RT, Unit> RespondObjectOrErrorAff<RT, T>(this Aff<RT, Option<T>> aff)
        where RT : struct, IActor<RT>
        where T : notnull =>
        aff.BiBind(
            x => from _1 in x.Case switch
                 {
                    { } v =>  IActor<RT>.RespondEff(v),
                    _ => unitEff
                 }
                 select unit,
            e => from _1 in IActor<RT>.RespondEff(e)
                 from _2 in FailEff<RT, Unit>(e)
                 select _2);
}
