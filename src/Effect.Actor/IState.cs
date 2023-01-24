using Effect.Abstractions;
using LanguageExt;
using Proto;
using static LanguageExt.Prelude;
namespace Effect.Actor;

public interface IStore<RT> : IHas<RT, IContextStore> where RT: struct, IStore<RT>
{
    public static Eff<RT, Unit> SetStoreEff<T>(T obj) =>
        from ctx in Eff
        from __1 in Eff(fun(() => ctx.Set(obj)))
        select unit;

    public static Eff<RT, T> GetStoreEff<T>() =>
        from ctx in Eff
        from ret in Eff(fun(() => ctx.Get<T>()))
        select ret;
}
