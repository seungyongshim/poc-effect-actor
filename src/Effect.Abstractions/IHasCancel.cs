using LanguageExt.Effects.Traits;

namespace Effect.Abstractions;

public interface IHasCancel<RT> : HasCancel<RT> where RT : struct, IHasCancel<RT>
{
    RT HasCancel<RT>.LocalCancel => default;
    CancellationToken HasCancel<RT>.CancellationToken => CancellationTokenSource.Token;
}
