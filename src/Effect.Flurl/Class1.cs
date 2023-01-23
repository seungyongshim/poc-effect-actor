using Effect.Abstractions;
using Flurl.Http;
using static LanguageExt.Prelude;
using LanguageExt;

namespace Effect.Flurl;

public interface IFlurl<RT> : IHas<RT, FlurlClient> where RT : struct, IFlurl<RT>
{
    public static Aff<RT, T> GetAff<T>(string parameter) =>
        from http in Eff
        from res in Aff(() => http.Request(parameter).GetAsync().ToValue())
        from ret in Aff(() => res.GetJsonAsync<T>().ToValue())
        select ret;
}
