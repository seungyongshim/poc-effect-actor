using Effect.Abstractions;
using LanguageExt;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public interface IState<RT> : IHas<RT, IContext> where RT: struct, IState<RT>
{
    public static Eff<RT, Unit> SetEff<T>(T obj) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Set(obj)))
        select unit;

    public static Eff<RT, T> GetEff<T>() =>
        from ctx in Eff
        from ret in Eff(fun(() => ctx.Get<T>()))
        select ret;
}
