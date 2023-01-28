using Effect.Abstractions;
using LanguageExt;
using LanguageExt.Pipes;
using Proto;
using static LanguageExt.Prelude;
using static LanguageExt.Pipes.Producer;
namespace Effect.Actor;


public partial interface IActor<RT> : IHas<RT, IContext> where RT : struct, IActor<RT>
{
    public static Producer<RT, T, Unit> ReceivePipe<T>() =>
        from ctx in Eff
        from __1 in ctx.Message switch
        {
            T m => yield<RT, T>(m),
            _ => Pure<RT, T, Unit>(unit)
        }
        select unit;
}
