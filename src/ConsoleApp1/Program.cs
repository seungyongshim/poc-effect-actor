using ConsoleApp1.Actor;
using Proto;

var system = new ActorSystem();
var root = system.Root;

var pid = root.Spawn(Props.FromProducer(() => new HelloActor()));

try
{
    var ret = await root.RequestAsync<string>(pid, "Hello", system.Shutdown);
    Console.WriteLine(ret);
}
catch(Exception ex)
{
}

try
{

    var ret1 = await root.RequestAsync<string>(pid, "Hello1", system.Shutdown);
    Console.WriteLine(ret1);
}
catch (Exception ex)
{
}


