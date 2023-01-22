using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Effects.Traits;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public interface IActor<RT> : IHas<RT, IContext> where RT : struct, IActor<RT>
{
    public static Eff<RT, Unit> SendEff(PID target, object message) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Send(target, message)))
        select unit;

    public static Eff<RT, Unit> RespondEff(object message) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Respond(message)))
        select unit;

    public static Aff<RT, Unit> ReceiveAff<T>(Func<T, Aff<RT, Unit>> func) =>
        from ctx in Eff
        from __1 in ctx.Message switch
        {
            T m => func(m),
            _ => unitAff
        } | @catch(e => RespondEff(e).Bind(_ => FailAff<RT, Unit>(e)))
        select unit; Error
}
