using LanguageExt;
using static LanguageExt.Prelude;

namespace Effect.Abstractions;

public interface IHas<RT, T> : IHasCancel<RT> where RT : struct, IHas<RT, T>
{
    protected T It { get; }

    public static Eff<RT, T> Eff => Eff<RT, T>(static rt => rt.It);
}
