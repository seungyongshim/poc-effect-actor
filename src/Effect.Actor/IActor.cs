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

    public static Aff<RT, Unit> ReceiveAndResponseResultOrErrorAff<T, R>(Func<T, Aff<RT, R>> func) where R : notnull =>
        ReceiveAff(func).Apply(RespondObjectOrErrorAff);

    private static Aff<RT, Option<R>> ReceiveAff<T, R>(Func<T, Aff<RT, R>> func) =>
        from ctx in Eff
        from __1 in ctx.Message switch
        {
            T m => from ret in func.Invoke(m)
                   select Option<R>.Some(ret),
            _ => Eff(() => Option<R>.None),
        }
        select __1;

    public static Aff<RT, Unit> ReceiveAff<T>(Func<T, Aff<RT, Unit>> func) =>
        from __1 in ReceiveAff<T, Unit>(func)
        select unit;

    private static Aff<RT, Unit> RespondObjectOrErrorAff<T>(Aff<RT, Option<T>> aff)
       where T : notnull =>
       aff.BiBind(
           x => from _1 in x.Case switch
                {
                    { } v => RespondEff(v),
                    _ => unitEff
                }
                select unit,
           e => from _1 in RespondEff(e)
                from _2 in FailEff<RT, Unit>(e)
                select _2);
}
