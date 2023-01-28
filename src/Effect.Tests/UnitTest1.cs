

var q = from __ in unitEff
        let f1 = Eff(fun(() => Console.WriteLine("1")))
        let f2 = Eff(fun(() => Console.WriteLine("2")))
        from _1 in f1
        from _2 in f2
        from _3 in f1
        select unit;

q.Run();
q.Run();

