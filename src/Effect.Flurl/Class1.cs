using Effect.Abstractions;
using Flurl.Http;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using static LanguageExt.Pipes.Pipe;
using static LanguageExt.Prelude;

namespace Effect.Flurl;

public record FlurlError(int Code, dynamic FlurlResponse) : Expected(Code.ToString(), Code, None);

public interface IFlurl<RT> : IHas<RT, FlurlClient> where RT : struct, IFlurl<RT>
{
    protected static Producer<RT, IFlurlResponse, Unit> GetProducer(string parameter) =>
        from http in Eff
        from ret in Aff(() => http.AllowAnyHttpStatus().Request(parameter).GetAsync().ToValue())
        from __1 in Producer.yield<RT, IFlurlResponse>(ret)
        select unit;

    public static Aff<RT, T> GetAff<T>(string parameter) =>
        from http in Eff
        from ret in use(Aff(() => http.AllowAnyHttpStatus().Request(parameter).GetAsync().ToValue()), res =>
            from __1 in when(res.StatusCode >= 300,
                from __1 in Aff(() => res.GetJsonAsync().ToValue())
                from __2 in FailAff<RT, Unit>(new FlurlError(res.StatusCode, __1))
                select unit)
            from ret in Aff(() => res.GetJsonAsync<T>().ToValue())
            select ret)
        select ret;
}

