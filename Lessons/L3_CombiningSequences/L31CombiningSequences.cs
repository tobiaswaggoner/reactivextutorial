using System.Reactive.Linq;

using RxTutorial.Lessons.StateMonitors;

namespace RxTutorial.Lessons.L3_CombiningSequences;

// ReSharper disable UnusedType.Global

// Lets start here: A simple timer with a "OnTick" event, running on a background task
// Don't forget to make it Disposable because the Task will run indefinitely.

public class L31CombiningSequences : ILesson
{
    public string Name => "Combine multiple sequences to create a joined sequence";

    public IDisposable ExecuteLesson()
    {
        var cpu = new StateMonitor( upPercentage : 90 );
        var mem = new StateMonitor( upPercentage : 85 );
        var disk = new StateMonitor( upPercentage : 80 );

        var combination = Observable.CombineLatest( cpu, mem, disk )
                                    .Select( DumpResultsFoReadability )
                                    .Select( results => results.All( result => result == "OK" ) )
                                    .Select( ok => ok ? "OK" : "WARN" )
                                    .DistinctUntilChanged();

        return combination.Subscribe( state => Console.WriteLine( $"State changed to: {state}" ) );
    }

    private static IList<string> DumpResultsFoReadability( IList<string> results )
    {
        Console.WriteLine( $"    ... {string.Join( ",", results )}" );
        return results;
    }
}