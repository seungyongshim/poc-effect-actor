using Effect.Abstractions;
using LanguageExt;
using LanguageExt.Common;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public interface ISender<RT> : IHas<RT, ISenderContext> where RT : struct, ISender<RT>
{
    public static Aff<RT, T> AskAff<T>(PID target, object message) =>
        from ctx in Eff
        from ct1 in cancelToken<RT>()
        from ret in Aff(() => ctx.RequestAsync<object>(target, message, ct1).ToValue())
        from __1 in guardnot(ret is Error, ret as Error)
        select (T)ret;

    public static Eff<RT, Unit> TellEff(PID target, object message) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Send(target, message)))
        select unit;
}
