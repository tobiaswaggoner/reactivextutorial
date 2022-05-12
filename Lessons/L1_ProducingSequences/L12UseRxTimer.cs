using System.Reactive.Linq;

// ReSharper disable UnusedType.Global

// The same thing with an Observable timer. Well obviously one existed. But wait a bit to see the difference in action

namespace RxTutorial.Lessons.L1_ProducingSequences;

public class L12UseRxTimer : ILesson
{
    public string Name => "Use an Rx Extension to create a timer";

    public IDisposable ExecuteLesson()
    {
        var rnd = new Random();
        
        var timer = Observable.Timer( TimeSpan.FromMilliseconds( 100 ), TimeSpan.FromMilliseconds( 100 ) )
                              .Select( i => rnd.NextInt64(1, 100) );
                              
        return timer.Subscribe( i => Console.WriteLine( $"The Rx.Net timer ticked: {i}" ) );
    }
}