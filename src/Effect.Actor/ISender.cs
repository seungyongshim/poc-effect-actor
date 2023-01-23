using Effect.Abstractions;
using LanguageExt;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public interface ISender<RT> : IHas<RT, ISenderContext> where RT : struct, ISender<RT>
{
    public static Aff<RT, Unit> AskAff<T>(PID target, object message) =>
        from ctx in Eff
        from ct1 in cancelToken<RT>()
        from __1 in Aff(() => ctx.RequestAsync<T>(target, message, ct1).ToValue())
        select unit;

    public static Eff<RT, Unit> TellEff(PID target, object message) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Send(target, message)))
        select unit;
}
