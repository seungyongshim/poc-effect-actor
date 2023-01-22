using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Effects.Traits;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public interface IActor<RT> : IHas<RT, IContext> where RT : struct, IActor<RT>
{
    static Eff<RT, Unit> SendEff(PID target, object message) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Send(target, message)))
        select unit;
}
