using LanguageExt.Effects.Traits;

namespace Effect.Actor;

public interface IHasCancel<RT> : HasCancel<RT> where RT : struct, IHasCancel<RT>
{
    RT HasCancel<RT>.LocalCancel => default;
    CancellationToken HasCancel<RT>.CancellationToken => CancellationTokenSource.Token;
}
