using RestSharp;
using Effect.Abstractions;
using LanguageExt;
using static LanguageExt.Prelude;
using LanguageExt.Pipes;

namespace Effect.RestSharp;

public interface IRest<RT> : IHas<RT, RestClient> where RT : struct, IRest<RT>
{
    public static Aff<RT, T> GetAff<T>(RestRequest request) =>
        from rest in Eff
        from ct__ in cancelToken<RT>()
        from res_ in Aff(() => rest.GetAsync<T>(request, ct__).ToValue())
        select res_;
}
